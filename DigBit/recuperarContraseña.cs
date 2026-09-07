using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigBit
{
    public partial class recuperarContraseña : Form
    {
        public recuperarContraseña()
        {
            InitializeComponent();
        }

        // Se crea el siguiente Evento para el boton "btnCancelar" para que al momento de dar clic nos regrese a la pestaña de Login
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }


        private void recuperarContraseña_Load(object sender, EventArgs e)
        {
            //Colocamos el nombre del Label para que tengo un color de fondo transparente acorde a la imagen de fondo
            lblPasswd.Parent = imgRecuperarContraseña;
            lblPasswd.BackColor = Color.Transparent; 

        }


        //Evento Enter para que el texto "ingrese la matricula" que se encuentra dentro del TextBox se borre al momento de dar clic 
        private void txtMatricula_Enter(object sender, EventArgs e)
        {
            if (txtMatricula.Text == "Ingrese la matricula")
            {
                txtMatricula.Text = "";
                txtMatricula.ForeColor = Color.DarkGreen;
            }
        }

        //Evento para que cuando el texto dentro del TextBox se encuentre vacio se vuelva a mostrar el texto "Ingrese la matricula"
        private void txtMatricula_Leave(object sender, EventArgs e)
        {
            if (txtMatricula.Text == "")
            {
                txtMatricula.Text = "Ingrese la matricula";
                txtMatricula.ForeColor= Color.DarkGreen;  
            }
        }

        //Evento Enter para que el texto "ingrese la respuesta" que se encuentra dentro del TextBox se borre al momento de dar clic 
        private void txtRespuesta_Enter(object sender, EventArgs e)
        {
            if (txtRespuesta.Text == "Ingrese la respuesta")
            {
                txtRespuesta.Text = "";
                txtRespuesta.ForeColor= Color.DarkGreen;    
            }
        }

        //Evento para que cuando el texto dentro del TextBox se encuentre vacio se vuelva a mostrar el texto "Ingrese la respuesta"
        private void txtRespuesta_Leave(object sender, EventArgs e)
        {
            if (txtRespuesta.Text == "")
            {
                txtRespuesta.Text = "Ingrese la respuesta";
                txtRespuesta.ForeColor = Color.DarkGreen;   
            }
        }

    }
}
