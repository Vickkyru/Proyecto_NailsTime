using BE_VR750;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Modulo", modulo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Evento", evento ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Criticidad", criticidad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // b) Últimos 3 días por defecto (hoy, ayer y anteayer)
        public List<BEbitacora_750VR> LeerUltimos3Dias_750VR()
        {
            var list = new List<BEbitacora_750VR>();
            DateTime desde = DateTime.Today.AddDays(-2); // incluye hoy

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string sql = @"
SELECT e.Id_Evento, e.Login, e.Fecha, e.Hora, e.Modulo, e.Evento, e.Criticidad
FROM   EVENTOS_VR750 e
WHERE  e.Fecha >= @Desde
ORDER BY e.Fecha DESC, e.Hora DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Desde", desde.Date);

                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            list.Add(new BEbitacora_750VR(
                                idEvento: Convert.ToInt32(rd["Id_Evento"]),
                                login: rd["Login"].ToString(),
                                fecha: Convert.ToDateTime(rd["Fecha"]),
                                hora: (TimeSpan)rd["Hora"],
                                modulo: rd["Modulo"].ToString(),
                                evento: rd["Evento"].ToString(),
                                criticidad: Convert.ToByte(rd["Criticidad"])
                            ));
                        }
                    }
                }
            }

            return list;
        }

        // c) Filtros generales (para la GUI). Cualquier parámetro puede ser null.
        public List<BEbitacora_750VR> FiltrarEventos(
            int? dniUsuario,
            int? criticidad,
            string evento,
            string modulo,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            var lista = new List<BEbitacora_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string sql = @"
SELECT  e.Id_Evento, e.Login, e.Fecha, e.Hora, e.Modulo, e.Evento, e.Criticidad
FROM    EVENTOS_VR750 e
LEFT JOIN Usuario_VR750 u
       ON u.Usuario_VR750 = e.Login
WHERE   (@dni   IS NULL OR u.DNI_VR750   = @dni)
  AND   (@crit  IS NULL OR e.Criticidad  = @crit)
  AND   (@ev    IS NULL OR e.Evento      = @ev)
  AND   (@mod   IS NULL OR e.Modulo      = @mod)
  AND   (@fini  IS NULL OR e.Fecha       >= @fini)
  AND   (@ffin  IS NULL OR e.Fecha       <= @ffin)
ORDER BY e.Fecha DESC, e.Hora DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // INT
                    var pDni = cmd.Parameters.Add("@dni", SqlDbType.Int);
                    pDni.Value = dniUsuario.HasValue ? (object)dniUsuario.Value : DBNull.Value;

                    var pCrit = cmd.Parameters.Add("@crit", SqlDbType.TinyInt);
                    pCrit.Value = criticidad.HasValue ? (object)criticidad.Value : DBNull.Value;

                    // VARCHAR
                    var pEv = cmd.Parameters.Add("@ev", SqlDbType.VarChar, 150);
                    pEv.Value = !string.IsNullOrWhiteSpace(evento) ? (object)evento : DBNull.Value;

                    var pMod = cmd.Parameters.Add("@mod", SqlDbType.VarChar, 100);
                    pMod.Value = !string.IsNullOrWhiteSpace(modulo) ? (object)modulo : DBNull.Value;

                    // DATE
                    var pFini = cmd.Parameters.Add("@fini", SqlDbType.Date);
                    pFini.Value = fechaInicio.HasValue ? (object)fechaInicio.Value.Date : DBNull.Value;

                    var pFfin = cmd.Parameters.Add("@ffin", SqlDbType.Date);
                    pFfin.Value = fechaFin.HasValue ? (object)fechaFin.Value.Date : DBNull.Value;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new BEbitacora_750VR(
                                idEvento: Convert.ToInt32(rd["Id_Evento"]),
                                login: rd["Login"].ToString(),
                                fecha: Convert.ToDateTime(rd["Fecha"]),
                                hora: (TimeSpan)rd["Hora"],
                                modulo: rd["Modulo"].ToString(),
                                evento: rd["Evento"].ToString(),
                                criticidad: Convert.ToByte(rd["Criticidad"])
                            ));
                        }
                    }
                }
            }

            return lista;
        }

        // (Opcional) lectura completa para debug/exports
        public List<BEbitacora_750VR> LeerTodo_750VR()
        {
            var list = new List<BEbitacora_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string sql = @"
SELECT e.Id_Evento, e.Login, e.Fecha, e.Hora, e.Modulo, e.Evento, e.Criticidad
FROM   EVENTOS_VR750 e
ORDER BY e.Fecha DESC, e.Hora DESC;";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new BEbitacora_750VR(
                            idEvento: Convert.ToInt32(rd["Id_Evento"]),
                            login: rd["Login"].ToString(),
                            fecha: Convert.ToDateTime(rd["Fecha"]),
                            hora: (TimeSpan)rd["Hora"],
                            modulo: rd["Modulo"].ToString(),
                            evento: rd["Evento"].ToString(),
                            criticidad: Convert.ToByte(rd["Criticidad"])
                        ));
                    }
                }
            }

            return list;
        }

    }
}
