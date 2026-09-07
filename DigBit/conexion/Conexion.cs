using MySql.Data.MySqlClient;
using System;
using System.Configuration;

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

        private readonly string sCadenaConexion;

        public Conexion()
        {
            sCadenaConexion = ObtenerCadenaConexion();
        }

        public MySqlConnection GetConexion()
        {
            MySqlConnection conexion = new MySqlConnection(sCadenaConexion);
            conexion.Open();
            return conexion;
        }

        private static string ObtenerCadenaConexion()
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
