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
    public partial class BorrarLabo : Form
    {
        public BorrarLabo()
        {
            InitializeComponent();
            cargarLaboratorio();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void rjButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
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
                cbLaboratorio.Items.Clear();

                // Agregar los nombres de las carreras al ComboBox
                foreach (string nombreLaboratorio in nombreLaboratorios)
                {
                    cbLaboratorio.Items.Add(nombreLaboratorio);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los nombres de las carreras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            Borrar borrar = new Borrar();
            string labo = cbLaboratorio.SelectedItem.ToString();
            borrar.borrarlabo(labo);
            cargarLaboratorio();

        }
    }
}
