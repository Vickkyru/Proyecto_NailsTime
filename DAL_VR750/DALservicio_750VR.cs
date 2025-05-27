using BE_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALservicio_750VR
    {


        public void CrearServicio_750VR(BEServicio_750VR servicio) //alta user
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"INSERT INTO Servicio_VR750 
        (IdServicio_VR750, Nombre_VR750, Tecnica_VR750, DuracionMinutos_VR750, Precio_VR750, Activo_VR750) 
        VALUES (@ID, @Nombre, @Tecnica, @Duracion, @Precio, @Activo)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", servicio.idServicio_750VR);
                cmd.Parameters.AddWithValue("@Nombre", servicio.nombre_750VR);
                cmd.Parameters.AddWithValue("@Tecnica", servicio.tecnica_750VR);
                cmd.Parameters.AddWithValue("@Duracion", servicio.duracion_750VR);
                cmd.Parameters.AddWithValue("@Precio", servicio.precio_750VR);
                cmd.Parameters.AddWithValue("@Activo", servicio.activo_750VR);


                cmd.ExecuteNonQuery();
            }
        }


        public bool ModificarServicio_750VR(int id, string nombre, string tecnica, int duracion, decimal precio) //mod user
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"UPDATE Servicio_VR750 
                         SET Nombre_VR750 = @Nombre, Tecnica_VR750 = @Tecnica, DuracionMinutos_VR750 = @Duracion, Precio_VR750 = @precio
                         WHERE IdServicio_VR750 = @ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Tecnica", tecnica);
                cmd.Parameters.AddWithValue("@Duracion", duracion);
                cmd.Parameters.AddWithValue("@Precio", precio);
                cmd.Parameters.AddWithValue("@ID", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CambiarEstadoServicio_750VR(string nombre, bool nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE servicio_VR750 SET Activo_VR750 = @Activo WHERE Nombre_VR750 = @Nombre";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Activo", nuevoEstado ? 1 : 0);
                cmd.Parameters.AddWithValue("@Nombre", nombre);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        //public List<BECliente_750VR> BuscarClientes_750VR(/*int id, */string nombre, string tecnica, int duracion, decimal precio)
        //{
        //    List<BECliente_750VR> lista = new List<BECliente_750VR>();

        //    using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
        //    {
        //        conn.Open();

        //        string query = "SELECT * FROM Servicio_VR750 WHERE 1=1";

        //        //if (!string.IsNullOrEmpty(id))
        //        //    query += " AND CAST(IdServicio_VR750 AS VARCHAR) LIKE @DNI";

        //        if (!string.IsNullOrEmpty(nombre))
        //            query += " AND Nombre_VR750 LIKE @Nombre";

        //        if (!string.IsNullOrEmpty(tecnica))
        //            query += " AND Apellido_VR750 LIKE @Apellido";

        //        if (!string.IsNullOrEmpty(duracion))
        //            query += " AND Email_VR750 LIKE @Email";

        //        if (!string.IsNullOrEmpty(precio))
        //            query += " AND Direccion_VR750 LIKE @Direccion";

        //        if (!string.IsNullOrEmpty(celu))
        //            query += " AND Celular_VR750 LIKE @Celular";

        //        SqlCommand cmd = new SqlCommand(query, conn);

        //        //if (!string.IsNullOrEmpty(dni))
        //        //    cmd.Parameters.AddWithValue("@DNI", "%" + dni + "%");

        //        if (!string.IsNullOrEmpty(nombre))
        //            cmd.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");

        //        if (!string.IsNullOrEmpty(apellido))
        //            cmd.Parameters.AddWithValue("@Apellido", "%" + apellido + "%");

        //        if (!string.IsNullOrEmpty(email))
        //            cmd.Parameters.AddWithValue("@Email", "%" + email + "%");

        //        if (!string.IsNullOrEmpty(dire))
        //            cmd.Parameters.AddWithValue("@Direccion", "%" + dire + "%");

        //        if (!string.IsNullOrEmpty(celu))
        //            cmd.Parameters.AddWithValue("@Celular", "%" + celu + "%");

        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            BECliente_750VR userr = new BECliente_750VR
        //            {
        //                dni_750VR = Convert.ToInt32(reader["DNI_VR750"]),
        //                nombre_750VR = reader["Nombre_VR750"].ToString(),
        //                apellido_750VR = reader["Apellido_VR750"].ToString(),
        //                gmail_750VR = reader["Email_VR750"].ToString(),
        //                direccion_750VR = reader["Direccion_VR750"].ToString(),
        //                celular_750VR = Convert.ToInt32(reader["Celular_VR750"]),
        //                activo_750VR = Convert.ToBoolean(reader["Activo_VR750"]),

        //            };

        //            lista.Add(userr);
        //        }
        //    }

        //    return lista;
        //}
    }
}
