using System;
using System.Drawing;
using System.Windows.Forms;
using DigBit.conexion;

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

        //Se configura la imagen "imgSalir" para que al momento de dar clic se cierre la ventana
        private void imgSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Se configura la imagen "imgMinimizar" para que al momento de dar clic de minimice la ventana
        private void imgMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

                // Verificar si el usuario es administrador y la contraseña es correcta
                if (numeroIdentificadorIngresado == "ADMINISTRADOR" && contraseñaIngresada == "123")
                {
                    // Iniciar sesión como administrador
                    PrincipalAdministrador pa = new PrincipalAdministrador();
                    pa.Show();
                    this.Hide();
                    return;
                }

                // Si no es administrador, intentar realizar el inicio de sesión
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
