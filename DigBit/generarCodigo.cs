using DigBit.conexion;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DigBit
{
    public partial class generarCodigo : Form
    {
        private Consultas consultas;
        private int carreraProfesorId;
        // Declaración de las variables que se usarán en la interfaz
        DateTime fecha;
        DateTime hora;
        string fechaActualTexto;
        string horaActualTexto;
        string codigo;
        DateTime horaActual;


        public generarCodigo()
        {
            // Llamado de la clase Codigo_Generador
            InitializeComponent(); // Esto debe ir primero para asegurar que los controles están inicializados
            consultas = new Consultas();
            txtNombreProfesor.Enabled = false;
            lblMatricula.Visible = false;
            string smatricula = Datos_User.getUser();

            if (!string.IsNullOrEmpty(smatricula))
            {
                lblMatricula.Text = smatricula;

            }

            carreraProfesorId = consultas.ObtenerCarreraIdDelUsuario(lblMatricula.Text);
            CargarDatoscbGrupo();
            CargarDatoscbLaboratorio();
            CargarDatoscbMateria();
            mostrarNombreProf();

            // Deshabilitamos el combo de fecha
            cbFecha.Enabled = false;
            // Deshabilitamos un textfield de la hora de entrada
            txtHoraEntrada.Enabled = false;
            hora = DateTime.Now; // Traemos la hora del sistema
            fecha = DateTime.Now; // Traemos la fecha del sistema
            horaActualTexto = hora.ToString("HH:mm:ss"); // Le damos un formato a la hora        
            fechaActualTexto = fecha.ToString("yyyy/MM/dd"); // Le damos un formato a la fecha
            cbFecha.Texts = fechaActualTexto; // Asentuamos la fecha
            txtHoraEntrada.Texts = horaActualTexto; // Asentuamos la horadadadada
            codigo = CodigoGenerador.GenerarCodigoAleatorio(5); 

        }

        private void btnGenerarCodigo_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnGenerarCodigo_Click_1(object sender, EventArgs e)
        {
            string codigoenviar = codigo;
            horaActual = DateTime.Now;
            InsercionDatos insertarcodigo = new InsercionDatos();
            string horaActualStr = horaActual.ToString("yyyy-MM-dd HH:mm:ss");

            int id = int.Parse(lblMatricula.Text);
            Consultas con = new Consultas();
            int matricula = con.ObtenerIdPorMatricula(id);
            OpcionCombo materiaSeleccionada = cbMateria.SelectedItem as OpcionCombo;
            OpcionCombo grupoSeleccionado = cbGrupo.SelectedItem as OpcionCombo;
            OpcionCombo laboratorioSeleccionado = cbLaboratorio.SelectedItem as OpcionCombo;

            if (materiaSeleccionada == null || grupoSeleccionado == null || laboratorioSeleccionado == null)
            {
                MessageBox.Show("Selecciona una materia, un grupo y un laboratorio validos.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ResultadoInsercionCodigo resultado = insertarcodigo.insertarcodigo(codigoenviar, horaActualStr, materiaSeleccionada.Id, grupoSeleccionado.Id, laboratorioSeleccionado.Id, matricula, cbFecha.Texts, txtHoraEntrada.Texts, cbHoraSalida.Texts);

            if (resultado == ResultadoInsercionCodigo.Error)
            {
                return;
            }

            // Guardar el código en Datos_User
            Datos_User.Setcodigo(codigoenviar);

            mostrar_Codigo_Profesor codProf = new mostrar_Codigo_Profesor();
            codProf.Show();

            if (resultado == ResultadoInsercionCodigo.Duplicado)
            {
                MessageBox.Show("Ese codigo ya fue generado. Vuelvelo a guardar.", "Codigo ya generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Codigo generado correctamente. Guardalo antes de cerrar la ventana.", "Codigo generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
        }

        private void CargarDatoscbLaboratorio()
        {
            try
            {
                cbLaboratorio.Items.Clear();
                foreach (OpcionCombo laboratorio in consultas.ObtenerLaboratorios())
                {
                    cbLaboratorio.Items.Add(laboratorio);
                }

                cbLaboratorio.SelectedIndex = -1;
                cbLaboratorio.Texts = "Laboratorio";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los nombres de los laboratorios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarDatoscbGrupo()
        {
            try
            {
                cbGrupo.Items.Clear();
                List<OpcionCombo> grupos = carreraProfesorId > 0
                    ? consultas.ObtenerGruposPorCarrera(carreraProfesorId)
                    : consultas.ObtenerGrupos();

                foreach (OpcionCombo grupo in grupos)
                {
                    cbGrupo.Items.Add(grupo);
                }

                cbGrupo.SelectedIndex = -1;
                cbGrupo.Texts = "Grupo";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los nombres de los grupos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarDatoscbMateria()
        {
            try
            {
                cbMateria.Items.Clear();
                List<OpcionCombo> materias = carreraProfesorId > 0
                    ? consultas.ObtenerMateriasPorCarrera(carreraProfesorId)
                    : consultas.ObtenerMaterias();

                foreach (OpcionCombo materia in materias)
                {
                    cbMateria.Items.Add(materia);
                }

                cbMateria.SelectedIndex = -1;
                cbMateria.Texts = "Materia";
             }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error al cargar los nombres de las materias: {ex.Message}","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void mostrarNombreProf()
        {
            try
            {
                string idUsuario = lblMatricula.Text;
                string nombre = consultas.MostrarNombreProfesor(idUsuario);
                txtNombreProfesor.Texts = nombre;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mostrar el nombre del profesor: {ex.Message}");
                MessageBox.Show($"Error al mostrar el nombre del profesor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
