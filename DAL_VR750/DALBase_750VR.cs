using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALBase_750VR
    {
        // readonly + nombre estándar
        protected readonly string _connStr =
            "Server=.;Database=ProyectoNailsTime_VR750;Trusted_Connection=True;TrustServerCertificate=True;";

        // Usá este helper en todos tus DAL concretos (INSERT/UPDATE/DELETE)
        protected int ExecuteNonQueryAndReDV(string sql, params SqlParameter[] ps)
        {
            using (var cn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (ps != null && ps.Length > 0)
                    cmd.Parameters.AddRange(ps);

                cn.Open();
                int rows = cmd.ExecuteNonQuery();

                // Recalcular DV (por tabla + totales) después de cada persistencia
                DVService_750VR.RecalcularYGuardarDV_BD();

                return rows;
            }
        }
    }
}
