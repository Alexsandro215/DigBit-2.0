using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DigBit.conexion;

namespace DigBit.Infraestructura
{
    /// <summary>
    /// "Equipo liberado": lo que pasa despues de que el alumno guarda su bitacora.
    /// En kiosco lanza el escritorio de Windows, esconde a DigBit y lo deja
    /// vigilando con un icono en la bandeja; avisa 5 minutos antes de la hora de
    /// salida y, al llegar, cierra la sesion de Windows (como respaldo del
    /// servicio DigBit.Vigilante, que es quien lo garantiza). Fuera del kiosco no
    /// toca Windows: al vencer vuelve al login.
    /// </summary>
    internal static class SesionEquipo
    {
        private static readonly TimeSpan AvisoAntes = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan PeriodoLatido = TimeSpan.FromSeconds(30);

        // Para probar en un equipo de desarrollo con el kiosco activo sin que se
        // cierre la sesion de Windows de verdad.
        private const string VariableSimularLogoff = "DIGBIT_SIMULAR_LOGOFF";

        // Lo mismo para el apagado que pide el alumno con equipo propio.
        private const string VariableSimularApagado = "DIGBIT_SIMULAR_APAGADO";

        private static SesionActiva sesion;
        private static NotifyIcon icono;
        private static Timer temporizador;
        private static DateTime ultimoLatido;
        private static bool avisado;
        private static AvisoSesion ventanaAviso;

        public static bool Activa
        {
            get { return sesion != null; }
        }

        public static SesionActiva Sesion
        {
            get { return sesion; }
        }

        /// <summary>
        /// Llamado por Registro_Bitacora cuando la bitacora quedo guardada.
        /// </summary>
        public static void Liberar(VentanaCodigo ventana, string usuario, string codigo)
        {
            if (sesion != null)
            {
                Log.Aviso("Liberar: ya habia una sesion liberada (" + sesion.Usuario + "); se sustituye.");
                Limpiar();
            }

            DateTime ahora = DateTime.Now;
            sesion = new SesionActiva
            {
                Usuario = usuario,
                Codigo = codigo,
                Equipo = Environment.MachineName,
                SesionWindows = Process.GetCurrentProcess().SessionId,
                ArranqueSistema = SesionActiva.ArranqueActual(),
                InicioLocal = ventana.Inicio - ventana.Desfase,
                FinLocal = ventana.FinEnRelojLocal,
                Escrito = ahora,
                Latido = ahora
            };

            try
            {
                sesion.Guardar();
                Log.Info("Equipo liberado para " + usuario + " con el codigo " + codigo + " hasta " + sesion.FinLocal.ToString("HH:mm") + " (traspaso en " + SesionActiva.Ruta + ").");
            }
            catch (Exception ex)
            {
                // Sin traspaso el vigilante no se entera; DigBit sigue siendo el
                // respaldo. Queda en el log para que el administrador lo vea.
                Log.Error("No se pudo escribir el traspaso para el vigilante", ex);
            }

            Iniciar();
        }

        /// <summary>
        /// Acceso de emergencia: abre el escritorio SIN bitacora y SIN hora de
        /// salida. A proposito no se escribe el archivo de traspaso: sin el, el
        /// vigilante no tiene nada que vigilar y no cierra la sesion a media
        /// emergencia, que es justo lo que se pidio. El equipo queda abierto
        /// hasta que se reinicie o se apague.
        ///
        /// Ojo con lo que esto significa: en modo kiosco la cuenta tiene
        /// desactivado "cerrar sesion", asi que la unica forma de terminarlo es
        /// apagar o reiniciar. Para eso esta el boton de apagar del login.
        /// </summary>
        public static bool LiberarEmergencia()
        {
            if (sesion != null)
            {
                Log.Aviso("Acceso de emergencia con una sesion liberada en curso (" + sesion.Usuario + "); se sustituye.");
                Limpiar();
            }

            // Si quedaba un traspaso de antes, fuera: no queremos que el
            // vigilante cierre la sesion de emergencia por una hora vieja.
            SesionActiva.Borrar();

            Log.Aviso("ACCESO DE EMERGENCIA concedido con la memoria autorizada. El equipo queda abierto"
                + " SIN bitacora y SIN hora de salida; termina al reiniciar o apagar.");

            if (!Kiosco.Activo)
            {
                Log.Aviso("Modo escritorio: aqui se abriria el escritorio. No se abre nada.");
                return false;
            }

            AbrirEscritorioOAvisar();
            return true;
        }

