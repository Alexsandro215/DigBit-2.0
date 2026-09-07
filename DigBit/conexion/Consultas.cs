using Google.Protobuf;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigBit.conexion
{
    internal class Consultas
    {
        private Conexion mconexion;

        public Consultas()
        {
            mconexion = new Conexion();
            Profesor_Principal profPrincipal = new Profesor_Principal();
        }
        public bool RealizarInicioSesion(string numeroIdentificador, string contraseña)
        {
            mconexion = new Conexion();
            try
            {
                // Conectar a la base de datos
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    if (conexion != null)
                    {
                        // Consulta SQL para obtener el tipo de usuario que coincide con el número de identificador ingresado
                        // y la contraseña (asegúrate de que la contraseña en la base de datos esté almacenada como hash MD5)
                        string consulta = "SELECT fk_tipo_usuario, password FROM usuarios WHERE numero_identificador = @NumeroIdentificador";

                        using (MySqlCommand mysqlcomand = new MySqlCommand(consulta, conexion))
                        {
                            mysqlcomand.Parameters.AddWithValue("@NumeroIdentificador", numeroIdentificador);

                            using (MySqlDataReader mySqldatareader = mysqlcomand.ExecuteReader())
                            {
                                if (mySqldatareader.Read())
                                {
                                    // Obtener la contraseña almacenada en la base de datos
                                    string contraseñaAlmacenada = mySqldatareader[1].ToString();
                                   
                                    // Verificar la contraseña utilizando MD5
                                    if (VerificarContraseñaMD5(contraseña, contraseñaAlmacenada))
                                    {
                                        
                                        // Contraseña válida, obtener el tipo de usuario desde la consulta
                                        int tipoUsuarioId = (int)Convert.ToInt64(mySqldatareader["fk_tipo_usuario"]);

                                        // Establecer el número de identificación en la propiedad de la otra pestaña

                                        // Verificar el tipo de usuario y abrir la pestaña correspondiente
                                        AbrirPestanaSegunTipoUsuario(tipoUsuarioId, numeroIdentificador);

                                        return true;
                                    }
                                    else
                                    {
                                       
                                        MessageBox.Show("Número de identificador o contraseña incorrectos. Por favor, verifica tus credenciales.", "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar realizar la consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        public bool VerificarContraseñaMD5(string contraseñaIngresada, string contraseñaAlmacenada)
        {
            // Obtener el hash MD5 de la contraseña ingresada
            string hashContraseñaIngresada = ObtenerHashMD5(contraseñaIngresada);

            // Log para depuración

            // Comparar el hash de la contraseña ingresada con el almacenado en la base de datos
            return string.Equals(hashContraseñaIngresada, contraseñaAlmacenada, StringComparison.OrdinalIgnoreCase);
        }

        public string ObtenerHashMD5(string input)
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



        public void AbrirPestanaSegunTipoUsuario(int tipoUsuarioId, String numeroIdentificador)
        {
            switch (tipoUsuarioId)
            {
                case 1:
                    Datos_User.SetUser(numeroIdentificador);
                    Alumno_Principal registroAlumnos = new Alumno_Principal();
                    registroAlumnos.Show();
                    ocultarVentana();
                    break;

                case 2:
                    Datos_User.SetUser(numeroIdentificador);
                    Profesor_Principal profPrincipal = new Profesor_Principal();
                    profPrincipal.Show();
                    ocultarVentana();
                    break;

                default:
                    MessageBox.Show($"Tipo de usuario no manejado: {tipoUsuarioId}");
                    break;
            }
        }

        public void ocultarVentana()
        {
            Form ventanaActiva = Form.ActiveForm;

        }

        public UsuarioConInformacionAdicional ConsultaEditar(string numeroIdentificador)
        {
            UsuarioConInformacionAdicional usuarioConInfoAdicional = null;
            mconexion = new Conexion();
            try
            {
                // Conectar a la base de datos
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    if (conexion != null)
                    {
                        // Consulta SQL para obtener la información del usuario con información adicional
                        string consulta = @"SELECT
                    u.idusuarios,
                    u.fk_tipo_usuario,
                    u.nombre,
                    u.apellido_paterno,
                    u.apellido_materno,
                    u.correo,
                    carreras.nombre_carrera AS NombreCarrera,
                    semestres.numero_semestre AS NombreSemestre,
                    grupos.nombre_grupo AS NombreGrupo
                FROM teschi_otru.usuarios AS u
                LEFT JOIN teschi_otru.carrera_grupo_semestre AS cgs ON u.idusuarios = cgs.usuarios_numero_identificador
                LEFT JOIN teschi_otru.carreras AS carreras ON cgs.carreras_idcarreras = carreras.idcarreras
                LEFT JOIN teschi_otru.semestres AS semestres ON cgs.semestres_idsemestres = semestres.idsemestres
                LEFT JOIN teschi_otru.grupos AS grupos ON cgs.grupos_idgrupos = grupos.idgrupos
                WHERE u.numero_identificador = @NumeroIdentificador";

                        using (MySqlCommand mysqlcomand = new MySqlCommand(consulta, conexion))
                        {
                            mysqlcomand.Parameters.AddWithValue("@NumeroIdentificador", numeroIdentificador);

                            using (MySqlDataReader mySqldatareader = mysqlcomand.ExecuteReader())
                            {
                                if (mySqldatareader.Read())
                                {
                                    // Aquí puedes procesar los resultados como lo necesites
                                    usuarioConInfoAdicional = new UsuarioConInformacionAdicional
                                    {
                                        id = mySqldatareader["idusuarios"].ToString(),
                                        TipoUsuarioId = Convert.ToInt32(mySqldatareader["fk_tipo_usuario"]),
                                        Nombre = mySqldatareader["nombre"].ToString(),
                                        ApellidoPaterno = mySqldatareader["apellido_paterno"].ToString(),
                                        ApellidoMaterno = mySqldatareader["apellido_materno"].ToString(),
                                        Correo = mySqldatareader["correo"].ToString(),
                                        NombreCarrera = mySqldatareader["NombreCarrera"].ToString(),
                                        NombreSemestre = mySqldatareader["NombreSemestre"].ToString(),
                                        NombreGrupo = mySqldatareader["NombreGrupo"].ToString(),
                                        // Asigna otros campos si es necesario
                                    };

                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al conectar a la base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar realizar la consulta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return usuarioConInfoAdicional;
        }


        private int NombreCarrera(string nombreCarrera)
        {
            int idCarrera = -1; // Valor predeterminado en caso de que no se encuentre la carrera

            try
            {
                // Realizar la consulta para obtener el ID de la carrera a partir de su nombre
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = "SELECT idcarreras FROM carreras WHERE nombre_carrera = @NombreCarrera";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NombreCarrera", nombreCarrera);
                        var resultado = comando.ExecuteScalar();
                        if (resultado != null)
                        {
                            idCarrera = Convert.ToInt32(resultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el ID de la carrera: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return idCarrera;
        }

        public List<string> ObtenerNombresCarreras()
        {
            List<string> nombresCarreras = new List<string>();

            try
            {
                foreach (OpcionCombo carrera in ObtenerCarreras())
                {
                    nombresCarreras.Add(carrera.Texto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los nombres de las carreras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return nombresCarreras;
        }

        public List<OpcionCombo> ObtenerCarreras()
        {
            List<OpcionCombo> carreras = new List<OpcionCombo>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    const string consulta = "SELECT idcarreras, nombre_carrera FROM carreras ORDER BY nombre_carrera";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            carreras.Add(new OpcionCombo
                            {
                                Id = Convert.ToInt32(reader["idcarreras"]),
                                Texto = reader["nombre_carrera"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener las carreras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return carreras;
        }

        public List<string> ObtenerNombresGrupos()
        {
            List<string> nombreGrupos = new List<string>();

            try
            {
                foreach (OpcionCombo grupo in ObtenerGrupos())
                {
                    nombreGrupos.Add(grupo.Texto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los nombres de los grupos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return nombreGrupos;
        }

        public List<OpcionCombo> ObtenerGrupos()
        {
            List<OpcionCombo> grupos = new List<OpcionCombo>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    const string consulta = "SELECT idgrupos, nombre_grupo FROM grupos ORDER BY nombre_grupo";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grupos.Add(new OpcionCombo
                            {
                                Id = Convert.ToInt32(reader["idgrupos"]),
                                Texto = reader["nombre_grupo"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los grupos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return grupos;
        }

        public List<OpcionCombo> ObtenerGruposPorCarrera(int idCarrera)
        {
            List<OpcionCombo> grupos = new List<OpcionCombo>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    const string consulta = "SELECT idgrupos, nombre_grupo FROM grupos WHERE Carrera_id = @IdCarrera ORDER BY nombre_grupo";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdCarrera", idCarrera);
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                grupos.Add(new OpcionCombo
                                {
                                    Id = Convert.ToInt32(reader["idgrupos"]),
                                    Texto = reader["nombre_grupo"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los grupos de la carrera: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return grupos;
        }


        public List<string> obtenerNombreSemestres()
        {
            List<string> nombreSemestre = new List<string>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = "SELECT numero_semestre FROM semestres"; // Ajusta esto según el nombre de tu tabla
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nombreSemestre.Add(reader["numero_semestre"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los nombres de los grupos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return nombreSemestre;
        }

       public List<string> ObtenerNombresSemestre()
        {
            List<string> nombreSemestre = new List<string>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = "SELECT numero_semestre FROM semestres"; // Ajusta esto según el nombre de tu tabla
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nombreSemestre.Add(reader["numero_semestre"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los nombres de los semestres: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return nombreSemestre;
        }

        public List<string> ObtenerNombresLaboratorio()
        {
            List<string> nombresLaboratorios = new List<string>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = "SELECT nombre_laboratorio FROM laboratorios"; // Ajusta esto según el nombre de tu tabla
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nombresLaboratorios.Add(reader["nombre_laboratorio"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los nombres de las carreras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return nombresLaboratorios;
        }

        public List<string> ObtenerNombresLaboratorios()
        {
            List<string> nombresLaboratorios = new List<string>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = "SELECT nombre_laboratorio FROM laboratorios"; // Ajusta esto según el nombre de tu tabla
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nombresLaboratorios.Add(reader["nombre_laboratorio"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los nombres de las carreras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return nombresLaboratorios;
        }


        public bool ActualizarLabo(string nombreLaboratorio, string nuevoNombre)
        {
            try
            {
                string consultaID = "SELECT idlaboratorios FROM laboratorios WHERE nombre_laboratorio = @nombreLaboratorio";

                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    int idLaboratorio = -1; // Variable para almacenar el ID del laboratorio
                    using (MySqlCommand cmd = new MySqlCommand(consultaID, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombreLaboratorio", nombreLaboratorio);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                idLaboratorio = reader.GetInt32("idlaboratorios");
                            }
                        }
                    }

                    if (idLaboratorio != -1)
                    {
                        string consulta = "UPDATE laboratorios SET nombre_laboratorio = @nuevoNombre WHERE idlaboratorios = @idLaboratorio";
                        using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                        {
                            cmd.Parameters.AddWithValue("@nuevoNombre", nuevoNombre);
                            cmd.Parameters.AddWithValue("@idLaboratorio", idLaboratorio);
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Registro actualizado exitosamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                // Llama a CargarComboBox para actualizar los datos
                                return true;
                            }
                            else
                            {
                                MessageBox.Show($"Error al actualizar el nombre del laboratorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontró ningún laboratorio con ese nombre.");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        }

        public bool buscarCodigoRegistro(string codigoAlumno, string idUsuario)
        {
            try
            {
                string consultaCodigo = "SELECT codigo, hora_registro FROM codigos_accesos WHERE codigo = @codigoAlumno;";
                string consultaRegistro = "SELECT COUNT(*) FROM usuarios WHERE idusuarios = @idUsuario;";

                using (MySqlConnection conexion = mconexion.GetConexion())
                {

                    // Verificar si el código existe en la tabla codigos_accesos
                    using (MySqlCommand cmdCodigo = new MySqlCommand(consultaCodigo, conexion))
                    {
                        cmdCodigo.Parameters.AddWithValue("@codigoAlumno", codigoAlumno);
                        using (MySqlDataReader readerCodigo = cmdCodigo.ExecuteReader())
                        {
                            if (readerCodigo.Read())
                            {
                                DateTime horaRegistro = readerCodigo.GetDateTime("hora_registro");
                                DateTime horaActual = DateTime.Now;

                                TimeSpan diferencia = horaActual - horaRegistro;

                                if (diferencia.TotalMinutes > 8000000)
                                {
                                    MessageBox.Show("Acceso denegado. El código ha expirado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }

                                readerCodigo.Close();

                                // Verificar si el usuario ya se ha registrado con ese código
                                using (MySqlCommand cmdRegistro = new MySqlCommand(consultaRegistro, conexion))
                                {
                                    cmdRegistro.Parameters.AddWithValue("@codigoAlumno", codigoAlumno);
                                    cmdRegistro.Parameters.AddWithValue("@idUsuario", idUsuario);
                                    int count = Convert.ToInt32(cmdRegistro.ExecuteScalar());

                                    if (count > 0)
                                    {
                                        MessageBox.Show("Acceso denegado. El usuario ya ha registrado este código.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Código encontrado y dentro del tiempo permitido", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return true;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Código no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error al buscar el código ingresado: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        public int ObtenerIdSemestre(string nombreSemestre)
        {
            int idSemestre = -1;
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = "SELECT idsemestres FROM semestres WHERE numero_semestre = @NombreSemestre";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NombreSemestre", nombreSemestre);
                        var resultado = comando.ExecuteScalar();
                        if (resultado != null)
                        {
                            idSemestre = Convert.ToInt32(resultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el ID del semestre: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return idSemestre;
        }
         public int ObtenerIdGrupo(string nombreGrupo)
        {
            int idGrupo = -1;
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = "SELECT idgrupos FROM grupos WHERE nombre_grupo = @NombreGrupo";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NombreGrupo", nombreGrupo);
                        var resultado = comando.ExecuteScalar();
                        if (resultado != null)
                        {
                            idGrupo = Convert.ToInt32(resultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el ID del grupo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return idGrupo;
        }
         public int ObtenerIdCarrera(string nombreCarrera)
        {
            int idCarrera = -1;
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = "SELECT idcarreras FROM carreras WHERE nombre_carrera = @NombreCarrera";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NombreCarrera", nombreCarrera);
                        var resultado = comando.ExecuteScalar();
                        if (resultado != null)
                        {
                            idCarrera = Convert.ToInt32(resultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el ID de la carrera: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return idCarrera;
     
        }
        public List<string> ObtenerNombresMaterias()
        {
            List<string> nombresMaterias = new List<string>();

            try
            {
                foreach (OpcionCombo materia in ObtenerMaterias())
                {
                    nombresMaterias.Add(materia.Texto);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error al obtener los nombres de las materia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return nombresMaterias;
        }

        public List<OpcionCombo> ObtenerMaterias()
        {
            List<OpcionCombo> materias = new List<OpcionCombo>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    const string consulta = "SELECT id_materia, nombre_materia FROM materias ORDER BY nombre_materia";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            materias.Add(new OpcionCombo
                            {
                                Id = Convert.ToInt32(reader["id_materia"]),
                                Texto = reader["nombre_materia"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener las materias: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return materias;
        }

        public List<OpcionCombo> ObtenerMateriasPorCarrera(int idCarrera)
        {
            List<OpcionCombo> materias = new List<OpcionCombo>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    const string consulta = "SELECT id_materia, nombre_materia FROM materias WHERE idcarreras = @IdCarrera ORDER BY nombre_materia";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdCarrera", idCarrera);
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                materias.Add(new OpcionCombo
                                {
                                    Id = Convert.ToInt32(reader["id_materia"]),
                                    Texto = reader["nombre_materia"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener las materias de la carrera: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return materias;
        }

         public List<string> ObtenerNombreLaboratorio()
        {
            List<string> nombresLaboratorio = new List<string>();

            try
            {
                foreach (OpcionCombo laboratorio in ObtenerLaboratorios())
                {
                    nombresLaboratorio.Add(laboratorio.Texto);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error al obtener el nombre de los laboratorios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return nombresLaboratorio;
        }

        public List<OpcionCombo> ObtenerLaboratorios()
        {
            List<OpcionCombo> laboratorios = new List<OpcionCombo>();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    const string consulta = "SELECT idlaboratorios, nombre_laboratorio FROM laboratorios ORDER BY nombre_laboratorio";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    using (MySqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            laboratorios.Add(new OpcionCombo
                            {
                                Id = Convert.ToInt32(reader["idlaboratorios"]),
                                Texto = reader["nombre_laboratorio"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los laboratorios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return laboratorios;
        }

        public int ObtenerIdMateria(string nombreMateria)
        {
            int idMateria = -1;
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    const string consulta = "SELECT id_materia FROM materias WHERE nombre_materia = @NombreMateria";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NombreMateria", nombreMateria);
                        object resultado = comando.ExecuteScalar();
                        if (resultado != null)
                        {
                            idMateria = Convert.ToInt32(resultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el ID de la materia: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return idMateria;
        }

        public int ObtenerIdLaboratorio(string nombreLaboratorio)
        {
            int idLaboratorio = -1;
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    const string consulta = "SELECT idlaboratorios FROM laboratorios WHERE nombre_laboratorio = @NombreLaboratorio";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NombreLaboratorio", nombreLaboratorio);
                        object resultado = comando.ExecuteScalar();
                        if (resultado != null)
                        {
                            idLaboratorio = Convert.ToInt32(resultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el ID del laboratorio: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return idLaboratorio;
        }

        public int ObtenerCarreraIdDelUsuario(string numeroIdentificador)
        {
            int idCarrera = -1;

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    const string consulta = @"SELECT cgs.carreras_idcarreras
                                              FROM usuarios u
                                              INNER JOIN carrera_grupo_semestre cgs ON cgs.usuarios_numero_identificador = u.idusuarios
                                              WHERE u.numero_identificador = @NumeroIdentificador
                                              LIMIT 1";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NumeroIdentificador", numeroIdentificador);
                        object resultado = comando.ExecuteScalar();
                        if (resultado != null)
                        {
                            idCarrera = Convert.ToInt32(resultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener la carrera del usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return idCarrera;
        }

         public string MostrarNombreProfesor(string numeroIdentificador)
        {
            string nombreProfesor = "No se encontró el profesor.";
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
            

                    string consulta = "SELECT nombre, apellido_paterno, apellido_materno FROM teschi_otru.usuarios WHERE numero_identificador = @NumeroIdentificador";
                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NumeroIdentificador", numeroIdentificador);
                        
                        using (MySqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Concatenar los nombres y apellidos
                                nombreProfesor = $"{reader["nombre"]} {reader["apellido_paterno"]} {reader["apellido_materno"]}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener el nombre del profesor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return nombreProfesor;
        }




        public bool ActualizarUsuario(string idUsuarios, string nombre, string apellidoPaterno, string apellidoMaterno, string correo, int idCarrera, int idGrupo, int idSemestre)
        {
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = @"UPDATE usuarios
                                    SET nombre = @Nombre, apellido_paterno = @ApellidoPaterno, apellido_materno = @ApellidoMaterno, correo = @Correo
                                    WHERE idusuarios = @idUsuarios;
                                    UPDATE carrera_grupo_semestre
                                    SET carreras_idcarreras = @IdCarrera, grupos_idgrupos = @IdGrupo, semestres_idsemestres = @IdSemestre
                                    WHERE usuarios_numero_identificador = @idUsuarios";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@idUsuarios", idUsuarios);
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        comando.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                        comando.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                        comando.Parameters.AddWithValue("@Correo", correo);
                        comando.Parameters.AddWithValue("@IdCarrera", idCarrera);
                        comando.Parameters.AddWithValue("@IdGrupo", idGrupo);
                        comando.Parameters.AddWithValue("@IdSemestre", idSemestre);

                        int filasAfectadas = comando.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar los datos del usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool ActualizarUsuarioBasico(string idUsuarios, string nombre, string apellidoPaterno, string apellidoMaterno, string correo)
        {
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = @"UPDATE usuarios
                                    SET nombre = @Nombre, apellido_paterno = @ApellidoPaterno, apellido_materno = @ApellidoMaterno, correo = @Correo
                                    WHERE idusuarios = @idUsuarios";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@idUsuarios", idUsuarios);
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        comando.Parameters.AddWithValue("@ApellidoPaterno", apellidoPaterno);
                        comando.Parameters.AddWithValue("@ApellidoMaterno", apellidoMaterno);
                        comando.Parameters.AddWithValue("@Correo", correo);

                        int filasAfectadas = comando.ExecuteNonQuery();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar los datos del usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
 
public int ObtenerIdPorMatricula(int matricula)
        {
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = "SELECT idusuarios FROM usuarios WHERE numero_identificador = @Matricula";
                    using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                    {
                        cmd.Parameters.AddWithValue("@Matricula", matricula);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                        else
                        {
                            throw new Exception("No se encontró el usuario con la matrícula especificada.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, puedes registrar el error o mostrar un mensaje al usuario
                Console.WriteLine("Error al obtener el ID por matrícula: " + ex.Message);
                throw;
            }
        }
public string ObtenerNombreLaboratorioPorCodigo(string codigoLaboratorio)
        {
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    // Primero obtenemos el ID del laboratorio usando el código guardado
                    string consultaId = "SELECT laboratorios_idlaboratorios FROM codigos_accesos WHERE codigo = @Codigo";
                    int idLaboratorio;
                    using (MySqlCommand cmdId = new MySqlCommand(consultaId, conexion))
                    {
                        cmdId.Parameters.AddWithValue("@Codigo", codigoLaboratorio);
                        object resultId = cmdId.ExecuteScalar();
                        if (resultId != null)
                        {
                            idLaboratorio = Convert.ToInt32(resultId);
                        }
                        else
                        {
                            throw new Exception("No se encontró el código especificado en la tabla.");
                        }
                    }

                    // Luego obtenemos el nombre del laboratorio usando el ID obtenido
                    string consultaNombre = "SELECT nombre_laboratorio FROM laboratorios WHERE idLaboratorios = @IdLaboratorio";
                    string nombreLaboratorio;
                    using (MySqlCommand cmdNombre = new MySqlCommand(consultaNombre, conexion))
                    {
                        cmdNombre.Parameters.AddWithValue("@IdLaboratorio", idLaboratorio);
                        object resultNombre = cmdNombre.ExecuteScalar();
                        if (resultNombre != null)
                        {
                            nombreLaboratorio = resultNombre.ToString();
                        }
                        else
                        {
                            throw new Exception("No se encontró el laboratorio con el ID especificado.");
                        }
                    }

                    return nombreLaboratorio;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, puedes registrar el error o mostrar un mensaje al usuario
                Console.WriteLine("Error al obtener el nombre del laboratorio por código: " + ex.Message);
                throw;
            }
        }
        public Usuario ConsultarDatosPdf(string codigoAcceso)
        {
            try
            {
                using (MySqlConnection con = mconexion.GetConexion())
                {
                    string consulta = @"
                SELECT 
                    ca.hora_entrada,
                    ca.hora_salida,
                    ca.usuarios_idusuarios,
                    g.nombre_grupo AS GrupoNombre,
                    m.nombre_materia AS MateriaNombre,
                    ca.hora_registro
                FROM codigos_accesos ca
                INNER JOIN grupos g ON ca.grupos_idgrupos = g.idgrupos
                INNER JOIN materias m ON ca.materias_id_materia = m.id_materia
                WHERE ca.codigo = @Codigo";

                    using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                    {
                        cmd.Parameters.AddWithValue("@Codigo", codigoAcceso);
                         using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var datosPdf = new Usuario
                                {
                                    HoraEntrada = reader["hora_entrada"].ToString(),
                                    HoraSalida = reader["hora_salida"].ToString(),
                                    GruposId = reader["GrupoNombre"].ToString(), // Nombre del grupo
                                    MateriasId = reader["MateriaNombre"].ToString(), // Nombre de la materia
                                    HoraRegistro = DateTime.Parse(reader["hora_registro"].ToString()).ToString("yyyy-MM-dd")
                                };

                                return datosPdf;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error al encontrar los datos: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public DataTable ObtenerCodigosProfesor(string numeroIdentificador)
        {
            DataTable tablaResultado = new DataTable();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = @"
                SELECT 
                    ca.codigo,
                    DATE_FORMAT(ca.hora_registro, '%Y-%m-%d %H:%i:%s') AS fecha_generacion,
                    ca.hora_entrada,
                    ca.hora_salida,
                    m.nombre_materia AS materia,
                    g.nombre_grupo AS grupo,
                    l.nombre_laboratorio AS laboratorio
                FROM codigos_accesos ca
                INNER JOIN usuarios u ON ca.usuarios_idusuarios = u.idusuarios
                INNER JOIN materias m ON ca.materias_id_materia = m.id_materia
                INNER JOIN grupos g ON ca.grupos_idgrupos = g.idgrupos
                INNER JOIN laboratorios l ON ca.laboratorios_idlaboratorios = l.idlaboratorios
                WHERE u.numero_identificador = @NumeroIdentificador
                ORDER BY ca.hora_registro DESC";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NumeroIdentificador", numeroIdentificador);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(comando))
                        {
                            adapter.Fill(tablaResultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los codigos del profesor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return tablaResultado;
        }



        public DateTime ConsultaFecha(string codigoAcceso)
        {
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consultaFecha = "SELECT hora_registro FROM codigos_accesos WHERE codigo = @Codigo";
                    DateTime horaRegistro;
                    using (MySqlCommand cmdCodigo = new MySqlCommand(consultaFecha, conexion))
                    {
                        cmdCodigo.Parameters.AddWithValue("@Codigo", codigoAcceso);
                        object resultCodigo = cmdCodigo.ExecuteScalar();
                        if (resultCodigo != null)
                        {
                            horaRegistro = Convert.ToDateTime(resultCodigo);
                        }
                        else
                        {
                            throw new Exception("No se encontró la fecha.");
                        }
                    }
                    return horaRegistro;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar la consulta: " + ex);
                throw;
            }
        }

        public DataTable consultaRegistro(string codigo)
        {
            DataTable tablaResultado = new DataTable();

            try
            {
                using (MySqlConnection con = mconexion.GetConexion()) // Asumiendo que mconexion.GetConexion() devuelve la conexión
                {
                    string consultaIdCodigo = "SELECT idcodigos_accesos FROM codigos_accesos WHERE codigo = @Codigo";
                    string idcodigoAccess;

                    using (MySqlCommand cmdCodigo = new MySqlCommand(consultaIdCodigo, con))
                    {
                        cmdCodigo.Parameters.AddWithValue("@Codigo", codigo);
                        object resultCodigo = cmdCodigo.ExecuteScalar();

                        if (resultCodigo != null)
                        {
                            idcodigoAccess = resultCodigo.ToString();
                        }
                        else
                        {
                            throw new Exception("No se encontró el código especificado.");
                        }
                    }

                    // Consulta adicional para obtener los detalles del registro
                    string consultaDetalles = @"
                SELECT rb.fk_usuario, u.nombre, u.apellido_paterno, u.apellido_materno, 
                       rb.numero_computadora, rb.comentarios_red, rb.comentarios_hardware, rb.comentarios_software 
                FROM registros_bitacoras rb
                INNER JOIN usuarios u ON rb.fk_usuario = u.idusuarios
                WHERE rb.fk_codigo_accesos = @IdcodigoAccess";

                    using (MySqlCommand cmdDetalles = new MySqlCommand(consultaDetalles, con))
                    {
                        cmdDetalles.Parameters.AddWithValue("@IdcodigoAccess", idcodigoAccess);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmdDetalles);
                        adapter.Fill(tablaResultado); // Llena el DataTable con los resultados de la consulta

                        // Agrega una columna para el nombre completo
                        tablaResultado.Columns.Add("nombre_completo", typeof(string));

                        foreach (DataRow row in tablaResultado.Rows)
                        {
                            string nombreCompleto = $"{row["nombre"]} {row["apellido_paterno"]} {row["apellido_materno"]}";
                            row["nombre_completo"] = nombreCompleto;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, puedes registrar el error o mostrar un mensaje al usuario
                Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
                throw;
            }

            return tablaResultado;
        }

        public DataTable ObtenerBitacorasAdministrador()
        {
            DataTable tablaResultado = new DataTable();

            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = @"
                SELECT
                    ca.codigo,
                    DATE_FORMAT(ca.hora_registro, '%Y-%m-%d %H:%i:%s') AS fecha_generacion,
                    prof.numero_identificador AS numero_empleado,
                    CONCAT_WS(' ', prof.nombre, prof.apellido_paterno, prof.apellido_materno) AS profesor,
                    alumno.numero_identificador AS matricula_alumno,
                    CONCAT_WS(' ', alumno.nombre, alumno.apellido_paterno, alumno.apellido_materno) AS alumno,
                    rb.numero_computadora,
                    COALESCE(rb.falla_red, '') AS falla_red,
                    COALESCE(rb.comentarios_red, '') AS comentarios_red,
                    COALESCE(rb.falla_hardware, '') AS falla_hardware,
                    COALESCE(rb.comentarios_hardware, '') AS comentarios_hardware,
                    COALESCE(rb.falla_software, '') AS falla_software,
                    COALESCE(rb.comentarios_software, '') AS comentarios_software,
                    COALESCE(ca.hora_entrada, '') AS hora_entrada,
                    COALESCE(ca.hora_salida, '') AS hora_salida,
                    COALESCE(g.nombre_grupo, '') AS grupo,
                    COALESCE(m.nombre_materia, '') AS materia,
                    COALESCE(l.nombre_laboratorio, '') AS laboratorio,
                    rb.fk_codigo_accesos,
                    rb.fk_usuario
                FROM registros_bitacoras rb
                INNER JOIN usuarios alumno ON rb.fk_usuario = alumno.idusuarios
                INNER JOIN codigos_accesos ca ON rb.fk_codigo_accesos = ca.idcodigos_accesos
                INNER JOIN usuarios prof ON ca.usuarios_idusuarios = prof.idusuarios
                LEFT JOIN grupos g ON ca.grupos_idgrupos = g.idgrupos
                LEFT JOIN materias m ON ca.materias_id_materia = m.id_materia
                LEFT JOIN laboratorios l ON ca.laboratorios_idlaboratorios = l.idlaboratorios
                ORDER BY ca.hora_registro DESC, alumno.apellido_paterno, alumno.nombre";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(comando))
                    {
                        adapter.Fill(tablaResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener las bitacoras: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return tablaResultado;
        }

        public bool ActualizarRegistroBitacora(
            string fkCodigoAcceso,
            string fkUsuario,
            string numeroOriginal,
            string fallaRedOriginal,
            string comentarioRedOriginal,
            string fallaHardwareOriginal,
            string comentarioHardwareOriginal,
            string fallaSoftwareOriginal,
            string comentarioSoftwareOriginal,
            string nuevoNumeroComputadora,
            string nuevaFallaRed,
            string nuevoComentarioRed,
            string nuevaFallaHardware,
            string nuevoComentarioHardware,
            string nuevaFallaSoftware,
            string nuevoComentarioSoftware)
        {
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    string consulta = @"
                UPDATE registros_bitacoras
                SET
                    numero_computadora = @NuevoNumeroComputadora,
                    falla_red = @NuevaFallaRed,
                    comentarios_red = @NuevoComentarioRed,
                    falla_hardware = @NuevaFallaHardware,
                    comentarios_hardware = @NuevoComentarioHardware,
                    falla_software = @NuevaFallaSoftware,
                    comentarios_software = @NuevoComentarioSoftware
                WHERE fk_codigo_accesos = @FkCodigoAcceso
                    AND fk_usuario = @FkUsuario
                    AND IFNULL(numero_computadora, '') = @NumeroOriginal
                    AND IFNULL(falla_red, '') = @FallaRedOriginal
                    AND IFNULL(comentarios_red, '') = @ComentarioRedOriginal
                    AND IFNULL(falla_hardware, '') = @FallaHardwareOriginal
                    AND IFNULL(comentarios_hardware, '') = @ComentarioHardwareOriginal
                    AND IFNULL(falla_software, '') = @FallaSoftwareOriginal
                    AND IFNULL(comentarios_software, '') = @ComentarioSoftwareOriginal
                LIMIT 1";

                    using (MySqlCommand comando = new MySqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@NuevoNumeroComputadora", nuevoNumeroComputadora ?? string.Empty);
                        comando.Parameters.AddWithValue("@NuevaFallaRed", nuevaFallaRed ?? string.Empty);
                        comando.Parameters.AddWithValue("@NuevoComentarioRed", nuevoComentarioRed ?? string.Empty);
                        comando.Parameters.AddWithValue("@NuevaFallaHardware", nuevaFallaHardware ?? string.Empty);
                        comando.Parameters.AddWithValue("@NuevoComentarioHardware", nuevoComentarioHardware ?? string.Empty);
                        comando.Parameters.AddWithValue("@NuevaFallaSoftware", nuevaFallaSoftware ?? string.Empty);
                        comando.Parameters.AddWithValue("@NuevoComentarioSoftware", nuevoComentarioSoftware ?? string.Empty);
                        comando.Parameters.AddWithValue("@FkCodigoAcceso", fkCodigoAcceso);
                        comando.Parameters.AddWithValue("@FkUsuario", fkUsuario);
                        comando.Parameters.AddWithValue("@NumeroOriginal", numeroOriginal ?? string.Empty);
                        comando.Parameters.AddWithValue("@FallaRedOriginal", fallaRedOriginal ?? string.Empty);
                        comando.Parameters.AddWithValue("@ComentarioRedOriginal", comentarioRedOriginal ?? string.Empty);
                        comando.Parameters.AddWithValue("@FallaHardwareOriginal", fallaHardwareOriginal ?? string.Empty);
                        comando.Parameters.AddWithValue("@ComentarioHardwareOriginal", comentarioHardwareOriginal ?? string.Empty);
                        comando.Parameters.AddWithValue("@FallaSoftwareOriginal", fallaSoftwareOriginal ?? string.Empty);
                        comando.Parameters.AddWithValue("@ComentarioSoftwareOriginal", comentarioSoftwareOriginal ?? string.Empty);

                        int filasAfectadas = comando.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Bitacora actualizada correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }

                        MessageBox.Show("No se pudo actualizar la bitacora. Intenta recargar la lista y vuelve a intentarlo.", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar la bitacora: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        public bool consultaFinal(string codigoAcceso,string matricula, string computadora, string fallared, string comentariored, string fallahardware, string comentariohardware, string fallasotware, string comentariosoftware)
        {
            try
            {
                using (MySqlConnection conexion = mconexion.GetConexion())
                {
                    // Obtener el ID del código de acceso
                    string consultaIdCodigo = "SELECT idcodigos_accesos FROM codigos_accesos WHERE codigo = @Codigo";
                    int idCodigoAcceso;
                    using (MySqlCommand cmdIdCodigo = new MySqlCommand(consultaIdCodigo, conexion))
                    {
                        cmdIdCodigo.Parameters.AddWithValue("@Codigo", codigoAcceso);
                        object resultIdCodigo = cmdIdCodigo.ExecuteScalar();
                        if (resultIdCodigo != null)
                        {
                            idCodigoAcceso = Convert.ToInt32(resultIdCodigo);
                        }
                        else
                        {
                            throw new Exception("No se encontró el código de acceso especificado en la tabla.");
                        }
                    }


                    // Obtener el ID del laboratorio usando el código de acceso
                    string consultaIdusuario = "SELECT idusuarios FROM usuarios WHERE numero_identificador = @matricula";
                    int idusuario;
                    using (MySqlCommand cmdIdLaboratorio = new MySqlCommand(consultaIdusuario, conexion))
                    {
                        cmdIdLaboratorio.Parameters.AddWithValue("@matricula", matricula);
                        object resultIdusuario = cmdIdLaboratorio.ExecuteScalar();
                        if (resultIdusuario != null)
                        {
                            idusuario = Convert.ToInt32(resultIdusuario);
                        }
                        else
                        {
                            throw new Exception("No se encontró el laboratorio para el código de acceso especificado.");
                        }
                    }


                    // Insertar en la tabla registros_bitacoras
                    string consultaInsertar = "INSERT INTO registros_bitacoras (fk_codigo_accesos, fk_usuario, numero_computadora, falla_red, comentarios_red, falla_hardware, comentarios_hardware, falla_software, comentarios_software) VALUES (@IdCodigoAcceso, @codigo_usuario, @computadora, @fallared, @comentariored, @fallahardware, @comentariohardware, @fallasoftware, @comentariosoftware)";
                    using (MySqlCommand cmdInsertar = new MySqlCommand(consultaInsertar, conexion))
                    {
                        cmdInsertar.Parameters.AddWithValue("@IdCodigoAcceso", idCodigoAcceso);
                        cmdInsertar.Parameters.AddWithValue("@codigo_usuario", idusuario);
                        cmdInsertar.Parameters.AddWithValue("@computadora", computadora);
                        cmdInsertar.Parameters.AddWithValue("@fallared", fallared);
                        cmdInsertar.Parameters.AddWithValue("@comentariored", comentariored);
                        cmdInsertar.Parameters.AddWithValue("@fallahardware", fallahardware);
                        cmdInsertar.Parameters.AddWithValue("@comentariohardware", comentariohardware);
                        cmdInsertar.Parameters.AddWithValue("@fallasoftware", fallasotware);
                        cmdInsertar.Parameters.AddWithValue("@comentariosoftware", comentariosoftware);

                        int filasAfectadas = cmdInsertar.ExecuteNonQuery();


                        if (filasAfectadas <= 0)
                        {
                            throw new Exception("No se pudo insertar el registro en la bitácora.");
                        }
                        else {
                            MessageBox.Show($" Se inserto de manera exitosa el registro", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, puedes registrar el error o mostrar un mensaje al usuario
                MessageBox.Show($"Error al insertar el registro en la bitacora: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //Consulta para buscar los datos de identificacion de un usuario PARA DESPUES ELIMINARLO.
        public (string Nombre, string ApellidoPaterno, string ApellidoMaterno) BuscarParaEliminar(string matricula)
        {
            try
            {
                using (MySqlConnection con = mconexion.GetConexion())
                {
                    string consulta = "SELECT nombre, apellido_paterno, apellido_materno FROM usuarios WHERE numero_identificador = @Matricula";

                    using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                    {
                        cmd.Parameters.AddWithValue("@Matricula", matricula);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string nombre = reader["nombre"].ToString();
                                string apellidoPaterno = reader["apellido_paterno"].ToString();
                                string apellidoMaterno = reader["apellido_materno"].ToString();

                                return (nombre, apellidoPaterno, apellidoMaterno);
                            }
                            else
                            {
                                return (null, null, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (puedes loguear el error o lanzar una excepción personalizada)
                throw new Exception("Ocurrió un error al buscar el usuario", ex);
            }
        }

        //FUNCION PARA ELIMINAR UN USUARIO
        public void EliminarUsuario(string matricula)
        {
            try
            {
                using (MySqlConnection con = mconexion.GetConexion())
                {
                    string consulta = "DELETE FROM usuarios WHERE numero_identificador = @Matricula";

                    using (MySqlCommand cmd = new MySqlCommand(consulta, con))
                    {
                        cmd.Parameters.AddWithValue("@Matricula", matricula);
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Usuario eliminado con exito.", "información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Console.WriteLine("No se encontró un usuario con la matrícula proporcionada.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (puedes loguear el error o lanzar una excepción personalizada)
                throw new Exception("Ocurrió un error al eliminar el usuario", ex);
            }
        }

        //Consulta para obtener el ID del usuario
        public string idusuario(string matricula)
        {
            try
            {
                string consulta = "SELECT idusuarios FROM usuarios WHERE numero_identificador = @matricula;";
                using (MySqlConnection conexion = mconexion.GetConexion())
                {   
                    using (MySqlCommand cmd = new MySqlCommand(consulta, conexion))
                    {
                        cmd.Parameters.AddWithValue("@matricula", matricula);
                        object resultado = cmd.ExecuteScalar();
                        if (resultado != null)
                        {
                            return resultado.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Matrícula no encontrada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error al buscar la matrícula: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }



    }

}

//SELECT nombre_carrera FROM carreras;
