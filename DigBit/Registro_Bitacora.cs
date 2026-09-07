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
    public partial class Registro_Bitacora : Form
    {
        public Registro_Bitacora()
        {
            InitializeComponent();
            logoTeschi.Visible = false;
            string machineName = Environment.MachineName;
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
            string smatricula = Datos_User.getUser();
            if (!string.IsNullOrEmpty(smatricula))
            {
                txtNumeroControl.Texts = smatricula;
            }

            // Linea de llamada de los metodos pasados
            Consultas con = new Consultas();
            string nombreprofesor = con.MostrarNombreProfesor(txtNumeroControl.Texts);
            txtNombreAlumno.Texts = nombreprofesor;
            string codigoRecuperado = Datos_User.getcodigo();
            txtCodigoAcceso.Texts = codigoRecuperado;
            string laboratorio = con.ObtenerNombreLaboratorioPorCodigo(codigoRecuperado);
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
            Consultas con = new Consultas();
            bool guardado = false;
            if (chkBoxNinguno.Checked)
            {
                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, "Ninguno ", "Ninguno ", "Ninguno ", "Ninguno ", " Ninguno ", "");
            }
            //Problemas de red
            else if (chkBoxOtroRed.Checked)
            {
                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, "Otro", txtOtroProblemaRed.Texts, rjComboHardware.SelectedIndex.ToString(), rjComboHardware.SelectedItem.ToString(), rjComboSoftware.SelectedIndex.ToString(), rjComboSoftware.SelectedItem.ToString());

            } else if (chkBoxOtroSoftware.Checked && chkBoxOtroRed.Checked) {

                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, "Otro", txtOtroProblemaRed.Texts, rjComboHardware.SelectedIndex.ToString(), rjComboHardware.SelectedItem.ToString(), "Otro", txtOtroProblemaSoftware.Texts);

            } else if (chkBoxOtroRed.Checked && chkBoxOtroSoftware.Checked && chkBoxOtroHardware.Checked) {

                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, "Otro", txtOtroProblemaRed.Texts, "Otro", txtOtroProblemaHardware.Texts, "Otro", txtOtroProblemaSoftware.Texts);

            }

            //Problemas de hardware
            else if (chkBoxOtroHardware.Checked)
            {
                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, rjComboRed.SelectedIndex.ToString(), rjComboRed.SelectedItem.ToString(), "Otro", txtOtroProblemaHardware.Texts, rjComboSoftware.SelectedIndex.ToString(), rjComboSoftware.SelectedItem.ToString());

            }
            else if (chkBoxOtroHardware.Checked && chkBoxOtroRed.Checked)
            {

                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, "Otro", txtOtroProblemaRed.Texts, "Otro", txtOtroProblemaHardware.Texts, rjComboSoftware.SelectedIndex.ToString(), rjComboSoftware.SelectedItem.ToString());

            }
            else if (chkBoxOtroRed.Checked && chkBoxOtroSoftware.Checked && chkBoxOtroHardware.Checked)
            {

                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, "Otro", txtOtroProblemaRed.Texts, "Otro", txtOtroProblemaHardware.Texts, "Otro", txtOtroProblemaSoftware.Texts);

            }

            //Problemas de software
            else if (chkBoxOtroSoftware.Checked)
            {
                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, rjComboRed.SelectedIndex.ToString(), rjComboRed.SelectedItem.ToString(), rjComboHardware.SelectedIndex.ToString(), rjComboHardware.SelectedItem.ToString(), "Otro", txtOtroProblemaSoftware.Texts);

            }
            else if (chkBoxOtroSoftware.Checked && chkBoxOtroRed.Checked)
            {

                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, "Otro", txtOtroProblemaRed.Texts, rjComboHardware.SelectedIndex.ToString(), rjComboHardware.SelectedItem.ToString(), "Otro", txtOtroProblemaSoftware.Texts);

            }
            else if (chkBoxOtroSoftware.Checked && chkBoxOtroRed.Checked && chkBoxOtroHardware.Checked)
            {

                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, "Otro", txtOtroProblemaRed.Texts, "Otro", txtOtroProblemaHardware.Texts, "Otro", txtOtroProblemaSoftware.Texts);

            }
            else if (chkBoxEquipoPropioSi.Checked) {
                guardado = con.consultaFinal(txtCodigoAcceso.Texts, txtNumeroControl.Texts, txtNumeroMaquina.Texts, "EP", "Equipo Propio", "EP", "Equipo Propio", "EP", "Equipo Propio");
            }

            if (guardado)
            {
                this.Close();
            }

        }
    }
}
