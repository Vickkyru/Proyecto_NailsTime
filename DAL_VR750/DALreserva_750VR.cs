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

    using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
    {
        string query = @"
        SELECT r.*, 
               c.Nombre_VR750 AS NombreCliente, c.Apellido_VR750 AS ApellidoCliente,
               u.Nombre_VR750 AS NombreManic, u.Apellido_VR750 AS ApellidoManic,
               s.Tecnica_VR750 AS Tecnica, s.Precio_VR750 AS PrecioServ
        FROM Reserva_VR750 r
        LEFT JOIN Cliente_VR750 c ON r.DNIcli_VR750 = c.DNI_VR750
        LEFT JOIN Usuario_VR750 u ON r.DNImanic_VR750 = u.DNI_VR750
        LEFT JOIN Servicio_VR750 s ON r.IdServicio_VR750 = s.idServicio_VR750";

        SqlCommand cmd = new SqlCommand(query, con);
        con.Open();

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
                HoraInicio_750VR = TimeSpan.Parse(reader["HoraInicio_VR750"].ToString()),
                HoraFin_750VR = TimeSpan.Parse(reader["HoraFin_VR750"].ToString()),
                Precio_750VR = Convert.ToDecimal(reader["Precio_VR750"]),
                Estado_750VR = Convert.ToBoolean(reader["Estado_VR750"]),
                Cobrado_750VR = Convert.ToBoolean(reader["Cobrado_VR750"]),

                cliente = new BECliente_750VR
                {
                    nombre_750VR = reader["NombreCliente"].ToString(),
                    apellido_750VR = reader["ApellidoCliente"].ToString()
                },

                manic = new BEusuario_750VR
                {
                    nombre_750VR = reader["NombreManic"].ToString(),
                    apellido_750VR = reader["ApellidoManic"].ToString()
                },

                serv = new BEServicio_750VR
                {
                    tecnica_750VR = reader["Tecnica"].ToString(),
                    precio_750VR = Convert.ToDecimal(reader["PrecioServ"])
                }
            };

            lista.Add(reserva);
        }

        reader.Close();
    }

    return lista;
        }

    }
}
