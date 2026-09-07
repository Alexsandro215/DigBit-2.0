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
    public partial class PrincipalAdministrador : Form
    {
        private Bitacora_profesor bitProf;
        private GestionBitacorasAdministrador verBitacorasAdmin;
        private GestionBitacorasAdministrador modificarBitacorasAdmin;
        private RegistroAlumnos regAlum;
        private EliminarUser elimUser;
        private editar_Perfil ePerfi;
        private RegistroProfesores regProf;
        private ingresar ingLabo;
        private BorrarLabo borrarlabo;
        private editarLab editLab;
        private IngresarNuevoGrupo ingGrupo;
        private IngresarPregunta ingPregunta;

        public PrincipalAdministrador()
        {
            InitializeComponent();
            txtAlumnos.Enabled = false;
            txtBitacoras.Enabled = false;
            txtLaboratorio.Enabled = false;
            txtProfesores.Enabled = false;
            txtAdmin.ForeColor = System.Drawing.Color.Black;
        }

        private void btnEditarPassword_Click(object sender, EventArgs e)
        {
            // Mostrar un cuadro de diálogo de entrada para que el administrador ingrese la nueva contraseña
            string nuevaContraseña = Microsoft.VisualBasic.Interaction.InputBox("Ingrese la nueva contraseña:", "Editar Contraseña", "");

            // Verificar si se ingresó una contraseña
            if (!string.IsNullOrEmpty(nuevaContraseña))
            {
                // Mostrar un mensaje de confirmación con la nueva contraseña ingresada
                MessageBox.Show($"La nueva contraseña es: {nuevaContraseña}", "Contraseña Actualizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Mostrar un mensaje si no se ingresó ninguna contraseña
                MessageBox.Show("No se ingresó ninguna contraseña.", "Contraseña No Cambiada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDescargarBitacora_Click(object sender, EventArgs e)
        {
            if (bitProf == null || bitProf.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                bitProf = new Bitacora_profesor();
            }
            //Muestra el formulario
            bitProf.Show();

            //Si esta en segundo plano lo trae al frente
            bitProf.BringToFront();
        }

        private void btnVerBitacoras_Click(object sender, EventArgs e)
        {
            if (verBitacorasAdmin == null || verBitacorasAdmin.IsDisposed)
            {
                verBitacorasAdmin = new GestionBitacorasAdministrador(false);
            }

            verBitacorasAdmin.Show();
            verBitacorasAdmin.BringToFront();
        }

        private void btnModificarBitacoras_Click(object sender, EventArgs e)
        {
            if (modificarBitacorasAdmin == null || modificarBitacorasAdmin.IsDisposed)
            {
                modificarBitacorasAdmin = new GestionBitacorasAdministrador(true);
            }

            modificarBitacorasAdmin.Show();
            modificarBitacorasAdmin.BringToFront();
        }

        private void btnIngresarAlumno_Click(object sender, EventArgs e)
        {
            if (regAlum == null || regAlum.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                regAlum = new RegistroAlumnos();
            }
            //Muestra el formulario
            regAlum.Show();

            //Si esta en segundo plano lo trae al frente
            regAlum.BringToFront();
        }

        private void btnBorrarAlumno_Click(object sender, EventArgs e)
        {
            if (elimUser == null || elimUser.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                elimUser = new EliminarUser();
            }
            //Muestra el formulario
            elimUser.Show();

            //Si esta en segundo plano lo trae al frente
            elimUser.BringToFront();
        }

        private void btnEditarAlumno_Click(object sender, EventArgs e)
        {
            if (ePerfi == null || ePerfi.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                ePerfi = new editar_Perfil("alumno");
            }
            //Muestra el formulario
            ePerfi.Show();

            //Si esta en segundo plano lo trae al frente
            ePerfi.BringToFront();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnIngresarProfesor_Click(object sender, EventArgs e)
        {
            if (regProf == null || regProf.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                regProf = new RegistroProfesores();
            }
            //Muestra el formulario
            regProf.Show();

            //Si esta en segundo plano lo trae al frente
            regProf.BringToFront();
        }

        private void btnEliminarProfesor_Click(object sender, EventArgs e)
        {
            EliminarUser eliminarUser = new EliminarUser();
            eliminarUser.Show();

        }

        private void btnEditarProfesor_Click(object sender, EventArgs e)
        {
            editar_Perfil ePerfil = new editar_Perfil("profesor");
            ePerfil.Show();
        }

        private void btnIngresarLaboratorio_Click(object sender, EventArgs e)
        {
            if (ingLabo == null || ingLabo.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                ingLabo = new ingresar();
            }
            //Muestra el formulario
            ingLabo.Show();

            //Si esta en segundo plano lo trae al frente
            ingLabo.BringToFront();
        }

        private void btnBorrarLaboratorio_Click(object sender, EventArgs e)
        {
            if (borrarlabo == null || borrarlabo.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                borrarlabo = new BorrarLabo();
            }
            //Muestra el formulario
            borrarlabo.Show();

            //Si esta en segundo plano lo trae al frente
            borrarlabo.BringToFront();
        }

        private void btnEditarLaboratorio_Click(object sender, EventArgs e)
        {
            if (editLab == null || editLab.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                editLab = new editarLab();
            }
            //Muestra el formulario
            editLab.Show();

            //Si esta en segundo plano lo trae al frente
            editLab.BringToFront();
        }

        private void btnIngresarGrupo_Click(object sender, EventArgs e)
        {
            if (ingGrupo == null || ingGrupo.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                ingGrupo = new IngresarNuevoGrupo();
            }
            //Muestra el formulario
            ingGrupo.Show();

            //Si esta en segundo plano lo trae al frente
            ingGrupo.BringToFront();
        }

        private void btnNuevaPreguntaSeguridad_Click(object sender, EventArgs e)
        {
            if (ingPregunta == null || ingPregunta.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                ingPregunta = new IngresarPregunta();
            }
            //Muestra el formulario
            ingPregunta.Show();

            //Si esta en segundo plano lo trae al frente
            ingPregunta.BringToFront();
        }
    }
}
