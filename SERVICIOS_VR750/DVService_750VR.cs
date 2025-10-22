using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS_VR750
{
    public static class DVService_750VR
    {
        private static readonly string _connStr =
    "Server=localhost\\SQLEXPRESS;Database=ProyectoNailsTime_VR750;Trusted_Connection=True;TrustServerCertificate=True;";

        private static readonly HashSet<string> _excluirCols =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                // "password_hash", "rowversion_col"
            };

        // ===== Utilidades =====
        private static long HexSum(object value)
        {
            if (value == null || value == DBNull.Value) return 0L;
            string s = Convert.ToString(value)?.Trim() ?? "";
            if (s.Length == 0) return 0L;
            // s = s.ToUpperInvariant(); // opcional (case-insensitive)

            byte[] bytes = Encoding.UTF8.GetBytes(s);
            long sum = 0;
            foreach (byte b in bytes) sum += b;
            return sum;
        }

        private static List<(string Schema, string Name)> GetUserTables(SqlConnection cn)
        {
            const string sql = @"
                SELECT s.name, t.name
                FROM sys.tables t
                JOIN sys.schemas s ON s.schema_id = t.schema_id
                WHERE t.is_ms_shipped = 0
                  AND t.name NOT IN ('DV_Tabla_VR750','DV_DB_VR750');";

            using (var cmd = new SqlCommand(sql, cn))
            using (var rd = cmd.ExecuteReader())
            {
                var list = new List<(string, string)>();
                while (rd.Read())
                    list.Add((rd.GetString(0), rd.GetString(1)));
                return list;
            }
        }

        private static DataTable ReadTable(SqlConnection cn, string schema, string table)
        {
            const string sqlCols = @"
              SELECT STUFF((
                SELECT ',' + QUOTENAME(c.name)
                FROM sys.columns c
                WHERE c.[object_id] = OBJECT_ID(@full)
                  AND c.is_computed = 0
                  AND TYPE_NAME(c.user_type_id) NOT IN (
                      'image','text','ntext','sql_variant','timestamp','rowversion',
                      'hierarchyid','geography','geometry','xml'
                  )
                FOR XML PATH(''), TYPE).value('.','nvarchar(max)'),1,1,'') AS cols";

            string full = schema + "." + table;
            string cols;

            using (var cmdCols = new SqlCommand(sqlCols, cn))
            {
                cmdCols.Parameters.AddWithValue("@full", full);
                cols = (cmdCols.ExecuteScalar() as string) ?? "";
            }

            var dt = new DataTable(full);
            if (string.IsNullOrWhiteSpace(cols)) return dt;

            var colsList = cols.Split(',')
                               .Select(c => c.Trim().Trim('[', ']'))
                               .Where(c => !_excluirCols.Contains(c))
                               .ToList();
            if (colsList.Count == 0) return dt;

            string finalCols = string.Join(",", colsList.Select(c => $"[{c}]"));
            string sql = $"SELECT {finalCols} FROM {schema}.{table};";

            using (var da = new SqlDataAdapter(sql, cn))
            {
                da.Fill(dt);
            }
            return dt;
        }

        private static long CalcularDVH_Tabla(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0 || dt.Rows.Count == 0) return 0L;

            long tableDVH = 0L;
            foreach (DataRow row in dt.Rows)
            {
                long rowDVH = 0L;
                foreach (DataColumn col in dt.Columns)
                    rowDVH += HexSum(row[col]);
                tableDVH += rowDVH;
            }
            return tableDVH;
        }

        private static long CalcularDVV_Tabla(DataTable dt)
        {
            if (dt == null || dt.Columns.Count == 0 || dt.Rows.Count == 0) return 0L;

            long tableDVV = 0L;
            foreach (DataColumn col in dt.Columns)
            {
                long colDVV = 0L;
                foreach (DataRow row in dt.Rows)
                    colDVV += HexSum(row[col]);
                tableDVV += colDVV;
            }
            return tableDVV;
        }

        // ===== Persistencia de DV (por tabla + totales de BD) =====
        public static void RecalcularYGuardarDV_BD()
        {
            using (var cn = new SqlConnection(_connStr))
            {
                cn.Open();

                long dvhDB = 0, dvvDB = 0;
                var tablas = GetUserTables(cn);

                foreach (var t in tablas)
                {
                    var dt = ReadTable(cn, t.Schema, t.Name);
                    long dvhT = CalcularDVH_Tabla(dt);
                    long dvvT = CalcularDVV_Tabla(dt);

                    using (var cmd = new SqlCommand(@"
                        MERGE dbo.DV_Tabla_VR750 AS target
                        USING (SELECT @tabla AS Tabla, @dvh AS DVH_Tabla, @dvv AS DVV_Tabla) AS src
                        ON (target.Tabla = src.Tabla)
                        WHEN MATCHED THEN
                            UPDATE SET DVH_Tabla = src.DVH_Tabla, DVV_Tabla = src.DVV_Tabla, UpdatedAt = SYSDATETIME()
                        WHEN NOT MATCHED THEN
                            INSERT (Tabla, DVH_Tabla, DVV_Tabla) VALUES (src.Tabla, src.DVH_Tabla, src.DVV_Tabla);", cn))
                    {
                        cmd.Parameters.AddWithValue("@tabla", t.Schema + "." + t.Name);
                        cmd.Parameters.AddWithValue("@dvh", dvhT);
                        cmd.Parameters.AddWithValue("@dvv", dvvT);
                        cmd.ExecuteNonQuery();
                    }

                    dvhDB += dvhT;
                    dvvDB += dvvT;
                }

                using (var cmdDB = new SqlCommand(@"
                    UPDATE dbo.DV_DB_VR750
                       SET DVH_DB = @dvh, DVV_DB = @dvv, UpdatedAt = SYSDATETIME()
                     WHERE IdDV = (SELECT TOP(1) IdDV FROM dbo.DV_DB_VR750 ORDER BY IdDV);
                    IF @@ROWCOUNT = 0
                        INSERT INTO dbo.DV_DB_VR750(DVH_DB, DVV_DB) VALUES(@dvh, @dvv);", cn))
                {
                    cmdDB.Parameters.AddWithValue("@dvh", dvhDB);
                    cmdDB.Parameters.AddWithValue("@dvv", dvvDB);
                    cmdDB.ExecuteNonQuery();
                }
            }
        }

        public static (long DVH_DB, long DVV_DB, List<(string Tabla, long DVH_Tabla, long DVV_Tabla)> Detalle)
            GenerarDV_BD_SinPersistir()
        {
            using (var cn = new SqlConnection(_connStr))
            {
                cn.Open();

                long dvhDB = 0, dvvDB = 0;
                var detalle = new List<(string, long, long)>();

                var tablas = GetUserTables(cn);
                foreach (var t in tablas)
                {
                    var dt = ReadTable(cn, t.Schema, t.Name);
                    long dvhT = CalcularDVH_Tabla(dt);
                    long dvvT = CalcularDVV_Tabla(dt);
                    detalle.Add((t.Schema + "." + t.Name, dvhT, dvvT));
                    dvhDB += dvhT; dvvDB += dvvT;
                }

                return (dvhDB, dvvDB, detalle);
            }
        }

        public static (long DVH_DB, long DVV_DB, List<(string Tabla, long DVH_Tabla, long DVV_Tabla)> Detalle)
            LeerDV_Persistido()
        {
            using (var cn = new SqlConnection(_connStr))
            {
                cn.Open();

                long dvhDB = 0, dvvDB = 0;
                using (var cmd = new SqlCommand("SELECT TOP(1) DVH_DB, DVV_DB FROM dbo.DV_DB_VR750;", cn))
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read()) { dvhDB = rd.GetInt64(0); dvvDB = rd.GetInt64(1); }
                }

                var detalle = new List<(string, long, long)>();
                using (var cmd = new SqlCommand("SELECT Tabla, DVH_Tabla, DVV_Tabla FROM dbo.DV_Tabla_VR750;", cn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        detalle.Add((rd.GetString(0), rd.GetInt64(1), rd.GetInt64(2)));
                }

                return (dvhDB, dvvDB, detalle);
            }
        }
    }
}
