using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using BE_VR750;
using SERVICIOS_VR750;

namespace DAL_VR750
{
    public class DALusuario_750VR
    {
        BaseDeDatos_750VR db { get; }
        Encriptador_VR750 hasher { get; set; }

        public DALusuario_750VR()
        {
            db = new BaseDeDatos_750VR();
            hasher = new Encriptador_VR750();
        }

        public void CrearUsuario(Usuario_750VR usuario) //alta user
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"INSERT INTO Usuario_VR750 
        (DNI_VR750, Nombre_VR750, Apellido_VR750, Email_VR750, Usuario_VR750, Contra_VR750, Salt_VR750, Rol_VR750, Activo_VR750, Bloqueado_VR750) 
        VALUES (@DNI, @Nombre, @Apellido, @Email, @Usuario, @Contra, @Salt, @Rol, @Activo, @Bloqueado)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", usuario.dni);
                cmd.Parameters.AddWithValue("@Nombre", usuario.nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.apellido);
                cmd.Parameters.AddWithValue("@Email", usuario.mail);
                cmd.Parameters.AddWithValue("@Usuario", usuario.user);
                cmd.Parameters.AddWithValue("@Contra", usuario.contraseña);
                cmd.Parameters.AddWithValue("@Salt", usuario.salt);
                cmd.Parameters.AddWithValue("@Rol", usuario.rol);
                cmd.Parameters.AddWithValue("@Activo", usuario.activo);
                cmd.Parameters.AddWithValue("@Bloqueado", usuario.bloqueado);

