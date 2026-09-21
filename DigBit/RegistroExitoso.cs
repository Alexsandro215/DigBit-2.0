using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DigBit.Infraestructura;
using DrawingFont = System.Drawing.Font;

namespace DigBit
{
    /// <summary>
    /// Lo que ve el alumno que trajo su propia computadora despues de guardar su
    /// bitacora. No se libera el equipo del laboratorio (ese alumno no lo va a
    /// usar), asi que en vez del escritorio se le ofrecen las dos unicas salidas
    /// sensatas: apagar la maquina o dejarla en el login para el siguiente.
    ///
    /// Si nadie toca nada vuelve sola al login, para que la maquina no se quede
    /// en esta pantalla cuando el alumno simplemente se levanta y se va.
    /// </summary>
    public class RegistroExitoso : Form
    {
        /// <summary>Que eligio el alumno.</summary>
        public enum Opcion
        {
            /// <summary>Volver al login (tambien lo que pasa al agotarse el tiempo).</summary>
            Login,

            /// <summary>Apagar el equipo.</summary>
            Apagar
        }

        private const int SegundosParaVolver = 30;

        private readonly string alumno;
        private readonly string matricula;
        private readonly string laboratorio;
        private readonly string codigo;

        private int segundosRestantes = SegundosParaVolver;
        private Opcion eleccion = Opcion.Login;
        private Timer temporizador;
        private Label lblCuentaAtras;

        private RegistroExitoso(string alumno, string matricula, string laboratorio, string codigo)
        {
            this.alumno = alumno;
            this.matricula = matricula;
            this.laboratorio = laboratorio;
            this.codigo = codigo;
            InicializarComponentes();
        }

        /// <summary>
        /// Muestra la pantalla y devuelve lo que eligio el alumno. Quien la llama
        /// es el responsable de apagar o de volver al login: aqui solo se decide.
        /// </summary>
        public static Opcion Mostrar(string alumno, string matricula, string laboratorio, string codigo)
        {
            using (RegistroExitoso pantalla = new RegistroExitoso(alumno, matricula, laboratorio, codigo))
            {
                pantalla.ShowDialog();
                return pantalla.eleccion;
            }
        }

