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
    public partial class EliminarUser : Form
    {
        private Consultas consultas;
        public EliminarUser()
        {
            InitializeComponent();
            txtNombre.Enabled = false;
            txtApellidoMaterno.Enabled = false;
            txtApellidoPaterno.Enabled = false;
            txtMatricula.Enter += txtMatricula_Enter;
            txtMatricula.Leave += txtMatricula_Leave;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Consultas consultas = new Consultas();
            string matricula = txtMatricula.Texts?.Trim();

            if (string.IsNullOrWhiteSpace(matricula) || string.Equals(matricula, "Matricula", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Ingresa una matrícula válida para buscar al usuario.", "Matrícula requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = consultas.BuscarParaEliminar(matricula);

            if (resultado.Nombre != null)
            {
                txtNombre.Texts = resultado.Nombre;
                txtApellidoPaterno.Texts = resultado.ApellidoPaterno;
                txtApellidoMaterno.Texts = resultado.ApellidoMaterno;
            }
            else
            {
                MessageBox.Show("Usuario no encontrado.");
                txtNombre.Texts = string.Empty;
                txtApellidoPaterno.Texts = string.Empty;
                txtApellidoMaterno.Texts = string.Empty;
            }
        }

        private void txtApellidoPaterno__TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            string matricula = txtMatricula.Texts?.Trim();
            if (string.IsNullOrWhiteSpace(matricula) || string.Equals(matricula, "Matricula", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Ingresa una matrícula válida antes de eliminar.", "Matrícula requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            consultas = new Consultas();
            consultas.EliminarUsuario(matricula);
            txtNombre.Texts = string.Empty;
            txtApellidoPaterno.Texts = string.Empty;
            txtApellidoMaterno.Texts = string.Empty;
            txtMatricula.Texts = "Matricula";
        }

        private void txtMatricula_Enter(object sender, EventArgs e)
        {
            if (string.Equals(txtMatricula.Texts, "Matricula", StringComparison.OrdinalIgnoreCase))
            {
                txtMatricula.Texts = string.Empty;
            }
        }

        private void txtMatricula_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMatricula.Texts))
            {
                txtMatricula.Texts = "Matricula";
            }
        }
    }
}