        /// <summary>
        /// Al arrancar: si hay una sesion liberada vigente de este mismo arranque y
        /// sesion de Windows, vuelve a ella sin pedir login (Shell Launcher relanzo
        /// DigBit, o el alumno lo cerro). Solo en kiosco. Devuelve true si reanudo.
        /// </summary>
        public static bool Reanudar()
        {
            SesionActiva guardada = SesionActiva.Leer();
            if (guardada == null)
            {
                return false;
            }

            SesionActiva.Vigencia vigencia = guardada.Evaluar(DateTime.Now, SesionActiva.ArranqueActual(), Process.GetCurrentProcess().SessionId);
            if (vigencia == SesionActiva.Vigencia.Caducada || vigencia == SesionActiva.Vigencia.DeOtroArranque)
            {
                Log.Info("Traspaso anterior descartado (" + vigencia + "): " + guardada.Usuario + " hasta " + guardada.FinLocal.ToString("dd/MM HH:mm") + ".");
                SesionActiva.Borrar();
                return false;
            }

            if (vigencia != SesionActiva.Vigencia.Vigente || DateTime.Now >= guardada.FinLocal)
            {
                return false;
            }

            if (!Kiosco.Activo)
            {
                Log.Info("Hay una sesion liberada vigente (" + guardada.Usuario + ") pero el kiosco esta apagado; se ignora.");
                return false;
            }

            sesion = guardada;
            Log.Info("Reanudando la sesion liberada de " + sesion.Usuario + " hasta " + sesion.FinLocal.ToString("HH:mm") + ".");
            Iniciar();
            return true;
        }

        /// <summary>El alumno se va antes ("Cerrar mi sesion ahora" en la bandeja).</summary>
        public static void TerminarAhora()
        {
            Log.Info("El alumno pidio cerrar su sesion antes de la hora de salida.");
            Terminar();
        }

        private static void Iniciar()
        {
            avisado = false;
            ultimoLatido = DateTime.Now;
            CrearIcono();

            if (Kiosco.Activo)
            {
                AbrirEscritorioOAvisar();
            }

            temporizador = new Timer { Interval = 1000 };
            temporizador.Tick += Temporizador_Tick;
            temporizador.Start();
            ActualizarIcono();
        }

        private static void Temporizador_Tick(object sender, EventArgs e)
        {
            if (sesion == null)
            {
                return;
            }

            DateTime ahora = DateTime.Now;
            TimeSpan restante = sesion.FinLocal - ahora;

            if (ahora - ultimoLatido >= PeriodoLatido)
            {
                ultimoLatido = ahora;
                sesion.Latido = ahora;
                try
                {
                    sesion.Guardar();
                }
                catch (Exception ex)
                {
                    Log.Error("Renovar el latido del traspaso", ex);
                }
            }

            ActualizarIcono();

            if (!avisado && restante <= AvisoAntes)
            {
                avisado = true;
                MostrarAviso(restante);
            }

            if (restante <= TimeSpan.Zero)
            {
                Log.Info("Llego la hora de salida (" + sesion.FinLocal.ToString("HH:mm") + ") de " + sesion.Usuario + ".");
                Terminar();
            }
        }

        private static void Terminar()
        {
            bool kiosco = Kiosco.Activo;
            Limpiar();
            SesionActiva.Borrar();

            if (kiosco && CerrarSesionWindows())
            {
                // Windows cerrara la sesion y, con ella, este proceso.
                return;
            }

            // Modo escritorio, cierre simulado o ExitWindowsEx fallido: se vuelve al
            // login. En kiosco eso deja el login a pantalla completa encima del
            // escritorio, que es lo que toca si la hora ya vencio (el vigilante
            // cerrara la sesion de todas formas).
            if (AppContexto.Actual != null)
            {
                Log.Info(kiosco
                    ? "Sin cierre de sesion de Windows; se vuelve al login."
                    : "Modo escritorio: fin de la sesion liberada; se vuelve al login.");
                AppContexto.Actual.CerrarSesion();
            }
        }

