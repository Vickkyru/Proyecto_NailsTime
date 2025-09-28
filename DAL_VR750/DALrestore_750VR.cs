using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALrestore_750VR
    {
        private readonly string _connToMaster;
        private readonly string _dbName;

        public DALrestore_750VR(string connectionStringToMaster, string databaseName)
        {
            _connToMaster = connectionStringToMaster ?? throw new ArgumentNullException(nameof(connectionStringToMaster));
            _dbName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public void DoRestore(string bakPath, Action<string> onInfo = null,
                              string dataFilePhysicalPath = null, string logFilePhysicalPath = null)
        {
            using (var cn = new SqlConnection(_connToMaster))
            {
                if (onInfo != null)
                    cn.InfoMessage += (s, e) => onInfo(e.Message);

                cn.Open();

                string toSingle = @"ALTER DATABASE [" + _dbName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                string toMulti = @"ALTER DATABASE [" + _dbName + "] SET MULTI_USER;";

                string moveClause = string.Empty;
                if (!string.IsNullOrWhiteSpace(dataFilePhysicalPath) && !string.IsNullOrWhiteSpace(logFilePhysicalPath))
                {
                    string logicalData = _dbName;
                    string logicalLog = _dbName + "_log";
                    moveClause = ", MOVE N'" + logicalData + "' TO N'" + dataFilePhysicalPath + "', " +
                                 "MOVE N'" + logicalLog + "'  TO N'" + logFilePhysicalPath + "'";
                }

                string restore = @"
RESTORE DATABASE [" + _dbName + @"]
FROM DISK = @p
WITH REPLACE, STATS=10" + moveClause + ";";

                using (var cmd = new SqlCommand(toSingle, cn) { CommandTimeout = 0 })
                {
                    cmd.ExecuteNonQuery();
                }

                try
                {
                    using (var cmd = new SqlCommand(restore, cn) { CommandTimeout = 0 })
                    {
                        cmd.Parameters.AddWithValue("@p", bakPath);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch
                {
                    using (var cmd = new SqlCommand(toMulti, cn) { CommandTimeout = 0 })
                    {
                        cmd.ExecuteNonQuery();
                    }
                    throw;
                }

                using (var cmd = new SqlCommand(toMulti, cn) { CommandTimeout = 0 })
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
