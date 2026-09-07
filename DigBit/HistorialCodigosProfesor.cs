using DigBit.conexion;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using DrawingFont = System.Drawing.Font;

namespace DigBit
{
    public class HistorialCodigosProfesor : Form
    {
        private readonly Consultas consultas;
        private readonly string numeroIdentificador;
        private string nombreProfesor;
        private DataTable tablaCodigos;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblBusqueda;
        private Label lblFecha;
        private Label lblCantidad;
        private TextBox txtBusqueda;
        private DateTimePicker dtpFecha;
        private CheckBox chkFiltrarFecha;
        private DataGridView dgvCodigos;
        private Button btnAplicarFiltro;
        private Button btnLimpiarFiltro;
        private Button btnDescargarPdf;
        private Button btnCerrar;

        public HistorialCodigosProfesor()
        {
            consultas = new Consultas();
            numeroIdentificador = Datos_User.getUser();
            nombreProfesor = numeroIdentificador;

            InicializarComponentes();
            CargarNombreProfesor();
            CargarCodigos();
        }

        private void InicializarComponentes()
        {
            BackColor = Color.White;
            ClientSize = new Size(980, 560);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Historial de codigos";

            lblTitulo = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 18F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(28, 18),
                Text = "Mis codigos generados"
            };

            lblSubtitulo = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 10.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 50, 50),
                Location = new Point(30, 55),
                Text = "Aqui puedes consultar tus codigos y descargar su PDF en automatico."
            };

            lblBusqueda = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 10F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(30, 98),
                Text = "Buscar"
            };

            txtBusqueda = new TextBox
            {
                Font = new DrawingFont("Century Gothic", 10F),
                Location = new Point(33, 120),
                Size = new Size(300, 24)
            };
            txtBusqueda.KeyDown += txtBusqueda_KeyDown;

            chkFiltrarFecha = new CheckBox
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 9.75F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(360, 122),
                Text = "Filtrar por fecha"
            };

            lblFecha = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 10F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(530, 98),
                Text = "Fecha"
            };

            dtpFecha = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Location = new Point(533, 120),
                Size = new Size(120, 24),
                Value = DateTime.Today
            };

            btnAplicarFiltro = CrearBoton("Aplicar filtro", new Point(690, 112), new Size(115, 38));
            btnAplicarFiltro.Click += btnAplicarFiltro_Click;

            btnLimpiarFiltro = CrearBoton("Limpiar", new Point(820, 112), new Size(115, 38));
            btnLimpiarFiltro.Click += btnLimpiarFiltro_Click;

            dgvCodigos = new DataGridView
            {
                Location = new Point(33, 170),
                Size = new Size(902, 305),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                ScrollBars = ScrollBars.Both,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvCodigos.CellDoubleClick += dgvCodigos_CellDoubleClick;

            lblCantidad = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 9.75F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(30, 490),
                Text = "Registros encontrados: 0"
            };

            btnDescargarPdf = CrearBoton("Descargar PDF", new Point(620, 485), new Size(150, 42));
            btnDescargarPdf.Click += btnDescargarPdf_Click;

            btnCerrar = CrearBoton("Cerrar", new Point(785, 485), new Size(150, 42));
            btnCerrar.Click += btnCerrar_Click;

            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblBusqueda);
            Controls.Add(txtBusqueda);
            Controls.Add(chkFiltrarFecha);
            Controls.Add(lblFecha);
            Controls.Add(dtpFecha);
            Controls.Add(btnAplicarFiltro);
            Controls.Add(btnLimpiarFiltro);
            Controls.Add(dgvCodigos);
            Controls.Add(lblCantidad);
            Controls.Add(btnDescargarPdf);
            Controls.Add(btnCerrar);
        }

        private Button CrearBoton(string texto, Point posicion, Size tamaño)
        {
            Button boton = new Button
            {
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new DrawingFont("Century Gothic", 10F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = posicion,
                Size = tamaño,
                Text = texto,
                UseVisualStyleBackColor = false
            };
            boton.FlatAppearance.BorderColor = Color.DarkGreen;
            boton.FlatAppearance.BorderSize = 2;
            return boton;
        }

        private void CargarNombreProfesor()
        {
            if (string.IsNullOrWhiteSpace(numeroIdentificador))
            {
                return;
            }

            try
            {
                string nombre = consultas.MostrarNombreProfesor(numeroIdentificador);
                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    nombreProfesor = nombre;
                    lblSubtitulo.Text = $"Docente: {nombre}. Aqui puedes consultar tus codigos y descargar su PDF en automatico.";
                }
            }
            catch
            {
                lblSubtitulo.Text = $"Docente: {numeroIdentificador}. Aqui puedes consultar tus codigos y descargar su PDF en automatico.";
            }
        }

        private void CargarCodigos()
        {
            if (string.IsNullOrWhiteSpace(numeroIdentificador))
            {
                MessageBox.Show("No se encontro el numero de empleado del docente actual.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            tablaCodigos = consultas.ObtenerCodigosProfesor(numeroIdentificador);
            dgvCodigos.DataSource = tablaCodigos;
            ConfigurarColumnas();
            ActualizarContador();
        }

        private void ConfigurarColumnas()
        {
            if (dgvCodigos.Columns.Count == 0)
            {
                return;
            }

            dgvCodigos.Columns["codigo"].HeaderText = "Codigo";
            dgvCodigos.Columns["fecha_generacion"].HeaderText = "Fecha de generacion";
            dgvCodigos.Columns["hora_entrada"].HeaderText = "Hora de entrada";
            dgvCodigos.Columns["hora_salida"].HeaderText = "Hora de salida";
            dgvCodigos.Columns["materia"].HeaderText = "Materia";
            dgvCodigos.Columns["grupo"].HeaderText = "Grupo";
            dgvCodigos.Columns["laboratorio"].HeaderText = "Laboratorio";

            dgvCodigos.Columns["codigo"].FillWeight = 18;
            dgvCodigos.Columns["fecha_generacion"].FillWeight = 22;
            dgvCodigos.Columns["hora_entrada"].FillWeight = 15;
            dgvCodigos.Columns["hora_salida"].FillWeight = 15;
            dgvCodigos.Columns["materia"].FillWeight = 24;
            dgvCodigos.Columns["grupo"].FillWeight = 12;
            dgvCodigos.Columns["laboratorio"].FillWeight = 20;
        }

        private void AplicarFiltros()
        {
            if (tablaCodigos == null)
            {
                return;
            }

            List<string> filtros = new List<string>();

            if (chkFiltrarFecha.Checked)
            {
                filtros.Add($"fecha_generacion LIKE '{dtpFecha.Value:yyyy-MM-dd}%'");
            }

            string textoBusqueda = txtBusqueda.Text.Trim().Replace("'", "''");
            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                filtros.Add(
                    $"codigo LIKE '%{textoBusqueda}%' OR " +
                    $"fecha_generacion LIKE '%{textoBusqueda}%' OR " +
                    $"hora_entrada LIKE '%{textoBusqueda}%' OR " +
                    $"hora_salida LIKE '%{textoBusqueda}%' OR " +
                    $"materia LIKE '%{textoBusqueda}%' OR " +
                    $"grupo LIKE '%{textoBusqueda}%' OR " +
                    $"laboratorio LIKE '%{textoBusqueda}%'");
            }

            tablaCodigos.DefaultView.RowFilter = filtros.Count > 0
                ? string.Join(" AND ", filtros.Select(filtro => $"({filtro})"))
                : string.Empty;

            dgvCodigos.DataSource = tablaCodigos.DefaultView;
            ConfigurarColumnas();
            ActualizarContador();
        }

        private void ActualizarContador()
        {
            lblCantidad.Text = $"Registros encontrados: {dgvCodigos.Rows.Count}";
        }

        private void DescargarSeleccionado()
        {
            if (dgvCodigos.CurrentRow == null || dgvCodigos.CurrentRow.Cells["codigo"]?.Value == null)
            {
                MessageBox.Show("Selecciona un codigo para descargar su PDF.", "Codigo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string codigo = dgvCodigos.CurrentRow.Cells["codigo"].Value.ToString();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("Selecciona un codigo valido.", "Codigo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DescargarPdfCodigo(codigo);
        }

        private void DescargarPdfCodigo(string codigoAcceso)
        {
            string nombreArchivo = DateTime.Now.ToString("dd-M-yyyy-HH_mm_ss") + "_" + nombreProfesor + ".pdf";
            Usuario datos = consultas.ConsultarDatosPdf(codigoAcceso);
            if (datos == null)
            {
                MessageBox.Show("No se encontraron datos para el codigo seleccionado.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string rutaCompleta = ObtenerRutaDestinoPdf(nombreArchivo);
            if (string.IsNullOrWhiteSpace(rutaCompleta))
            {
                return;
            }

            string paginahtml_texto = Properties.Resources.plantilla.ToString();
            paginahtml_texto = paginahtml_texto.Replace("@HORARIODEENTRADA", datos.HoraEntrada);
            paginahtml_texto = paginahtml_texto.Replace("@HORARIODESALIDA", datos.HoraSalida);
            paginahtml_texto = paginahtml_texto.Replace("@PROFESOR", nombreProfesor);
            paginahtml_texto = paginahtml_texto.Replace("@GRUPO", datos.GruposId);
            paginahtml_texto = paginahtml_texto.Replace("@MATERIA", datos.MateriasId);
            paginahtml_texto = paginahtml_texto.Replace("@FECHA", datos.HoraRegistro);

            using (FileStream stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                pdfDoc.AddTitle("Bitacora DigBit");
                pdfDoc.AddAuthor("DigBit");

                iTextSharp.text.Image logoTeschi = iTextSharp.text.Image.GetInstance(Properties.Resources.logoteschi, System.Drawing.Imaging.ImageFormat.Png);
                logoTeschi.ScaleToFit(100, 60);
                logoTeschi.Alignment = iTextSharp.text.Image.UNDERLYING;
                logoTeschi.SetAbsolutePosition(470, 750);
                pdfDoc.Add(logoTeschi);

                iTextSharp.text.Image logoEDOMEX = iTextSharp.text.Image.GetInstance(Properties.Resources.estadoMexico, System.Drawing.Imaging.ImageFormat.Png);
                logoEDOMEX.ScaleToFit(100, 60);
                logoEDOMEX.Alignment = iTextSharp.text.Image.UNDERLYING;
                logoEDOMEX.SetAbsolutePosition(40, 750);
                pdfDoc.Add(logoEDOMEX);

                using (StringReader sr = new StringReader(paginahtml_texto))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                }

                DataTable detalles = consultas.consultaRegistro(codigoAcceso);
                AgregarTablaAlPDF(pdfDoc, detalles);

                pdfDoc.Close();
            }

            MessageBox.Show("El PDF se guardo correctamente en: " + rutaCompleta, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AbrirVistaPreviaPdf(rutaCompleta);
        }

        private string ObtenerRutaDestinoPdf(string nombreArchivo)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "Guardar bitacora PDF";
                saveFileDialog.Filter = "Archivo PDF (*.pdf)|*.pdf";
                saveFileDialog.DefaultExt = "pdf";
                saveFileDialog.FileName = nombreArchivo;
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                return saveFileDialog.ShowDialog() == DialogResult.OK
                    ? saveFileDialog.FileName
                    : null;
            }
        }

        private void AbrirVistaPreviaPdf(string rutaCompleta)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = rutaCompleta,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("El PDF se guardó, pero no se pudo abrir la vista previa automáticamente: " + ex.Message,
                    "Vista previa no disponible",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void AgregarTablaAlPDF(Document pdfDoc, DataTable detalles)
        {
            PdfPTable pdfTable = new PdfPTable(4);
            pdfTable.WidthPercentage = 100;
            pdfTable.SetWidths(new float[] { 8f, 28f, 20f, 44f });

            pdfTable.AddCell(new PdfPCell(new Phrase("No.")) { BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 4f });
            pdfTable.AddCell(new PdfPCell(new Phrase("Nombre Completo")) { BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 4f });
            pdfTable.AddCell(new PdfPCell(new Phrase("Numero de Computadora")) { BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 4f });
            pdfTable.AddCell(new PdfPCell(new Phrase("Comentarios")) { BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 4f });

            int numeroFila = 1;
            foreach (DataRow row in detalles.Rows)
            {
                pdfTable.AddCell(new PdfPCell(new Phrase(numeroFila.ToString())) { Padding = 4f });
                pdfTable.AddCell(new PdfPCell(new Phrase(row["nombre_completo"].ToString())) { Padding = 4f });
                pdfTable.AddCell(new PdfPCell(new Phrase(row["numero_computadora"].ToString())) { Padding = 4f });

                string comentarios = $"Red: {row["comentarios_red"]}, Hardware: {row["comentarios_hardware"]}, Software: {row["comentarios_software"]}";
                pdfTable.AddCell(new PdfPCell(new Phrase(comentarios)) { Padding = 4f });
                numeroFila++;
            }

            pdfDoc.Add(pdfTable);
        }

        private void btnAplicarFiltro_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            txtBusqueda.Clear();
            chkFiltrarFecha.Checked = false;
            dtpFecha.Value = DateTime.Today;
            AplicarFiltros();
        }

        private void btnDescargarPdf_Click(object sender, EventArgs e)
        {
            DescargarSeleccionado();
        }

        private void dgvCodigos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DescargarSeleccionado();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtBusqueda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AplicarFiltros();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
