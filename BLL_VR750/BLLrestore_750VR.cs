using DAL_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLrestore_750VR
    {
        private readonly DALrestore_750VR _dal;

     
        public BLLrestore_750VR(string connectionStringToDb)
        {
            if (string.IsNullOrWhiteSpace(connectionStringToDb))
                throw new ArgumentException("Cadena de conexión inválida.", nameof(connectionStringToDb));

            var cb = new SqlConnectionStringBuilder(connectionStringToDb)
            {
                InitialCatalog = "master",
                TrustServerCertificate = true
            };

            _dal = new DALrestore_750VR(cb.ToString(), "ProyectoNailsTime_VR750");
        }

        public void Restaurar(string bakPath, Action<string> onInfo = null,
                              string dataFilePhysicalPath = null, string logFilePhysicalPath = null)
        {
            if (string.IsNullOrWhiteSpace(bakPath))
                throw new ArgumentException("Ruta .bak inválida.", nameof(bakPath));
            if (!File.Exists(bakPath))
                throw new FileNotFoundException("No se encontró el archivo .bak.", bakPath);

            _dal.DoRestore(bakPath, onInfo, dataFilePhysicalPath, logFilePhysicalPath);
            var bllBitacora = new BLLbitacora_750VR();
            bllBitacora.RestoreEjecutado(bakPath);
        }
    }
}
