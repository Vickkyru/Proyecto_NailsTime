using BE_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALbitacoraCambios_750VR
    {
        public List<BEinsumoCambios_750VR> FiltrarCambios(int? codInsumo, string nombre, DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<BEinsumoCambios_750VR>();

            using (var conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = @"
                    SELECT * FROM InsumoCambios_VR750
                    WHERE
                        (@CodInsumo IS NULL OR CodInsumo_750VR = @CodInsumo)
                        AND (@Nombre IS NULL OR nombre_VR750 LIKE '%' + @Nombre + '%')
                        AND (Fecha BETWEEN @FechaInicio AND @FechaFin)
                    ORDER BY Fecha DESC, Hora DESC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CodInsumo", (object)codInsumo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nombre", (object)nombre ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", fechaFin);

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new BEinsumoCambios_750VR
                            {
                                CodInsumo_750VR = Convert.ToInt32(reader["CodInsumo_750VR"]),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                Hora = (TimeSpan)reader["Hora"],
                                nombre_750VR = reader["nombre_VR750"].ToString(),
                                descripcion_750VR = reader["descripcion_VR750"].ToString(),
                                cantidadActual_750VR = Convert.ToInt32(reader["cantidadActual_VR750"]),
                                stockMinimo_750VR = Convert.ToInt32(reader["stockMinimo_VR750"]),
                                unidadMedida_750VR = reader["unidadMedida_VR750"].ToString(),
                                activo_750VR = Convert.ToBoolean(reader["activo_VR750"]),
                                Act = Convert.ToBoolean(reader["Act"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public void ActivarRegistro(int codInsumo, DateTime fecha, TimeSpan hora)
        {
            using (var conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = @"
                    UPDATE InsumoCambios_VR750
                    SET Act = CASE 
                                WHEN Fecha = @Fecha AND Hora = @Hora THEN 1 
                                ELSE 0 
                              END
                    WHERE CodInsumo_750VR = @CodInsumo;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CodInsumo", codInsumo);
                    cmd.Parameters.AddWithValue("@Fecha", fecha);
                    cmd.Parameters.AddWithValue("@Hora", hora);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
