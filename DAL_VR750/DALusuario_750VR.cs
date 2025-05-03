using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using BE_VR750;
using SERVICIOS_VR750;

namespace DAL_VR750
{
    public class DALusuario_750VR
    {
        BaseDeDatos_750VR db { get; }
        //Encriptador_VR750 hasher { get; set; }

        public DALusuario_750VR()
        {
            db = new BaseDeDatos_750VR();
            //hasher = new Encriptador_VR750();
        }
        public void CrearUsuario(Usuario_750VR usuario) //alta user
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"INSERT INTO Usuario_VR750 
        (DNI, Nombre, Apellido, Email, Usuario, Contra, Salt, Rol, Activo, Bloqueado) 
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


        public void ModificarUsuario(Usuario_750VR usuario) //no puse q se cambie el activo o el bloqueo
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"UPDATE Usuario_VR750 
                        SET Nombre = @Nombre, 
                            Apellido = @Apellido, 
                            Email = @Email, 
                            Rol = @Rol, 
                            Activo = @Activo
                        WHERE DNI = @DNI";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", usuario.nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.apellido);
                cmd.Parameters.AddWithValue("@Email", usuario.mail);
                cmd.Parameters.AddWithValue("@Rol", usuario.rol);
                cmd.Parameters.AddWithValue("@Activo", usuario.activo); //esto creo q no
                cmd.Parameters.AddWithValue("@DNI", usuario.dni);

                cmd.ExecuteNonQuery();
            }
        }

        public bool BorrarUsuarioLogico(string dni) //inactivos 
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE Usuarios SET Activo = 0 WHERE DNI = @dni";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dni", dni);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public void DesbloquearUsuario(int dni)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE Usuario_VR750 SET Bloqueado = 0 WHERE DNI = @DNI";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", dni);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Usuario_750VR> ObtenerUsuarios(bool soloActivos) //obtengo usuarios activos
        {
            List<Usuario_750VR> lista = new List<Usuario_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = soloActivos
                    ? "SELECT * FROM Usuarios WHERE Activo = 1"
                    : "SELECT * FROM Usuarios";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Usuario_750VR user = new Usuario_750VR
                    {
                        dni = Convert.ToInt32(reader["DNI"]),
                        nombre = reader["Nombre"].ToString(),
                        apellido = reader["Apellido"].ToString(),
                        mail = reader["Email"].ToString(),
                        user = reader["Usuario"].ToString(),
                        rol = reader["Rol"].ToString(),
                        activo = Convert.ToBoolean(reader["Activo"]),
                        bloqueado = Convert.ToBoolean(reader["Bloqueado"])
                    };

                    lista.Add(user);
                }
            }

            return lista;
        }


        //hacerlo mejor
        public List<Usuario_750VR> BuscarUsuarios(string dni, string nombre, string apellido, string email) //busca todos 
        {
            List<Usuario_750VR> lista = new List<Usuario_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Usuarios WHERE 1=1";

                if (!string.IsNullOrEmpty(dni)) query += " AND DNI LIKE @DNI";
                if (!string.IsNullOrEmpty(nombre)) query += " AND Nombre LIKE @Nombre";
                if (!string.IsNullOrEmpty(apellido)) query += " AND Apellido LIKE @Apellido";
                if (!string.IsNullOrEmpty(email)) query += " AND Email LIKE @Email";

                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(dni)) cmd.Parameters.AddWithValue("@DNI", "%" + dni + "%");
                if (!string.IsNullOrEmpty(nombre)) cmd.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");
                if (!string.IsNullOrEmpty(apellido)) cmd.Parameters.AddWithValue("@Apellido", "%" + apellido + "%");
                if (!string.IsNullOrEmpty(email)) cmd.Parameters.AddWithValue("@Email", "%" + email + "%");

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Usuario_750VR user = new Usuario_750VR
                    {
                        dni = Convert.ToInt32(reader["DNI"]),
                        nombre = reader["Nombre"].ToString(),
                        apellido = reader["Apellido"].ToString(),
                        mail = reader["Email"].ToString(),
                        user = reader["Usuario"].ToString(),
                        contraseña = reader["Contra"].ToString(),
                        salt = reader["Salt"].ToString(),
                        rol = reader["Rol"].ToString(),
                        activo = Convert.ToBoolean(reader["Activo"]),
                        bloqueado = Convert.ToBoolean(reader["Bloqueo"]),

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

                string query = "SELECT * FROM Usuario_VR750 WHERE Usuario = @Usuario";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario", usuarioLogin);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Usuario_750VR
                    {
                        dni = Convert.ToInt32(reader["DNI"]),
                        nombre = reader["Nombre"].ToString(),
                        apellido = reader["Apellido"].ToString(),
                        mail = reader["Email"].ToString(),
                        user = reader["Usuario"].ToString(),
                        contraseña = reader["Contra"].ToString(),
                        salt = reader["Salt"].ToString(),
                        rol = reader["Rol"].ToString(),
                        activo = Convert.ToBoolean(reader["Activo"]),
                        bloqueado = Convert.ToBoolean(reader["Bloqueado"])
                    };
                }
                return null;
            }
        
            
        }

        public Usuario_750VR ObtenerUsuarioPorDNI(int dni)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Usuario_VR750 WHERE DNI = @DNI";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", dni);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Usuario_750VR user = new Usuario_750VR
                    {
                        dni = Convert.ToInt32(reader["DNI"]),
                        nombre = reader["Nombre"].ToString(),
                        apellido = reader["Apellido"].ToString(),
                        mail = reader["Email"].ToString(),
                        user = reader["Usuario"].ToString(),
                        contraseña = reader["Contra"].ToString(),
                        salt = reader["Salt"].ToString(),
                        rol = reader["Rol"].ToString(),
                        activo = Convert.ToBoolean(reader["Activo"]),
                        bloqueado = Convert.ToBoolean(reader["Bloqueado"])
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
                string query = "UPDATE Usuario_VR750 SET Bloqueado = 1 WHERE Usuario = @Usuario";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Usuario", usuarioLogin);

                cmd.ExecuteNonQuery();
            }
        }

        public Resultado_VR750<Usuario_750VR> recuperarUsuario(string user, string contraseña)
        {
            Resultado_VR750<Usuario_750VR> resultado = new Resultado_VR750<Usuario_750VR>();

            string sqlQuery = "USE Proyecto_NailsTime_750VR; SELECT * FROM Usuario_VR750 WHERE Usuario = @Usuario";

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
                            string hashIngresado = Encriptador_VR750.HashearConSalt(contraseña, saltAlmacenado);

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
    }
}
