using DigBit.conexion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DigBit
{

    public partial class RegistroAlumnos : Form
    {
        // Declaración de variables
        private RegistroProfesores profesoresForm = new RegistroProfesores(); // Instancia del formulario de profesores
        private Timer animacionTimer; // Temporizador para la animación
        private double dtransparencia = 0.0; // Valor de transparencia para la animación
        private double dtransparencia2 = 1.0; // Valor de transparencia para la animación
        private Conexion mconexion;
        private InsercionDatos insercionDatos;
        private Consultas consultas;


        public RegistroAlumnos()
        {
            InitializeComponent();
            consultas = new Consultas();
            CargarDatoscbCarrera();
            LimpiarComboGrupo();
            mconexion = new Conexion();
            insercionDatos = new InsercionDatos();
        }

        // Evento del botón para ocultar el formulario actual y mostrar el formulario de inicio de sesión
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();
        }

        // Evento del botón para iniciar la animación y mostrar el formulario de profesores
        private void btnProfesores_Click(object sender, EventArgs e)
        {
            //Iniciar la animación
            dtransparencia = 0;
            animacionTimer = new Timer();
            animacionTimer.Interval = 15; // Intervalo de tiempo para la animación (en milisegundos)
            animacionTimer.Tick += AnimacionTimer_Tick; // Aquí se conecta el evento Tick del temporizador
            animacionTimer.Start();
        }

        // Evento del temporizador para controlar la animación
        private void AnimacionTimer_Tick(object sender, EventArgs e)
        {
            // Incrementar la transparencia para la animación
            dtransparencia += 0.15;
            dtransparencia2 -= 0.05;

            // Detener la animación cuando alcanza la opacidad completa
            if (dtransparencia >= 1)
            {
                dtransparencia = 1;
                dtransparencia2 = 0;
                animacionTimer.Stop();
                this.Hide();
            }

            // Establecer la opacidad del formulario de profesores y del formulario actual
            profesoresForm.Opacity = dtransparencia;
            this.Opacity = dtransparencia2;
            profesoresForm.Show();
        }


        private void txtNombre_Enter(object sender, EventArgs e)
        {
            if (txtNombre.Texts == "Nombre")
            {
                txtNombre.Texts = "";
                txtNombre.ForeColor = Color.Black;

            }
        }
        private void txtNombre_Leave(object sender, EventArgs e)
        {
            if(txtNombre.Texts == "")
            {
                txtNombre.Texts += "Nombre";
                txtNombre.ForeColor = Color.Black;
            }
        }

        private void txtApellidoP_Enter(object sender, EventArgs e)
        {
            if(txtApellidoP.Texts== "Apellido Paterno")
            {
                txtApellidoP.Texts="";
                txtApellidoP.ForeColor = Color.Black;
            }
        }

        private void txtApellidoP_Leave(object sender, EventArgs e)
        {
            if(txtApellidoP.Texts == "")
            {
                txtApellidoP.Texts = "Apellido Paterno";
                txtApellidoP.ForeColor= Color.Black;
            }
        }

        private void txtApellidoM_Enter(object sender, EventArgs e)
        {
            if(txtApellidoM.Texts == "Apellido Materno")
            {
                txtApellidoM.Texts = "";
                txtApellidoM.ForeColor= Color.Black;
            }
        }

        private void txtApellidoM_Leave(object sender, EventArgs e)
        {
            if(txtApellidoM.Texts == "")
            {
                txtApellidoM.Texts = "Apellido Materno";
                txtApellidoM.ForeColor = Color.Black;
            }
        }

        private void txtMatricula_Enter(object sender, EventArgs e)
        {
            if(txtMatricula.Texts == "Matricula")
            {
                txtMatricula.Texts = "";
                txtMatricula.ForeColor= Color.Black;
            }
        }

        private void txtMatricula_Leave(object sender, EventArgs e)
        {
            if(txtMatricula.Texts == "")
            {
                txtMatricula.Texts = "Matricula";
                txtMatricula.ForeColor = Color.Black;   
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Texts == "Contraseña")
            {
                txtPassword.Texts = "";
                txtPassword.ForeColor = Color.Black;

            }
        }

        private void rjTcorreo_Leave(object sender, EventArgs e)
        {
            if (rjTcorreo.Texts == "")
            {
                rjTcorreo.Texts = "Correo";
                rjTcorreo.ForeColor = Color.Black;    
            }
        }

        private void rjTcorreo_Enter(object sender, EventArgs e)
        {
            if (rjTcorreo.Texts == "Correo")
            {
                rjTcorreo.Texts = "";
                rjTcorreo.ForeColor = Color.Black;

            }
        }
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Texts == "")
            {
                txtPassword.Texts += "Contraseña";
                txtPassword.ForeColor = Color.Black;
            }
        }

        // Evento TextChanged para el nombre
        private void txtNombre__TextChanged(object sender, EventArgs e)
        {
        }


        private void txtApellidoP__TextChanged(object sender, EventArgs e)
        {
        }

        private void txtApellidoM__TextChanged(object sender, EventArgs e)
        {
        }

        private void txtMatricula__TextChanged(object sender, EventArgs e)
        {
        }

        private void txtPassword__TextChanged(object sender, EventArgs e)
        {
        }
        private bool RealizarInsercion()
        {
            string nombre = txtNombre.Texts;
            string apellidoPaterno = txtApellidoP.Texts;
            string apellidoMaterno = txtApellidoM.Texts;
            string matricula = txtMatricula.Texts;
            string contraseña = txtPassword.Texts;
            string correo = rjTcorreo.Texts;

            // Llama a la función InsertarUsuario con los valores de los TextBox
            return insercionDatos.InsertarUsuario(nombre, apellidoPaterno, apellidoMaterno, matricula, contraseña, 1, correo);
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(txtNombre.Texts) || txtNombre.Texts == "Nombre" ||
                    string.IsNullOrWhiteSpace(txtApellidoP.Texts) || txtApellidoP.Texts == "Apellido Paterno" ||
                    string.IsNullOrWhiteSpace(txtApellidoM.Texts) || txtApellidoM.Texts == "Apellido Materno" ||
                    string.IsNullOrWhiteSpace(txtMatricula.Texts) || txtMatricula.Texts == "Matricula" ||
                    string.IsNullOrWhiteSpace(rjTcorreo.Texts) || rjTcorreo.Texts == "Correo" ||
                    string.IsNullOrWhiteSpace(txtPassword.Texts))
                {
                    // Mostrar mensaje si algún campo está vacío
                    MessageBox.Show("Todos los campos son obligatorios. Por favor, completa la información.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Salir del método si algún campo está vacío
                }

                // Realizar el registro
                bool registrado = RealizarInsercion();
                if (registrado)
                {
                    MessageBox.Show("Alumno registrado exitosamente.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si hay una excepción
                MessageBox.Show($"Error al intentar registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void CargarDatoscbCarrera()
        {
            try
            {
                cbCarrera.Items.Clear();

                foreach (OpcionCombo carrera in consultas.ObtenerCarreras())
                {
                    cbCarrera.Items.Add(carrera);
                }

                cbCarrera.SelectedIndex = -1;
                cbCarrera.Texts = "Carrera";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los nombres de las carreras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarDatoscbGrupo(int idCarrera)
        {
            try
            {
                cbGrupo.Items.Clear();

                foreach (OpcionCombo grupo in consultas.ObtenerGruposPorCarrera(idCarrera))
                {
                    cbGrupo.Items.Add(grupo);
                }

                cbGrupo.SelectedIndex = -1;
                cbGrupo.Texts = "Grupo";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los nombres del grupo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarComboGrupo()
        {
            cbGrupo.Items.Clear();
            cbGrupo.SelectedIndex = -1;
            cbGrupo.Texts = "Grupo";
        }

        private void cbCarrera_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OpcionCombo carreraSeleccionada = cbCarrera.SelectedItem as OpcionCombo;
            if (carreraSeleccionada == null)
            {
                LimpiarComboGrupo();
                return;
            }

            CargarDatoscbGrupo(carreraSeleccionada.Id);
        }
    }
}