        private void InicializarComponentes()
        {
            BackColor = Color.White;
            ClientSize = new Size(700, 470);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ControlBox = false;
            ShowInTaskbar = true;
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = Kiosco.Activo;
            Text = "DigBit - Bitacora registrada";

            Panel palomita = new Panel
            {
                BackColor = Color.White,
                Location = new Point(310, 30),
                Size = new Size(80, 80)
            };
            palomita.Paint += Palomita_Paint;

            Label lblTitulo = new Label
            {
                AutoSize = false,
                Font = new DrawingFont("Century Gothic", 20F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(30, 125),
                Size = new Size(640, 40),
                Text = "Bitacora registrada",
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblAlumno = new Label
            {
                AutoSize = false,
                Font = new DrawingFont("Century Gothic", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(30, 172),
                Size = new Size(640, 26),
                Text = Describir(alumno, matricula),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblDetalle = new Label
            {
                AutoSize = false,
                Font = new DrawingFont("Century Gothic", 10F),
                ForeColor = Color.FromArgb(70, 70, 70),
                Location = new Point(30, 202),
                Size = new Size(640, 24),
                Text = DescribirSesion(),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblNota = new Label
            {
                AutoSize = false,
                Font = new DrawingFont("Century Gothic", 10.5F),
                ForeColor = Color.FromArgb(70, 70, 70),
                Location = new Point(60, 240),
                Size = new Size(580, 60),
                Text = "Registraste tu asistencia con tu propia computadora, asi que no necesitas "
                    + "usar este equipo. Apagalo si eres el ultimo, o dejalo en la pantalla de "
                    + "inicio para el siguiente alumno.",
                TextAlign = ContentAlignment.TopCenter
            };

            Button btnApagar = CrearBoton("Apagar el equipo", new Point(140, 330), new Size(190, 48));
            btnApagar.Click += (s, e) => Elegir(Opcion.Apagar);

            Button btnLogin = CrearBoton("Volver al inicio", new Point(370, 330), new Size(190, 48));
            btnLogin.Click += (s, e) => Elegir(Opcion.Login);

            lblCuentaAtras = new Label
            {
                AutoSize = false,
                Font = new DrawingFont("Century Gothic", 9F),
                ForeColor = Color.Gray,
                Location = new Point(30, 400),
                Size = new Size(640, 22),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(palomita);
            Controls.Add(lblTitulo);
            Controls.Add(lblAlumno);
            Controls.Add(lblDetalle);
            Controls.Add(lblNota);
            Controls.Add(btnApagar);
            Controls.Add(btnLogin);
            Controls.Add(lblCuentaAtras);

            AcceptButton = btnLogin;

            // En kiosco ocupa toda la pantalla y no se puede cerrar con Alt+F4 ni
            // con "Finalizar tarea". Cerrar asignando DialogResult si pasa (llega
            // como CloseReason.None), que es como salen los dos botones.
            Kiosco.Aplicar(this);

            if (!Kiosco.Activo)
            {
                // Fuera del kiosco Aplicar() no hace nada, asi que el bloqueo del
                // cierre hay que ponerlo aqui: la pantalla tiene que resolverse por
                // uno de los dos botones o por la cuenta atras.
                FormClosing += (s, e) =>
                {
                    if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.TaskManagerClosing)
                    {
                        e.Cancel = true;
                    }
                };
            }

            ActualizarCuentaAtras();
            temporizador = new Timer { Interval = 1000 };
            temporizador.Tick += Temporizador_Tick;
            temporizador.Start();
        }

        private string Describir(string nombre, string numero)
        {
            bool hayNombre = !string.IsNullOrWhiteSpace(nombre);
            bool hayNumero = !string.IsNullOrWhiteSpace(numero);

            if (hayNombre && hayNumero && nombre.Trim() != numero.Trim())
            {
                return nombre.Trim() + " - " + numero.Trim();
            }

            return hayNombre ? nombre.Trim() : (hayNumero ? numero.Trim() : "");
        }

        private string DescribirSesion()
        {
            string texto = "Registrado con equipo propio";
            if (!string.IsNullOrWhiteSpace(codigo))
            {
                texto += ", clase " + codigo.Trim();
            }
            if (!string.IsNullOrWhiteSpace(laboratorio))
            {
                texto += ", " + laboratorio.Trim();
            }

            return texto + ".";
        }

        private Button CrearBoton(string texto, Point posicion, Size tamano)
        {
            Button boton = new Button
            {
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new DrawingFont("Century Gothic", 11F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = posicion,
                Size = tamano,
                Text = texto,
                UseVisualStyleBackColor = false
            };
            boton.FlatAppearance.BorderColor = Color.DarkGreen;
            boton.FlatAppearance.BorderSize = 2;
            return boton;
        }

        // Circulo verde con una palomita, dibujado a mano: no depende de que el
        // equipo tenga una fuente concreta con el simbolo.
        private void Palomita_Paint(object sender, PaintEventArgs e)
        {
            Panel lienzo = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            System.Drawing.Rectangle circulo = new System.Drawing.Rectangle(2, 2, lienzo.Width - 5, lienzo.Height - 5);
            using (SolidBrush relleno = new SolidBrush(Color.DarkGreen))
            {
                e.Graphics.FillEllipse(relleno, circulo);
            }

            using (Pen trazo = new Pen(Color.White, 7F))
            {
                trazo.StartCap = LineCap.Round;
                trazo.EndCap = LineCap.Round;
                trazo.LineJoin = LineJoin.Round;
                e.Graphics.DrawLines(trazo, new[]
                {
                    new Point((int)(lienzo.Width * 0.27), (int)(lienzo.Height * 0.52)),
                    new Point((int)(lienzo.Width * 0.44), (int)(lienzo.Height * 0.69)),
                    new Point((int)(lienzo.Width * 0.74), (int)(lienzo.Height * 0.33))
                });
            }
        }

        private void Temporizador_Tick(object sender, EventArgs e)
        {
            segundosRestantes--;
            if (segundosRestantes <= 0)
            {
                Log.Info("Nadie eligio en la pantalla de registro con equipo propio; se vuelve al login.");
                Elegir(Opcion.Login);
                return;
            }

            ActualizarCuentaAtras();
        }

        private void ActualizarCuentaAtras()
        {
            lblCuentaAtras.Text = "Si no eliges nada, se volvera a la pantalla de inicio en "
                + segundosRestantes + " s.";
        }

        private void Elegir(Opcion opcion)
        {
            eleccion = opcion;
            if (temporizador != null)
            {
                temporizador.Stop();
            }

            // En un formulario modal basta con asignar DialogResult para cerrarlo;
            // Close() se reportaria como UserClosing y lo bloquearia la guarda.
            DialogResult = DialogResult.OK;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && temporizador != null)
            {
                temporizador.Dispose();
                temporizador = null;
            }

            base.Dispose(disposing);
        }
    }
}
