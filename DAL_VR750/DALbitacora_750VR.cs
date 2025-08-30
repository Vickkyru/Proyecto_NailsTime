using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_VR750;

namespace DAL_VR750
{
    public class DALbitacora_750VR
    {
        // a) Registrar evento (se guarda en el momento)
        public void RegistrarEvento_750VR(string login, string evento, string modulo, byte criticidad)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string sql = @"
INSERT INTO EVENTOS_VR750 (Login, Fecha, Hora, Modulo, Evento, Criticidad)
VALUES (@Login, CAST(GETDATE() AS DATE), CAST(GETDATE() AS TIME(0)), @Modulo, @Evento, @Criticidad);";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.Parameters.AddWithValue("@Modulo", modulo);
                cmd.Parameters.AddWithValue("@Evento", evento);
                cmd.Parameters.AddWithValue("@Criticidad", criticidad);
                cmd.ExecuteNonQuery();
            }
        }

        // b) Últimos 3 días (por defecto al abrir la GUI)
        //    *3 días calendario*: hoy + ayer + anteayer
        public List<BEbitacora_750VR> LeerUltimos3Dias_750VR()
        {
            List<BEbitacora_750VR> list = new List<BEbitacora_750VR>();
            DateTime desde = DateTime.Today.AddDays(-2);

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string sql = @"
SELECT e.Id_Evento, e.Login, e.Fecha, e.Hora, e.Modulo, e.Evento, e.Criticidad,
       u.Nombre_VR750 AS Nombre, u.Apellido_VR750 AS Apellido
FROM EVENTOS_VR750 e
JOIN Usuario_VR750 u ON u.Usuario_VR750 = e.Login
WHERE e.Fecha >= @Desde
ORDER BY e.Fecha DESC, e.Hora DESC;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Desde", desde);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        while (rd.Read())
                        {
                            var item = new BEbitacora_750VR(
                                Convert.ToInt32(rd["Id_Evento"]),
                                rd["Login"].ToString(),
                                Convert.ToDateTime(rd["Fecha"]),
                                (TimeSpan)rd["Hora"],
                                rd["Modulo"].ToString(),
                                rd["Evento"].ToString(),
                                Convert.ToByte(rd["Criticidad"]),
                                rd["Nombre"].ToString(),
                                rd["Apellido"].ToString()
                            );
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        // c) Búsqueda con filtros para la GUI (todas las columnas del modelo)
        public List<BEbitacora_750VR> BuscarEventos_750VR(
            string login = null, DateTime? fecha = null, string modulo = null,
            string evento = null, byte? criticidad = null)
        {
            var list = new List<BEbitacora_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string sql = @"
SELECT e.Id_Evento, e.Login, e.Fecha, e.Hora, e.Modulo, e.Evento, e.Criticidad,
       u.Nombre_VR750 AS Nombre, u.Apellido_VR750 AS Apellido
FROM EVENTOS_VR750 e
JOIN Usuario_VR750 u ON u.Usuario_VR750 = e.Login
WHERE 1=1";

                if (!string.IsNullOrEmpty(login)) sql += " AND e.Login = @Login";
                if (fecha.HasValue) sql += " AND e.Fecha = @Fecha";
                if (!string.IsNullOrEmpty(modulo)) sql += " AND e.Modulo = @Modulo";
                if (!string.IsNullOrEmpty(evento)) sql += " AND e.Evento = @Evento";
                if (criticidad.HasValue) sql += " AND e.Criticidad = @Criticidad";

                sql += " ORDER BY e.Fecha DESC, e.Hora DESC;";

                SqlCommand cmd = new SqlCommand(sql, conn);

                if (!string.IsNullOrEmpty(login)) cmd.Parameters.AddWithValue("@Login", login);
                if (fecha.HasValue) cmd.Parameters.AddWithValue("@Fecha", fecha.Value.Date);
                if (!string.IsNullOrEmpty(modulo)) cmd.Parameters.AddWithValue("@Modulo", modulo);
                if (!string.IsNullOrEmpty(evento)) cmd.Parameters.AddWithValue("@Evento", evento);
                if (criticidad.HasValue) cmd.Parameters.AddWithValue("@Criticidad", criticidad.Value);

                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        while (rd.Read())
                        {
                            var item = new BEbitacora_750VR(
                                Convert.ToInt32(rd["Id_Evento"]),
                                rd["Login"].ToString(),
                                Convert.ToDateTime(rd["Fecha"]),
                                (TimeSpan)rd["Hora"],
                                rd["Modulo"].ToString(),
                                rd["Evento"].ToString(),
                                Convert.ToByte(rd["Criticidad"]),
                                rd["Nombre"].ToString(),
                                rd["Apellido"].ToString()
                            );
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }

    }
}
