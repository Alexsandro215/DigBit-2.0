using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DigBit.Instalador
{
    internal sealed class FormInstalador : Form
    {
        private readonly RadioButton rbServidor = new RadioButton();
        private readonly RadioButton rbMaquina = new RadioButton();
        private readonly RadioButton rbAdm = new RadioButton();
        private readonly RadioButton rbMaestro = new RadioButton();

        private readonly TextBox txtServidor = new TextBox();
        private readonly TextBox txtBase = new TextBox();
        private readonly TextBox txtUsuarioBd = new TextBox();
        private readonly TextBox txtClaveBd = new TextBox();

        private readonly ComboBox cboLaboratorio = new ComboBox();
        private readonly Button btnCargarLabs = new Button();
        private readonly TextBox txtNumeroMaquina = new TextBox();
        private readonly TextBox txtCuenta = new TextBox();
        private readonly TextBox txtClaveCuenta = new TextBox();
        private readonly TextBox txtCarpetaDatos = new TextBox();
        private readonly CheckBox chkMemoria = new CheckBox();
        private readonly Label lblMemoria = new Label();

        private readonly Button btnInstalar = new Button();
        private readonly Button btnDiagnostico = new Button();
        private readonly TextBox txtSalida = new TextBox();
        private readonly Label lblEstado = new Label();

        private readonly TextBox txtNuevaClaveEquipo = new TextBox();
        private readonly TextBox txtNuevaClaveAdmin = new TextBox();

        private readonly List<Control> soloMaquina = new List<Control>();
        private readonly List<Control> soloServidor = new List<Control>();
        private readonly string raiz;

        public FormInstalador()
        {
            Text = "Instalador de DigBit";
            ClientSize = new Size(760, 720);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;
            Font = new Font("Century Gothic", 9F);

            raiz = Guiones.RaizPaquete();

            int y = 16;
            Titulo("Que equipo estas preparando", ref y);

            rbServidor.SetBounds(32, y, 700, 22);
            rbServidor.Text = "Servidor de base de datos  -  PRIMERO, y una sola vez en todo el laboratorio";
            rbServidor.CheckedChanged += (s, e) => AjustarModo();
            Controls.Add(rbServidor); y += 26;

            rbMaquina.SetBounds(32, y, 700, 22);
            rbMaquina.Text = "Maquina de laboratorio  -  pantalla bloqueada, la usa el alumno";
            rbMaquina.Checked = true;
            rbMaquina.CheckedChanged += (s, e) => AjustarModo();
            Controls.Add(rbMaquina); y += 26;

            rbAdm.SetBounds(32, y, 700, 22);
            rbAdm.Text = "Administrador  -  sin bloquear, gestiona laboratorios y horarios";
            rbAdm.CheckedChanged += (s, e) => AjustarModo();
            Controls.Add(rbAdm); y += 26;

            rbMaestro.SetBounds(32, y, 700, 22);
            rbMaestro.Text = "Profesor  -  sin bloquear, ve sus clases y bitacoras";
            rbMaestro.CheckedChanged += (s, e) => AjustarModo();
            Controls.Add(rbMaestro); y += 34;

            Titulo("Base de datos", ref y);
            Campo("Servidor (IP o nombre)", txtServidor, ref y, "127.0.0.1");
            Campo("Base", txtBase, ref y, "teschi_otru");
            Campo("Usuario", txtUsuarioBd, ref y, "digbit_equipo");
            Campo("Contrasena", txtClaveBd, ref y, "");
            txtClaveBd.UseSystemPasswordChar = true;
            y += 8;

            Titulo("Contrasenas que se van a CREAR (solo al preparar el servidor)", ref y);
            Campo("Para los equipos (digbit_equipo)", txtNuevaClaveEquipo, ref y, "");
            txtNuevaClaveEquipo.UseSystemPasswordChar = true;
            soloServidor.Add(txtNuevaClaveEquipo);
            Campo("Para el administrador (digbit_admin)", txtNuevaClaveAdmin, ref y, "");
            txtNuevaClaveAdmin.UseSystemPasswordChar = true;
            soloServidor.Add(txtNuevaClaveAdmin);
            y += 8;

            Titulo("Este equipo", ref y);

            Etiqueta("Laboratorio", y);
            cboLaboratorio.SetBounds(240, y, 380, 24);
            cboLaboratorio.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(cboLaboratorio);
            btnCargarLabs.SetBounds(628, y - 1, 104, 26);
            btnCargarLabs.Text = "Cargar";
            btnCargarLabs.FlatStyle = FlatStyle.Flat;
            btnCargarLabs.Click += (s, e) => CargarLaboratorios();
            Controls.Add(btnCargarLabs);
            soloMaquina.Add(cboLaboratorio); soloMaquina.Add(btnCargarLabs);
            y += 32;

            Campo("Numero de maquina", txtNumeroMaquina, ref y, "", true);
            Campo("Cuenta del laboratorio", txtCuenta, ref y, "laboratorio", true);
            Campo("Contrasena de esa cuenta", txtClaveCuenta, ref y, "", true);
            txtClaveCuenta.UseSystemPasswordChar = true;
            Campo("Carpeta de datos (Deep Freeze)", txtCarpetaDatos, ref y, "", true);

            chkMemoria.SetBounds(240, y, 492, 22);
            chkMemoria.Text = "Autorizar la memoria USB conectada como llave de emergencia";
            chkMemoria.CheckedChanged += (s, e) => MostrarMemoria();
            Controls.Add(chkMemoria);
            soloMaquina.Add(chkMemoria);
            y += 24;

            lblMemoria.SetBounds(262, y, 470, 20);
            lblMemoria.ForeColor = Color.DimGray;
            Controls.Add(lblMemoria);
            soloMaquina.Add(lblMemoria);
            y += 30;

            btnInstalar.SetBounds(32, y, 200, 40);
            btnInstalar.Text = "Instalar";
            btnInstalar.FlatStyle = FlatStyle.Flat;
            btnInstalar.BackColor = Color.FromArgb(0, 122, 94);
            btnInstalar.ForeColor = Color.White;
            btnInstalar.FlatAppearance.BorderSize = 0;
            btnInstalar.Font = new Font("Century Gothic", 10F, FontStyle.Bold);
            btnInstalar.Click += (s, e) => Instalar();
            Controls.Add(btnInstalar);

            btnDiagnostico.SetBounds(244, y, 180, 40);
            btnDiagnostico.Text = "Solo diagnosticar";
            btnDiagnostico.FlatStyle = FlatStyle.Flat;
            btnDiagnostico.Click += (s, e) => Correr(Guiones.Guion(raiz, "diagnostico_equipo.ps1"), "", "Diagnostico");
            Controls.Add(btnDiagnostico);

            lblEstado.SetBounds(436, y + 12, 296, 20);
            Controls.Add(lblEstado);
            y += 50;

            txtSalida.SetBounds(32, y, 700, ClientSize.Height - y - 20);
            txtSalida.Multiline = true;
            txtSalida.ReadOnly = true;
            txtSalida.ScrollBars = ScrollBars.Vertical;
            txtSalida.Font = new Font("Consolas", 8.5F);
            txtSalida.BackColor = Color.FromArgb(30, 30, 30);
            txtSalida.ForeColor = Color.Gainsboro;
            Controls.Add(txtSalida);

            AjustarModo();
            MostrarMemoria();

            if (raiz == null)
            {
                txtSalida.Text = @"No encuentro deploy\\configurar_equipo.ps1." + Environment.NewLine +
                    "Este instalador tiene que estar dentro del paquete, junto a las carpetas deploy y db.";
                btnInstalar.Enabled = false;
                btnDiagnostico.Enabled = false;
            }
            else
            {
                txtSalida.Text = "Paquete: " + raiz + Environment.NewLine +
                    "Rellena los datos y pulsa Instalar. Lo que se ejecuta por debajo son los guiones de la carpeta deploy.";
            }
        }

        private void Titulo(string texto, ref int y)
        {
            Label l = new Label();
            l.SetBounds(20, y, 700, 22);
            l.Text = texto;
            l.Font = new Font("Century Gothic", 10F, FontStyle.Bold);
            l.ForeColor = Color.FromArgb(0, 122, 94);
            Controls.Add(l);
            y += 26;
        }

        private void Etiqueta(string texto, int y)
        {
            Label l = new Label();
            l.SetBounds(32, y + 3, 200, 20);
            l.Text = texto;
            Controls.Add(l);
        }

        private void Campo(string etiqueta, TextBox caja, ref int y, string valor, bool esDeMaquina = false)
        {
            Label l = new Label();
            l.SetBounds(32, y + 3, 200, 20);
            l.Text = etiqueta;
            Controls.Add(l);
            caja.SetBounds(240, y, 492, 24);
            caja.Text = valor;
            Controls.Add(caja);
            if (esDeMaquina) { soloMaquina.Add(caja); soloMaquina.Add(l); }
            y += 30;
        }

        private bool EsMaquina { get { return rbMaquina.Checked; } }
        private bool EsServidor { get { return rbServidor.Checked; } }

        private void AjustarModo()
        {
            foreach (Control c in soloMaquina)
            {
                c.Enabled = EsMaquina;
            }

            foreach (Control c in soloServidor)
            {
                c.Enabled = EsServidor;
            }

            // Al preparar el servidor no hay a donde conectarse: la base se crea
            // aqui mismo. Pedir usuario y contrasena en ese momento es lo que
            // deja a cualquiera mirando la ventana sin saber que poner.
            txtServidor.Enabled = !EsServidor;
            txtBase.Enabled = !EsServidor;
            txtUsuarioBd.Enabled = !EsServidor;
            txtClaveBd.Enabled = !EsServidor;
            btnInstalar.Text = EsServidor ? "Preparar la base" : "Instalar";

            // El equipo del administrador usa el usuario de MySQL que puede
            // administrar; los otros dos, el restringido.
            if (rbAdm.Checked && txtUsuarioBd.Text == "digbit_equipo") { txtUsuarioBd.Text = "digbit_admin"; }
            if (!rbAdm.Checked && txtUsuarioBd.Text == "digbit_admin") { txtUsuarioBd.Text = "digbit_equipo"; }
        }

        private void MostrarMemoria()
        {
            if (!chkMemoria.Checked) { lblMemoria.Text = ""; return; }

            List<string> memorias = Guiones.MemoriasUsb();
            if (memorias.Count == 0) { lblMemoria.Text = "No veo ninguna memoria USB conectada."; }
            else if (memorias.Count == 1) { lblMemoria.Text = "Se autorizara: " + memorias[0]; }
            else { lblMemoria.Text = memorias.Count + " memorias conectadas: deja solo la que quieras autorizar."; }
        }

        private string CadenaConexion()
        {
            return "Database=" + txtBase.Text.Trim() +
                   ";Server=" + txtServidor.Text.Trim() +
                   ";Port=3306;User Id=" + txtUsuarioBd.Text.Trim() +
                   ";Password=" + txtClaveBd.Text;
        }

        /// <summary>
        /// La lista de laboratorios sale de la base, no se teclea. Asi no hay
        /// forma de equivocarse con un acento o un espacio, que era el fallo
        /// que dejaba un equipo rechazando todos los codigos.
        /// </summary>
        private void CargarLaboratorios()
        {
            cboLaboratorio.Items.Clear();
            lblEstado.Text = "Consultando...";
            Application.DoEvents();

            string consulta =
                "$ErrorActionPreference='Stop';" +
                "Add-Type -Path " + Guiones.Comillas(Path.Combine(raiz, "DigBit", "bin", "Release", "MySql.Data.dll")) + ";" +
                "$c = New-Object MySql.Data.MySqlClient.MySqlConnection(" + Guiones.Comillas(CadenaConexion() + ";Connection Timeout=8") + ");" +
                "$c.Open(); $m = $c.CreateCommand();" +
                "$m.CommandText = 'SELECT nombre_laboratorio FROM laboratorios ORDER BY idlaboratorios';" +
                "$r = $m.ExecuteReader(); while ($r.Read()) { $r.GetString(0) }; $r.Close(); $c.Close()";

            string salida, error;
            int codigo = Ejecutar("powershell.exe", "-NoProfile -ExecutionPolicy Bypass -Command \"" + consulta.Replace("\"", "\\\"") + "\"", out salida, out error);

            if (codigo != 0)
            {
                lblEstado.Text = "No se pudo consultar la base.";
                txtSalida.Text = (salida + Environment.NewLine + error).Trim();
                return;
            }

            foreach (string linea in salida.Split('\n'))
            {
                string nombre = linea.Trim();
                if (nombre.Length > 0) { cboLaboratorio.Items.Add(nombre); }
            }

            if (cboLaboratorio.Items.Count > 0) { cboLaboratorio.SelectedIndex = 0; }
            lblEstado.Text = cboLaboratorio.Items.Count + " laboratorio(s)";
            txtSalida.Text = cboLaboratorio.Items.Count == 0
                ? "La base responde, pero no hay ningun laboratorio dado de alta. Creálos primero desde el equipo del administrador."
                : "Laboratorios leidos de la base. Elige el de este equipo.";
        }

        private void Instalar()
        {
            string faltan = Validar();
            if (faltan != null)
            {
                MessageBox.Show(faltan, "Faltan datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (EsServidor)
            {
                PrepararServidor();
                return;
            }

            StringBuilder args = new StringBuilder();
            if (EsMaquina)
            {
                args.Append(" -Cuenta ").Append(Guiones.Comillas(txtCuenta.Text.Trim()));
                args.Append(" -Contrasena ").Append(Guiones.Comillas(txtClaveCuenta.Text));
                args.Append(" -Laboratorio ").Append(Guiones.Comillas(cboLaboratorio.Text));
                if (txtNumeroMaquina.Text.Trim().Length > 0) { args.Append(" -NumeroMaquina ").Append(Guiones.Comillas(txtNumeroMaquina.Text.Trim())); }
                if (txtCarpetaDatos.Text.Trim().Length > 0) { args.Append(" -CarpetaDatos ").Append(Guiones.Comillas(txtCarpetaDatos.Text.Trim())); }
                if (chkMemoria.Checked) { args.Append(" -AutorizarMemoria"); }
            }
            else
            {
                args.Append(" -Modo ").Append(rbAdm.Checked ? "adm" : "maestro");
            }

            // El guion sabe deducir su carpeta de origen, pero el instalador ya conoce
            // la raiz del paquete: pasarla evita depender de $PSScriptRoot, que llega
            // vacio dentro de los valores por defecto del param cuando se lanza -File.
            args.Append(" -Origen ").Append(Guiones.Comillas(raiz));
            args.Append(" -CadenaConexion ").Append(Guiones.Comillas(CadenaConexion()));

            if (MessageBox.Show(
                    "Se va a preparar este equipo. Puede tardar un minuto." + Environment.NewLine + Environment.NewLine +
                    "Si tiene Deep Freeze, tiene que estar DESCONGELADO o todo esto se perdera al reiniciar.",
                    "Instalar", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
            {
                return;
            }

            Correr(Guiones.Guion(raiz, "configurar_equipo.ps1"), args.ToString(), "Instalacion");
        }

        private string Validar()
        {
            if (EsServidor)
            {
                if (txtNuevaClaveEquipo.Text.Length == 0) { return "Pon una contrasena para digbit_equipo: es la que llevaran los equipos del laboratorio."; }
                if (txtNuevaClaveAdmin.Text.Length == 0) { return "Pon una contrasena para digbit_admin: es la de este equipo."; }
                if (txtNuevaClaveEquipo.Text == txtNuevaClaveAdmin.Text) { return "Pon contrasenas DISTINTAS. Si son la misma, quien lea la de un equipo del laboratorio tiene tambien la de administrador."; }
                return null;
            }

            if (txtServidor.Text.Trim().Length == 0) { return "Falta el servidor de la base de datos."; }
            if (txtBase.Text.Trim().Length == 0) { return "Falta el nombre de la base."; }
            if (txtUsuarioBd.Text.Trim().Length == 0) { return "Falta el usuario de la base."; }
            if (!EsMaquina) { return null; }
            if (cboLaboratorio.Text.Trim().Length == 0) { return "Elige el laboratorio. Pulsa Cargar para traer la lista de la base."; }
            if (txtCuenta.Text.Trim().Length == 0) { return "Falta el nombre de la cuenta del laboratorio."; }
            if (txtClaveCuenta.Text.Length == 0) { return "Falta la contrasena de la cuenta del laboratorio."; }
            return null;
        }


        /// <summary>
        /// Monta la base en ESTE equipo y crea los dos usuarios. Es el primer
        /// paso de todo el despliegue y se hace una sola vez; hasta que esto no
        /// existe, no hay a donde conectar ningun otro equipo.
        ///
        /// Se genera un guion temporal en vez de encadenar comandos: las
        /// contrasenas pasan por archivo y no por la linea de comandos, que la
        /// ve cualquiera que liste los procesos.
        /// </summary>
        private void PrepararServidor()
        {
            string sqlUsuarios = Path.Combine(raiz, "db", "07_usuarios_minimos.sql");
            if (!File.Exists(sqlUsuarios))
            {
                MessageBox.Show("No encuentro " + sqlUsuarios, "Preparar la base", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // preparar_bd.ps1 se niega a escribir encima de una base que ya
            // existe, y hace bien: ahi estan las bitacoras. Pero sin esto el
            // instalador soltaba su "throw" en crudo, que no dice que hacer, y
            // no habia forma de pedir -Rehacer desde la ventana.
            bool hayBase = Directory.Exists(@"C:\DigBitDB\data");
            string rehacer = "";
            if (hayBase)
            {
                if (MessageBox.Show(
                        @"Ya hay una base de datos montada en C:\DigBitDB." + Environment.NewLine + Environment.NewLine +
                        "Si lo que quieres es instalar la aplicacion en este equipo, cancela y marca" + Environment.NewLine +
                        @"""Administrador"" aqui arriba: la base ya esta lista y no hay que volver a montarla." + Environment.NewLine + Environment.NewLine +
                        "Continuar BORRA la base actual y la deja de cero. Se pierden las bitacoras," + Environment.NewLine +
                        "los laboratorios y los horarios que ya hubiera dentro.",
                        "Ya hay una base", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                {
                    return;
                }
                rehacer = " -Rehacer";
            }
            else if (MessageBox.Show(
                    @"Se va a instalar MySQL en C:\DigBitDB como servicio, con el esquema y los datos de prueba," + Environment.NewLine +
                    "y se crearan los usuarios digbit_equipo y digbit_admin." + Environment.NewLine + Environment.NewLine +
                    "Esto se hace UNA SOLA VEZ, en el equipo que hara de servidor.",
                    "Preparar la base", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
            {
                return;
            }

            string temporal = Path.Combine(Path.GetTempPath(), "digbit_preparar_" + Guid.NewGuid().ToString("N") + ".ps1");
            StringBuilder guion = new StringBuilder();
            guion.AppendLine("$ErrorActionPreference = 'Stop'");
            guion.AppendLine("& " + Comilla(Path.Combine(raiz, "preparar_bd.ps1")) + @" -Destino 'C:\DigBitDB'" + rehacer);
            guion.AppendLine("if ($LASTEXITCODE -ne 0) { throw 'preparar_bd.ps1 fallo.' }");
            guion.AppendLine("Write-Host ''");
            guion.AppendLine("Write-Host '== Usuarios de MySQL con permisos minimos' -ForegroundColor Cyan");
            guion.AppendLine("$sql = Get-Content -Raw " + Comilla(sqlUsuarios));
            guion.AppendLine("$sql = $sql.Replace('CAMBIAME-EQUIPO', " + Comilla(txtNuevaClaveEquipo.Text) + ")");
            guion.AppendLine("$sql = $sql.Replace('CAMBIAME-ADMIN', " + Comilla(txtNuevaClaveAdmin.Text) + ")");
            guion.AppendLine("$previo = $ErrorActionPreference; $ErrorActionPreference = 'Continue'");
            guion.AppendLine(@"$sql | & 'C:\DigBitDB\bin\mysql.exe' -u root");
            guion.AppendLine("$codigo = $LASTEXITCODE; $ErrorActionPreference = $previo");
            guion.AppendLine("if ($codigo -ne 0) { throw \"mysql devolvio $codigo al crear los usuarios.\" }");
            guion.AppendLine("Write-Host ''");
            guion.AppendLine("Write-Host 'Base lista. Ahora elige Administrador arriba y usa:' -ForegroundColor Green");
            guion.AppendLine("Write-Host '   Servidor 127.0.0.1   Base teschi_otru   Usuario digbit_admin'");
            guion.AppendLine("Write-Host '   y la contrasena de administrador que acabas de poner.'");

            File.WriteAllText(temporal, guion.ToString(), new UTF8Encoding(false));
            try
            {
                Correr(temporal, "", "Preparar la base");
            }
            finally
            {
                try { File.Delete(temporal); } catch (Exception) { }
            }

            // Se dejan rellenos los datos del siguiente paso, para no tener que
            // acordarse de nada.
            txtServidor.Text = "127.0.0.1";
            txtBase.Text = "teschi_otru";
            txtClaveBd.Text = txtNuevaClaveAdmin.Text;
        }

        private static string Comilla(string valor)
        {
            return Guiones.Comillas(valor);
        }

        private void Correr(string guion, string argumentos, string que)
        {
            if (!File.Exists(guion))
            {
                MessageBox.Show("No encuentro " + guion, que, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnInstalar.Enabled = false;
            btnDiagnostico.Enabled = false;
            lblEstado.Text = que + " en marcha...";
            txtSalida.Text = "";
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();

            string salida, error;
            int codigo = Ejecutar("powershell.exe",
                "-NoProfile -ExecutionPolicy Bypass -File \"" + guion + "\"" + argumentos,
                out salida, out error);

            txtSalida.Text = (salida + Environment.NewLine + error).Trim();
            txtSalida.SelectionStart = txtSalida.TextLength;
            txtSalida.ScrollToCaret();

            Cursor = Cursors.Default;
            btnInstalar.Enabled = true;
            btnDiagnostico.Enabled = true;

            if (codigo == 0)
            {
                lblEstado.Text = que + " correcta.";
                lblEstado.ForeColor = Color.FromArgb(0, 122, 94);
            }
            else
            {
                lblEstado.Text = que + " FALLO (codigo " + codigo + ").";
                lblEstado.ForeColor = Color.Firebrick;
            }
        }

        /// <summary>
        /// Lanza un proceso y devuelve lo que escribio. Se captura la salida en
        /// vez de enseñar una consola porque el guion imprime cosas que hay que
        /// leer, y una ventana negra que se cierra sola no sirve de nada.
        /// </summary>
        private static int Ejecutar(string programa, string argumentos, out string salida, out string error)
        {
            ProcessStartInfo inicio = new ProcessStartInfo(programa, argumentos);
            inicio.UseShellExecute = false;
            inicio.RedirectStandardOutput = true;
            inicio.RedirectStandardError = true;
            inicio.CreateNoWindow = true;
            inicio.StandardOutputEncoding = Encoding.UTF8;
            inicio.StandardErrorEncoding = Encoding.UTF8;

            using (Process proceso = Process.Start(inicio))
            {
                salida = proceso.StandardOutput.ReadToEnd();
                error = proceso.StandardError.ReadToEnd();
                proceso.WaitForExit();
                return proceso.ExitCode;
            }
        }
    }
}
