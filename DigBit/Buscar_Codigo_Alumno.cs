using DigBit.conexion;
using DigBit.Infraestructura;
using System;
using System.Windows.Forms;

namespace DigBit
{
    public partial class Buscar_Codigo_Alumno : Form
    {
        private Registro_Bitacora regbit;
        private string codigoDeRegbit;
        private Label lblClaseEnCurso;

        public Buscar_Codigo_Alumno()
        {
            InitializeComponent();

            // Fase 6: debajo del cuadro se muestra la clase en curso del laboratorio.
            // Se crea en codigo para no tocar el Designer; el formulario crece 50 px.
            ClientSize = new System.Drawing.Size(ClientSize.Width, ClientSize.Height + 50);
            lblClaseEnCurso = new Label
            {
                AutoSize = false,
                Location = new System.Drawing.Point(12, ClientSize.Height - 46),
                Size = new System.Drawing.Size(ClientSize.Width - 24, 40),
                Font = new System.Drawing.Font("Century Gothic", 8.5F),
                ForeColor = System.Drawing.Color.DarkGreen,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Text = ""
            };
            Controls.Add(lblClaseEnCurso);
            lblClaseEnCurso.BringToFront();
            MostrarClaseEnCurso();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                MostrarClaseEnCurso();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private static string TituloRechazo(MotivoCodigo motivo)
        {
            switch (motivo)
            {
                case MotivoCodigo.NoExiste:
                case MotivoCodigo.Inactiva:
                case MotivoCodigo.FueraDePeriodo:
                    return "Código no válido";
                case MotivoCodigo.HoyNoHayClase:
                case MotivoCodigo.Cancelada:
                    return "Sin clase";
                case MotivoCodigo.OtroLaboratorio:
                    return "Otro laboratorio";
                case MotivoCodigo.AunNoEmpieza:
                    return "Todavía no empieza";
                case MotivoCodigo.Expirado:
                    return "Clase terminada";
                case MotivoCodigo.YaRegistrado:
                    return "Registro duplicado";
                default:
                    return "Código";
            }
        }

        /// <summary>
        /// Le devuelve el equipo a quien ya registro y sigue en clase. Devuelve
        /// true si ya se atendio el caso (se reanudo, o el alumno dijo que no) y
        /// false para seguir con el mensaje de siempre.
        ///
        /// La hora se compara con el reloj del SERVIDOR, que es el que manda en
        /// todo lo demas: si el reloj del equipo esta mal puesto, no queremos ni
        /// negarle el acceso a quien esta en clase ni dárselo a quien ya no.
        /// </summary>
        private bool OfrecerReanudar(VentanaCodigo ventana, string codigo, SesionLocal sesionLocal)
        {
            if (ventana.AhoraServidor >= ventana.Fin)
            {
                // La clase ya termino: que siga el aviso normal.
                return false;
            }

            if (!Kiosco.Activo)
            {
                // Fuera del kiosco no hay ningun equipo que liberar.
                return false;
            }

            DialogResult respuesta = MessageBox.Show(
                "Ya registraste tu bitácora en esta clase." + Environment.NewLine + Environment.NewLine +
                "¿Quieres volver a usar el equipo hasta las " + ventana.Fin.ToString("HH:mm") + "?",
                "Continuar en esta clase",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (respuesta != DialogResult.Yes)
            {
                Log.Info("Se ofrecio reanudar la clase de " + Datos_User.getUser() + " con el codigo " + codigo + " y dijo que no.");
                return true;
            }

            string usuario = Datos_User.getUser();
            Log.Info("Reanudando la clase de " + usuario + " con el codigo " + codigo + " hasta "
                + ventana.Fin.ToString("HH:mm") + ": ya tenia bitacora de esta sesion.");

            Datos_User.Setcodigo(codigo);
            Datos_User.VentanaActual = ventana;
            Datos_User.SesionSinConexion = sesionLocal;
            this.Hide();
            SesionEquipo.Liberar(ventana, usuario, codigo);
            return true;
        }

        /// <summary>
        /// Panel informativo: que clase hay ahora en este laboratorio (o que no
        /// hay ninguna), para que el alumno sepa que codigo esperar.
        /// </summary>
        private void MostrarClaseEnCurso()
        {
            if (lblClaseEnCurso == null)
            {
                return;
            }

            try
            {
                if (!LaboratorioEquipo.Resuelto)
                {
                    lblClaseEnCurso.Text = "Este equipo no tiene laboratorio asignado.";
                    return;
                }

                SesionAgenda enCurso;
                if (SinConexion.Activo)
                {
                    enCurso = SinConexion.Cache.ClaseEnCurso(SinConexion.Cache.AhoraEstimado);
                }
                else
                {
                    DateTime ahora;
                    enCurso = new HorariosDatos().ClaseEnCurso(LaboratorioEquipo.Id, out ahora);
                }
                lblClaseEnCurso.Text = enCurso == null
                    ? LaboratorioEquipo.Nombre + (SinConexion.Activo ? " (sin conexión)" : "") + " · ahora no hay clase programada."
                    : LaboratorioEquipo.Nombre + (SinConexion.Activo ? " (sin conexión) · " : " · ") + enCurso.Materia + " · " + enCurso.Grupo + " · " + enCurso.Profesor
                      + " · hasta las " + HorariosDatos.Hora(enCurso.Fin) + (enCurso.EsExtra ? " (sesión extra)" : "");
            }
            catch (Exception ex)
            {
                Log.Error("Consultar la clase en curso", ex);
                lblClaseEnCurso.Text = LaboratorioEquipo.Nombre ?? "";
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string codigo = (txtIngresarCodigo.Texts ?? string.Empty).Trim();
            if (codigo.Length == 0)
            {
                MessageBox.Show("Ingresa el código de acceso que te dio tu profesor.", "Código requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idUsuario = 0;
            if (!SinConexion.Activo)
            {
                // idusuario ya avisa con un MessageBox si la matricula no existe.
                if (!int.TryParse(new Consultas().idusuario(Datos_User.getUser()), out idUsuario))
                {
                    return;
                }
            }

            // Fase 6: el codigo se resuelve para ESTE laboratorio y este momento
            // (reloj del servidor). Sin laboratorio configurado no hay nada que hacer.
            if (!LaboratorioEquipo.Resuelto)
            {
                MessageBox.Show("Este equipo no tiene laboratorio asignado. Avisa al encargado del laboratorio.", "Equipo sin laboratorio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ResultadoCodigo resultado;
            SesionLocal sesionLocal = null;
            if (SinConexion.Activo)
            {
                resultado = SinConexion.Cache.ResolverCodigo(codigo, Datos_User.getUser(), out sesionLocal);
            }
            else
            {
                try
                {
                    resultado = new HorariosDatos().ResolverCodigo(codigo, LaboratorioEquipo.Id, idUsuario);
                }
                catch (Exception ex)
                {
                    // Fase 4: si el servidor se cayo a media manana, se sigue con la copia local.
                    Log.Error("Validar codigo de acceso '" + codigo + "'", ex);
                    if (!SinConexion.IntentarActivar(ex.Message))
                    {
                        MessageBox.Show("No se pudo validar el código. Intenta de nuevo; si sigue fallando avisa al encargado del laboratorio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    resultado = SinConexion.Cache.ResolverCodigo(codigo, Datos_User.getUser(), out sesionLocal);
                    MostrarClaseEnCurso();
                }
            }

            // Quien ya registro su bitacora y vuelve durante la misma clase NO es
            // un error. Pasa cuando se va la luz o alguien desconecta el equipo a
            // media hora: al arrancar de nuevo, el traspaso guardado se descarta
            // porque el arranque del sistema es otro (SesionActiva.Evaluar
            // devuelve DeOtroArranque), asi que SesionEquipo.Reanudar() no lo
            // recupera. Hasta ahora esto le contestaba "Registro duplicado" y lo
            // dejaba sin equipo el resto de la clase, sin salida ninguna.
            if (resultado.Motivo == MotivoCodigo.YaRegistrado
                && resultado.Ventana != null
                && OfrecerReanudar(resultado.Ventana, codigo, sesionLocal))
            {
                return;
            }

            if (!resultado.Aceptado)
            {
                Log.Info("Codigo '" + codigo + "' rechazado (" + resultado.Motivo + ") en " + LaboratorioEquipo.Nombre + ".");
                MessageBox.Show(resultado.Mensaje, TituloRechazo(resultado.Motivo), MessageBoxButtons.OK,
                    resultado.Motivo == MotivoCodigo.YaRegistrado ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                return;
            }

            VentanaCodigo ventana = resultado.Ventana;
            Datos_User.Setcodigo(codigo);
            Datos_User.VentanaActual = ventana;
            Datos_User.SesionSinConexion = sesionLocal;
            this.Hide();

            // Se abre con dueno (el principal a pantalla completa) para que no pueda
            // perderse detras de el en modo kiosco. Si ya hay una bitacora a medias
            // para ESTE mismo codigo se recupera tal cual, con lo que el alumno lleve
            // escrito; si es otro codigo, se crea nueva (Registro_Bitacora lee el
            // codigo y el laboratorio en su constructor).
            if (regbit != null && !regbit.IsDisposed && codigoDeRegbit == codigo)
            {
                regbit.Show(Owner);
                regbit.BringToFront();
                return;
            }

            if (regbit != null && !regbit.IsDisposed)
            {
                regbit.Close();
            }

            regbit = new Registro_Bitacora();
            codigoDeRegbit = codigo;
            regbit.Show(Owner);
            regbit.BringToFront();
        }
    }
}
