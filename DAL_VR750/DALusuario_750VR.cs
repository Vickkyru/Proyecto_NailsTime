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
        Encriptador_750VR hasher { get; set; }

        public DALusuario_750VR()
        {
            db = new BaseDeDatos_750VR();
            hasher = new Encriptador_750VR();
        }

        public void CrearUsuario_750VR(BEusuario_750VR usuario) //alta user
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"INSERT INTO Usuario_VR750 
        (DNI_VR750, Nombre_VR750, Apellido_VR750, Email_VR750, Usuario_VR750, Contra_VR750, Salt_VR750, Rol_VR750, Activo_VR750, Bloqueado_VR750) 
        VALUES (@DNI, @Nombre, @Apellido, @Email, @Usuario, @Contra, @Salt, @Rol, @Activo, @Bloqueado)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", usuario.dni_750VR);
                cmd.Parameters.AddWithValue("@Nombre", usuario.nombre_750VR);
                cmd.Parameters.AddWithValue("@Apellido", usuario.apellido_750VR);
                cmd.Parameters.AddWithValue("@Email", usuario.mail_750VR);
                cmd.Parameters.AddWithValue("@Usuario", usuario.user_750VR);
                cmd.Parameters.AddWithValue("@Contra", usuario.contraseña_750VR);
                cmd.Parameters.AddWithValue("@Salt", usuario.salt_750VR);
                cmd.Parameters.AddWithValue("@Rol", usuario.rol_750VR);
                cmd.Parameters.AddWithValue("@Activo", usuario.activo_750VR);
                cmd.Parameters.AddWithValue("@Bloqueado", usuario.bloqueado_750VR);

                cmd.ExecuteNonQuery();
            }
        }


        public bool ModificarUsuario_750VR(int dni, string nombre, string apellido, string mail, string rol, string usuario) //mod user
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"UPDATE Usuario_VR750 
                         SET Nombre_VR750 = @Nombre, Apellido_VR750 = @Apellido, Email_VR750 = @Mail, Rol_VR750 = @Rol, Usuario_VR750 = @Usuario
                         WHERE DNI_VR750 = @DNI";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@Mail", mail);
                cmd.Parameters.AddWithValue("@Rol", rol);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@DNI", dni);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CambiarEstadoUsuario_750VR(int dni, bool nuevoEstado)
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

        public bool DesbloquearUsuario_750VR(int dni) //desb
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

      

        public List<BEusuario_750VR> BuscarUsuarios_750VR(string dni, string nombre, string apellido, string email, string user, string rol)
        {
            List<BEusuario_750VR> lista = new List<BEusuario_750VR>();

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
                    BEusuario_750VR userr = new BEusuario_750VR
                    {
                        dni_750VR = Convert.ToInt32(reader["DNI_VR750"]),
                        nombre_750VR = reader["Nombre_VR750"].ToString(),
                        apellido_750VR = reader["Apellido_VR750"].ToString(),
                        mail_750VR = reader["Email_VR750"].ToString(),
                        user_750VR = reader["Usuario_VR750"].ToString(),
                        contraseña_750VR = reader["Contra_VR750"].ToString(),
                        salt_750VR = reader["Salt_VR750"].ToString(),
                        rol_750VR = reader["Rol_VR750"].ToString(),
                        activo_750VR = Convert.ToBoolean(reader["Activo_VR750"]),
                        bloqueado_750VR = Convert.ToBoolean(reader["Bloqueado_VR750"])
                    };

                    lista.Add(userr);
                }
            }

            return lista;
        }
        public List<BEusuario_750VR> leerEntidades_750VR() //busca todos
        {
            List<BEusuario_750VR> lista = new List<BEusuario_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Usuario_VR750";  

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BEusuario_750VR user = new BEusuario_750VR
                    {
                        dni_750VR = Convert.ToInt32(reader["DNI_VR750"]),
                        nombre_750VR = reader["Nombre_VR750"].ToString(),
                        apellido_750VR = reader["Apellido_VR750"].ToString(),
                        mail_750VR = reader["Email_VR750"].ToString(),
                        user_750VR = reader["Usuario_VR750"].ToString(),
                        contraseña_750VR = reader["Contra_VR750"].ToString(),
                        salt_750VR = reader["Salt_VR750"].ToString(),
                        rol_750VR = reader["Rol_VR750"].ToString(),
                        activo_750VR = Convert.ToBoolean(reader["Activo_VR750"]),
                        bloqueado_750VR = Convert.ToBoolean(reader["Bloqueado_VR750"])
                    };

                    lista.Add(user);
                }
            }

            return lista;
        }

        public BEusuario_750VR ObtenerUsuarioPorLogin_750VR(string usuarioLogin)
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
                        return new BEusuario_750VR
                        {
                            dni_750VR = Convert.ToInt32(reader["DNI_VR750"]),
                            nombre_750VR = reader["Nombre_VR750"].ToString(),
                            apellido_750VR = reader["Apellido_VR750"].ToString(),
                            mail_750VR = reader["Email_VR750"].ToString(),
                            user_750VR = reader["Usuario_VR750"].ToString(),
                            contraseña_750VR = reader["Contra_VR750"].ToString(),
                            salt_750VR = reader["Salt_VR750"].ToString(),
                            rol_750VR = reader["Rol_VR750"].ToString(),
                            activo_750VR = Convert.ToBoolean(reader["Activo_VR750"]),
                            bloqueado_750VR = Convert.ToBoolean(reader["Bloqueado_VR750"])
                        };
                    }
                }
            }
            return null; // no encontrado


        }

        public BEusuario_750VR ObtenerUsuarioPorDNI_750VR(int dni)
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
                    BEusuario_750VR user = new BEusuario_750VR
                    {
                        dni_750VR = Convert.ToInt32(reader["DNI_VR750"]),
                        nombre_750VR = reader["Nombre_VR750"].ToString(),
                        apellido_750VR = reader["Apellido_VR750"].ToString(),
                        mail_750VR = reader["Email_VR750"].ToString(),
                        user_750VR = reader["Usuario_VR750"].ToString(),
                        contraseña_750VR = reader["Contra_VR750"].ToString(),
                        salt_750VR = reader["Salt_VR750"].ToString(),
                        rol_750VR = reader["Rol_VR750"].ToString(),
                        activo_750VR = Convert.ToBoolean(reader["Activo_VR750"]),
                        bloqueado_750VR = Convert.ToBoolean(reader["Bloqueado_VR750"])
                    };
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public void BloquearUsuario_750VR(string usuarioLogin)
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

     
        public BEusuario_750VR recuperarUsuario_750VR(string user, string contraseña)
        {
            string sqlQuery = "USE ProyectoNailsTime_VR750; SELECT * FROM Usuario_VR750 WHERE Usuario_VR750 = @Usuario";

            try
            {
                if (!db.Conectar_750VR())
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
                            string hashIngresado = hasher.HashearConSalt_750VR(contraseña, saltAlmacenado);

                            if (hashIngresado != contraseñaHashAlmacenada)
                                throw new Exception("Contraseña incorrecta");

                            // Crear y devolver entidad
                            return new BEusuario_750VR
                            {
                                dni_750VR = dni,
                                nombre_750VR = nombre,
                                apellido_750VR = apellido,
                                mail_750VR = email,
                                user_750VR = usuarioDB,
                                contraseña_750VR = contraseñaHashAlmacenada,
                                salt_750VR = saltAlmacenado,
                                rol_750VR = rol,
                                activo_750VR = activo,
                                bloqueado_750VR = bloqueado
                            };
                        }
                    }
                }

                throw new Exception("No se pudo leer el usuario.");
            }
            finally
            {
                db.Desconectar_750VR();
            }
        }

        public void CambiarContraseña_750VR(BEusuario_750VR usuario, string NuevaContraseña)
        {
        

            string sqlUpdateUsuario = "USE ProyectoNailsTime_VR750; UPDATE Usuario_VR750 SET Contra_VR750 = @Contraseña, Salt_VR750 = @Salt WHERE DNI_VR750 = @DNI";

            try
            {
                bool conectado = db.Conectar_750VR();
                if (!conectado) throw new Exception("Error al conectarse a la base de datos");

                string NuevoSalt = hasher.GenerarSalt_750VR();
                string nuevaContraseñaHasheada = hasher.HashearConSalt_750VR(NuevaContraseña, NuevoSalt);

                using (SqlCommand updateCommand = new SqlCommand(sqlUpdateUsuario, db.Connection))
                {
                    updateCommand.Parameters.AddWithValue("@Contraseña", nuevaContraseñaHasheada);
                    updateCommand.Parameters.AddWithValue("@Salt", NuevoSalt);
                    updateCommand.Parameters.AddWithValue("@DNI", usuario.dni_750VR);

                    int filasAfectadas = updateCommand.ExecuteNonQuery();
                    if (filasAfectadas == 0)
                    {
                        throw new Exception("No se encontró el usuario para actualizar la contraseña.");
                    }
                }
            }
            finally
            {
                db.Desconectar_750VR();
            }



        }
    }
}
