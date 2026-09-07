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
    public partial class ingresar : Form
    {
        private Conexion mconexion;
        private InsercionDatos insercionDatos;
        public ingresar()
        {
            InitializeComponent();
            mconexion = new Conexion();
            insercionDatos = new InsercionDatos();
        }

        private void RealizarInsercion()
        {
            string laboratorio = rjnombreLaboNuevo.Texts;
            // Llama a la función InsertarUsuarioProfe con los valores de los TextBox
            insercionDatos.insertarLaboratorio(laboratorio);
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void rjCancelarLaboButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void rjIngresarLaboButon_Click(object sender, EventArgs e)
        {
            try { 
                if (string.IsNullOrWhiteSpace(rjnombreLaboNuevo.Texts)|| rjnombreLaboNuevo.Texts == "")
                {
                    MessageBox.Show("Favor de completar el campo");
                }
                RealizarInsercion();
                MessageBox.Show("Registro realizado con éxito","Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error al intentar registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
