using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using DigBit.Infraestructura;

namespace DigBit
{
    internal class Conexion
    {
        // Entrada de <connectionStrings>; los datos reales viven en connections.config.
        private const string NombreConexion = "DigBit";

        // Permite sobrescribir la cadena sin editar archivos (util para despliegues).
        private const string VariableEntorno = "DIGBIT_CONNECTION_STRING";

        private const string Ayuda = "Copia 'connections.config.example' como 'connections.config' " +
            "junto al ejecutable y coloca ahi los datos de la base de datos.";

        // MySql.Data espera 15 s por defecto antes de rendirse, y todas las consultas
        // van en el hilo de interfaz: con el servidor caido la ventana se congela ese
        // tiempo en cada consulta. En una red local 5 s es de sobra. Si la cadena ya
        // trae su propio timeout, se respeta.
        private const uint TimeoutConexionSegundos = 5;

        private readonly string sCadenaConexion;

        public Conexion()
        {
            sCadenaConexion = AplicarTimeoutPorDefecto(ObtenerCadenaConexion());
        }

        public MySqlConnection GetConexion()
        {
            MySqlConnection conexion = new MySqlConnection(sCadenaConexion);
            conexion.Open();
            return conexion;
        }

        /// <summary>
        /// Abre y cierra una conexion de prueba. Si algo falla (configuracion,
        /// red, credenciales, base inexistente) deja pasar la excepcion original
        /// para que el arranque la muestre con contexto.
        /// </summary>
        public static void Probar()
        {
            string cadena = AplicarTimeoutPorDefecto(ObtenerCadenaConexion());
            Log.Info("Probando conexion: " + DescribirSinSecretos(cadena));
            using (MySqlConnection conexion = new MySqlConnection(cadena))
            {
                conexion.Open();
                using (MySqlCommand comando = new MySqlCommand("SELECT 1", conexion))
                {
                    comando.ExecuteScalar();
                }
            }
        }

        // Para el log: servidor, base, usuario y ajustes efectivos; nunca la contrasena.
        private static string DescribirSinSecretos(string cadena)
        {
            try
            {
                MySqlConnectionStringBuilder c = new MySqlConnectionStringBuilder(cadena);
                return "server=" + c.Server + " port=" + c.Port + " database=" + c.Database
                    + " user=" + c.UserID + " timeout=" + c.ConnectionTimeout + "s sslmode=" + c.SslMode;
            }
            catch (Exception)
            {
                return "(cadena no interpretable por el driver)";
            }
        }

        private static string AplicarTimeoutPorDefecto(string cadena)
        {
            if (Regex.IsMatch(cadena, @"connect(ion)?\s*timeout", RegexOptions.IgnoreCase))
            {
                return cadena;
            }

            try
            {
                MySqlConnectionStringBuilder constructor = new MySqlConnectionStringBuilder(cadena);
                constructor.ConnectionTimeout = TimeoutConexionSegundos;
                return constructor.ConnectionString;
            }
            catch (Exception)
            {
                // Cadena que el constructor no entiende: se deja tal cual para que
                // Open() de el error real en vez de uno de sintaxis aqui.
                return cadena;
            }
        }

        internal static string ObtenerCadenaConexion()
        {
            string desdeEntorno = Environment.GetEnvironmentVariable(VariableEntorno);
            if (!string.IsNullOrEmpty(desdeEntorno))
            {
                return desdeEntorno;
            }

            ConnectionStringSettings ajuste;
            try
            {
                ajuste = ConfigurationManager.ConnectionStrings[NombreConexion];
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new ConfigurationErrorsException("No se pudo leer la configuracion de conexion. " + Ayuda, ex);
            }

            if (ajuste == null || string.IsNullOrEmpty(ajuste.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "Falta la cadena de conexion '" + NombreConexion + "'. " + Ayuda);
            }

            return ajuste.ConnectionString;
        }
    }
}
