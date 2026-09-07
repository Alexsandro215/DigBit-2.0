using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Diagnostics;
using DigBit.conexion;


namespace DigBit
{
    public partial class Bitacora_profesor : Form
    {
        private System.Windows.Forms.Timer timer;
        private DateTime horaRegistro;
        private Consultas consultas;
        private string codigoIngresado;

        public Bitacora_profesor()
        {
            InitializeComponent();
            txtBienvenido.Enabled = false;           
            txtTiempoRestante.Enabled = false;
            lblMatricula.Visible = false;
            consultas = new Consultas();
            String smatricula = Datos_User.getUser();
            if (!string.IsNullOrEmpty(smatricula))
            {
                lblMatricula.Text = smatricula;
            }
            MostrarNombreProfesor();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // Intervalo de 1 segundo
            timer.Tick += Timer_Tick;

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void MostrarNombreProfesor()
        {
            try
            {
                string idUsuario = lblMatricula.Text;
                string nombre = consultas.MostrarNombreProfesor(idUsuario);
                txtBienvenido.Texts = nombre;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mostrar el nombre del profesor: {ex.Message}");
                MessageBox.Show($"Error al mostrar el nombre del profesor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDescargarBitacora_Click(object sender, EventArgs e)
        {
            DescargarPdfCodigo(txtCodigo.Texts);
        }

        private void DescargarPdfCodigo(string codigoAcceso)
        {
            string nombreProfesor = txtBienvenido.Texts;
            string nombreArchivo = DateTime.Now.ToString("dd-M-yyyy-HH_mm_ss") + "_" + nombreProfesor + ".pdf";

            if (string.IsNullOrWhiteSpace(codigoAcceso) || codigoAcceso == "Ingrese el código")
            {
                MessageBox.Show("Selecciona un codigo para descargar la bitacora.", "Codigo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario datos = consultas.ConsultarDatosPdf(codigoAcceso);

            if (datos != null)
            {
                string rutaCompleta = ObtenerRutaDestinoPdf(nombreArchivo);
                if (string.IsNullOrWhiteSpace(rutaCompleta))
                {
                    return;
                }

                string paginahtml_texto = Properties.Resources.plantilla.ToString();
                paginahtml_texto = paginahtml_texto.Replace("@HORARIODEENTRADA", datos.HoraEntrada);
                paginahtml_texto = paginahtml_texto.Replace("@HORARIODESALIDA", datos.HoraSalida);
                paginahtml_texto = paginahtml_texto.Replace("@PROFESOR", txtBienvenido.Texts);
                paginahtml_texto = paginahtml_texto.Replace("@GRUPO", datos.GruposId);
                paginahtml_texto = paginahtml_texto.Replace("@MATERIA", datos.MateriasId);

                // Formatear la hora de registro para mostrar solo la fecha
                string horaRegistro = datos.HoraRegistro;
                paginahtml_texto = paginahtml_texto.Replace("@FECHA", horaRegistro);

                using (FileStream stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.AddTitle("Bitacora DigBit");
                    pdfDoc.AddAuthor("DigBit");

                    pdfDoc.Add(new Phrase(""));

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

                    // Agregar tabla al PDF
                    DataTable detalles = consultas.consultaRegistro(codigoAcceso);
                    AgregarTablaAlPDF(pdfDoc, detalles);

                    pdfDoc.Close();
                    stream.Close();
                }

                MessageBox.Show("El archivo PDF se ha guardado correctamente en: " + rutaCompleta, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AbrirVistaPreviaPdf(rutaCompleta);
            }
            else
            {
                MessageBox.Show("No se encontraron datos para el código proporcionado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
        private void txtCodigo_Enter(object sender, EventArgs e)
        {
            if(txtCodigo.Texts == "Ingrese el código")
            {
                txtCodigo.Texts = "";
                txtCodigo.ForeColor = Color.DarkGreen;
            }
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if(txtCodigo.Texts == "")
            {
                txtCodigo.Texts = "Ingrese el código";
                txtCodigo.ForeColor= Color.Black;
            }
        }
        private void IngresarDatosPdf()
        {
            string id = txtCodigo.Texts;
            Usuario datos = consultas.ConsultarDatosPdf(id);

            if (datos != null)
            {
                // Aquí puedes usar los datos obtenidos y almacenarlos en variables
                string horaEntrada = datos.HoraEntrada;
                string horaSalida = datos.HoraSalida;
                string gruposId = datos.GruposId;
                string materiasId = datos.MateriasId;

                // Formatear la hora de registro para mostrar solo la fecha
                string horaRegistro = DateTime.Parse(datos.HoraRegistro).ToString("dd-MM-yyyy");

            }
            
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string codigoBuscado = txtCodigo.Texts;
            if (string.IsNullOrWhiteSpace(codigoBuscado) || codigoBuscado == "Ingrese el código")
            {
                MessageBox.Show("Ingresa un codigo valido.", "Codigo requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Usuario datos = consultas.ConsultarDatosPdf(codigoBuscado);
            if (datos == null)
            {
                MessageBox.Show("No se encontraron datos para el codigo proporcionado.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            codigoIngresado = codigoBuscado;
            IngresarDatosPdf();

            try
            {
                ActualizarHoraRegistro();
                ActualizarTabla();
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error al cargar codigo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                ActualizarHoraRegistro();

                TimeSpan tiempoTranscurrido = DateTime.Now - horaRegistro;
                TimeSpan tiempoRestante = TimeSpan.FromMinutes(15) - tiempoTranscurrido;

                if (tiempoRestante <= TimeSpan.Zero)
                {
                    timer.Stop();
                    txtTiempoRestante.Texts = "00:00:00";
                }
                else
                {
                    txtTiempoRestante.Texts = tiempoRestante.ToString(@"hh\:mm\:ss");
                    ActualizarTabla(); // Actualizar la tabla cada vez que el temporizador hace tic
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error al actualizar la hora de registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarHoraRegistro()
        {
            horaRegistro = consultas.ConsultaFecha(codigoIngresado);
        }

       
        private void ActualizarTabla()
        {
            try
            {
                DataTable detalles = consultas.consultaRegistro(codigoIngresado);
                MostrarDetallesEnTabla(detalles);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error al actualizar la tabla", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AgregarTablaAlPDF(Document pdfDoc, DataTable detalles)
        {
            PdfPTable pdfTable = new PdfPTable(4);
            pdfTable.WidthPercentage = 100;
            pdfTable.SetWidths(new float[] { 8f, 28f, 20f, 44f });

            // Agregar encabezados de columna
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

            // Añadir la tabla al documento PDF
            pdfDoc.Add(pdfTable);
        }


        private void MostrarDetallesEnTabla(DataTable detalles)
        {
            tableLista.Controls.Clear();
            tableLista.RowStyles.Clear();
            tableLista.ColumnStyles.Clear();
            tableLista.RowCount = 0;

            tableLista.ColumnCount = 4;
            tableLista.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
            tableLista.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tableLista.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tableLista.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 420F));

            // Agrega encabezados de columna
            tableLista.RowCount = 1;
            tableLista.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLista.Controls.Add(new Label() { Text = "No.", AutoSize = true }, 0, 0);
            tableLista.Controls.Add(new Label() { Text = "Nombre Completo", AutoSize = true}, 1, 0);
            tableLista.Controls.Add(new Label() { Text = "Número de Computadora", AutoSize = true}, 2, 0);
            tableLista.Controls.Add(new Label() { Text = "Comentarios", AutoSize = true }, 3, 0);

            int numeroFila = 1;
            foreach (DataRow row in detalles.Rows)
            {
                tableLista.RowCount++;
                tableLista.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                tableLista.Controls.Add(new Label() { Text = numeroFila.ToString(), AutoSize = true}, 0, tableLista.RowCount - 1);
                tableLista.Controls.Add(new Label() { Text = row["nombre_completo"].ToString(), AutoSize = true }, 1, tableLista.RowCount - 1);
                tableLista.Controls.Add(new Label() { Text = row["numero_computadora"].ToString(), AutoSize = true}, 2, tableLista.RowCount - 1);

                string comentarios = $"Red: {row["comentarios_red"]}, Hardware: {row["comentarios_hardware"]}, Software: {row["comentarios_software"]}";
                tableLista.Controls.Add(new Label() { Text = comentarios, AutoSize = true }, 3, tableLista.RowCount - 1);

                numeroFila++;
            }

            tableLista.Refresh();
        }



    }
}
