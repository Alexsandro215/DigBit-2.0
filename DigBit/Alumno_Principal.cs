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
using DigBit.Infraestructura;
namespace DigBit
{
    public partial class Alumno_Principal : Form
    {

        private Buscar_Codigo_Alumno bucAlum;
        public Alumno_Principal()
        {
            InitializeComponent();
            Kiosco.Aplicar(this);
            if (AppContexto.Actual != null)
            {
                AppContexto.Actual.Registrar(this);
            }
            txtBienvenido.Enabled = false;
            string smatricula = Datos_User.getUser();

            if (!string.IsNullOrEmpty(smatricula))
            {
                txtBienvenido.Texts = smatricula;
            }
        }

        private void btnCambiarUsuario_Click(object sender, EventArgs e)
        {
            // Vuelve a un login limpio (olvida al usuario actual y cierra todo).
            AppContexto.Actual.CerrarSesion();
        }


        private void btnSalir_Click(object sender, EventArgs e)
        {
            // "Salir" es salir de la sesion, nunca de la aplicacion: en el
            // laboratorio DigBit es el shell y no puede terminar.
            AppContexto.Actual.CerrarSesion();
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
            bucAlum.Show(this);

            //Si esta en segundo plano lo trae al frente
            bucAlum.BringToFront();

        }

        private void btnEditarPerfil_Click(object sender, EventArgs e)
        {
            editar_Perfil ep = new editar_Perfil("alumno");
            ep.Show(this);
        }
    }
}
