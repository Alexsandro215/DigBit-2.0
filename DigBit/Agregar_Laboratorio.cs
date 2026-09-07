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
    public partial class Agregar_Laboratorio : Form
    {
        public Agregar_Laboratorio()
        {
            InitializeComponent();
        }

        private void btnCancelarLaboratorio_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