        private static void Limpiar()
        {
            if (temporizador != null)
            {
                temporizador.Stop();
                temporizador.Dispose();
                temporizador = null;
            }

            if (ventanaAviso != null && !ventanaAviso.IsDisposed)
            {
                ventanaAviso.Close();
            }
            ventanaAviso = null;

            if (icono != null)
            {
                icono.Visible = false;
                icono.Dispose();
                icono = null;
            }

            sesion = null;
        }

        // --- Escritorio y cierre de sesion -----------------------------------

        /// <summary>Segundos que se espera a que aparezca el escritorio tras pedirlo.</summary>
        private const int EsperaEscritorioSegundos = 10;

        /// <summary>Veces que se reintenta antes de avisar al alumno.</summary>
        private const int IntentosDeEscritorio = 3;

        /// <summary>
        /// Entrega el equipo al alumno. Si el escritorio no arranca, NO se esconde:
        /// antes se escondia siempre y, cuando explorer fallaba, el alumno se
        /// quedaba con la pantalla en negro y sin ninguna salida.
        /// </summary>
        private static void AbrirEscritorioOAvisar()
        {
            for (int intento = 1; intento <= IntentosDeEscritorio; intento++)
            {
                if (LanzarEscritorio(intento))
                {
                    OcultarVentanas();
                    return;
                }

                if (intento < IntentosDeEscritorio)
                {
                    Log.Aviso("El escritorio no aparecio en el intento " + intento + "; se reintenta.");
                }
            }

            Log.Error("El escritorio de Windows no arranca despues de "
                + IntentosDeEscritorio + " intentos; DigBit se queda visible.", null);

            // Modal a proposito: el temporizador de la sesion sigue corriendo
            // mientras hay un dialogo, asi que la hora de salida se respeta igual.
            while (true)
            {
                PantallaError.Mostrar(
                    "No se pudo abrir el escritorio",
                    "Tu bitacora quedo registrada correctamente." + Environment.NewLine + Environment.NewLine
                        + "El escritorio de Windows no arranco en este equipo. Puedes reintentar; "
                        + "si vuelve a fallar, avisa al encargado del laboratorio."
                        + Environment.NewLine + Environment.NewLine
                        + "Tu sesion termina a las " + (sesion != null ? sesion.FinLocal.ToString("HH:mm") : "?") + ".",
                    "Reintentar", false, 30);

                if (LanzarEscritorio(0))
                {
                    OcultarVentanas();
                    return;
                }
            }
        }

        /// <summary>
        /// Lanza el escritorio y devuelve true solo si quedo EN MARCHA. Antes esto
        /// escribia "lanzado" justo despues de pedirlo, sin comprobar nada, asi que
        /// un explorer que moria al instante quedaba registrado como un exito.
        /// </summary>
        private static bool LanzarEscritorio(int intento)
        {
            try
            {
                if (HayEscritorio())
                {
                    Log.Info("Ya hay escritorio en esta sesion; no se vuelve a lanzar.");
                    return true;
                }

                // Sin ningun shell registrado en la sesion, explorer.exe arranca el
                // escritorio completo (barra de tareas incluida), no una carpeta.
                string ruta = RutaDelExplorador();
                Process lanzado = Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = false });
                Log.Info("Lanzado " + ruta + (intento > 0 ? " (intento " + intento + ")" : "")
                    + (lanzado != null ? ", pid " + lanzado.Id : "") + "; esperando al escritorio.");

                for (int pasada = 0; pasada < EsperaEscritorioSegundos * 4; pasada++)
                {
                    // En rodajas cortas y bombeando mensajes: la espera ocurre en el
                    // hilo de interfaz y no debe parecer que la aplicacion se colgo.
                    System.Threading.Thread.Sleep(250);
                    Application.DoEvents();

                    if (HayEscritorio())
                    {
                        Log.Info("Escritorio de Windows en marcha.");
                        return true;
                    }
                }

