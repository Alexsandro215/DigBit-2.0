using DigBit.conexion;
using System;
using System.Windows.Forms;

namespace DigBit
{
    public partial class mostrar_Codigo_Profesor : Form
    {
        DateTime horaActual;

        public mostrar_Codigo_Profesor()
        {
            InitializeComponent();
            horaActual = DateTime.Now;

            // Recuperar el código almacenado y asignarlo al campo de texto
            string codigoRecuperado = Datos_User.getcodigo();
            txtCodigoGenerado.Texts = codigoRecuperado;
        }

        private void btnCopiar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCodigoGenerado.Texts))
            {
                Clipboard.SetText(txtCodigoGenerado.Texts);
                MessageBox.Show("Texto copiado al portapapeles.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}