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
                //cmd.Parameters.AddWithValue("@Telefono", usuario.telefono);
                cmd.Parameters.AddWithValue("@Mail", usuario.mail);
                cmd.Parameters.AddWithValue("@User", usuario.user);
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.contraseña);
                cmd.Parameters.AddWithValue("@Rol", usuario.rol);
                cmd.Parameters.AddWithValue("@Estado", usuario.estado);

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



        public bool ModificarUsuario(UsuarioBE usuario)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
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
                cmd.Parameters.AddWithValue("@Telefono", usuario.telefono);
                cmd.Parameters.AddWithValue("@Mail", usuario.mail);
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.contraseña);
                cmd.Parameters.AddWithValue("@Rol", usuario.rol);
                cmd.Parameters.AddWithValue("@Estado", usuario.estado);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool BorrarUsuarioLogico(string dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
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
            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Usuarios SET Bloqueo = 1 WHERE DNI = @dni", conn);
                cmd.Parameters.AddWithValue("@dni", dni);
                cmd.ExecuteNonQuery();
            }
        }

        public List<UsuarioBE> ObtenerUsuarios(bool soloActivos)
        {
            List<UsuarioBE> lista = new List<UsuarioBE>();

            using (SqlConnection conn = new SqlConnection(Conexion.cadena))
            {
                conn.Open();
                string query = soloActivos
                    ? "SELECT * FROM Usuarios WHERE Activo = 1"
                    : "SELECT * FROM Usuarios";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UsuarioBE user = new UsuarioBE
                    {
                        dni = Convert.ToInt32(reader["DNI"]),
                        nombre = reader["Nombre"].ToString(),
                        apellido = reader["Apellido"].ToString(),
                        telefono = Convert.ToInt32(reader["Telefono"]),
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

    }
}
