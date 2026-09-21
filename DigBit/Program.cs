using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DigBit.Infraestructura;

namespace DigBit
{
    internal static class Program
    {
        // "Local\" limita el mutex a la sesion de Windows actual: una instancia por
        // sesion, que es exactamente lo que necesita un shell.
        private const string NombreMutex = @"Local\DigBit.InstanciaUnica";

        private static bool mostrandoError;

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool primeraInstancia;
            using (Mutex mutex = new Mutex(true, NombreMutex, out primeraInstancia))
            {
                if (!primeraInstancia)
                {
                    ActivarInstanciaExistente();
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Tiene que ir antes de crear cualquier control. Sin esto, una
                // excepcion no capturada muestra el dialogo generico de .NET y, si
                // DigBit es el shell, deja la sesion en negro.
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += (s, e) =>
                    ManejarExcepcion("Excepcion no controlada en el hilo de interfaz", e.Exception, false);
                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                    ManejarExcepcion("Excepcion no controlada", e.ExceptionObject as Exception, e.IsTerminating);

                Log.Info("Arranque de DigBit " + Assembly.GetExecutingAssembly().GetName().Version
                    + " | kiosco=" + Kiosco.Activo
                    + " | equipo=" + Environment.MachineName
                    + " | usuario de Windows=" + Environment.UserName
                    + " | carpeta=" + AppDomain.CurrentDomain.BaseDirectory);

                Log.Info(CarpetaDatos.Diagnostico);

                // Que se vea en el log si este equipo tiene salida de emergencia.
                // Enterarse el dia que hace falta, con el servidor caido y una
                // clase esperando, es tarde.
                Log.Info("Memoria de emergencia: " + (Emergencia.Configurada
                    ? "autorizada"
                    : "SIN autorizar; el boton avisara y no abrira nada"));

                try
                {
                    AppContexto contexto = new AppContexto();
                    if (contexto.Iniciar())
                    {
                        Application.Run(contexto);
                    }
                }
                catch (Exception ex)
                {
                    ManejarExcepcion("Fallo durante el arranque", ex, true);
                }

                Log.Info("Fin de la aplicacion.");
                GC.KeepAlive(mutex);
            }
        }

        private static void ManejarExcepcion(string contexto, Exception ex, bool terminando)
        {
            Log.Error(contexto, ex);

            // Si falla la propia pantalla de error no hay que volver a entrar aqui.
            if (mostrandoError)
            {
                return;
            }

            mostrandoError = true;
            try
            {
                string mensaje = terminando
                    ? "Ocurrio un error grave y la aplicacion tiene que cerrarse."
                    : "Ocurrio un error inesperado. La aplicacion continuara funcionando.";
                mensaje += Environment.NewLine + Environment.NewLine
                    + "Si el problema se repite, avisa al encargado del laboratorio."
                    + Environment.NewLine + Environment.NewLine
                    + "Detalle: " + (ex != null ? ex.Message : "(desconocido)");

                PantallaError.Mostrar("Error inesperado", mensaje, terminando ? "Cerrar" : "Continuar", false, 0);
            }
            catch (Exception exPantalla)
            {
                Log.Error("Fallo al mostrar la pantalla de error", exPantalla);
            }
            finally
            {
                mostrandoError = false;
            }
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;

        // Segunda instancia: en vez de abrir otra ventana, se trae al frente la que
        // ya existe y se termina en silencio.
        private static void ActivarInstanciaExistente()
        {
            try
            {
                Process actual = Process.GetCurrentProcess();
                foreach (Process proceso in Process.GetProcessesByName(actual.ProcessName))
                {
                    if (proceso.Id == actual.Id)
                    {
                        continue;
                    }

                    IntPtr ventana = proceso.MainWindowHandle;
                    if (ventana != IntPtr.Zero)
                    {
                        ShowWindow(ventana, SW_RESTORE);
                        SetForegroundWindow(ventana);
                    }
                    else
                    {
                        // No deberia pasar (los principales terminan el proceso al
                        // cerrarse), pero si pasa que al menos quede rastro.
                        Log.Aviso("Ya hay una instancia de DigBit (pid " + proceso.Id + ") sin ventana visible; no se abre otra.");
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                Log.Error("No se pudo activar la instancia existente", ex);
            }
        }
    }
}
