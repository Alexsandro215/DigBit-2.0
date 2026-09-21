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
    /// <summary>
    /// Bitacoras para el administrador, en dos niveles: arriba las SESIONES (una
    /// clase en una fecha, que es la bitacora de esa hora) y abajo los alumnos
    /// que registraron en la sesion elegida. Antes era una sola tabla con una
    /// fila por alumno y los datos de la sesion repetidos en cada una.
    /// </summary>
    public class GestionBitacorasAdministrador : Form
    {
        // Fila de una bitacora que reporta algo: fondo suave. La celda con la
        // falla concreta va en un tono fuerte, para localizarla de un vistazo.
        private static readonly Color ColorConFalla = Color.FromArgb(255, 240, 214);
        private static readonly Color ColorFallaCelda = Color.FromArgb(255, 176, 79);

        private readonly Consultas consultas;
        private readonly bool permitirEdicion;
        private DataTable tablaSesiones;
        private DataTable tablaDetalle;
        private int idSesionMostrada;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblBusqueda;
        private Label lblDocente;
        private Label lblFecha;
        private Label lblSesiones;
        private Label lblDetalle;
        private Label lblCantidad;
        private Panel pnlLeyenda;
        private Label lblLeyenda;

        /// <summary>Lo que hace falta de una sesion para nombrar y generar su PDF.</summary>
        private sealed class SesionFila
        {
            public int IdSesion;
            public string Codigo;
            public string Profesor;
            public string Fecha;
            public string HoraEntrada;
        }

        private TextBox txtBusqueda;
        private TextBox txtDocente;
        private DateTimePicker dtpFecha;
        private CheckBox chkFiltrarFecha;
        private CheckBox chkSeleccionarTodo;
        private DataGridView dgvSesiones;
        private DataGridView dgvDetalle;
        private Button btnAplicarFiltro;
        private Button btnLimpiarFiltro;
        private Button btnDescargarPdf;
        private Button btnDescargarLote;
        private Button btnEditarSeleccion;
        private Button btnCerrar;

        public GestionBitacorasAdministrador(bool permitirEdicion)
        {
            this.permitirEdicion = permitirEdicion;
            consultas = new Consultas();

            InicializarComponentes();
            CargarSesiones();
        }

        private void InicializarComponentes()
        {
            BackColor = Color.White;
            ClientSize = new Size(1280, 726);
            MinimumSize = new Size(1100, 640);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
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
                    ? "Elige una sesion arriba y, abajo, el alumno cuyo registro quieres editar."
                    : "Cada sesion es la bitacora de una clase en una fecha. Elige una para ver quien registro."
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
            btnAplicarFiltro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAplicarFiltro.Click += btnAplicarFiltro_Click;

            btnLimpiarFiltro = CrearBoton("Limpiar", new Point(1115, 112), new Size(120, 38));
            btnLimpiarFiltro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLimpiarFiltro.Click += btnLimpiarFiltro_Click;

            // --- Nivel 1: sesiones ------------------------------------------
            lblSesiones = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 10.5F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(30, 160),
                Text = "Sesiones"
            };

            dgvSesiones = CrearTabla(new Point(33, 184), new Size(1210, 236));
            dgvSesiones.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvSesiones.MultiSelect = true;
            // Los dos eventos: al pulsar una fila, SelectionChanged puede llegar
            // antes de que CurrentRow apunte a la nueva. CargarDetalle se protege
            // de la repeticion comparando con la sesion que ya esta mostrada.
            dgvSesiones.SelectionChanged += (s, e) => CargarDetalle();
            dgvSesiones.CurrentCellChanged += (s, e) => CargarDetalle();
            dgvSesiones.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) DescargarSeleccionado(); };
            dgvSesiones.CellFormatting += dgvSesiones_CellFormatting;

            // --- Nivel 2: alumnos de la sesion -------------------------------
            lblDetalle = new Label
            {
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 10.5F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(30, 430),
                Text = "Alumnos registrados"
            };

            dgvDetalle = CrearTabla(new Point(33, 454), new Size(1210, 186));
            dgvDetalle.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvDetalle.MultiSelect = false;
            dgvDetalle.CellDoubleClick += dgvDetalle_CellDoubleClick;
            dgvDetalle.CellFormatting += dgvDetalle_CellFormatting;

            chkSeleccionarTodo = new CheckBox
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 9.75F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(33, 652),
                Text = "Seleccionar todas las sesiones filtradas"
            };
            chkSeleccionarTodo.CheckedChanged += chkSeleccionarTodo_CheckedChanged;

            lblCantidad = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 9.75F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(30, 678),
                Text = "Sesiones encontradas: 0"
            };

            pnlLeyenda = new Panel
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                BackColor = ColorFallaCelda,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(33, 703),
                Size = new Size(14, 14)
            };

            lblLeyenda = new Label
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                AutoSize = true,
                Font = new DrawingFont("Century Gothic", 8.25F),
                ForeColor = Color.FromArgb(50, 50, 50),
                Location = new Point(52, 702),
                Text = "Naranja intenso: la falla reportada. Naranja claro: el resto de esa bitacora."
            };

            btnDescargarPdf = CrearBoton("Descargar PDF", new Point(650, 651), new Size(140, 42));
            btnDescargarPdf.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDescargarPdf.Click += btnDescargarPdf_Click;

            btnDescargarLote = CrearBoton("Descargar lote", new Point(805, 651), new Size(140, 42));
            btnDescargarLote.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDescargarLote.Click += btnDescargarLote_Click;

            btnEditarSeleccion = CrearBoton("Editar alumno", new Point(960, 651), new Size(140, 42));
            btnEditarSeleccion.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnEditarSeleccion.Click += btnEditarSeleccion_Click;
            btnEditarSeleccion.Visible = permitirEdicion;

            btnCerrar = CrearBoton("Cerrar", new Point(1115, 651), new Size(128, 42));
            btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
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
            Controls.Add(lblSesiones);
            Controls.Add(dgvSesiones);
            Controls.Add(lblDetalle);
            Controls.Add(dgvDetalle);
            Controls.Add(chkSeleccionarTodo);
            Controls.Add(lblCantidad);
            Controls.Add(pnlLeyenda);
            Controls.Add(lblLeyenda);
            Controls.Add(btnDescargarPdf);
            Controls.Add(btnDescargarLote);
            Controls.Add(btnEditarSeleccion);
            Controls.Add(btnCerrar);
        }

        private DataGridView CrearTabla(Point posicion, Size tamaño)
        {
            return new DataGridView
            {
                Location = posicion,
                Size = tamaño,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true,
                RowHeadersVisible = false,
                ScrollBars = ScrollBars.Both,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
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

        // =====================================================================
        // Sesiones
        // =====================================================================

        private void CargarSesiones()
        {
            try
            {
                tablaSesiones = consultas.ObtenerSesionesAdministrador();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron leer las sesiones: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tablaSesiones = new DataTable();
            }

            dgvSesiones.DataSource = tablaSesiones;
            ConfigurarColumnasSesiones();
            ActualizarContador();
            AplicarSeleccionAutomatica();
            CargarDetalle();
        }

        private void ConfigurarColumnasSesiones()
        {
            if (dgvSesiones.Columns.Count == 0)
            {
                return;
            }

            dgvSesiones.Columns["idcodigos_accesos"].Visible = false;
            dgvSesiones.Columns["fecha"].HeaderText = "Fecha";
            dgvSesiones.Columns["hora_entrada"].HeaderText = "Entrada";
            dgvSesiones.Columns["hora_salida"].HeaderText = "Salida";
            dgvSesiones.Columns["codigo"].HeaderText = "Codigo";
            dgvSesiones.Columns["materia"].HeaderText = "Materia";
            dgvSesiones.Columns["grupo"].HeaderText = "Grupo";
            dgvSesiones.Columns["numero_empleado"].HeaderText = "No. empleado";
            dgvSesiones.Columns["profesor"].HeaderText = "Profesor";
            dgvSesiones.Columns["laboratorio"].HeaderText = "Laboratorio";
            dgvSesiones.Columns["alumnos"].HeaderText = "Alumnos";
            dgvSesiones.Columns["con_fallas"].HeaderText = "Con fallas";
            dgvSesiones.Columns["sin_conexion"].HeaderText = "Sin conexion";

            dgvSesiones.Columns["materia"].FillWeight = 22;
            dgvSesiones.Columns["profesor"].FillWeight = 22;
            dgvSesiones.Columns["laboratorio"].FillWeight = 18;

            foreach (string columna in new[] { "alumnos", "con_fallas", "sin_conexion" })
            {
                dgvSesiones.Columns[columna].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }

        /// <summary>La cuenta de bitacoras con falla se marca en naranja cuando no es cero.</summary>
        private void dgvSesiones_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvSesiones.Columns[e.ColumnIndex].Name != "con_fallas")
            {
                return;
            }

            int conFallas = 0;
            if (e.Value != null && e.Value != DBNull.Value)
            {
                int.TryParse(e.Value.ToString(), out conFallas);
            }

            e.CellStyle.BackColor = conFallas > 0 ? ColorFallaCelda : Color.Empty;
            e.CellStyle.SelectionBackColor = conFallas > 0 ? ControlPaint.Dark(ColorFallaCelda, 0.12f) : dgvSesiones.DefaultCellStyle.SelectionBackColor;
        }

        private void AplicarFiltros()
        {
            if (tablaSesiones == null)
            {
                return;
            }

            List<string> filtros = new List<string>();
            if (chkFiltrarFecha.Checked)
            {
                filtros.Add($"fecha = '{dtpFecha.Value:yyyy-MM-dd}'");
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
                    $"fecha LIKE '%{textoBusqueda}%' OR " +
                    $"numero_empleado LIKE '%{textoBusqueda}%' OR " +
                    $"profesor LIKE '%{textoBusqueda}%' OR " +
                    $"grupo LIKE '%{textoBusqueda}%' OR " +
                    $"materia LIKE '%{textoBusqueda}%' OR " +
                    $"laboratorio LIKE '%{textoBusqueda}%'");
            }

            tablaSesiones.DefaultView.RowFilter = filtros.Count > 0
                ? string.Join(" AND ", filtros.Select(filtro => $"({filtro})"))
                : string.Empty;

            dgvSesiones.DataSource = tablaSesiones.DefaultView;
            ConfigurarColumnasSesiones();
            ActualizarContador();
            AplicarSeleccionAutomatica();
            CargarDetalle();
        }

        private void ActualizarContador()
        {
            int alumnos = 0;
            foreach (DataGridViewRow fila in dgvSesiones.Rows)
            {
                alumnos += LeerEntero(fila, "alumnos");
            }

            lblCantidad.Text = $"Sesiones encontradas: {dgvSesiones.Rows.Count} · {alumnos} bitacora(s) de alumnos en total";
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
            dgvSesiones.ClearSelection();

            foreach (DataGridViewRow fila in dgvSesiones.Rows)
            {
                if (fila.Visible)
                {
                    fila.Selected = true;
                }
            }
        }

        // =====================================================================
        // Detalle: alumnos de la sesion elegida
        // =====================================================================

        private void CargarDetalle()
        {
            int idSesion = ObtenerIdSesion(dgvSesiones.CurrentRow);
            if (idSesion == idSesionMostrada)
            {
                return;
            }

            idSesionMostrada = idSesion;
            if (idSesion == 0)
            {
                tablaDetalle = null;
                dgvDetalle.DataSource = null;
                lblDetalle.Text = "Alumnos registrados";
                return;
            }

            try
            {
                tablaDetalle = consultas.consultaRegistroSesion(idSesion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron leer los alumnos de la sesion: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tablaDetalle = null;
            }

            dgvDetalle.DataSource = tablaDetalle;
            ConfigurarColumnasDetalle();

            DataGridViewRow fila = dgvSesiones.CurrentRow;
            string descripcion = fila == null
                ? ""
                : " de " + LeerTexto(fila, "codigo") + " - " + LeerTexto(fila, "materia") + " - " + LeerTexto(fila, "grupo")
                  + ", " + LeerTexto(fila, "fecha") + " " + LeerTexto(fila, "hora_entrada") + " a " + LeerTexto(fila, "hora_salida");
            lblDetalle.Text = "Alumnos registrados" + descripcion + " (" + (tablaDetalle == null ? 0 : tablaDetalle.Rows.Count) + ")";
        }

        private void ConfigurarColumnasDetalle()
        {
            if (dgvDetalle.Columns.Count == 0)
            {
                return;
            }

            foreach (string oculta in new[] { "fk_codigo_accesos", "fk_usuario", "nombre", "apellido_paterno", "apellido_materno", "con_falla", "sin_conexion" })
            {
                if (dgvDetalle.Columns.Contains(oculta))
                {
                    dgvDetalle.Columns[oculta].Visible = false;
                }
            }

            dgvDetalle.Columns["numero_identificador"].HeaderText = "Matricula";
            dgvDetalle.Columns["nombre_completo"].HeaderText = "Alumno";
            dgvDetalle.Columns["numero_computadora"].HeaderText = "Computadora";
            dgvDetalle.Columns["falla_red"].HeaderText = "Falla red";
            dgvDetalle.Columns["comentarios_red"].HeaderText = "Comentario red";
            dgvDetalle.Columns["falla_hardware"].HeaderText = "Falla hardware";
            dgvDetalle.Columns["comentarios_hardware"].HeaderText = "Comentario hardware";
            dgvDetalle.Columns["falla_software"].HeaderText = "Falla software";
            dgvDetalle.Columns["comentarios_software"].HeaderText = "Comentario software";

            dgvDetalle.Columns["nombre_completo"].DisplayIndex = 0;
            dgvDetalle.Columns["nombre_completo"].FillWeight = 24;
            dgvDetalle.Columns["comentarios_red"].FillWeight = 20;
            dgvDetalle.Columns["comentarios_hardware"].FillWeight = 20;
            dgvDetalle.Columns["comentarios_software"].FillWeight = 20;
        }

        /// <summary>
        /// La fila de una bitacora con reportes va en tono suave y la celda que
        /// contiene la falla, en tono fuerte: asi se localiza sin leer toda la
        /// fila. Se decide en cada pintado, por si la tabla se recarga o reordena.
        /// </summary>
        private void dgvDetalle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            DataRowView vista = dgvDetalle.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (vista == null || !Consultas.ConFalla(vista.Row))
            {
                return;
            }

            string area;
            bool esLaFalla = Consultas.EsColumnaDeFalla(dgvDetalle.Columns[e.ColumnIndex].Name, out area)
                && Consultas.ConFallaEn(vista.Row, area);

            Color fondo = esLaFalla ? ColorFallaCelda : ColorConFalla;
            e.CellStyle.BackColor = fondo;
            e.CellStyle.SelectionBackColor = ControlPaint.Dark(fondo, 0.12f);
            e.CellStyle.SelectionForeColor = Color.Black;
            if (esLaFalla)
            {
                e.CellStyle.Font = new DrawingFont(dgvDetalle.Font, FontStyle.Bold);
            }
        }

        // =====================================================================
        // Lectura de filas
        // =====================================================================

        private static string LeerTexto(DataGridViewRow fila, string columna)
        {
            if (fila == null || !fila.DataGridView.Columns.Contains(columna))
            {
                return string.Empty;
            }

            object valor = fila.Cells[columna].Value;
            return valor == null || valor == DBNull.Value ? string.Empty : valor.ToString();
        }

        private static int LeerEntero(DataGridViewRow fila, string columna)
        {
            int numero;
            return int.TryParse(LeerTexto(fila, columna), out numero) ? numero : 0;
        }

        /// <summary>Sesion (idcodigos_accesos) de una fila de la tabla de sesiones; 0 si no la tiene.</summary>
        private static int ObtenerIdSesion(DataGridViewRow fila)
        {
            return LeerEntero(fila, "idcodigos_accesos");
        }

        private static SesionFila LeerSesionFila(DataGridViewRow fila)
        {
            return new SesionFila
            {
                IdSesion = ObtenerIdSesion(fila),
                Codigo = LeerTexto(fila, "codigo"),
                Profesor = string.IsNullOrWhiteSpace(LeerTexto(fila, "profesor")) ? "Bitacora" : LeerTexto(fila, "profesor"),
                Fecha = LeerTexto(fila, "fecha"),
                HoraEntrada = LeerTexto(fila, "hora_entrada")
            };
        }

        /// <summary>
        /// Nombre de archivo "codigo_fecha_HHmm_profesor.pdf": el mismo codigo tiene
        /// una sesion por fecha, asi que la fecha y la hora de entrada las distinguen.
        /// </summary>
        private string NombreArchivoSesion(SesionFila sesion)
        {
            string fecha = sesion.Fecha.Length >= 10 ? sesion.Fecha.Substring(0, 10) : sesion.Fecha;
            string hora = sesion.HoraEntrada.Replace(":", "");
            if (hora.Length > 4)
            {
                hora = hora.Substring(0, 4);
            }

            string[] partes = { sesion.Codigo, fecha, hora, sesion.Profesor };
            string nombre = string.Join("_", partes.Where(parte => !string.IsNullOrWhiteSpace(parte)));
            return SanitizarNombreArchivo(nombre) + ".pdf";
        }

        // =====================================================================
        // Acciones
        // =====================================================================

        private void DescargarSeleccionado()
        {
            if (dgvSesiones.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una sesion de la tabla de arriba.", "Seleccion requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SesionFila sesion = LeerSesionFila(dgvSesiones.CurrentRow);
            if (sesion.IdSesion == 0)
            {
                MessageBox.Show("La fila seleccionada no tiene una sesion valida.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DescargarPdfSesion(sesion);
        }

        private void DescargarPorLote()
        {
            List<DataGridViewRow> filasOrigen = dgvSesiones.SelectedRows.Count > 0
                ? dgvSesiones.SelectedRows.Cast<DataGridViewRow>().ToList()
                : dgvSesiones.Rows.Cast<DataGridViewRow>().Where(fila => fila.Visible).ToList();

            List<SesionFila> sesiones = filasOrigen
                .Select(LeerSesionFila)
                .Where(sesion => sesion.IdSesion != 0)
                .GroupBy(sesion => sesion.IdSesion)
                .Select(grupo => grupo.First())
                .ToList();

            if (sesiones.Count == 0)
            {
                MessageBox.Show("No hay sesiones disponibles para descargar.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string carpetaDestino = ObtenerCarpetaDestinoLote();
            if (string.IsNullOrWhiteSpace(carpetaDestino))
            {
                return;
            }

            int descargadas = 0;
            HashSet<string> nombresUsados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (SesionFila sesion in sesiones)
            {
                string nombreArchivo = NombreArchivoSesion(sesion);
                if (!nombresUsados.Add(nombreArchivo))
                {
                    // Dos sesiones con el mismo codigo, fecha y hora: se distinguen por su id.
                    nombreArchivo = Path.GetFileNameWithoutExtension(nombreArchivo) + "_" + sesion.IdSesion + ".pdf";
                    nombresUsados.Add(nombreArchivo);
                }

                string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

                if (GenerarPdfSesion(sesion.IdSesion, sesion.Profesor, rutaCompleta))
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

            if (dgvDetalle.CurrentRow == null)
            {
                MessageBox.Show("Selecciona el alumno cuyo registro quieres editar, en la tabla de abajo.", "Seleccion requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (EditarBitacoraAdministrador editor = new EditarBitacoraAdministrador(dgvDetalle.CurrentRow))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    int idSesion = idSesionMostrada;
                    CargarSesiones();
                    AplicarFiltros();
                    SeleccionarSesion(idSesion);
                }
            }
        }

        /// <summary>Vuelve a dejar seleccionada una sesion tras recargar la tabla.</summary>
        private void SeleccionarSesion(int idSesion)
        {
            foreach (DataGridViewRow fila in dgvSesiones.Rows)
            {
                if (ObtenerIdSesion(fila) == idSesion)
                {
                    dgvSesiones.ClearSelection();
                    fila.Selected = true;
                    dgvSesiones.CurrentCell = fila.Cells[dgvSesiones.Columns["codigo"].Index];
                    return;
                }
            }
        }

        private void DescargarPdfSesion(SesionFila sesion)
        {
            string rutaCompleta = ObtenerRutaDestinoPdf(NombreArchivoSesion(sesion));
            if (string.IsNullOrWhiteSpace(rutaCompleta))
            {
                return;
            }

            if (GenerarPdfSesion(sesion.IdSesion, sesion.Profesor, rutaCompleta))
            {
                MessageBox.Show("El PDF se guardo correctamente en: " + rutaCompleta, "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AbrirVistaPreviaPdf(rutaCompleta);
            }
        }

        private bool GenerarPdfSesion(int idSesion, string nombreProfesor, string rutaCompleta)
        {
            Usuario datos = consultas.ConsultarDatosPdfSesion(idSesion);
            if (datos == null)
            {
                MessageBox.Show("No se encontraron datos de la sesion seleccionada.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                DataTable detalles = consultas.consultaRegistroSesion(idSesion);
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

            BaseColor suave = new BaseColor(ColorConFalla.R, ColorConFalla.G, ColorConFalla.B);
            BaseColor fuerte = new BaseColor(ColorFallaCelda.R, ColorFallaCelda.G, ColorFallaCelda.B);
            iTextSharp.text.Font negrita = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10f);
            bool hayFallas = false;

            int numeroFila = 1;
            foreach (DataRow row in detalles.Rows)
            {
                // Fila en tono suave si reporto algo; la celda de la falla, en tono fuerte.
                bool conFalla = Consultas.ConFalla(row);
                hayFallas |= conFalla;

                string[] celdas = { numeroFila.ToString(), row["nombre_completo"].ToString(), row["numero_computadora"].ToString(), Consultas.DescribirFallas(row) };
                for (int i = 0; i < celdas.Length; i++)
                {
                    bool esLaFalla = conFalla && i == celdas.Length - 1;
                    PdfPCell celda = new PdfPCell(new Phrase(celdas[i], esLaFalla ? negrita : null)) { Padding = 4f };
                    if (conFalla)
                    {
                        celda.BackgroundColor = esLaFalla ? fuerte : suave;
                    }

                    pdfTable.AddCell(celda);
                }

                numeroFila++;
            }

            pdfDoc.Add(pdfTable);

            if (hayFallas)
            {
                pdfDoc.Add(new Paragraph("Celda naranja intensa: la falla o el comentario que reporto el alumno.",
                    FontFactory.GetFont(FontFactory.HELVETICA, 8f)) { SpacingBefore = 6f });
            }
        }

        // =====================================================================
        // Eventos
        // =====================================================================

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

            dgvSesiones.ClearSelection();
        }

        private void btnEditarSeleccion_Click(object sender, EventArgs e)
        {
            EditarSeleccionado();
        }

        private void dgvDetalle_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (permitirEdicion)
            {
                EditarSeleccionado();
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
