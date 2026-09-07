using DigBit.conexion;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DrawingFont = System.Drawing.Font;

namespace DigBit
{
    public class GestionBitacorasAdministrador : Form
    {
        private readonly Consultas consultas;
        private readonly bool permitirEdicion;
        private DataTable tablaBitacoras;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblBusqueda;
        private Label lblDocente;
        private Label lblFecha;
        private Label lblCantidad;
        private TextBox txtBusqueda;
        private TextBox txtDocente;
        private DateTimePicker dtpFecha;
        private CheckBox chkFiltrarFecha;
        private CheckBox chkSeleccionarTodo;
        private DataGridView dgvBitacoras;
        private Button btnAplicarFiltro;
        private Button btnLimpiarFiltro;
        private Button btnDescargarPdf;
        private Button btnDescargarLote;
        private Button btnEditarSeleccion;
        private Button btnCerrar;

        public GestionBitacorasAdministrador(bool permitirEdicion)
        {
            consultas = new Consultas();
            this.permitirEdicion = permitirEdicion;

            InicializarComponentes();
            CargarBitacoras();
        }

        private void InicializarComponentes()
        {
            BackColor = Color.White;
            ClientSize = new Size(1280, 620);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = permitirEdicion ? "Modificar bitacoras" : "Ver bitacoras";

            lblTitulo = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 18F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(28, 18),
                Text = permitirEdicion ? "Modificar bitacoras" : "Ver bitacoras"
            };

