using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigBit
{
    internal class Conexion
    {
        //Se dan los valores bases para la conexion
        private MySqlConnection conexion;
        private string sServer = "localhost";
        private string sDatabase = "digbit"; //nombre de la base de datos 
        private string sUser = "root"; // usuario
        private string SPassword = "1234"; // contraseña 
        private string sCadenaConexion;


        public Conexion()
        {
            //Se crea una cadena que sera la que valide el 
            sCadenaConexion = "Database=" + sDatabase + "; DataSource= " + sServer + "; User Id=" + sUser + "; Password= " + SPassword;
        }

        public MySqlConnection GetConexion()
        {
            if (conexion == null)
            {
                conexion = new MySqlConnection(sCadenaConexion);
                conexion.Open();

            }
            return conexion;
        }
    }
}
