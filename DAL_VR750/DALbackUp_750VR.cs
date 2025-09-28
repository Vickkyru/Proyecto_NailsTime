using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALbackUp_750VR
    {
        private readonly string _connToMaster;
        private readonly string _dbName;

        public DALbackUp_750VR(string connectionStringToMaster, string databaseName)
        {
            _connToMaster = connectionStringToMaster ?? throw new ArgumentNullException(nameof(connectionStringToMaster));
            _dbName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public void DoBackup(string fullPath, Action<string> onInfo = null)
        {
            using (var cn = new SqlConnection(_connToMaster))
            {
                if (onInfo != null)
                    cn.InfoMessage += (s, e) => onInfo(e.Message);

                cn.Open();

                var sql = @"
DECLARE @p nvarchar(4000) = @path;
DECLARE @supportsCompression bit =
    CASE WHEN CAST(SERVERPROPERTY('Edition') AS nvarchar(128)) LIKE '%Express%' THEN 0 ELSE 1 END;

DECLARE @stmt nvarchar(max) =
N'BACKUP DATABASE [ProyectoNailsTime_VR750] TO DISK = @p WITH INIT, STATS=10, CHECKSUM'
+ CASE WHEN @supportsCompression = 1 THEN N', COMPRESSION' ELSE N'' END
+ N';';

EXEC sp_executesql @stmt, N'@p nvarchar(4000)', @p=@p;

RESTORE VERIFYONLY FROM DISK = @p WITH CHECKSUM;";

                using (var cmd = new SqlCommand(sql, cn))
                {
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@path", fullPath);
                    cmd.ExecuteNonQuery();
                }
            }
        }
}
}