                cmd.ExecuteNonQuery();
            }
        }


        public bool ModificarUsuario(int dni, string nombre, string apellido, string mail, string rol, bool activo) //mod user
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"UPDATE Usuario_VR750 
                         SET Nombre_VR750 = @Nombre, Apellido_VR750 = @Apellido, Email_VR750 = @Mail, Rol_VR750 = @Rol
                         WHERE DNI = @DNI";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@Mail", mail);
                cmd.Parameters.AddWithValue("@Rol", rol);
                cmd.Parameters.AddWithValue("@DNI", dni);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CambiarEstadoUsuario(int dni, bool nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE Usuario_VR750 SET Activo_VR750 = @Activo WHERE DNI_VR750 = @DNI";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Activo", nuevoEstado ? 1 : 0);
                cmd.Parameters.AddWithValue("@DNI", dni);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DesbloquearUsuario(int dni) //desb
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE Usuario_VR750 SET Bloqueado_VR750 = 0 WHERE DNI_VR750 = @DNI";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", dni);

                int filasAfectadas = cmd.ExecuteNonQuery();
                return filasAfectadas > 0; // Devuelve true si al menos una fila fue actualizada
            }
        }

        public List<Usuario_750VR> ObtenerUsuarios(bool soloActivos) //obtengo usuarios activos
        {
            List<Usuario_750VR> lista = new List<Usuario_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = soloActivos
                    ? "SELECT * FROM Usuario_VR750 WHERE Activo_VR750 = 1"
                    : "SELECT * FROM Usuario_VR750";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Usuario_750VR user = new Usuario_750VR
                    {
                        dni = Convert.ToInt32(reader["DNI_VR750"]),
                        nombre = reader["Nombre_VR750"].ToString(),
                        apellido = reader["Apellido_VR750"].ToString(),
                        mail = reader["Email_VR750"].ToString(),
                        user = reader["Usuario_VR750"].ToString(),
                        rol = reader["Rol_VR750"].ToString(),
                        activo = Convert.ToBoolean(reader["Activo_VR750"]),
                        bloqueado = Convert.ToBoolean(reader["Bloqueado_VR750"])
                    };

                    lista.Add(user);
                }
            }

            return lista;
        }


        public List<Usuario_750VR> BuscarUsuarios(string dni, string nombre, string apellido, string email, string user, string rol)
        {
            List<Usuario_750VR> lista = new List<Usuario_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string query = "SELECT * FROM Usuario_VR750 WHERE 1=1";

                if (!string.IsNullOrEmpty(dni))
                    query += " AND CAST(DNI_VR750 AS VARCHAR) LIKE @DNI";

                if (!string.IsNullOrEmpty(nombre))
                    query += " AND Nombre_VR750 LIKE @Nombre";

                if (!string.IsNullOrEmpty(apellido))
                    query += " AND Apellido_VR750 LIKE @Apellido";

                if (!string.IsNullOrEmpty(email))
                    query += " AND Email_VR750 LIKE @Email";

                if (!string.IsNullOrEmpty(user))
                    query += " AND Usuario_VR750 LIKE @Usuario";

                if (!string.IsNullOrEmpty(rol))
                    query += " AND Rol_VR750 LIKE @Rol";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(dni))
                    cmd.Parameters.AddWithValue("@DNI", "%" + dni + "%");

                if (!string.IsNullOrEmpty(nombre))
                    cmd.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");

                if (!string.IsNullOrEmpty(apellido))
                    cmd.Parameters.AddWithValue("@Apellido", "%" + apellido + "%");

                if (!string.IsNullOrEmpty(email))
                    cmd.Parameters.AddWithValue("@Email", "%" + email + "%");

                if (!string.IsNullOrEmpty(user))
                    cmd.Parameters.AddWithValue("@Usuario", "%" + user + "%");

                if (!string.IsNullOrEmpty(rol))
                    cmd.Parameters.AddWithValue("@Rol", "%" + rol + "%");

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Usuario_750VR userr = new Usuario_750VR
                    {
                        dni = Convert.ToInt32(reader["DNI_VR750"]),
                        nombre = reader["Nombre_VR750"].ToString(),
                        apellido = reader["Apellido_VR750"].ToString(),
                        mail = reader["Email_VR750"].ToString(),
                        user = reader["Usuario_VR750"].ToString(),
                        contraseña = reader["Contra_VR750"].ToString(),
                        salt = reader["Salt_VR750"].ToString(),
                        rol = reader["Rol_VR750"].ToString(),
                        activo = Convert.ToBoolean(reader["Activo_VR750"]),
                        bloqueado = Convert.ToBoolean(reader["Bloqueado_VR750"])
                    };

                    lista.Add(userr);
                }
            }

            return lista;
        }
        public List<Usuario_750VR> leerEntidades() //busca todos
        {
            List<Usuario_750VR> lista = new List<Usuario_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Usuario_VR750";  // SIN filtros dinámicos

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Usuario_750VR user = new Usuario_750VR
                    {
                        dni = Convert.ToInt32(reader["DNI_VR750"]),
                        nombre = reader["Nombre_VR750"].ToString(),
                        apellido = reader["Apellido_VR750"].ToString(),
                        mail = reader["Email_VR750"].ToString(),
                        user = reader["Usuario_VR750"].ToString(),
                        contraseña = reader["Contra_VR750"].ToString(),
                        salt = reader["Salt_VR750"].ToString(),
                        rol = reader["Rol_VR750"].ToString(),
                        activo = Convert.ToBoolean(reader["Activo_VR750"]),
                        bloqueado = Convert.ToBoolean(reader["Bloqueado_VR750"])
                    };

                    lista.Add(user);
                }
            }

            return lista;
        }

        public Usuario_750VR ObtenerUsuarioPorLogin(string usuarioLogin)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Usuario_VR750 WHERE Usuario_VR750 = @Usuario";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario", usuarioLogin);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario_750VR
                        {
                            dni = Convert.ToInt32(reader["DNI_VR750"]),
                            nombre = reader["Nombre_VR750"].ToString(),
                            apellido = reader["Apellido_VR750"].ToString(),
                            mail = reader["Email_VR750"].ToString(),
                            user = reader["Usuario_VR750"].ToString(),
                            contraseña = reader["Contra_VR750"].ToString(),
                            salt = reader["Salt_VR750"].ToString(),
                            rol = reader["Rol_VR750"].ToString(),
                            activo = Convert.ToBoolean(reader["Activo_VR750"]),
                            bloqueado = Convert.ToBoolean(reader["Bloqueado_VR750"])
                        };
                    }
                }
            }
            return null; // no encontrado


        }

        public Usuario_750VR ObtenerUsuarioPorDNI(int dni)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Usuario_VR750 WHERE DNI_VR750 = @DNI";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", dni);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Usuario_750VR user = new Usuario_750VR
                    {
                        dni = Convert.ToInt32(reader["DNI_VR750"]),
                        nombre = reader["Nombre_VR750"].ToString(),
                        apellido = reader["Apellido_VR750"].ToString(),
                        mail = reader["Email_VR750"].ToString(),
                        user = reader["Usuario_VR750"].ToString(),
                        contraseña = reader["Contra_VR750"].ToString(),
                        salt = reader["Salt_VR750"].ToString(),
                        rol = reader["Rol_VR750"].ToString(),
                        activo = Convert.ToBoolean(reader["Activo_VR750"]),
                        bloqueado = Convert.ToBoolean(reader["Bloqueado_VR750"])
                    };
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public void BloquearUsuario(string usuarioLogin)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE Usuario_VR750 SET Bloqueado_VR750 = 1 WHERE Usuario_VR750 = @Usuario";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario", usuarioLogin);

                cmd.ExecuteNonQuery();
            }
        }

        public Resultado_VR750<Usuario_750VR> recuperarUsuario(string user, string contraseña)
        {
            Resultado_VR750<Usuario_750VR> resultado = new Resultado_VR750<Usuario_750VR>();

            string sqlQuery = "USE ProyectoNailsTime_VR750; SELECT * FROM Usuario_VR750 WHERE Usuario_VR750 = @Usuario";

            try
            {
                if (!db.Conectar())
                    throw new Exception("Error al conectar a la base de datos.");

                using (SqlCommand command = new SqlCommand(sqlQuery, db.Connection))
                {
                    command.Parameters.AddWithValue("@Usuario", user);

                    using (SqlDataReader lector = command.ExecuteReader())
                    {
                        if (!lector.HasRows)
                            throw new Exception("Usuario no encontrado.");

                        if (lector.Read())
                        {
                            // Recuperar campos
                            int dni = lector.GetInt32(0);
                            string nombre = lector.GetString(1);
                            string apellido = lector.GetString(2);
                            string email = lector.GetString(3);
                            string usuarioDB = lector.GetString(4);
                            string contraseñaHashAlmacenada = lector.GetString(5);
                            string saltAlmacenado = lector.GetString(6);
                            string rol = lector.GetString(7);
                            bool activo = lector.GetBoolean(8);
                            bool bloqueado = lector.GetBoolean(9);

                            if (!activo)
                                throw new Exception("El usuario está inactivo.");

                            if (bloqueado)
                                throw new Exception("El usuario está bloqueado.");

                            // Encriptar contraseña ingresada
                            string hashIngresado = hasher.HashearConSalt(contraseña, saltAlmacenado);

                            if (hashIngresado != contraseñaHashAlmacenada)
                                throw new Exception("Contraseña incorrecta.");

                            // Crear entidad
                            Usuario_750VR usuario = new Usuario_750VR
                            {
                                dni = dni,
                                nombre = nombre,
                                apellido = apellido,
                                mail = email,
                                user = usuarioDB,
                                contraseña = contraseñaHashAlmacenada,
                                salt = saltAlmacenado,
                                rol = rol,
                                activo = activo,
                                bloqueado = bloqueado
                            };

                            resultado.resultado = true;
                            resultado.entidad = usuario;
                            resultado.mensaje = "Login exitoso";
                        }
                    }
                }

                db.Desconectar();
                return resultado;
            }
            catch (Exception ex)
            {
                resultado.resultado = false;
                resultado.mensaje = ex.Message;
                resultado.entidad = null;
                db.Desconectar();
                return resultado;
            }
        }

        public void CambiarContraseña(Usuario_750VR usuario, string NuevaContraseña)
        {
        

            string sqlUpdateUsuario = "USE ProyectoNailsTime_VR750; UPDATE Usuario_VR750 SET Contra_VR750 = @Contraseña, Salt_VR750 = @Salt WHERE DNI_VR750 = @DNI";

            try
            {
                bool conectado = db.Conectar();
                if (!conectado) throw new Exception("Error al conectarse a la base de datos");

                string NuevoSalt = hasher.GenerarSalt();
                string nuevaContraseñaHasheada = hasher.HashearConSalt(NuevaContraseña, NuevoSalt);

                using (SqlCommand updateCommand = new SqlCommand(sqlUpdateUsuario, db.Connection))
                {
                    updateCommand.Parameters.AddWithValue("@Contraseña", nuevaContraseñaHasheada);
                    updateCommand.Parameters.AddWithValue("@Salt", NuevoSalt);
                    updateCommand.Parameters.AddWithValue("@DNI", usuario.dni);

                    int filasAfectadas = updateCommand.ExecuteNonQuery();
                    if (filasAfectadas == 0)
                    {
                        throw new Exception("No se encontró el usuario para actualizar la contraseña.");
                    }
                }
            }
            finally
            {
                db.Desconectar();
            }



        }
    }
}
