using DigBit.conexion;
using DigBit.Infraestructura;
using System;
using System.Windows.Forms;

namespace DigBit
{
    public partial class Profesor_Principal : Form
    {
        private MisClasesProfesor misClases;
        private editar_Perfil edit;
        private BitacoraMisClases bitProf;
        private HistorialCodigosProfesor historialCodigos;

        public Profesor_Principal()
        {
            InitializeComponent();
            Kiosco.Aplicar(this);
            if (AppContexto.Actual != null)
            {
                AppContexto.Actual.Registrar(this);
            }
            txtBienvenido.Enabled = false;
            string matricula = Datos_User.getUser();

            if (!string.IsNullOrEmpty(matricula))
            {
                txtBienvenido.Texts = matricula;
            }
        }

        private void btnGenerarCodigoAcceso_Click(object sender, EventArgs e)
        {
            if (misClases == null || misClases.IsDisposed)
            {
                misClases = new MisClasesProfesor();
            }

            misClases.Show(this);
            misClases.BringToFront();
        }

        private void btnEditarPerfil_Click(object sender, EventArgs e)
        {
            if (edit == null || edit.IsDisposed)
            {
                edit = new editar_Perfil("profesor");
            }

            edit.Show(this);
        }

        private void btnDescargarListaAsistencia_Click(object sender, EventArgs e)
        {
            if (bitProf == null || bitProf.IsDisposed)
            {
                bitProf = new BitacoraMisClases();
            }

            bitProf.Show(this);
            bitProf.BringToFront();
        }

        private void btnMisCodigos_Click(object sender, EventArgs e)
        {
            if (historialCodigos == null || historialCodigos.IsDisposed)
            {
                historialCodigos = new HistorialCodigosProfesor();
            }

            historialCodigos.Show(this);
            historialCodigos.BringToFront();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Salir de la sesion, no de la aplicacion (DigBit puede ser el shell).
            AppContexto.Actual.CerrarSesion();
        }
    }
}
