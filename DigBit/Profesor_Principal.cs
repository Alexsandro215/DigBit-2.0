using DigBit.conexion;
using System;
using System.Windows.Forms;

namespace DigBit
{
    public partial class Profesor_Principal : Form
    {
        private generarCodigo bucProf;
        private editar_Perfil edit;
        private Bitacora_profesor bitProf;
        private HistorialCodigosProfesor historialCodigos;

        public Profesor_Principal()
        {
            InitializeComponent();
            txtBienvenido.Enabled = false;
            string matricula = Datos_User.getUser();

            if (!string.IsNullOrEmpty(matricula))
            {
                txtBienvenido.Texts = matricula;
            }
        }

        private void btnGenerarCodigoAcceso_Click(object sender, EventArgs e)
        {
            if (bucProf == null || bucProf.IsDisposed)
            {
                bucProf = new generarCodigo();
            }

            bucProf.Show();
            bucProf.BringToFront();
        }

        private void btnEditarPerfil_Click(object sender, EventArgs e)
        {
            if (edit == null || edit.IsDisposed)
            {
                edit = new editar_Perfil("profesor");
            }

            edit.Show();
        }

        private void btnDescargarListaAsistencia_Click(object sender, EventArgs e)
        {
            if (bitProf == null || bitProf.IsDisposed)
            {
                bitProf = new Bitacora_profesor();
            }

            bitProf.Show();
            bitProf.BringToFront();
        }

        private void btnMisCodigos_Click(object sender, EventArgs e)
        {
            if (historialCodigos == null || historialCodigos.IsDisposed)
            {
                historialCodigos = new HistorialCodigosProfesor();
            }

            historialCodigos.Show();
            historialCodigos.BringToFront();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
