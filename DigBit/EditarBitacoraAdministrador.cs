using DigBit.conexion;
using System;
using System.Drawing;
using System.Windows.Forms;
using DrawingFont = System.Drawing.Font;

namespace DigBit
{
    public class EditarBitacoraAdministrador : Form
    {
        private readonly Consultas consultas;
        private readonly string fkCodigoAcceso;
        private readonly string fkUsuario;
        private readonly string numeroOriginal;
        private readonly string fallaRedOriginal;
        private readonly string comentarioRedOriginal;
        private readonly string fallaHardwareOriginal;
        private readonly string comentarioHardwareOriginal;
        private readonly string fallaSoftwareOriginal;
        private readonly string comentarioSoftwareOriginal;

        private TextBox txtNumeroComputadora;
        private TextBox txtFallaRed;
        private TextBox txtComentarioRed;
        private TextBox txtFallaHardware;
        private TextBox txtComentarioHardware;
        private TextBox txtFallaSoftware;
        private TextBox txtComentarioSoftware;
        private Button btnGuardar;
        private Button btnCancelar;

        public EditarBitacoraAdministrador(DataGridViewRow fila)
        {
            consultas = new Consultas();
            fkCodigoAcceso = fila.Cells["fk_codigo_accesos"]?.Value?.ToString();
            fkUsuario = fila.Cells["fk_usuario"]?.Value?.ToString();
            numeroOriginal = fila.Cells["numero_computadora"]?.Value?.ToString() ?? string.Empty;
            fallaRedOriginal = fila.Cells["falla_red"]?.Value?.ToString() ?? string.Empty;
            comentarioRedOriginal = fila.Cells["comentarios_red"]?.Value?.ToString() ?? string.Empty;
            fallaHardwareOriginal = fila.Cells["falla_hardware"]?.Value?.ToString() ?? string.Empty;
            comentarioHardwareOriginal = fila.Cells["comentarios_hardware"]?.Value?.ToString() ?? string.Empty;
            fallaSoftwareOriginal = fila.Cells["falla_software"]?.Value?.ToString() ?? string.Empty;
            comentarioSoftwareOriginal = fila.Cells["comentarios_software"]?.Value?.ToString() ?? string.Empty;

            InicializarComponentes();
            CargarDatosIniciales();
        }

        private void InicializarComponentes()
        {
            BackColor = Color.White;
            ClientSize = new Size(760, 520);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Editar bitacora";

            Controls.Add(CrearLabel("Numero de computadora", 30, 25));
            Controls.Add(CrearLabel("Falla de red", 30, 90));
            Controls.Add(CrearLabel("Comentario de red", 390, 90));
            Controls.Add(CrearLabel("Falla de hardware", 30, 195));
            Controls.Add(CrearLabel("Comentario de hardware", 390, 195));
            Controls.Add(CrearLabel("Falla de software", 30, 300));
            Controls.Add(CrearLabel("Comentario de software", 390, 300));

            txtNumeroComputadora = CrearTextBox(30, 50, 300, 26);
            txtFallaRed = CrearTextBox(30, 115, 300, 26);
            txtComentarioRed = CrearTextBox(390, 115, 320, 70, true);
            txtFallaHardware = CrearTextBox(30, 220, 300, 26);
            txtComentarioHardware = CrearTextBox(390, 220, 320, 70, true);
            txtFallaSoftware = CrearTextBox(30, 325, 300, 26);
            txtComentarioSoftware = CrearTextBox(390, 325, 320, 70, true);

            btnGuardar = CrearBoton("Guardar", 420, 440);
            btnGuardar.Click += btnGuardar_Click;

            btnCancelar = CrearBoton("Cancelar", 575, 440);
            btnCancelar.Click += btnCancelar_Click;

            Controls.Add(txtNumeroComputadora);
            Controls.Add(txtFallaRed);
            Controls.Add(txtComentarioRed);
            Controls.Add(txtFallaHardware);
            Controls.Add(txtComentarioHardware);
            Controls.Add(txtFallaSoftware);
            Controls.Add(txtComentarioSoftware);
            Controls.Add(btnGuardar);
            Controls.Add(btnCancelar);
        }

        private Label CrearLabel(string texto, int x, int y)
        {
            return new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 10F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(x, y),
                Text = texto
            };
        }

        private TextBox CrearTextBox(int x, int y, int width, int height, bool multiline = false)
        {
            return new TextBox
            {
                Font = new DrawingFont("Century Gothic", 10F),
                Location = new Point(x, y),
                Multiline = multiline,
                Size = new Size(width, height)
            };
        }

        private Button CrearBoton(string texto, int x, int y)
        {
            Button boton = new Button
            {
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new DrawingFont("Century Gothic", 10F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(x, y),
                Size = new Size(130, 42),
                Text = texto,
                UseVisualStyleBackColor = false
            };
            boton.FlatAppearance.BorderColor = Color.DarkGreen;
            boton.FlatAppearance.BorderSize = 2;
            return boton;
        }

        private void CargarDatosIniciales()
        {
            txtNumeroComputadora.Text = numeroOriginal;
            txtFallaRed.Text = fallaRedOriginal;
            txtComentarioRed.Text = comentarioRedOriginal;
            txtFallaHardware.Text = fallaHardwareOriginal;
            txtComentarioHardware.Text = comentarioHardwareOriginal;
            txtFallaSoftware.Text = fallaSoftwareOriginal;
            txtComentarioSoftware.Text = comentarioSoftwareOriginal;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumeroComputadora.Text))
            {
                MessageBox.Show("El numero de computadora no puede quedar vacio.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool actualizado = consultas.ActualizarRegistroBitacora(
                fkCodigoAcceso,
                fkUsuario,
                numeroOriginal,
                fallaRedOriginal,
                comentarioRedOriginal,
                fallaHardwareOriginal,
                comentarioHardwareOriginal,
                fallaSoftwareOriginal,
                comentarioSoftwareOriginal,
                txtNumeroComputadora.Text.Trim(),
                txtFallaRed.Text.Trim(),
                txtComentarioRed.Text.Trim(),
                txtFallaHardware.Text.Trim(),
                txtComentarioHardware.Text.Trim(),
                txtFallaSoftware.Text.Trim(),
                txtComentarioSoftware.Text.Trim());

            if (actualizado)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
