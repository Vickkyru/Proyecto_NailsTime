using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_VR750;

namespace DAL_VR750
{
    public class DALusuario_750VR
    {
        public bool CrearUsuario(Usuario_750VR usuario)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena)) // Conexion.Cadena es tu cadena de conexión estática
            {
                conn.Open();

                string query = @"INSERT INTO Usuarios 
                (DNI, Nombre, Apellido, Telefono, Email, UsuarioLogin, Contrasenia, Rol, Estado)
                VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Mail, @User, @Contrasenia, @Rol, @Estado)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", usuario.dni);
                cmd.Parameters.AddWithValue("@Nombre", usuario.nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.apellido);
                cmd.Parameters.AddWithValue("@Mail", usuario.mail);
                cmd.Parameters.AddWithValue("@User", usuario.user);
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.contraseña);
                cmd.Parameters.AddWithValue("@Rol", usuario.rol);
                cmd.Parameters.AddWithValue("@Estado", usuario.estado);

                return cmd.ExecuteNonQuery() > 0;
            }
        }


        public bool ModificarUsuario(Usuario_750VR usuario)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"UPDATE Usuarios SET 
                                Nombre = @Nombre,
                                Apellido = @Apellido,
                                Telefono = @Telefono,
                                Email = @Mail,
                                Contrasenia = @Contrasenia,
                                Rol = @Rol,
                                Estado = @Estado
                            WHERE DNI = @DNI";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", usuario.dni);
                cmd.Parameters.AddWithValue("@Nombre", usuario.nombre);
                cmd.Parameters.AddWithValue("@Apellido", usuario.apellido);
                cmd.Parameters.AddWithValue("@Mail", usuario.mail);
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.contraseña);
                cmd.Parameters.AddWithValue("@Rol", usuario.rol);
                cmd.Parameters.AddWithValue("@Estado", usuario.estado);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool BorrarUsuarioLogico(string dni)
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

        public void DesbloquearUsuario(string dni)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Usuarios SET Bloqueo = 1 WHERE DNI = @dni", conn);
                cmd.Parameters.AddWithValue("@dni", dni);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Usuario_750VR> ObtenerUsuarios(bool soloActivos)
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
                        user = reader["UsuarioLogin"].ToString(),
                        contraseña = reader["Contrasenia"].ToString(),
                        rol = reader["Rol"].ToString(),
                        estado = reader["Estado"].ToString()
                    };

                    lista.Add(user);
                }
            }

            return lista;
        }


        public List<Usuario_750VR> BuscarUsuarios(string dni, string nombre, string apellido, string email)
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
                        user = reader["UsuarioLogin"].ToString(),
                        contraseña = reader["Contrasenia"].ToString(),
                        rol = reader["Rol"].ToString(),
                        estado = reader["Estado"].ToString(),
                        //activo = Convert.ToBoolean(reader["Activo"]),
                        //bloqueado = Convert.ToBoolean(reader["Bloqueo"]),
                      
                    };

                    lista.Add(user);
                }
            }

            return lista;
        }
    }
}
