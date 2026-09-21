using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace DigBit.Infraestructura
{
    /// <summary>
    /// Las dos senas de identidad de este equipo: en que laboratorio esta y que
    /// numero de maquina es. Las dos se escriben al instalar (una etiqueta por
    /// equipo, igual para todas las de un mismo laboratorio y distinta entre
    /// laboratorios) y las dos se leen igual: variable de entorno primero,
    /// appSettings despues.
    ///
    /// Laboratorio en el que esta este equipo (fase 6). Un codigo de acceso solo
    /// vale en el laboratorio de su franja, asi que cada equipo tiene que saber
    /// cual es el suyo. Se configura con el NOMBRE exacto del laboratorio
    /// (laboratorios.nombre_laboratorio): appSettings "Laboratorio" en
    /// App.config, o la variable de entorno DIGBIT_LABORATORIO, que tiene
    /// prioridad (igual que el resto de la configuracion). deploy\configurar_equipo.ps1
    /// lo escribe con -Laboratorio.
    /// </summary>
    internal static class LaboratorioEquipo
    {
        private const string ClaveAppSettings = "Laboratorio";
        private const string VariableEntorno = "DIGBIT_LABORATORIO";

        public static int Id { get; private set; }
        public static string Nombre { get; private set; }

        public static bool Resuelto
        {
            get { return Id > 0; }
        }

        /// <summary>Fase 4: sin servidor, el laboratorio sale de la copia local del horario.</summary>
        public static void UsarCopiaLocal(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        private const string ClaveNumeroMaquina = "NumeroMaquina";
        private const string VariableNumeroMaquina = "DIGBIT_NUMERO_MAQUINA";

        /// <summary>
        /// Como se identifica esta maquina en la bitacora. Sin configurar usa el
        /// nombre de Windows, que es lo que se hacia siempre; el problema es que
        /// si los equipos se llaman DESKTOP-A7F3K2 eso no le sirve de nada a
        /// quien tenga que ir a buscar "la 12 del laboratorio 1". Con esto se le
        /// pone el numero que de verdad esta pegado en la maquina.
        /// deploy\configurar_equipo.ps1 lo escribe con -NumeroMaquina.
        /// </summary>
        public static string NumeroMaquina
        {
            get
            {
                string configurado = Leer(VariableNumeroMaquina, ClaveNumeroMaquina);
                return configurado ?? Environment.MachineName;
            }
        }

        /// <summary>true si el numero sale de la configuracion y no del nombre de Windows.</summary>
        public static bool NumeroMaquinaConfigurado
        {
            get { return Leer(VariableNumeroMaquina, ClaveNumeroMaquina) != null; }
        }

        /// <summary>Variable de entorno primero, appSettings despues; null si no hay nada.</summary>
        private static string Leer(string variable, string clave)
        {
            string desdeEntorno = Environment.GetEnvironmentVariable(variable);
            if (!string.IsNullOrWhiteSpace(desdeEntorno))
            {
                return desdeEntorno.Trim();
            }

            try
            {
                string desdeConfig = ConfigurationManager.AppSettings[clave];
                return string.IsNullOrWhiteSpace(desdeConfig) ? null : desdeConfig.Trim();
            }
            catch (ConfigurationErrorsException)
            {
                return null;
            }
        }

        /// <summary>Nombre configurado, o null si no hay ninguno.</summary>
        public static string NombreConfigurado
        {
            get { return Leer(VariableEntorno, ClaveAppSettings); }
        }

        /// <summary>
        /// Busca el laboratorio configurado en la base. Devuelve null si quedo
        /// resuelto, o el problema en texto (sin configurar, o no existe) si no.
        /// Se llama al arrancar, despues de comprobar el servidor.
        /// </summary>
        public static string Resolver()
        {
            Id = 0;
            Nombre = null;

            string configurado = NombreConfigurado;
            if (configurado == null)
            {
                return "Este equipo no tiene laboratorio asignado." + Environment.NewLine + Environment.NewLine
                    + "Configura 'Laboratorio' en DigBit.exe.config (o la variable DIGBIT_LABORATORIO) con el nombre exacto del laboratorio.";
            }

            using (MySqlConnection conexion = new Conexion().GetConexion())
            using (MySqlCommand comando = new MySqlCommand(
                "SELECT idlaboratorios, nombre_laboratorio FROM laboratorios WHERE nombre_laboratorio = @Nombre", conexion))
            {
                comando.Parameters.AddWithValue("@Nombre", configurado);
                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return "El laboratorio configurado en este equipo ('" + configurado + "') no existe en la base de datos." + Environment.NewLine + Environment.NewLine
                            + "Revisa el nombre en DigBit.exe.config o da de alta el laboratorio.";
                    }

                    Id = reader.GetInt32("idlaboratorios");
                    Nombre = reader.GetString("nombre_laboratorio");
                }
            }

            Log.Info("Laboratorio del equipo: " + Nombre + " (id " + Id + ").");
            return null;
        }
    }
}