                string detalle = "";
                if (lanzado != null && lanzado.HasExited)
                {
                    detalle = " El proceso termino solo con codigo " + lanzado.ExitCode + ".";
                }
                Log.Aviso("Pasaron " + EsperaEscritorioSegundos + " s y no hay escritorio en esta sesion."
                    + detalle + " Procesos explorer en la sesion: " + ExploradoresEnLaSesion()
                    + ". DigBit corre en " + (Environment.Is64BitProcess ? "64" : "32") + " bits sobre un Windows de "
                    + (Environment.Is64BitOperatingSystem ? "64" : "32") + " bits.");
                return false;
            }
            catch (Exception ex)
            {
                Log.Error("No se pudo lanzar explorer.exe", ex);
                return false;
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetShellWindow();

        /// <summary>
        /// Ruta ABSOLUTA del explorador de Windows, normalmente C:\Windows\explorer.exe.
        ///
        /// Tiene que ser absoluta, y esta es la causa por la que el escritorio no
        /// aparecia. DigBit se compila AnyCPU con Prefer32Bit, que es lo que pone
        /// MSBuild por defecto, asi que en un Windows de 64 bits corre bajo WOW64.
        /// Al pedir "explorer.exe" a secas, CreateProcess busca por orden: carpeta
        /// del programa, carpeta actual y CARPETA DE SISTEMA. Para un proceso de 32
        /// bits esa tercera la redirige WOW64 a SysWOW64, donde hay OTRO explorer.exe
        /// de 32 bits que no puede ser el shell: arranca, no pinta nada y se muere en
        /// milisegundos. Nunca se llegaba a C:\Windows, que es donde vive el bueno.
        ///
        /// La carpeta de Windows NO esta sujeta a la redireccion de WOW64 (solo lo
        /// esta System32), asi que dando la ruta entera se obtiene el de verdad.
        /// </summary>
        private static string RutaDelExplorador()
        {
            try
            {
                string carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                if (!string.IsNullOrEmpty(carpeta))
                {
                    string ruta = System.IO.Path.Combine(carpeta, "explorer.exe");
                    if (System.IO.File.Exists(ruta))
                    {
                        return ruta;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("No se pudo resolver la ruta del explorador", ex);
            }

            Log.Aviso("No se encontro el explorador por ruta absoluta; se pide por nombre.");
            return "explorer.exe";
        }

        /// <summary>
        /// True si el escritorio esta montado EN ESTA sesion.
        ///
        /// Se mira la ventana de shell (la que registra explorer al crear el
        /// escritorio), no si existe un proceso llamado "explorer": el explorador de
        /// SysWOW64 tambien se llama asi, arranca y muere sin montar nada, de modo
        /// que contar procesos daba falsos positivos. Mirar procesos de TODO el
        /// equipo era peor todavia: si un encargado dejaba su sesion abierta con
        /// "Cambiar de usuario", DigBit creia que el alumno ya tenia escritorio, se
        /// escondia igual, y la pantalla quedaba en negro.
        /// </summary>
        private static bool HayEscritorio()
        {
            return GetShellWindow() != IntPtr.Zero;
        }

        /// <summary>Cuantos explorer.exe hay en la sesion de Windows de este proceso. Solo para el log.</summary>
        private static int ExploradoresEnLaSesion()
        {
            int sesionWindows = Process.GetCurrentProcess().SessionId;
            int cuantos = 0;
            foreach (Process proceso in Process.GetProcessesByName("explorer"))
            {
                try
                {
                    if (proceso.SessionId == sesionWindows)
                    {
                        cuantos++;
                    }
                }
                catch (InvalidOperationException)
                {
                    // Murio mientras lo miraba; no cuenta.
                }
                finally
                {
                    proceso.Dispose();
                }
            }

            return cuantos;
        }

        private static void OcultarVentanas()
        {
            foreach (Form formulario in Application.OpenForms.Cast<Form>().ToArray())
            {
                formulario.Hide();
            }
        }

        private const uint EWX_LOGOFF = 0x00000000;
        private const uint EWX_FORCEIFHUNG = 0x00000010;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        // --- Apagado -----------------------------------------------------------

        private const uint EWX_POWEROFF = 0x00000008;
        private const uint SHTDN_REASON_FLAG_PLANNED = 0x80000000;

        private const int TOKEN_ADJUST_PRIVILEGES = 0x0020;
        private const int TOKEN_QUERY = 0x0008;
        private const uint SE_PRIVILEGE_ENABLED = 0x00000002;
        private const int ERROR_NOT_ALL_ASSIGNED = 1300;
        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        [StructLayout(LayoutKind.Sequential)]
        private struct LUID
        {
            public uint LowPart;
            public int HighPart;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LUID_AND_ATTRIBUTES
        {
            public LUID Luid;
            public uint Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TOKEN_PRIVILEGES
        {
            public uint PrivilegeCount;
            public LUID_AND_ATTRIBUTES Privilegio;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr objeto);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr proceso, int acceso, out IntPtr token);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LookupPrivilegeValue(string sistema, string nombre, out LUID luid);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool AdjustTokenPrivileges(IntPtr token, bool quitarTodos,
            ref TOKEN_PRIVILEGES nuevas, int tamano, IntPtr anteriores, IntPtr devuelto);

        /// <summary>
        /// Apaga el equipo. Lo pide la pantalla del alumno que trajo su propia
        /// computadora: como no va a usar esta maquina, o la apaga o la deja en el
        /// login, y desde el boton "Apagar el equipo" del propio login, que en
        /// kiosco es la unica forma que tiene un alumno de apagar sin recurrir a
        /// Ctrl+Alt+Supr. Devuelve true si Windows acepto la orden; con false,
        /// quien llama tiene que volver al login.
        ///
        /// Fuera del kiosco no apaga nada: en un equipo de desarrollo apagar de
        /// verdad seria una sorpresa muy desagradable.
        /// </summary>
        public static bool ApagarEquipo()
        {
            if (!Kiosco.Activo)
            {
                Log.Aviso("Modo escritorio: aqui se apagaria el equipo. No se apaga nada.");
                return false;
            }

            if (Kiosco.EsVerdadero(Environment.GetEnvironmentVariable(VariableSimularApagado)))
            {
                Log.Aviso("SIMULACION (" + VariableSimularApagado + "): aqui se apagaria el equipo.");
                return false;
            }

            if (!HabilitarPrivilegio(SE_SHUTDOWN_NAME))
            {
                return false;
            }

            Log.Info("Apagando el equipo a peticion del usuario.");
            if (!ExitWindowsEx(EWX_POWEROFF | EWX_FORCEIFHUNG, SHTDN_REASON_FLAG_PLANNED))
            {
                Log.Aviso("ExitWindowsEx (apagado) fallo con el error " + Marshal.GetLastWin32Error() + ".");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Activa un privilegio en el token del proceso. Una cuenta estandar SI
        /// puede apagar el equipo, pero Windows entrega el privilegio presente y
        /// deshabilitado: hay que pedirlo antes de usarlo.
        /// </summary>
        private static bool HabilitarPrivilegio(string nombre)
        {
            IntPtr token = IntPtr.Zero;
            try
            {
                if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out token))
                {
                    Log.Aviso("OpenProcessToken fallo con el error " + Marshal.GetLastWin32Error() + ".");
                    return false;
                }

                LUID luid;
                if (!LookupPrivilegeValue(null, nombre, out luid))
                {
                    Log.Aviso("LookupPrivilegeValue(" + nombre + ") fallo con el error " + Marshal.GetLastWin32Error() + ".");
                    return false;
                }

                TOKEN_PRIVILEGES privilegios = new TOKEN_PRIVILEGES
                {
                    PrivilegeCount = 1,
                    Privilegio = new LUID_AND_ATTRIBUTES { Luid = luid, Attributes = SE_PRIVILEGE_ENABLED }
                };

                // Devuelve true aunque no haya asignado nada, asi que hay que mirar
                // el ultimo error justo despues para saber si de verdad se activo.
                bool llamada = AdjustTokenPrivileges(token, false, ref privilegios,
                    Marshal.SizeOf(typeof(TOKEN_PRIVILEGES)), IntPtr.Zero, IntPtr.Zero);
                int error = Marshal.GetLastWin32Error();

                if (!llamada)
                {
                    Log.Aviso("AdjustTokenPrivileges fallo con el error " + error + ".");
                    return false;
                }

                if (error == ERROR_NOT_ALL_ASSIGNED)
                {
                    Log.Aviso("La cuenta no tiene el privilegio " + nombre + "; no se puede apagar el equipo.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Habilitar el privilegio " + nombre, ex);
                return false;
            }
            finally
            {
                if (token != IntPtr.Zero)
                {
                    CloseHandle(token);
                }
            }
        }

        /// <summary>Devuelve true si Windows acepto la orden de cerrar la sesion.</summary>
        private static bool CerrarSesionWindows()
        {
            if (Kiosco.EsVerdadero(Environment.GetEnvironmentVariable(VariableSimularLogoff)))
            {
                Log.Aviso("SIMULACION (" + VariableSimularLogoff + "): aqui se cerraria la sesion de Windows.");
                return false;
            }

            Log.Info("Cerrando la sesion de Windows (respaldo de DigBit; el vigilante deberia haberlo hecho ya).");
            if (!ExitWindowsEx(EWX_LOGOFF | EWX_FORCEIFHUNG, 0))
            {
                Log.Aviso("ExitWindowsEx fallo con el error " + Marshal.GetLastWin32Error() + ".");
                return false;
            }

            return true;
        }

        // --- Bandeja y aviso --------------------------------------------------

        private static void CrearIcono()
        {
            try
            {
                Icon imagen = null;
                try
                {
                    imagen = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                }
                catch (Exception)
                {
                    // Sin icono propio se usa uno del sistema.
                }

                icono = new NotifyIcon
                {
                    Icon = imagen ?? SystemIcons.Information,
                    Visible = true,
                    ContextMenu = new ContextMenu(new[]
                    {
                        new MenuItem("Cerrar mi sesion ahora", (s, e) => ConfirmarCierre())
                    })
                };
            }
            catch (Exception ex)
            {
                // Sin bandeja (explorer aun no arranco) el icono se reintenta solo
                // cuando aparece la barra de tareas; si aun asi falla, se sigue sin el.
                Log.Error("No se pudo crear el icono de bandeja", ex);
            }
        }

        private static void ActualizarIcono()
        {
            if (icono == null || sesion == null)
            {
                return;
            }

            TimeSpan restante = sesion.FinLocal - DateTime.Now;
            int minutos = Math.Max(0, (int)Math.Ceiling(restante.TotalMinutes));
            string texto = "DigBit - sesion hasta " + sesion.FinLocal.ToString("HH:mm") + " (" + minutos + " min)";
            // NotifyIcon.Text admite 63 caracteres.
            icono.Text = texto.Length > 63 ? texto.Substring(0, 63) : texto;
        }

        private static void MostrarAviso(TimeSpan restante)
        {
            int minutos = Math.Max(1, (int)Math.Ceiling(restante.TotalMinutes));
            string mensaje = "Tu sesion en este equipo termina a las " + sesion.FinLocal.ToString("HH:mm")
                + " (en " + minutos + " min). Guarda tu trabajo y cierra tus programas.";
            Log.Info("Aviso de fin de sesion mostrado (" + minutos + " min).");

            if (icono != null)
            {
                icono.ShowBalloonTip(15000, "DigBit", mensaje, ToolTipIcon.Warning);
            }

            try
            {
                ventanaAviso = new AvisoSesion(mensaje);
                ventanaAviso.Show();
            }
            catch (Exception ex)
            {
                Log.Error("No se pudo mostrar la ventana de aviso", ex);
            }
        }

        private static void ConfirmarCierre()
        {
            DialogResult respuesta = MessageBox.Show(
                "Se cerrara tu sesion en este equipo. Guarda tu trabajo antes." + Environment.NewLine + Environment.NewLine + "¿Cerrar ahora?",
                "DigBit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2,
                MessageBoxOptions.ServiceNotification);

            if (respuesta == DialogResult.Yes)
            {
                TerminarAhora();
            }
        }
    }
}
