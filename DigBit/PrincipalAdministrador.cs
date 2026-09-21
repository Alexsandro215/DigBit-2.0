using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigBit.Infraestructura;

namespace DigBit
{
    public partial class PrincipalAdministrador : Form
    {

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
        private AdministrarHorarios adminHorarios;

        public PrincipalAdministrador()
        {
            InitializeComponent();
            Kiosco.Aplicar(this);
            if (AppContexto.Actual != null)
            {
                AppContexto.Actual.Registrar(this);
            }
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

        /// <summary>
        /// Descargar una bitacora es elegir su sesion, que es justo lo que hace
        /// "Ver bitacoras": una sola pantalla en vez de dos que hacian lo mismo.
        /// </summary>
        private void btnDescargarBitacora_Click(object sender, EventArgs e)
        {
            btnVerBitacoras_Click(sender, e);
        }

        private void btnVerBitacoras_Click(object sender, EventArgs e)
        {
            if (verBitacorasAdmin == null || verBitacorasAdmin.IsDisposed)
            {
                verBitacorasAdmin = new GestionBitacorasAdministrador(false);
            }

            verBitacorasAdmin.Show(this);
            verBitacorasAdmin.BringToFront();
        }

        private void btnModificarBitacoras_Click(object sender, EventArgs e)
        {
            if (modificarBitacorasAdmin == null || modificarBitacorasAdmin.IsDisposed)
            {
                modificarBitacorasAdmin = new GestionBitacorasAdministrador(true);
            }

            modificarBitacorasAdmin.Show(this);
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
            regAlum.Show(this);

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
            elimUser.Show(this);

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
            ePerfi.Show(this);

            //Si esta en segundo plano lo trae al frente
            ePerfi.BringToFront();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            // Salir de la sesion, no de la aplicacion (DigBit puede ser el shell).
            AppContexto.Actual.CerrarSesion();
        }

        private void btnIngresarProfesor_Click(object sender, EventArgs e)
        {
            if (regProf == null || regProf.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                regProf = new RegistroProfesores();
            }
            //Muestra el formulario
            regProf.Show(this);

            //Si esta en segundo plano lo trae al frente
            regProf.BringToFront();
        }

        private void btnEliminarProfesor_Click(object sender, EventArgs e)
        {
            EliminarUser eliminarUser = new EliminarUser();
            eliminarUser.Show(this);

        }

        private void btnEditarProfesor_Click(object sender, EventArgs e)
        {
            editar_Perfil ePerfil = new editar_Perfil("profesor");
            ePerfil.Show(this);
        }

        private void btnIngresarLaboratorio_Click(object sender, EventArgs e)
        {
            if (ingLabo == null || ingLabo.IsDisposed)
            {
                //Si no existe crea la instancia de la clase
                ingLabo = new ingresar();
            }
            //Muestra el formulario
            ingLabo.Show(this);

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
            borrarlabo.Show(this);

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
            editLab.Show(this);

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
            ingGrupo.Show(this);

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
            ingPregunta.Show(this);

            //Si esta en segundo plano lo trae al frente
            ingPregunta.BringToFront();
        }

        private void btnHorarios_Click(object sender, EventArgs e)
        {
            // Fase 6: clases con codigo fijo, horario por laboratorio y excepciones.
            if (adminHorarios == null || adminHorarios.IsDisposed)
            {
                adminHorarios = new AdministrarHorarios();
            }

            adminHorarios.Show(this);
            adminHorarios.BringToFront();
        }
    }
}
