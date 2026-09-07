using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigBit.conexion
{
    internal class Borrar
    {
        private Conexion mconexion;
        public Borrar()
        {
            mconexion = new Conexion();
        }
        public bool borrarlabo(string numerolaboratorio)
        {
            try
            {

                string consulta = "DELETE FROM laboratorios WHERE nombre_laboratorio = @numerolaboratorio";
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                    {
                        cmd.Parameters.AddWithValue("@numerolaboratorio", numerolaboratorio);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registro eliminado exitosamente.");
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ningún registro con ese ID.");
                            return false;
                        }
                    }
                }
            }
                catch (Exception e)
                                    {
                Console.WriteLine("Error: " + e.Message);
                return false;
            } 
        }
    }
}
