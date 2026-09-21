using DigBit.conexion;
using DigBit.Infraestructura;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DigBit
{
    /// <summary>
    /// Fase 6 (profesor): sus clases con el codigo fijo de cada una, su horario
    /// semanal y las excepciones de la semana, todo de solo lectura. Sustituye a
    /// la generacion de codigos: ahora los asigna el administrador.
    /// </summary>
    public class MisClasesProfesor : Form
    {
        /// <summary>Cuantos dias hacia adelante se muestran las excepciones.</summary>
        private const int DiasExcepciones = 7;

        private readonly HorariosDatos horarios = new HorariosDatos();
        private readonly string numeroIdentificador;

        private Label lblTitulo;
        private Label lblSubtitulo;
        private Label lblHorario;
        private Label lblExcepciones;
        private Label lblSinCambios;
        private DataGridView dgvClases;
        private DataGridView dgvHorario;
        private DataGridView dgvExcepciones;
        private Button btnCerrar;

        public MisClasesProfesor()
        {
            numeroIdentificador = Datos_User.getUser();

            InicializarComponentes();
            CargarNombreProfesor();
            CargarDatos();
        }

        private void InicializarComponentes()
        {
            BackColor = Color.White;
            ClientSize = new Size(980, 692);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Mis clases";

            lblTitulo = new Label
            {
                AutoSize = true,
                Font = new Font("Century Gothic", 18F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = new Point(28, 18),
                Text = "Mis clases"
            };

            lblSubtitulo = new Label
            {
                AutoSize = true,
                Font = new Font("Century Gothic", 10.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(50, 50, 50),
                Location = new Point(30, 55),
                Text = "El código de cada clase es el que dictas a tus alumnos. Los asigna el administrador."
            };

            // Clases: el codigo va destacado porque es lo que el profesor dicta.
            dgvClases = CrearTabla(new Point(33, 88), new Size(914, 150));
            DataGridViewColumn colCodigo = AgregarColumna(dgvClases, "Codigo", "Código", 14);
            colCodigo.DefaultCellStyle.Font = new Font("Century Gothic", 11F, FontStyle.Bold);
            colCodigo.DefaultCellStyle.ForeColor = Color.DarkGreen;
            AgregarColumna(dgvClases, "Materia", "Materia", 30);
            AgregarColumna(dgvClases, "Grupo", "Grupo", 12);
            AgregarColumna(dgvClases, "VigenteDesde", "Vigente desde", 15);
            AgregarColumna(dgvClases, "VigenteHasta", "Vigente hasta", 15);
            AgregarColumna(dgvClases, "Activa", "Activa", 10);

            lblHorario = CrearTituloSeccion("Mi horario", new Point(30, 250));

            dgvHorario = CrearTabla(new Point(33, 276), new Size(914, 180));
            AgregarColumna(dgvHorario, "Dia", "Día", 12);
            AgregarColumna(dgvHorario, "Inicio", "Inicio", 9);
            AgregarColumna(dgvHorario, "Fin", "Fin", 9);
            AgregarColumna(dgvHorario, "Laboratorio", "Laboratorio", 18);
            AgregarColumna(dgvHorario, "Materia", "Materia", 26);
            AgregarColumna(dgvHorario, "Grupo", "Grupo", 12);
            AgregarColumna(dgvHorario, "Codigo", "Código", 12);

            lblExcepciones = CrearTituloSeccion("Excepciones de los próximos " + DiasExcepciones + " días", new Point(30, 468));

            dgvExcepciones = CrearTabla(new Point(33, 494), new Size(914, 128));
            AgregarColumna(dgvExcepciones, "Fecha", "Fecha", 11);
            AgregarColumna(dgvExcepciones, "Tipo", "Tipo", 12);
            AgregarColumna(dgvExcepciones, "Laboratorio", "Laboratorio", 15);
            AgregarColumna(dgvExcepciones, "Inicio", "Inicio", 8);
            AgregarColumna(dgvExcepciones, "Fin", "Fin", 8);
            AgregarColumna(dgvExcepciones, "Materia", "Materia", 20);
            AgregarColumna(dgvExcepciones, "Grupo", "Grupo", 10);
            AgregarColumna(dgvExcepciones, "Motivo", "Motivo", 16);

            lblSinCambios = new Label
            {
                AutoSize = true,
                Font = new Font("Century Gothic", 10.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(110, 110, 110),
                Location = new Point(33, 500),
                Text = "Sin cambios esta semana",
                Visible = false
            };

            btnCerrar = CrearBoton("Cerrar", new Point(797, 636), new Size(150, 42));
            btnCerrar.Click += btnCerrar_Click;
            CancelButton = btnCerrar;

            Controls.Add(lblTitulo);
            Controls.Add(lblSubtitulo);
            Controls.Add(dgvClases);
            Controls.Add(lblHorario);
            Controls.Add(dgvHorario);
            Controls.Add(lblExcepciones);
            Controls.Add(dgvExcepciones);
            Controls.Add(lblSinCambios);
            Controls.Add(btnCerrar);
        }

        private static Label CrearTituloSeccion(string texto, Point posicion)
        {
            return new Label
            {
                AutoSize = true,
                Font = new Font("Century Gothic", 12F, FontStyle.Bold),
                ForeColor = Color.DarkGreen,
                Location = posicion,
                Text = texto
            };
        }

        private static DataGridView CrearTabla(Point posicion, Size tamaño)
        {
            DataGridView tabla = new DataGridView
            {
                Location = posicion,
                Size = tamaño,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                EnableHeadersVisualStyles = false,
                Font = new Font("Century Gothic", 9.75F),
                MultiSelect = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                ScrollBars = ScrollBars.Both,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            tabla.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold);
            tabla.ColumnHeadersDefaultCellStyle.ForeColor = Color.DarkGreen;
            tabla.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            tabla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            return tabla;
        }

        private static DataGridViewColumn AgregarColumna(DataGridView tabla, string nombre, string titulo, float peso)
        {
            DataGridViewTextBoxColumn columna = new DataGridViewTextBoxColumn
            {
                Name = nombre,
                HeaderText = titulo,
                FillWeight = peso,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            tabla.Columns.Add(columna);
            return columna;
        }

        private Button CrearBoton(string texto, Point posicion, Size tamaño)
        {
            Button boton = new Button
            {
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Century Gothic", 10F, FontStyle.Bold),
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
                string nombre = new Consultas().MostrarNombreProfesor(numeroIdentificador);
                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    lblSubtitulo.Text = "Docente: " + nombre + ". " + lblSubtitulo.Text;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Leer el nombre del profesor " + numeroIdentificador, ex);
            }
        }

        private void CargarDatos()
        {
            if (string.IsNullOrWhiteSpace(numeroIdentificador))
            {
                MessageBox.Show("No se encontró el número de empleado del docente actual.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int idProfesor;
            try
            {
                idProfesor = new Consultas().ObtenerIdPorMatricula(numeroIdentificador);
            }
            catch (Exception ex)
            {
                Log.Error("Buscar el id del profesor " + numeroIdentificador, ex);
                MessageBox.Show("No se encontró al docente con número de empleado " + numeroIdentificador + ".\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DateTime hoy = DateTime.Today;
                List<ClaseInfo> clases = horarios.ObtenerClasesDeProfesor(idProfesor);
                List<FranjaInfo> franjas = horarios.ObtenerFranjasDeProfesor(idProfesor);

                // Las excepciones vienen de todos los laboratorios; se quedan solo las de sus clases.
                HashSet<int> misClases = new HashSet<int>(clases.Select(c => c.Id));
                List<ExcepcionInfo> excepciones = horarios.ObtenerExcepciones(hoy, hoy.AddDays(DiasExcepciones), 0)
                    .Where(x => misClases.Contains(x.ClaseId))
                    .ToList();

                MostrarClases(clases, hoy);
                MostrarHorario(franjas);
                MostrarExcepciones(excepciones);
            }
            catch (Exception ex)
            {
                Log.Error("Cargar las clases y el horario del profesor " + numeroIdentificador, ex);
                MessageBox.Show("No se pudieron cargar tus clases y tu horario.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarClases(List<ClaseInfo> clases, DateTime hoy)
        {
            dgvClases.Rows.Clear();
            foreach (ClaseInfo clase in clases)
            {
                int fila = dgvClases.Rows.Add(
                    clase.Codigo,
                    clase.Materia,
                    clase.Grupo,
                    clase.VigenteDesde.ToString("dd/MM/yyyy"),
                    clase.VigenteHasta.ToString("dd/MM/yyyy"),
                    clase.Activa ? "Sí" : "No");

                // Inactiva o fuera de su periodo: en gris, el codigo ya no entra.
                if (!clase.VigenteEn(hoy))
                {
                    dgvClases.Rows[fila].DefaultCellStyle.ForeColor = Color.Gray;
                }
            }

            if (clases.Count == 0)
            {
                lblSubtitulo.Text = "No tienes clases asignadas. El administrador es quien crea las clases y asigna su código.";
            }
        }

        private void MostrarHorario(List<FranjaInfo> franjas)
        {
            dgvHorario.Rows.Clear();
            foreach (FranjaInfo franja in franjas)
            {
                dgvHorario.Rows.Add(
                    NombreDia(franja.DiaSemana),
                    HorariosDatos.Hora(franja.Inicio),
                    HorariosDatos.Hora(franja.Fin),
                    franja.Laboratorio,
                    franja.Materia,
                    franja.Grupo,
                    franja.Codigo);
            }
        }

        private void MostrarExcepciones(List<ExcepcionInfo> excepciones)
        {
            dgvExcepciones.Rows.Clear();
            foreach (ExcepcionInfo excepcion in excepciones)
            {
                int fila = dgvExcepciones.Rows.Add(
                    excepcion.Fecha.ToString("dd/MM/yyyy"),
                    excepcion.EsExtra ? "Sesión extra" : "Cancelada",
                    excepcion.Laboratorio,
                    HorariosDatos.Hora(excepcion.Inicio),
                    HorariosDatos.Hora(excepcion.Fin),
                    excepcion.Materia,
                    excepcion.Grupo,
                    excepcion.Motivo);

                dgvExcepciones.Rows[fila].DefaultCellStyle.ForeColor = excepcion.EsExtra ? Color.DarkGreen : Color.Firebrick;
            }

            bool hay = excepciones.Count > 0;
            dgvExcepciones.Visible = hay;
            lblSinCambios.Visible = !hay;
        }

        private static string NombreDia(int diaSemana)
        {
            return diaSemana >= 1 && diaSemana < HorariosDatos.NombresDia.Length
                ? HorariosDatos.NombresDia[diaSemana]
                : diaSemana.ToString();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
