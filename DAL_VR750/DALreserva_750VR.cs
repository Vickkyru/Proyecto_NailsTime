using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_VR750;

namespace DAL_VR750
{
    public class DALreserva_750VR
    {
        public List<BEReserva_750VR> ObtenerReservasPorManicurista(int dniManicurista)
        {
            List<BEReserva_750VR> lista = new List<BEReserva_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Reserva_VR750 WHERE DNImanic_VR750 = @DNI";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dniManicurista);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var reserva = new BEReserva_750VR
                        {
                            IdReserva = Convert.ToInt32(reader["IdReserva_VR750"]),
                            DNIcli = Convert.ToInt32(reader["DNIcli_VR750"]),
                            DNImanic = Convert.ToInt32(reader["DNImanic_VR750"]),
                            IdServicio = Convert.ToInt32(reader["IdServicio_VR750"]),
                            Fecha = Convert.ToDateTime(reader["Fecha_VR750"]),
                            HoraInicio = (TimeSpan)reader["HoraInicio_VR750"],
                            DuracionMinutos = Convert.ToInt32(reader["DuracionMinutos_VR750"]),
                            Precio = Convert.ToDecimal(reader["Precio_VR750"]),
                            Estado = Convert.ToBoolean(reader["Estado_VR750"]),
                            NombreCliente = reader["NombreCliente_VR750"].ToString(),
                            NombreManic = reader["NombreManicurista_VR750"].ToString(),
                            NombreServicio = reader["NombreServicio_VR750"].ToString()
                        };
                        lista.Add(reserva);
                    }
                }
            }

            return lista;
        }

        BaseDeDatos_750VR db = new BaseDeDatos_750VR();

        public void CrearReserva(BEReserva_750VR reserva)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
                INSERT INTO Reserva_VR750 
                (DNIcli_VR750, DNImanic_VR750, IdServicio_VR750, Fecha_VR750, HoraInicio_VR750, DuracionMinutos_VR750, Precio_VR750, Estado_VR750, NombreCliente_VR750, NombreManicurista_VR750, NombreServicio_VR750) 
                VALUES (@DNIcli, @DNImanic, @IdServicio, @Fecha, @HoraInicio, @Duracion, @Precio, 'Pendiente', @NombreCliente, @NombreManicurista, @NombreServicio)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNIcli", reserva.DNIcli);
                cmd.Parameters.AddWithValue("@DNImanic", reserva.DNImanic);
                cmd.Parameters.AddWithValue("@IdServicio", reserva.IdServicio);
                cmd.Parameters.AddWithValue("@Fecha", reserva.Fecha);
                cmd.Parameters.AddWithValue("@HoraInicio", reserva.HoraInicio);
                cmd.Parameters.AddWithValue("@Duracion", reserva.DuracionMinutos);
                cmd.Parameters.AddWithValue("@Precio", reserva.Precio);
                cmd.Parameters.AddWithValue("@NombreCliente", reserva.NombreCliente);
                cmd.Parameters.AddWithValue("@NombreManicurista", reserva.NombreManic);
                cmd.Parameters.AddWithValue("@NombreServicio", reserva.NombreServicio);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
