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
                            IdReserva_750VR = Convert.ToInt32(reader["IdReserva_VR750"]),
                            DNIcli_750VR = Convert.ToInt32(reader["DNIcli_VR750"]),
                            DNImanic_750VR = Convert.ToInt32(reader["DNImanic_VR750"]),
                            IdServicio_750VR = Convert.ToInt32(reader["IdServicio_VR750"]),
                            Fecha_750VR = Convert.ToDateTime(reader["Fecha_VR750"]),
                            HoraInicio_750VR = (TimeSpan)reader["HoraInicio_VR750"],
                            HoraFin_750VR = (TimeSpan)reader["HoraFin_VR750"],
                            Precio_750VR = Convert.ToDecimal(reader["Precio_VR750"]),
                            Estado_750VR = Convert.ToBoolean(reader["Estado_VR750"]),
                            Cobrado_750VR = Convert.ToBoolean(reader["Estado_VR750"]),
                        };
                        lista.Add(reserva);
                    }
                }
            }

            return lista;
        }

        BaseDeDatos_750VR db = new BaseDeDatos_750VR();

        public void CrearReserva_750VR(BEReserva_750VR reserva)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
                INSERT INTO Reserva_VR750 
                (DNIcli_VR750, DNImanic_VR750, IdServicio_VR750, Fecha_VR750, HoraInicio_VR750, HoraFin_VR750, Precio_VR750, Estado_VR750, Cobrado_VR750) 
                VALUES (@DNIcli, @DNImanic, @IdServicio, @Fecha, @HoraInicio, @Duracion, @Precio, @Estado, @Cobrado)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNIcli", reserva.DNIcli_750VR);
                cmd.Parameters.AddWithValue("@DNImanic", reserva.DNImanic_750VR);
                cmd.Parameters.AddWithValue("@IdServicio", reserva.IdServicio_750VR);
                cmd.Parameters.AddWithValue("@Fecha", reserva.Fecha_750VR);
                cmd.Parameters.AddWithValue("@HoraInicio", reserva.HoraInicio_750VR);
                cmd.Parameters.AddWithValue("@Duracion", reserva.HoraFin_750VR);
                cmd.Parameters.AddWithValue("@Precio", reserva.Precio_750VR);
                cmd.Parameters.AddWithValue("@Estado", reserva.Estado_750VR);
                cmd.Parameters.AddWithValue("@Cobrado", reserva.Cobrado_750VR);


                cmd.ExecuteNonQuery();
            }
        }
        public List<BEReserva_750VR> leerEntidades_750VR() //busca todos
        {
            List<BEReserva_750VR> lista = new List<BEReserva_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Reserva_VR750";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BEReserva_750VR user = new BEReserva_750VR
                    {
                        IdReserva_750VR = Convert.ToInt32(reader["IdReserva_VR750"]),
                        DNIcli_750VR = Convert.ToInt32(reader["DNIcli_VR750"]),
                        DNImanic_750VR = Convert.ToInt32(reader["DNImanic_VR750"]),
                        IdServicio_750VR = Convert.ToInt32(reader["IdServicio_VR750"]),
                        Fecha_750VR = Convert.ToDateTime(reader["Fecha_VR750"]),
                        HoraInicio_750VR = TimeSpan.Parse(reader["HoraInicio_VR750"].ToString()),
                        HoraFin_750VR = TimeSpan.Parse(reader["HoraFin_VR750"].ToString()),
                        Precio_750VR = Convert.ToDecimal(reader["Precio_VR750"]),
                        Estado_750VR = Convert.ToBoolean(reader["Estado_VR750"]),
                        Cobrado_750VR = Convert.ToBoolean(reader["Cobrado_VR750"]),
                    };

                    lista.Add(user);
                }
            }

            return lista;
        }

    }
}
