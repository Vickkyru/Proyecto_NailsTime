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
                string query = @"
        SELECT r.*, 
               c.Nombre_VR750 AS NombreCliente, c.Apellido_VR750 AS ApellidoCliente,
               u.Nombre_VR750 AS NombreManic, u.Apellido_VR750 AS ApellidoManic,
               s.Nombre_VR750 AS NombreServicio, s.Tecnica_VR750 AS TecnicaServicio, s.Precio_VR750 AS PrecioServicio
        FROM Reserva_VR750 r
        LEFT JOIN Cliente_VR750 c ON r.DNIcli_VR750 = c.DNI_VR750
        LEFT JOIN Usuario_VR750 u ON r.DNImanic_VR750 = u.DNI_VR750
        LEFT JOIN Servicio_VR750 s ON r.IdServicio_VR750 = s.IdServicio_VR750
        WHERE r.DNImanic_VR750 = @DNI";

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
                            HoraInicio_750VR = TimeSpan.Parse(reader["HoraInicio_VR750"].ToString()),
                            HoraFin_750VR = TimeSpan.Parse(reader["HoraFin_VR750"].ToString()),
                            Precio_750VR = Convert.ToDecimal(reader["Precio_VR750"]),
                            Estado_750VR = reader["Estado_VR750"].ToString(),
                            Cobrado_750VR = Convert.ToBoolean(reader["Cobrado_VR750"]),

                            cliente = new BECliente_750VR(
                                dni: Convert.ToInt32(reader["DNIcli_VR750"]),
                                nom: reader["NombreCliente"].ToString(),
                                ape: reader["ApellidoCliente"].ToString(),
                                gmail: "",
                                dire: "",
                                celu: "",
                                act: true
                            ),

                            manic = new BEusuario_750VR(
                                dni: Convert.ToInt32(reader["DNImanic_VR750"]),
                                nombre: reader["NombreManic"].ToString(),
                                ape: reader["ApellidoManic"].ToString(),
                                mail: "",
                                user: "",
                                contra: "",
                                salt: "",
                                rol: "",
                                activo: true,
                                bloqueado: false
                            ),

                            serv = new BEServicio_750VR(
                                id: Convert.ToInt32(reader["IdServicio_VR750"]),
                                nom: reader["NombreServicio"].ToString(),
                                tec: reader["TecnicaServicio"].ToString(),
                                dur: 0,
                                pre: Convert.ToDecimal(reader["PrecioServicio"]),
                                act: true
                            )
                        };

                        lista.Add(reserva);
                    }
                }
            }

            return lista;
        }

        BaseDeDatos_750VR db = new BaseDeDatos_750VR();

        public int CrearReserva_750VR(BEReserva_750VR reserva)
        {
            using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = @"
            INSERT INTO Reserva_VR750 
            (DNIcli_VR750, DNImanic_VR750, IdServicio_VR750, Fecha_VR750, HoraInicio_VR750, HoraFin_VR750, Precio_VR750, Estado_VR750, Cobrado_VR750)
            VALUES 
            (@DNIcli, @DNImanic, @IdServicio, @Fecha, @HoraInicio, @HoraFin, @Precio, @Estado, @Cobrado);
            SELECT SCOPE_IDENTITY();";  // <<< esto devuelve el ID generado

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DNIcli", reserva.DNIcli_750VR);
                cmd.Parameters.AddWithValue("@DNImanic", reserva.DNImanic_750VR);
                cmd.Parameters.AddWithValue("@IdServicio", reserva.IdServicio_750VR);
                cmd.Parameters.AddWithValue("@Fecha", reserva.Fecha_750VR);
                cmd.Parameters.AddWithValue("@HoraInicio", reserva.HoraInicio_750VR);
                cmd.Parameters.AddWithValue("@HoraFin", reserva.HoraFin_750VR);
                cmd.Parameters.AddWithValue("@Precio", reserva.Precio_750VR);
                cmd.Parameters.AddWithValue("@Estado", reserva.Estado_750VR);
                cmd.Parameters.AddWithValue("@Cobrado", reserva.Cobrado_750VR);

                con.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result); // <<< retorna el ID a tu objeto
            }
        }
        public string ObtenerEstadoReserva(int idReserva)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = "SELECT Estado_VR750 FROM Reserva_VR750 WHERE IdReserva_VR750 = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idReserva);

                conn.Open();
                object estado = cmd.ExecuteScalar();
                return estado != null ? estado.ToString() : null;
            }
        }
        public List<BEReserva_750VR> leerEntidades_750VR()
        {
            List<BEReserva_750VR> lista = new List<BEReserva_750VR>();

            using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = @"
            SELECT r.*, 
                   c.Nombre_VR750 AS NombreCliente, c.Apellido_VR750 AS ApellidoCliente,
                   u.Nombre_VR750 AS NombreManic, u.Apellido_VR750 AS ApellidoManic,
                   s.Nombre_VR750 AS NombreServicio, s.Tecnica_VR750 AS TecnicaServicio, s.Precio_VR750 AS PrecioServicio
            FROM Reserva_VR750 r
            LEFT JOIN Cliente_VR750 c ON r.DNIcli_VR750 = c.DNI_VR750
            LEFT JOIN Usuario_VR750 u ON r.DNImanic_VR750 = u.DNI_VR750
            LEFT JOIN Servicio_VR750 s ON r.IdServicio_VR750 = s.IdServicio_VR750";

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
                        Estado_750VR = reader["Estado_VR750"].ToString(),
                        Cobrado_750VR = Convert.ToBoolean(reader["Cobrado_VR750"]),

                        cliente = new BECliente_750VR(
                            dni: Convert.ToInt32(reader["DNIcli_VR750"]),
                            nom: reader["NombreCliente"].ToString(),
                            ape: reader["ApellidoCliente"].ToString(),
                            gmail: "", // opcional si no lo traés
                            dire: "",
                            celu: "",
                            act: true
                        ),

                        manic = new BEusuario_750VR(
                            dni: Convert.ToInt32(reader["DNImanic_VR750"]),
                            nombre: reader["NombreManic"].ToString(),
                            ape: reader["ApellidoManic"].ToString(),
                            mail: "",
                            user: "",
                            contra: "",
                            salt: "",
                            rol: "",
                            activo: true,
                            bloqueado: false
                        ),

                        serv = new BEServicio_750VR(
                            id: Convert.ToInt32(reader["IdServicio_VR750"]),
                            nom: reader["NombreServicio"].ToString(),
                            tec: reader["TecnicaServicio"].ToString(),
                            dur: 0,
                            pre: Convert.ToDecimal(reader["PrecioServicio"]),
                            act: true
                        )
                    };

                    lista.Add(reserva);
                }

                reader.Close();
            }

            return lista;
        }

        public BEReserva_750VR ObtenerReservaPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = "SELECT * FROM Reserva_VR750 WHERE IdReserva_VR750 = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    var reserva = new BEReserva_750VR
                    {
                        IdReserva_750VR = (int)dr["IdReserva_VR750"],
                        Precio_750VR = Convert.ToDecimal(dr["Precio_VR750"])
                    };

                    dr.Close(); // ✅ CERRÁS ANTES DE SALIR
                    return reserva;
                }
                return null;
            }
        }


        public bool MarcarComoCobrado(int id)
        {
            using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = "UPDATE Reserva_VR750 SET Cobrado_VR750 = 1 WHERE IdReserva_VR750 = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public void ActualizarEstadoReserva(int idReserva, string nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = "UPDATE Reserva_VR750 SET Estado_VR750 = @estado WHERE IdReserva_VR750 = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@id", idReserva);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

    }
}
