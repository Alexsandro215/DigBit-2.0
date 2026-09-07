using DigBit.conexion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigBit
{
    public partial class IngresarNuevoGrupo : Form
    {
        public IngresarNuevoGrupo()
        {
            InitializeComponent();
            CargarDatoscbCarrera();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();

        } 
        private void CargarDatoscbCarrera()
        {
            try
            {
                // Crear una instancia de Consultas
                Consultas consultas = new Consultas();

                // Obtener los nombres de las carreras
                List<string> nombresCarreras = consultas.ObtenerNombresCarreras();

                // Limpiar el ComboBox antes de agregar nuevos datos
                cbCarrera.Items.Clear();

                // Agregar los nombres de las carreras al ComboBox
                foreach (string nombreCarrera in nombresCarreras)
                {
                    cbCarrera.Items.Add(nombreCarrera);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los nombres de las carreras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            InsercionDatos insertarGrupo = new InsercionDatos();
            insertarGrupo.insertarGrupos(txtGrupo.Texts);
            // Obtener el formulario actual
            Form currentForm = this;

            // Cerrar el formulario actual
            currentForm.Close();

            // Crear una nueva instancia del formulario
            Form newForm = new editarLab();

            // Mostrar la nueva instancia del formulario
            newForm.Show();

        }



    }
}
