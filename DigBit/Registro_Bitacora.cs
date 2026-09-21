using DigBit.conexion;
using DigBit.Infraestructura;
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
    public partial class Registro_Bitacora : Form
    {
        // Sesion (idcodigos_accesos) con la que entro el alumno. La fija
        // Buscar_Codigo_Alumno en Datos_User.VentanaActual antes de abrir esto.
        private static int IdSesion
        {
            get { return Datos_User.VentanaActual != null ? Datos_User.VentanaActual.IdCodigo : 0; }
        }

        public Registro_Bitacora()
        {
            InitializeComponent();
            logoTeschi.Visible = false;
            // El numero que se apunta en la bitacora. Sin configurar es el nombre
            // de Windows, como siempre; con -NumeroMaquina es el numero pegado en
            // la maquina, que es lo que le sirve a quien vaya a buscarla.
            string machineName = LaboratorioEquipo.NumeroMaquina;
            txtNombreAlumno.Enabled = false;
            txtNumeroControl.Enabled = false;
            txtLaboratorio.Enabled = false;
            txtCodigoAcceso.Enabled = false;
            txtNumeroMaquina.Texts = (machineName);
            txtNumeroMaquina.Enabled = false;
            txtObservaciones.Enabled = false;
            txtEquipoPropio.Enabled = false;
            txtRed.Enabled = false;
            txtHardware.Enabled = false;
            txtSoftware.Enabled = false;
            txtNinguno.Enabled = false;

            // Manejar el evento CheckedChanged del CheckBox chkBoxEquipoPropioSi
            chkBoxEquipoPropioSi.CheckedChanged += chkBoxEquipoPropioSi_CheckedChanged;
            chkBoxOtroHardware.CheckedChanged += chkBoxOtroHardware_CheckedChanged;
            chkBoxOtroSoftware.CheckedChanged += chkBoxOtroSoftware_CheckedChanged;
            chkBoxOtroRed.CheckedChanged += chkBoxOtroRed_CheckedChanged;

            // Asocia el método Ninguno al evento CheckedChanged de chkBoxNinguno
            chkBoxNinguno.CheckedChanged += chkNinguno;

            // Inicializar los CheckBox basados en el estado de chkBoxEquipoPropioSi
            UpdateCheckBoxStates();
            PermitirOtros();
            PermitirOtros2();
            PermitirOtros3();

            PrepararPista(txtOtroProblemaRed);
            PrepararPista(txtOtroProblemaHardware);
            PrepararPista(txtOtroProblemaSoftware);

            string smatricula = Datos_User.getUser();
            if (!string.IsNullOrEmpty(smatricula))
            {
                txtNumeroControl.Texts = smatricula;
            }

            // Linea de llamada de los metodos pasados
            Consultas con = new Consultas();
            string nombreprofesor;
            if (SinConexion.Activo)
            {
                // Fase 4: sin servidor, el nombre sale de la copia local.
                AlumnoCache alumno = SinConexion.Cache.BuscarAlumno(txtNumeroControl.Texts);
                nombreprofesor = alumno != null ? alumno.Nombre : txtNumeroControl.Texts;
            }
            else
            {
                nombreprofesor = con.MostrarNombreProfesor(txtNumeroControl.Texts);
            }
            txtNombreAlumno.Texts = nombreprofesor;
            string codigoRecuperado = Datos_User.getcodigo();
            txtCodigoAcceso.Texts = codigoRecuperado;
            string laboratorio = Datos_User.VentanaActual != null ? Datos_User.VentanaActual.Laboratorio : con.ObtenerNombreLaboratorioPorCodigo(codigoRecuperado);
            Console.WriteLine(laboratorio);
            txtLaboratorio.Texts = laboratorio;
        }
        private void chkBoxOtroSoftware_CheckedChanged(object sender, EventArgs e) {
            PermitirOtros2();
        }
        private void chkBoxEquipoPropioSi_CheckedChanged(object sender, EventArgs e)
        {
            // Actualizar el estado de los CheckBox cuando cambie el estado de chkBoxEquipoPropioSi
            UpdateCheckBoxStates();
        }

        private void chkBoxOtroRed_CheckedChanged(object sender, EventArgs e) { 
            PermitirOtros3();
        }

        private void chkBoxOtroHardware_CheckedChanged(object sender, EventArgs e)
        {
            // Actualizar el estado de los CheckBox cuando cambie el estado de chkBoxEquipoPropioSi
            PermitirOtros();
        }
        private void chkNinguno(object sender, EventArgs e) {
            Ninguno();
        }
        private void UpdateCheckBoxStates()
        {
            bool isChecked = chkBoxEquipoPropioSi.Checked;
            chkBoxNinguno.Visible = !isChecked;
            chkBoxOtroHardware.Visible = !isChecked;
            chkBoxOtroSoftware.Visible = !isChecked;
            txtOtroProblemaHardware.Visible = !isChecked;
            txtOtroProblemaSoftware.Visible = !isChecked;
            label13.Visible = !isChecked;
            label19.Visible = !isChecked;
            label30.Visible = !isChecked;
            txtHardware.Visible = !isChecked;
            txtSoftware.Visible = !isChecked;
            txtNinguno.Visible = !isChecked;
            logoTeschi.Visible = isChecked;
            rjComboHardware.Visible =! isChecked;
            rjComboSoftware.Visible =! isChecked;
        }

        private void PermitirOtros() { 
        bool isChecked = chkBoxOtroHardware.Checked;
            txtOtroProblemaHardware.Enabled = isChecked;
        }

        private void PermitirOtros2() {
            bool isChecked2 = chkBoxOtroSoftware.Checked;
            txtOtroProblemaSoftware.Enabled = isChecked2;
        }

        private void PermitirOtros3()
        {
            bool isCheked3 = chkBoxOtroRed.Checked;
            txtOtroProblemaRed.Enabled = isCheked3;
        }

        private void Ninguno()
        {
            bool ischecked4 = chkBoxNinguno.Checked;
            rjComboHardware.Enabled = !ischecked4;
            rjComboSoftware.Enabled = !ischecked4;
            rjComboRed.Enabled = !ischecked4;
            chkBoxOtroHardware.Enabled = !ischecked4;
            chkBoxOtroRed.Enabled = !ischecked4;
            chkBoxOtroSoftware.Enabled = !ischecked4;
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Registro_Bitacora_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void chkBoxOtroRed_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!HayAlgoQueReportar())
            {
                MessageBox.Show(
                    "Elige una opcion en alguna de las listas, o marca \"Ninguno\" o \"Equipo propio\"."
                        + Environment.NewLine + Environment.NewLine
                        + "Usa \"Otro\" solo si tu problema no esta en la lista.",
                    "Falta informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Consultas con = new Consultas();
            bool equipoPropio = chkBoxEquipoPropioSi.Checked;
            bool guardado;

            if (equipoPropio)
            {
                // Trajo su computadora: no hay nada que reportar de esta maquina.
                guardado = con.consultaFinalSesion(IdSesion, txtNumeroControl.Texts, txtNumeroMaquina.Texts,
                    "EP", "Equipo Propio", "EP", "Equipo Propio", "EP", "Equipo Propio");
            }
            else if (chkBoxNinguno.Checked)
            {
                guardado = con.consultaFinalSesion(IdSesion, txtNumeroControl.Texts, txtNumeroMaquina.Texts,
                    "Ninguno", "Ninguno", "Ninguno", "Ninguno", "Ninguno", "Ninguno");
            }
            else
            {
                // Las tres areas son independientes. Antes esto era una cadena de
                // else-if con seis ramas inalcanzables: reportar red y hardware a la
                // vez guardaba solo la red, y podia tronar al leer una lista que el
                // alumno nunca desplego.
                string fallaRed, comentarioRed;
                string fallaHardware, comentarioHardware;
                string fallaSoftware, comentarioSoftware;

                LeerArea(chkBoxOtroRed, txtOtroProblemaRed, rjComboRed, out fallaRed, out comentarioRed);
                LeerArea(chkBoxOtroHardware, txtOtroProblemaHardware, rjComboHardware, out fallaHardware, out comentarioHardware);
                LeerArea(chkBoxOtroSoftware, txtOtroProblemaSoftware, rjComboSoftware, out fallaSoftware, out comentarioSoftware);

                guardado = con.consultaFinalSesion(IdSesion, txtNumeroControl.Texts, txtNumeroMaquina.Texts,
                    fallaRed, comentarioRed, fallaHardware, comentarioHardware, fallaSoftware, comentarioSoftware);
            }

            if (!guardado)
            {
                return;
            }

            string codigo = txtCodigoAcceso.Texts;
            string usuario = Datos_User.getUser();

            if (equipoPropio)
            {
                TerminarConEquipoPropio(usuario, codigo);
                return;
            }

            this.Close();

            // Bitacora guardada: se libera el equipo hasta la hora de salida del
            // codigo (fase 3). La ventana la dejo Buscar_Codigo_Alumno; si por lo
            // que sea no esta, se vuelve a pedir al servidor.
            VentanaCodigo ventana = Datos_User.VentanaActual;
            if (ventana == null)
            {
                try
                {
                    ventana = con.ObtenerVentanaCodigo(codigo);
                }
                catch (Exception ex)
                {
                    Log.Error("Recuperar la ventana del codigo al guardar la bitacora", ex);
                }
            }

            if (ventana != null)
            {
                SesionEquipo.Liberar(ventana, usuario, codigo);
            }
            else
            {
                Log.Aviso("Bitacora guardada pero sin ventana del codigo '" + codigo + "': no se libera el equipo automaticamente.");
            }
        }

        /// <summary>
        /// Lo que el alumno reporto en un area (red, hardware o software). Con
        /// "Otro" marcado vale lo que escribio; si no, lo que eligio en la lista, y
        /// "Ninguno" cuando no eligio nada, que es como el resto de la aplicacion
        /// entiende "sin novedad".
        /// </summary>
        /// <summary>
        /// True si el alumno ya dijo algo: eligio de alguna de las tres listas,
        /// marco "Ninguno" o "Equipo propio", o marco alguna casilla de "Otro".
        ///
        /// Antes esta comprobacion solo miraba las CASILLAS. Elegir "Con cable de
        /// red sin internet" en la lista no contaba, asi que la aplicacion le
        /// exigia al alumno marcar ademas "Otro" y escribir a mano justo lo que
        /// acababa de elegir de la lista.
        /// </summary>
        private bool HayAlgoQueReportar()
        {
            if (chkBoxNinguno.Checked || chkBoxEquipoPropioSi.Checked)
            {
                return true;
            }

            if (chkBoxOtroRed.Checked || chkBoxOtroHardware.Checked || chkBoxOtroSoftware.Checked)
            {
                return true;
            }

            return rjComboRed.SelectedItem != null
                || rjComboHardware.SelectedItem != null
                || rjComboSoftware.SelectedItem != null;
        }

        /// <summary>
        /// El texto con el que vienen rellenas las tres cajas de "Otro problema"
        /// en el disenador, a modo de pista. Se comporta como marcador de
        /// posicion: se quita al escribir y vuelve si la caja queda vacia.
        /// </summary>
        private const string PistaOtroProblema = "Otro problema";

        /// <summary>
        /// Hace que el texto de pista se comporte como tal: desaparece al entrar
        /// en la caja y vuelve al salir si no se escribio nada. Antes se quedaba
        /// fijo y acababa guardado en la bitacora como si fuera el reporte del
        /// alumno, o pegado detras de lo que si escribio.
        /// </summary>
        private void PrepararPista(RJControls.RJTextBox caja)
        {
            if (caja == null)
            {
                return;
            }

            caja.Texts = PistaOtroProblema;
            caja.ForeColor = Color.Gray;

            caja.Enter += (s, e) =>
            {
                if (string.Equals((caja.Texts ?? "").Trim(), PistaOtroProblema, StringComparison.OrdinalIgnoreCase))
                {
                    caja.Texts = "";
                    caja.ForeColor = Color.Black;
                }
            };

            caja.Leave += (s, e) =>
            {
                if ((caja.Texts ?? "").Trim().Length == 0)
                {
                    caja.Texts = PistaOtroProblema;
                    caja.ForeColor = Color.Gray;
                }
            };
        }

        private static void LeerArea(CheckBox otro, RJControls.RJTextBox texto, RJControls.RJComboBox lista,
            out string falla, out string comentario)
        {
            if (otro.Checked)
            {
                falla = "Otro";
                comentario = (texto.Texts ?? "").Trim();

                // Las tres cajas vienen rellenas con "Otro problema" a modo de
                // pista, y nadie la borraba: el alumno que no escribia nada
                // guardaba literalmente esa pista como si fuera su reporte.
                if (comentario.Equals(PistaOtroProblema, StringComparison.OrdinalIgnoreCase))
                {
                    comentario = "";
                }

                // "Otro" sin escribir nada no le dice nada a quien lea la bitacora.
                if (comentario.Length == 0)
                {
                    comentario = "Otro (sin detalle)";
                }

                return;
            }

            // El texto de la lista, no su indice: el numero no le dice nada a quien
            // lee la bitacora, y ademas hacia que elegir "Ninguno" contara como
            // falla y pintara de naranja una bitacora sin novedad.
            object elegido = lista.SelectedItem;
            falla = elegido != null ? elegido.ToString().Trim() : "Ninguno";
            if (falla.Length == 0)
            {
                falla = "Ninguno";
            }

            comentario = "";
        }

        /// <summary>
        /// El alumno registro con su propia computadora: el equipo del laboratorio
        /// NO se libera, porque no lo va a usar. En vez del escritorio se le ofrece
        /// apagarlo o dejarlo en el login para el siguiente.
        /// </summary>
        private void TerminarConEquipoPropio(string usuario, string codigo)
        {
            Log.Info("Bitacora de '" + usuario + "' guardada con equipo propio; el equipo no se libera.");

            this.Hide();
            RegistroExitoso.Opcion opcion = RegistroExitoso.Mostrar(
                txtNombreAlumno.Texts, usuario, txtLaboratorio.Texts, codigo);
            this.Close();

            if (opcion == RegistroExitoso.Opcion.Apagar)
            {
                if (SesionEquipo.ApagarEquipo())
                {
                    // Windows se esta apagando; no hay nada mas que hacer.
                    return;
                }

                if (!Kiosco.Activo)
                {
                    MessageBox.Show("Modo escritorio: aqui se apagaria el equipo. Se vuelve al inicio.",
                        "DigBit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if (AppContexto.Actual != null)
            {
                AppContexto.Actual.CerrarSesion();
            }
        }
    }
}
