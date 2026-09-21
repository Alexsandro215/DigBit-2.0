using System;
using System.Drawing;
using System.Windows.Forms;
using DigBit.conexion;
using DigBit.Infraestructura;

namespace DigBit
{
    /// <summary>
    /// Alta de profesores. SOLO para el administrador: se abre desde su panel y
    /// se cierra sola si la abre cualquier otro. Antes habia aqui codigo (sin
    /// cablear) para saltar entre esta pantalla y el alta de alumnos, que es la
    /// que sí puede abrir cualquiera desde el login; se quito para que nadie lo
    /// vuelva a conectar por descuido.
    /// </summary>
    public partial class RegistroProfesores : Form
    {
        private Conexion mconexion;
        private InsercionDatos insercionDatos;

        public RegistroProfesores()
        {
            InitializeComponent();
            mconexion = new Conexion();
            insercionDatos = new InsercionDatos();
        }

        /// <summary>
        /// True si la sesion actual puede dar de alta profesores. Solo el
        /// administrador; cualquier otro intento queda en el log.
        /// </summary>
        internal static bool PuedeAbrir()
        {
            if (Datos_User.EsAdministrador)
            {
                return true;
            }

            Log.Aviso("Intento de abrir el alta de profesores sin sesion de administrador (usuario: "
                + (Datos_User.getUser() ?? "-") + ", tipo: " + Datos_User.TipoUsuario + ").");
            return false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (PuedeAbrir())
            {
                return;
            }

            MessageBox.Show("Solo el administrador puede dar de alta profesores.", "Acceso restringido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            BeginInvoke(new Action(Close));
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
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
                txtNombre.Texts = "Nombre";
                txtNombre.ForeColor= Color.Black;   
            }
        }

        private void txtApellidoP_Enter(object sender, EventArgs e)
        {
            if(txtApellidoP.Texts=="Apellido Paterno")
            {
                txtApellidoP.Texts = "";
                txtApellidoP.ForeColor= Color.Black;    

            }
        }

        private void txtApellidoP_Leave(object sender, EventArgs e)
        {
            if (txtApellidoP.Texts == "")
            {
                txtApellidoP.Texts = "Apellido Paterno";
                txtApellidoP.ForeColor = Color.Black;
            }
        }

        private void txtApellidoM_Enter(object sender, EventArgs e)
        {
            if(txtApellidoM.Texts=="Apellido Materno")
            {
                txtApellidoM.Texts = "";
                txtApellidoM.ForeColor= Color.Black;
            }
        }

        private void txtApellidoM_Leave(object sender, EventArgs e)
        {
            if (txtApellidoM.Texts == "")
            {
                txtApellidoM.Texts = "Apellido Materno";
                txtApellidoM.ForeColor = Color.Black;
            }
        }

        private void txtMatricula_Enter(object sender, EventArgs e)
        {
            if (txtMatricula.Texts == "Numero de empleado")
            {
                txtMatricula.Texts = "";
                txtMatricula.ForeColor = Color.Black;
            }
        }

        private void txtMatricula_Leave(object sender, EventArgs e)
        {
            if(txtMatricula.Texts == "")
            {
                txtMatricula.Texts = "Numero de empleado";
                txtMatricula.ForeColor= Color.Black;
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

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if(txtPassword.Texts == "")
            {
                txtPassword.Texts = "Contraseña";
                txtPassword.ForeColor = Color.Black;    
            }
        }

        private bool RealizarInsercion()
        {
            string nombre = txtNombre.Texts;
            string apellidoPaterno = txtApellidoP.Texts;
            string apellidoMaterno = txtApellidoM.Texts;
            string matricula = txtMatricula.Texts;
            string contraseña = txtPassword.Texts;
            string correo = rjTcorreo.Texts;
            string numeroEmpleado = matricula;

            // Llama a la función InsertarUsuarioProfe con los valores de los TextBox
            return insercionDatos.InsertarUsuarioProfe(nombre, apellidoPaterno, apellidoMaterno, numeroEmpleado, contraseña, 2, correo);
        }

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

        private void RegistrarProfe_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(txtNombre.Texts) || txtNombre.Texts == "Nombre" ||
                    string.IsNullOrWhiteSpace(txtApellidoP.Texts) || txtApellidoP.Texts == "Apellido Paterno" ||
                    string.IsNullOrWhiteSpace(txtApellidoM.Texts) || txtApellidoM.Texts == "Apellido Materno" ||
                    string.IsNullOrWhiteSpace(txtMatricula.Texts) || txtMatricula.Texts == "Numero de empleado" ||
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
                    MessageBox.Show("Profesor registrado exitosamente.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error si hay una excepción
                MessageBox.Show($"Error al intentar registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
