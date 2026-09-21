using System;
using System.Drawing;
using System.Windows.Forms;
using DigBit.conexion;
using DigBit.Infraestructura;

namespace DigBit
{
    public partial class Login : Form
    {
        private Conexion mconexion;
        private Consultas consultas = new Consultas();
        private RegistroAlumnos regAlum;
        private recuperarContraseña recupCont;
        public Login()
        {
            InitializeComponent();
            mconexion = new Conexion();
            Kiosco.Aplicar(this);
            if (AppContexto.Actual != null)
            {
                AppContexto.Actual.Registrar(this);
            }

            // En kiosco no hay a donde salir ni sentido en minimizar.
            imgSalir.Visible = !Kiosco.Activo;
            imgMinimizar.Visible = !Kiosco.Activo;

            // Darse de alta uno mismo no tiene sentido en un equipo del
            // laboratorio: los alumnos los carga la escuela. Y tiene un coste
            // real, porque obliga a que el usuario de MySQL de los equipos
            // pueda INSERTAR en usuarios, y ese usuario y su contrasena estan
            // en connections.config, que el alumno puede leer. Con ese permiso,
            // desde cualquier cliente de MySQL se puede crear un usuario de
            // tipo 3, que es administrador. Ver db/07_usuarios_minimos.sql.
            btnRegistro.Visible = !Kiosco.Activo;

            // Fase 4: franja de aviso cuando se trabaja con la copia local.
            lblSinConexion = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Bottom,
                Height = 34,
                BackColor = Color.FromArgb(255, 204, 128),
                ForeColor = Color.Black,
                Font = new Font("Century Gothic", 9F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };
            Controls.Add(lblSinConexion);
            lblSinConexion.BringToFront();
            ActualizarAvisoSinConexion();

            temporizadorAviso = new Timer { Interval = 5000 };
            temporizadorAviso.Tick += (s, e) => ActualizarAvisoSinConexion();
            temporizadorAviso.Start();

            PrepararBotonApagar();
        }

        private Label lblSinConexion;
        private Timer temporizadorAviso;
        private Button btnApagar;

        /// <summary>
        /// En el kiosco no hay menu Inicio ni barra de tareas, asi que sin esto
        /// la unica forma de apagar es Ctrl+Alt+Supr, que mucha gente no conoce,
        /// o el boton fisico. Tiene que verse y decir lo que hace.
        ///
        /// Fuera del kiosco no se pone: ahi Windows ya tiene su propio menu, y
        /// ademas ApagarEquipo() no apaga nada en un equipo de desarrollo.
        /// </summary>
        private void PrepararBotonApagar()
        {
            if (!Kiosco.Activo)
            {
                return;
            }

            btnApagar = new Button
            {
                Text = "Apagar el equipo",
                Size = new Size(200, 46),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Century Gothic", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(63, 63, 70),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                // Sin TabStop: que no se apague el equipo por darle a Tab y Enter
                // mientras se teclea la contrasena.
                TabStop = false
            };
            btnApagar.FlatAppearance.BorderSize = 0;
            btnApagar.Click += (s, e) => ConfirmarApagado();
            Controls.Add(btnApagar);
            btnApagar.BringToFront();

            ColocarBotonApagar();
            PrepararBotonEmergencia();
            Resize += (s, e) => { ColocarBotonApagar(); ColocarBotonEmergencia(); };
        }

        private Button btnEmergencia;

