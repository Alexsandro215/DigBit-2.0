using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigBit.conexion;
namespace DigBit
{
    public partial class Alumno_Principal : Form
    {

        private Buscar_Codigo_Alumno bucAlum;
        public Alumno_Principal()
        {
            InitializeComponent();
            txtBienvenido.Enabled = false;
            string smatricula = Datos_User.getUser();

            if (!string.IsNullOrEmpty(smatricula))
            {
                txtBienvenido.Texts = smatricula;
            }
        }

        private void btnCambiarUsuario_Click(object sender, EventArgs e)
        {
            //Crea un objeto de tipo login para cerrar el formulario y mostrarlo
            Login log = new Login();

            //Mostramos el formulatio
            log.Show();

            //Ocultamos el formulario que esta mostrando
            this.Hide();
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            //Se cierra la aplicación
            Application.Exit();
        }

        private void btnIngresarCodigoAcceso_Click(object sender, EventArgs e)
        {
            //Bucle para verificar si ya existe el objeto
            if (bucAlum == null || bucAlum.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                bucAlum = new Buscar_Codigo_Alumno();
            }
            //Muestra el formulario
            bucAlum.Show();

            //Si esta en segundo plano lo trae al frente
            bucAlum.BringToFront();

        }

        private void btnEditarPerfil_Click(object sender, EventArgs e)
        {
            editar_Perfil ep = new editar_Perfil("alumno");
            ep.Show();
        }
    }
}
