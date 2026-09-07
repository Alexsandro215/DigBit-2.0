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
    public partial class Buscar_Codigo_Alumno : Form
    {
        private Registro_Bitacora regbit;
        private string smatricula;
        public Buscar_Codigo_Alumno()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            Consultas consul = new Consultas();
            smatricula = consul.idusuario(Datos_User.getUser());
            //Texts,smatricula
            bool codigoEncontrado = consul.buscarCodigoRegistro(txtIngresarCodigo.Texts,"9999");
            Console.WriteLine(smatricula);


            if (codigoEncontrado)
            {
                string codigoenviar = txtIngresarCodigo.Texts;
                Datos_User.Setcodigo(codigoenviar);
                this.Hide();
                //Bucle para verificar si ya existe el objeto
                if (regbit == null || regbit.IsDisposed)
                {
                    //Si no existe crea la instancia de la clase
                    regbit = new Registro_Bitacora();
                }
                //Muestra el formulario
                regbit.Show();

                //Si esta en segundo plano lo trae al frente
                regbit.BringToFront();
            }
        }
    }
}