        /// <summary>
        /// Salida para cuando DigBit no puede validar ningun codigo: sin
        /// servidor y sin copia local utilizable, el laboratorio se queda
        /// inservible. Pide clave y memoria USB (los dos), y abre el escritorio
        /// sin bitacora.
        ///
        /// Se muestra siempre, no solo cuando falla el servidor: asi tambien
        /// sirve si el problema es otro (un horario mal cargado, un codigo que
        /// no llega). A cambio es una puerta permanente, y lo unico que la
        /// cierra es que la memoria este en el llavero del encargado. Cada uso
        /// queda registrado.
        /// </summary>
        private void PrepararBotonEmergencia()
        {
            if (!Kiosco.Activo)
            {
                return;
            }

            btnEmergencia = new Button
            {
                Text = "Acceso de emergencia",
                Size = new Size(200, 32),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Century Gothic", 8.5F),
                BackColor = Color.FromArgb(94, 94, 102),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                TabStop = false
            };
            btnEmergencia.FlatAppearance.BorderSize = 0;
            btnEmergencia.Click += (s, e) => AbrirEmergencia();
            Controls.Add(btnEmergencia);
            btnEmergencia.BringToFront();
            ColocarBotonEmergencia();
        }

        /// <summary>Justo encima del de apagar, y mas discreto que el.</summary>
        private void ColocarBotonEmergencia()
        {
            if (btnEmergencia == null || IsDisposed)
            {
                return;
            }

            const int margen = 24;
            int franja = (lblSinConexion != null && lblSinConexion.Visible) ? lblSinConexion.Height : 0;
            int altoApagar = (btnApagar != null) ? btnApagar.Height + 10 : 0;
            btnEmergencia.Location = new Point(
                ClientSize.Width - btnEmergencia.Width - margen,
                ClientSize.Height - btnEmergencia.Height - margen - franja - altoApagar);
        }

