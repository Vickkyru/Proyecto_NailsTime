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
                cmd.Parameters.AddWithValue("@Telefono", usuario.telefono);
                cmd.Parameters.AddWithValue("@Mail", usuario.mail);
                cmd.Parameters.AddWithValue("@User", usuario.user);
                cmd.Parameters.AddWithValue("@Contrasenia", usuario.contraseña);
                cmd.Parameters.AddWithValue("@Rol", usuario.rol);
                cmd.Parameters.AddWithValue("@Estado", usuario.estado);

                return cmd.ExecuteNonQuery() > 0;
            }
        }


    }
}
