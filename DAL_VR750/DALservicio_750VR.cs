using BE_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DAL_VR750
{
    public class DALservicio_750VR
    {
        public void CrearServicio_750VR(BEServicio_750VR servicio)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
                INSERT INTO Servicio_VR750 
                (Nombre_VR750, Tecnica_VR750, DuracionMinutos_VR750, Precio_VR750, Activo_VR750) 
                VALUES (@Nombre, @Tecnica, @Duracion, @Precio, @Activo)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", servicio.nombre_750VR);
                cmd.Parameters.AddWithValue("@Tecnica", servicio.tecnica_750VR);
                cmd.Parameters.AddWithValue("@Duracion", servicio.duracion_750VR);
                cmd.Parameters.AddWithValue("@Precio", servicio.precio_750VR);
                cmd.Parameters.AddWithValue("@Activo", servicio.activo_750VR);

                cmd.ExecuteNonQuery();
            }
        }

        public bool ModificarServicio_750VR(int id, string nombre, string tecnica, int duracion, decimal precio)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
        UPDATE Servicio_VR750
        SET Nombre_VR750 = @Nombre,
            Tecnica_VR750 = @Tecnica,
            DuracionMinutos_VR750 = @Duracion,
            Precio_VR750 = @Precio
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

        public bool CambiarEstadoServicio_750VR(int idServicio, bool nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE Servicio_VR750 SET Activo_VR750 = @Activo WHERE IdServicio_VR750 = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Activo", nuevoEstado);
                cmd.Parameters.AddWithValue("@ID", idServicio);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<BEServicio_750VR> leerEntidadesActivas_750VR()
        {
            return LeerEntidades_750VR().Where(s => s.activo_750VR).ToList();
        }

        public List<BEServicio_750VR> LeerEntidades_750VR()
        {
            List<BEServicio_750VR> lista = new List<BEServicio_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Servicio_VR750";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var servicio = new BEServicio_750VR(
                        id: Convert.ToInt32(reader["IdServicio_VR750"]),
                        nom: reader["Nombre_VR750"].ToString(),
                        tec: reader["Tecnica_VR750"].ToString(),
                        dur: Convert.ToInt32(reader["DuracionMinutos_VR750"]),
                        pre: Convert.ToDecimal(reader["Precio_VR750"]),
                        act: Convert.ToBoolean(reader["Activo_VR750"])
                    );

                    lista.Add(servicio);
                }
            }

            return lista;
        }
    }
}
