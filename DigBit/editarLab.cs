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
    public partial class editarLab : Form
    {
        public editarLab()
        {
            InitializeComponent();
            cargarLaboratorio();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void actualizarLaboratorio_Click(object sender, EventArgs e)
        {
            try
            {
                Consultas consultas = new Consultas();
                string nombreactual = rjLaboratorioActualizar.SelectedItem.ToString();
                string nombrenuevo = txtLaboratorio.Texts;
                consultas.ActualizarLabo(nombreactual, nombrenuevo);
                cargarLaboratorio();
                // Obtener el formulario actual
                Form currentForm = this;

                // Cerrar el formulario actual
                currentForm.Close();

                // Crear una nueva instancia del formulario
                Form newForm = new editarLab();

                // Mostrar la nueva instancia del formulario
                newForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el laboratorio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cargarLaboratorio()
        {
            try
            {
                // Crear una instancia de Consultas
                Consultas consultas = new Consultas();

                // Obtener los nombres de las carreras
                List<string> nombreLaboratorios = consultas.ObtenerNombresLaboratorio();

                // Limpiar el ComboBox antes de agregar nuevos datos
                rjLaboratorioActualizar.Items.Clear();

                // Agregar los nombres de las carreras al ComboBox
                foreach (string nombreLaboratorio in nombreLaboratorios)
                {
                    rjLaboratorioActualizar.Items.Add(nombreLaboratorio);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los nombres de las carreras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