        private void AbrirEmergencia()
        {
            if (!DialogoEmergencia.Pedir(this))
            {
                return;
            }

            this.Hide();
            if (!SesionEquipo.LiberarEmergencia())
            {
                this.Show();
                MessageBox.Show(
                    "No se pudo abrir el escritorio." + Environment.NewLine + Environment.NewLine +
                    "Avisa al encargado del laboratorio.",
                    "Acceso de emergencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>Abajo a la derecha, por encima de la franja de sin conexion si la hay.</summary>
        private void ColocarBotonApagar()
        {
            if (btnApagar == null || IsDisposed)
            {
                return;
            }

            const int margen = 24;
            int franja = (lblSinConexion != null && lblSinConexion.Visible) ? lblSinConexion.Height : 0;
            btnApagar.Location = new Point(
                ClientSize.Width - btnApagar.Width - margen,
                ClientSize.Height - btnApagar.Height - margen - franja);
        }

        private void ConfirmarApagado()
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Seguro que quieres apagar el equipo?" + Environment.NewLine + Environment.NewLine +
                "Guarda antes tu trabajo.",
                "Apagar el equipo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            if (!SesionEquipo.ApagarEquipo())
            {
                MessageBox.Show(
                    "No se pudo apagar el equipo." + Environment.NewLine + Environment.NewLine +
                    "Avisa al encargado del laboratorio.",
                    "Apagar el equipo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void ActualizarAvisoSinConexion()
        {
            if (lblSinConexion == null || IsDisposed)
            {
                return;
            }

            string texto = SinConexion.TextoAviso();
            lblSinConexion.Text = texto;
            lblSinConexion.Visible = texto.Length > 0;
            ColocarBotonApagar();
            ColocarBotonEmergencia();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (temporizadorAviso != null)
            {
                temporizadorAviso.Stop();
                temporizadorAviso.Dispose();
                temporizadorAviso = null;
            }

            base.OnFormClosed(e);
        }


        //Evento Enter para que el texto "USUARIO e encuentra dentro del TextBox se borre al momento de dar clic 
        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "USUARIO")
            {
                txtUsuario.Text = "";
                txtUsuario.ForeColor = Color.DarkGreen;
            }
        }

        //Evento para que cuando el texto dentro del TextBox se encuentre vacio se vuelva a mostrar el texto "USUARIO"
        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                txtUsuario.Text = "USUARIO";
                txtUsuario.ForeColor = Color.DarkGreen;
            }
        }

        //Evento Enter para que el texto "CONTRASEÑA" que se encuentra dentro del TextBox se borre al momento de dar clic 
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "CONTRASEÑA")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.DarkGreen;
                txtPassword.UseSystemPasswordChar = true; // El texto ingresado se mostrara como caracteres de contraseña

            }

        }

        //Evento para que cuando el texto dentro del TextBox se encuentre vacio se vuelva a mostrar el texto "CONTRASEÑA"
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "CONTRASEÑA";
                txtPassword.ForeColor = Color.DarkGreen;
                txtPassword.UseSystemPasswordChar = false;
            }
        }

        //Se configura la imagen "imgSalir" para que al momento de dar clic se cierre la aplicacion (solo fuera del kiosco)
        private void imgSalir_Click(object sender, EventArgs e)
        {
            AppContexto.Actual.Salir();
        }

        // Se configura la imagen "imgMinimizar" para que al momento de dar clic de minimice la ventana
        private void imgMinimizar_Click(object sender, EventArgs e)
        {
            if (!Kiosco.Activo)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener los valores de los campos de usuario y contraseña
                string numeroIdentificadorIngresado = txtUsuario.Text;
                string contraseñaIngresada = txtPassword.Text;

                // Validar que los campos no estén vacíos y no contengan los valores por defecto
                if (numeroIdentificadorIngresado == "USUARIO" || contraseñaIngresada == "CONTRASEÑA" ||
                    string.IsNullOrWhiteSpace(numeroIdentificadorIngresado) || string.IsNullOrWhiteSpace(contraseñaIngresada))
                {
                    MessageBox.Show("Por favor, ingrese un usuario y una contraseña válidos.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Salir del método si algún campo está vacío o contiene el valor por defecto
                }

                // Validar que el usuario no contenga caracteres especiales
                if (!System.Text.RegularExpressions.Regex.IsMatch(numeroIdentificadorIngresado, @"^[a-zA-Z0-9]+$"))
                {
                    MessageBox.Show("El usuario no puede contener caracteres especiales.", "Usuario Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Salir del método si el usuario contiene caracteres especiales
                }

                // Fase 4: sin servidor, el alumno entra solo con su matricula contra la
                // copia local; su bitacora quedara marcada como registrada sin conexion.
                if (!SinConexion.Activo)
                {
                    try
                    {
                        Conexion.Probar();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("El servidor no responde al iniciar sesion", ex);
                        if (!SinConexion.IntentarActivar(ex.Message))
                        {
                            MessageBox.Show("No hay conexión con el servidor y este equipo no tiene una copia del horario. Avisa al encargado del laboratorio.", "Sin conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        ActualizarAvisoSinConexion();
                    }
                }

                if (SinConexion.Activo)
                {
                    AlumnoCache alumno = SinConexion.Cache.BuscarAlumno(numeroIdentificadorIngresado);
                    if (alumno == null)
                    {
                        MessageBox.Show("Sin conexión solo pueden entrar alumnos, y esa matrícula no está en la copia local de este equipo.", "Sin conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Log.Aviso("Alumno " + alumno.Matricula + " entra SIN CONEXION (matricula sin contrasena).");
                    consultas.AbrirPestanaSegunTipoUsuario(1, alumno.Matricula);
                    return;
                }

                // El administrador es un usuario mas de la base (fk_tipo_usuario = 3)
                // desde la fase 6; antes eran credenciales escritas aqui.
                if (consultas.RealizarInicioSesion(numeroIdentificadorIngresado, contraseñaIngresada))
                {
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si hay una excepción
                MessageBox.Show($"Error al intentar iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private void btnRegistro_Click(object sender, EventArgs e)
        {
            if (regAlum == null || regAlum.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                regAlum = new RegistroAlumnos();
            }
            //Muestra el formulario
            regAlum.Show();

            //Si esta en segundo plano lo trae al frente
            regAlum.BringToFront();
        }

        private void linkPasswd_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Bucle para verificar si ya existe el objeto
            if (recupCont == null || recupCont.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                recupCont = new recuperarContraseña();
                //Muestra el formulario
                recupCont.Show();

                //Si esta en segundo plano lo trae al frente
                recupCont.BringToFront();
            }
        }
    }
}
