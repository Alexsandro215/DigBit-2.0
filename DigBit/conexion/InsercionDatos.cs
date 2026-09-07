using MySql.Data.MySqlClient;
using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DigBit
{
    public enum ResultadoInsercionCodigo
    {
        Exito,
        Duplicado,
        Error
    }

    public class InsercionDatos
    {
        private Conexion mconexion;

        public InsercionDatos()
        {
            mconexion = new Conexion();
        }

        private string ObtenerHashMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public bool InsertarUsuario(string nombre, string apellidoPaterno, string apellidoMaterno, string matricula, string contraseña, int tipoUsuario, string correo)
        {
            try
            {
                // Aplicar MD5 a la contraseña
                contraseña = ObtenerHashMD5(contraseña);

                // Realizar la inserción en la base de datos
                string consulta = "INSERT INTO usuarios (nombre, apellido_paterno, apellido_materno, numero_identificador, password, fk_tipo_usuario, correo) " +
                                  "VALUES (@Nombre, @ApellidoPaterno, @ApellidoMaterno, @Matricula, @Password, @TipoUsuario, @correo)";

                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    if (conexion != null)
                    {
                        using (MySqlCommand mysqlcomand = new MySqlCommand(consulta, conexion))
                        {
                            mysqlcomand.Parameters.AddWithValue("@Nombre", nombre);
                            mysqlcomand.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                            mysqlcomand.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                            mysqlcomand.Parameters.AddWithValue("@Matricula", matricula);
                            mysqlcomand.Parameters.AddWithValue("@Password", contraseña);
                            mysqlcomand.Parameters.AddWithValue("@TipoUsuario", tipoUsuario);
                            mysqlcomand.Parameters.AddWithValue("@correo", correo);

                            mysqlcomand.ExecuteNonQuery();

                            return true; // Inserción exitosa
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Error al conectar a la base de datos
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Error durante la inserción
            }
        }
        private string ObtenerHashMD5Profe(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public bool InsertarUsuarioProfe(string nombre, string apellidoPaterno, string apellidoMaterno, string matricula, string contraseña, int tipoUsuario, string correo)
        {
            try
            {
                // Aplicar MD5 a la contraseña
                contraseña = ObtenerHashMD5Profe(contraseña);

                // Realizar la inserción en la base de datos
                string consulta = "INSERT INTO usuarios (nombre, apellido_paterno, apellido_materno, numero_identificador, password, fk_tipo_usuario, correo) " +
                                  "VALUES (@Nombre, @ApellidoPaterno, @ApellidoMaterno, @Matricula, @Password, @TipoUsuario, @correo)";

                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    if (conexion != null)
                    {
                        using (MySqlCommand mysqlcomand = new MySqlCommand(consulta, conexion))
                        {
                            mysqlcomand.Parameters.AddWithValue("@Nombre", nombre);
                            mysqlcomand.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                            mysqlcomand.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                            mysqlcomand.Parameters.AddWithValue("@Matricula", matricula);
                            mysqlcomand.Parameters.AddWithValue("@Password", contraseña);
                            mysqlcomand.Parameters.AddWithValue("@TipoUsuario", tipoUsuario);
                            mysqlcomand.Parameters.AddWithValue("@correo", correo);
                            mysqlcomand.ExecuteNonQuery();

                            return true; // Inserción exitosa
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Error al conectar a la base de datos
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Error durante la inserción
            }
        }

        public bool insertarLaboratorio(string nombreLaboratorio) {
            try {
                string consulta = "INSERT INTO laboratorios (nombre_laboratorio)" + 
                                  "VALUES (@nombreLaboratorio)";
                using (MySqlConnection conexion = mconexion.GetConexion()) {

                    if (conexion != null)
                    {
                        using (MySqlCommand mysqlcomand = new MySqlCommand(consulta, conexion))
                        {

                            mysqlcomand.Parameters.AddWithValue("@nombreLaboratorio", nombreLaboratorio);
                            mysqlcomand.ExecuteNonQuery();
                            return true;
                        }
                    }
                    else {
                        MessageBox.Show("Error al conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Error al conectar a la base de datos
                    }

                }
            
            } 
            catch (Exception ex) 
            {
                MessageBox.Show($"Error al intentar registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Error durante la inserción
            } 
        }

        public bool insertarGrupos(string nombreGrupos)
        {
            try
            {
                string consulta = "INSERT INTO grupos (nombre_grupo)" +
                                  "VALUES (@nombreGrupo)";
                using (MySqlConnection conexion = mconexion.GetConexion())
                {

                    if (conexion != null)
                    {
                        using (MySqlCommand mysqlcomand = new MySqlCommand(consulta, conexion))
                        {

                            mysqlcomand.Parameters.AddWithValue("@nombreGrupo", nombreGrupos);
                            mysqlcomand.ExecuteNonQuery();
                            MessageBox.Show("El grupo se ah registrado de manera correcta", "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Error al conectar a la base de datos
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar registrar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Error durante la inserción
            }
        }

        public ResultadoInsercionCodigo insertarcodigo(string codAleatorio, string hora, int materias, int grupo, int laboratorio, int usuario, string fecha, string hora_entrada, string hora_salida)
        {
            try
            {
                string consulta = "INSERT INTO codigos_accesos (codigo, hora_registro, materias_id_materia, grupos_idgrupos, laboratorios_idlaboratorios, usuarios_idusuarios, fecha, hora_entrada, hora_salida)" +
                                  " VALUES (@codAleatorio, @hora, @materias, @grupo, @laboratorio, @usuario, @fecha, @hora_entrada, @hora_salida)";
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    if (conexion != null)
                    {
                        using (MySqlCommand mysqlCommand = new MySqlCommand(consulta, conexion))
                        {
                            mysqlCommand.Parameters.AddWithValue("@codAleatorio", codAleatorio);
                            mysqlCommand.Parameters.AddWithValue("@hora", hora);
                            mysqlCommand.Parameters.AddWithValue("@materias", materias);
                            mysqlCommand.Parameters.AddWithValue("@grupo", grupo);
                            mysqlCommand.Parameters.AddWithValue("@laboratorio", laboratorio);
                            mysqlCommand.Parameters.AddWithValue("@usuario", usuario);
                            mysqlCommand.Parameters.AddWithValue("@fecha", fecha);
                            mysqlCommand.Parameters.AddWithValue("@hora_entrada", hora_entrada);
                            mysqlCommand.Parameters.AddWithValue("@hora_salida", hora_salida);

                            mysqlCommand.ExecuteNonQuery();
                            return ResultadoInsercionCodigo.Exito;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return ResultadoInsercionCodigo.Error;
                    }
                }

            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                return ResultadoInsercionCodigo.Duplicado;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar registrar el codigo, reintente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return ResultadoInsercionCodigo.Error;
            }
        }
    }
}
