using MySql.Data.MySqlClient;
using DigBit.conexion;
using System;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DigBit
{
    public class InsercionDatos
    {
        private Conexion mconexion;

        public InsercionDatos()
        {
            mconexion = new Conexion();
        }

        public bool InsertarUsuario(string nombre, string apellidoPaterno, string apellidoMaterno, string matricula, string contraseña, int tipoUsuario, string correo)
        {
            try
            {
                // PBKDF2 con sal aleatoria (ver Contrasenas): nunca la contrasena en claro.
                contraseña = Contrasenas.Hash(contraseña);

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
        /// <summary>
        /// Registra un alumno CON su carrera y grupo (carrera_grupo_semestre), en
        /// una transaccion. Antes RegistroAlumnos pedia carrera y grupo pero nunca
        /// los guardaba; desde la fase 6 hacen falta para marcar en el informe al
        /// los informes por grupo. Devuelve el mensaje de error, o
        /// null si todo fue bien.
        /// </summary>
        public string InsertarAlumno(string nombre, string apellidoPaterno, string apellidoMaterno, string matricula, string contraseña, string correo, int idCarrera, int idGrupo)
        {
            string hash = Contrasenas.Hash(contraseña);
            using (MySqlConnection conexion = mconexion.GetConexion())
            using (MySqlTransaction transaccion = conexion.BeginTransaction())
            {
                try
                {
                    int idUsuario;
                    using (MySqlCommand usuario = new MySqlCommand(
                        "INSERT INTO usuarios (nombre, apellido_paterno, apellido_materno, numero_identificador, password, fk_tipo_usuario, correo) " +
                        "VALUES (@Nombre, @ApellidoPaterno, @ApellidoMaterno, @Matricula, @Password, 1, @Correo); SELECT LAST_INSERT_ID();", conexion, transaccion))
                    {
                        usuario.Parameters.AddWithValue("@Nombre", nombre);
                        usuario.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                        usuario.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                        usuario.Parameters.AddWithValue("@Matricula", matricula);
                        usuario.Parameters.AddWithValue("@Password", hash);
                        usuario.Parameters.AddWithValue("@Correo", correo);
                        idUsuario = Convert.ToInt32(usuario.ExecuteScalar());
                    }

                    using (MySqlCommand grupo = new MySqlCommand(
                        "INSERT INTO carrera_grupo_semestre (usuarios_numero_identificador, carreras_idcarreras, grupos_idgrupos, semestres_idsemestres) " +
                        "VALUES (@Usuario, @Carrera, @Grupo, NULL)", conexion, transaccion))
                    {
                        grupo.Parameters.AddWithValue("@Usuario", idUsuario);
                        grupo.Parameters.AddWithValue("@Carrera", idCarrera);
                        grupo.Parameters.AddWithValue("@Grupo", idGrupo);
                        grupo.ExecuteNonQuery();
                    }

                    transaccion.Commit();
                    return null;
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    transaccion.Rollback();
                    return "Ya existe un usuario con la matricula " + matricula + ".";
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    return ex.Message;
                }
            }
        }

        public bool InsertarUsuarioProfe(string nombre, string apellidoPaterno, string apellidoMaterno, string matricula, string contraseña, int tipoUsuario, string correo)
        {
            try
            {
                // PBKDF2 con sal aleatoria (ver Contrasenas): nunca la contrasena en claro.
                contraseña = Contrasenas.Hash(contraseña);

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

        // hora_registro, fecha y hora_entrada las pone el SERVIDOR (NOW, CURDATE,
        // CURTIME): son la ventana de validez del codigo y no pueden depender del
        // reloj del equipo del profesor. Solo la hora de salida viaja como parametro.    }
}