            lblSubtitulo = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 10.5F),
                ForeColor = Color.FromArgb(50, 50, 50),
                Location = new Point(30, 55),
                Text = permitirEdicion
                    ? "Selecciona un registro de la tabla para editarlo."
                    : "Consulta todos los registros de bitacora en formato tabla."
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
                Size = new Size(350, 24)
            };
            txtBusqueda.KeyDown += txtBusqueda_KeyDown;

            lblDocente = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 10F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(405, 98),
                Text = "Docente"
            };

            txtDocente = new TextBox
            {
                Font = new DrawingFont("Century Gothic", 10F),
                Location = new Point(408, 120),
                Size = new Size(240, 24)
            };
            txtDocente.KeyDown += txtBusqueda_KeyDown;

            chkFiltrarFecha = new CheckBox
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 9.75F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(670, 122),
                Text = "Filtrar por fecha"
            };

            lblFecha = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 10F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(830, 98),
                Text = "Fecha"
            };

            dtpFecha = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Location = new Point(833, 120),
                Size = new Size(120, 24),
                Value = DateTime.Today
            };

            btnAplicarFiltro = CrearBoton("Aplicar filtro", new Point(980, 112), new Size(120, 38));
            btnAplicarFiltro.Click += btnAplicarFiltro_Click;

            btnLimpiarFiltro = CrearBoton("Limpiar", new Point(1115, 112), new Size(120, 38));
            btnLimpiarFiltro.Click += btnLimpiarFiltro_Click;

            chkSeleccionarTodo = new CheckBox
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 9.75F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(33, 545),
                Text = "Seleccionar todo lo filtrado"
            };
            chkSeleccionarTodo.CheckedChanged += chkSeleccionarTodo_CheckedChanged;

            dgvBitacoras = new DataGridView
            {
                Location = new Point(33, 170),
                Size = new Size(1210, 360),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                MultiSelect = true,
                ReadOnly = true,
                RowHeadersVisible = false,
                ScrollBars = ScrollBars.Both,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvBitacoras.CellDoubleClick += dgvBitacoras_CellDoubleClick;

            lblCantidad = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 9.75F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(30, 550),
                Text = "Registros encontrados: 0"
            };

            btnDescargarPdf = CrearBoton("Descargar PDF", new Point(650, 545), new Size(140, 42));
            btnDescargarPdf.Click += btnDescargarPdf_Click;

            btnDescargarLote = CrearBoton("Descargar lote", new Point(805, 545), new Size(140, 42));
            btnDescargarLote.Click += btnDescargarLote_Click;

            btnEditarSeleccion = CrearBoton("Editar seleccion", new Point(960, 545), new Size(140, 42));
            btnEditarSeleccion.Click += btnEditarSeleccion_Click;
            btnEditarSeleccion.Visible = permitirEdicion;

            btnCerrar = CrearBoton("Cerrar", new Point(1115, 545), new Size(128, 42));
            btnCerrar.Click += btnCerrar_Click;

            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(lblBusqueda);
            Controls.Add(txtBusqueda);
            Controls.Add(lblDocente);
            Controls.Add(txtDocente);
            Controls.Add(chkFiltrarFecha);
            Controls.Add(lblFecha);
            Controls.Add(dtpFecha);
            Controls.Add(btnAplicarFiltro);
            Controls.Add(btnLimpiarFiltro);
            Controls.Add(chkSeleccionarTodo);
            Controls.Add(dgvBitacoras);
            Controls.Add(lblCantidad);
            Controls.Add(btnDescargarPdf);
            Controls.Add(btnDescargarLote);
            Controls.Add(btnEditarSeleccion);
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

        private void CargarBitacoras()
        {
            tablaBitacoras = consultas.ObtenerBitacorasAdministrador();
            dgvBitacoras.DataSource = tablaBitacoras;
            ConfigurarColumnas();
            ActualizarContador();
            AplicarSeleccionAutomatica();
        }

        private void ConfigurarColumnas()
        {
            if (dgvBitacoras.Columns.Count == 0)
            {
                return;
            }

            dgvBitacoras.Columns["codigo"].HeaderText = "Codigo";
            dgvBitacoras.Columns["fecha_generacion"].HeaderText = "Fecha";
            dgvBitacoras.Columns["numero_empleado"].HeaderText = "No. empleado";
            dgvBitacoras.Columns["profesor"].HeaderText = "Profesor";
            dgvBitacoras.Columns["matricula_alumno"].HeaderText = "Matricula";
            dgvBitacoras.Columns["alumno"].HeaderText = "Alumno";
            dgvBitacoras.Columns["numero_computadora"].HeaderText = "Computadora";
            dgvBitacoras.Columns["falla_red"].HeaderText = "Falla red";
            dgvBitacoras.Columns["comentarios_red"].HeaderText = "Comentario red";
            dgvBitacoras.Columns["falla_hardware"].HeaderText = "Falla hardware";
            dgvBitacoras.Columns["comentarios_hardware"].HeaderText = "Comentario hardware";
            dgvBitacoras.Columns["falla_software"].HeaderText = "Falla software";
            dgvBitacoras.Columns["comentarios_software"].HeaderText = "Comentario software";
            dgvBitacoras.Columns["hora_entrada"].HeaderText = "Entrada";
            dgvBitacoras.Columns["hora_salida"].HeaderText = "Salida";
            dgvBitacoras.Columns["grupo"].HeaderText = "Grupo";
            dgvBitacoras.Columns["materia"].HeaderText = "Materia";
            dgvBitacoras.Columns["laboratorio"].HeaderText = "Laboratorio";

            dgvBitacoras.Columns["fk_codigo_accesos"].Visible = false;
            dgvBitacoras.Columns["fk_usuario"].Visible = false;

            dgvBitacoras.Columns["profesor"].FillWeight = 22;
            dgvBitacoras.Columns["alumno"].FillWeight = 22;
            dgvBitacoras.Columns["comentarios_red"].FillWeight = 20;
            dgvBitacoras.Columns["comentarios_hardware"].FillWeight = 20;
            dgvBitacoras.Columns["comentarios_software"].FillWeight = 20;
            dgvBitacoras.Columns["materia"].FillWeight = 18;
            dgvBitacoras.Columns["laboratorio"].FillWeight = 16;
        }

        private void AplicarFiltros()
        {
            if (tablaBitacoras == null)
            {
                return;
            }

            List<string> filtros = new List<string>();
            if (chkFiltrarFecha.Checked)
            {
                filtros.Add($"fecha_generacion LIKE '{dtpFecha.Value:yyyy-MM-dd}%'");
            }

            string textoDocente = txtDocente.Text.Trim().Replace("'", "''");
            if (!string.IsNullOrWhiteSpace(textoDocente))
            {
                filtros.Add(
                    $"profesor LIKE '%{textoDocente}%' OR " +
                    $"numero_empleado LIKE '%{textoDocente}%'");
            }

            string textoBusqueda = txtBusqueda.Text.Trim().Replace("'", "''");
            if (!string.IsNullOrWhiteSpace(textoBusqueda))
            {
                filtros.Add(
                    $"codigo LIKE '%{textoBusqueda}%' OR " +
                    $"fecha_generacion LIKE '%{textoBusqueda}%' OR " +
                    $"numero_empleado LIKE '%{textoBusqueda}%' OR " +
                    $"profesor LIKE '%{textoBusqueda}%' OR " +
                    $"matricula_alumno LIKE '%{textoBusqueda}%' OR " +
                    $"alumno LIKE '%{textoBusqueda}%' OR " +
                    $"numero_computadora LIKE '%{textoBusqueda}%' OR " +
                    $"grupo LIKE '%{textoBusqueda}%' OR " +
                    $"materia LIKE '%{textoBusqueda}%' OR " +
                    $"laboratorio LIKE '%{textoBusqueda}%'");
            }

            tablaBitacoras.DefaultView.RowFilter = filtros.Count > 0
                ? string.Join(" AND ", filtros.Select(filtro => $"({filtro})"))
                : string.Empty;

            dgvBitacoras.DataSource = tablaBitacoras.DefaultView;
            ConfigurarColumnas();
            ActualizarContador();
            AplicarSeleccionAutomatica();
        }

        private void ActualizarContador()
        {
            lblCantidad.Text = $"Registros encontrados: {dgvBitacoras.Rows.Count}";
        }

        private void AplicarSeleccionAutomatica()
        {
            if (chkSeleccionarTodo != null && chkSeleccionarTodo.Checked)
            {
                SeleccionarTodasLasFilasVisibles();
            }
        }

        private void SeleccionarTodasLasFilasVisibles()
        {
            dgvBitacoras.ClearSelection();

            foreach (DataGridViewRow fila in dgvBitacoras.Rows)
            {
                if (fila.Visible)
                {
                    fila.Selected = true;
                }
            }
        }

        private DataGridViewRow ObtenerFilaSeleccionada()
        {
            if (dgvBitacoras.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una bitacora de la tabla.", "Seleccion requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return dgvBitacoras.CurrentRow;
        }

        private void DescargarSeleccionado()
        {
            DataGridViewRow fila = ObtenerFilaSeleccionada();
            if (fila == null)
            {
                return;
            }

            string codigo = fila.Cells["codigo"]?.Value?.ToString();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                MessageBox.Show("La fila seleccionada no tiene un codigo valido.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DescargarPdfCodigo(codigo, fila.Cells["profesor"]?.Value?.ToString());
        }

        private void DescargarPorLote()
        {
            List<DataGridViewRow> filasOrigen = dgvBitacoras.SelectedRows.Count > 0
                ? dgvBitacoras.SelectedRows.Cast<DataGridViewRow>().ToList()
                : dgvBitacoras.Rows.Cast<DataGridViewRow>().Where(fila => fila.Visible).ToList();

            List<(string Codigo, string Profesor)> codigos = filasOrigen
                .Where(fila => fila.Cells["codigo"]?.Value != null)
                .Select(fila => (
                    Codigo: fila.Cells["codigo"].Value.ToString(),
                    Profesor: fila.Cells["profesor"]?.Value?.ToString() ?? "Bitacora"))
                .Where(item => !string.IsNullOrWhiteSpace(item.Codigo))
                .GroupBy(item => item.Codigo)
                .Select(grupo => grupo.First())
                .ToList();

            if (codigos.Count == 0)
            {
                MessageBox.Show("No hay bitacoras disponibles para descargar.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string carpetaDestino = ObtenerCarpetaDestinoLote();
            if (string.IsNullOrWhiteSpace(carpetaDestino))
            {
                return;
            }

            int descargadas = 0;
            foreach ((string codigo, string profesor) in codigos)
            {
                string nombreArchivo = $"{codigo}_{SanitizarNombreArchivo(profesor)}.pdf";
                string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

                if (GenerarPdfCodigo(codigo, profesor, rutaCompleta))
                {
                    descargadas++;
                }
            }

            MessageBox.Show(
                $"Se descargaron {descargadas} bitacoras en:{Environment.NewLine}{carpetaDestino}",
                "Descarga completa",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            AbrirCarpetaDestino(carpetaDestino);
        }

        private void EditarSeleccionado()
        {
            if (!permitirEdicion)
            {
                return;
            }

            DataGridViewRow fila = ObtenerFilaSeleccionada();
            if (fila == null)
            {
                return;
            }

            using (EditarBitacoraAdministrador editor = new EditarBitacoraAdministrador(fila))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    CargarBitacoras();
                    AplicarFiltros();
                }
            }
        }

        private void DescargarPdfCodigo(string codigoAcceso, string nombreProfesor)
        {
            string nombreArchivo = DateTime.Now.ToString("dd-M-yyyy-HH_mm_ss") + "_" + (string.IsNullOrWhiteSpace(nombreProfesor) ? "Bitacora" : nombreProfesor) + ".pdf";
            string rutaCompleta = ObtenerRutaDestinoPdf(nombreArchivo);
            if (string.IsNullOrWhiteSpace(rutaCompleta))
            {
                return;
            }

            if (GenerarPdfCodigo(codigoAcceso, nombreProfesor, rutaCompleta))
            {
                MessageBox.Show("El PDF se guardo correctamente en: " + rutaCompleta, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AbrirVistaPreviaPdf(rutaCompleta);
            }
        }

        private bool GenerarPdfCodigo(string codigoAcceso, string nombreProfesor, string rutaCompleta)
        {
            Usuario datos = consultas.ConsultarDatosPdf(codigoAcceso);
            if (datos == null)
            {
                MessageBox.Show("No se encontraron datos para el codigo seleccionado.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            string paginahtml_texto = Properties.Resources.plantilla.ToString();
            paginahtml_texto = paginahtml_texto.Replace("@HORARIODEENTRADA", datos.HoraEntrada);
            paginahtml_texto = paginahtml_texto.Replace("@HORARIODESALIDA", datos.HoraSalida);
            paginahtml_texto = paginahtml_texto.Replace("@PROFESOR", nombreProfesor ?? string.Empty);
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

            return true;
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

        private string ObtenerCarpetaDestinoLote()
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Selecciona la carpeta donde se guardaran las bitacoras";
                folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                return folderBrowserDialog.ShowDialog() == DialogResult.OK
                    ? folderBrowserDialog.SelectedPath
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

        private void AbrirCarpetaDestino(string carpetaDestino)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = carpetaDestino,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Las bitacoras se guardaron, pero no se pudo abrir la carpeta automáticamente: " + ex.Message,
                    "Carpeta no disponible",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private string SanitizarNombreArchivo(string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo))
            {
                return "Bitacora";
            }

            foreach (char caracterInvalido in Path.GetInvalidFileNameChars())
            {
                nombreArchivo = nombreArchivo.Replace(caracterInvalido, '_');
            }

            return nombreArchivo;
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
            txtDocente.Clear();
            chkFiltrarFecha.Checked = false;
            dtpFecha.Value = DateTime.Today;
            AplicarFiltros();
        }

        private void btnDescargarPdf_Click(object sender, EventArgs e)
        {
            DescargarSeleccionado();
        }

        private void btnDescargarLote_Click(object sender, EventArgs e)
        {
            DescargarPorLote();
        }

        private void chkSeleccionarTodo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSeleccionarTodo.Checked)
            {
                SeleccionarTodasLasFilasVisibles();
                return;
            }

            dgvBitacoras.ClearSelection();
        }

        private void btnEditarSeleccion_Click(object sender, EventArgs e)
        {
            EditarSeleccionado();
        }

        private void dgvBitacoras_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (permitirEdicion)
            {
                EditarSeleccionado();
                return;
            }

            DescargarSeleccionado();
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
