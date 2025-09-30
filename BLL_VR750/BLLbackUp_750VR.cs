using DAL_VR750;
using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLbackUp_750VR
    {
        private readonly DALbackUp_750VR _dal;

        public BLLbackUp_750VR(string connectionStringToDb)
        {
            if (string.IsNullOrWhiteSpace(connectionStringToDb))
                throw new ArgumentException(Lenguaje_750VR.ObtenerEtiqueta("Backup.Error.CadenaConexion"), nameof(connectionStringToDb));

            var cb = new SqlConnectionStringBuilder(connectionStringToDb)
            {
                InitialCatalog = "master",
                TrustServerCertificate = true
            };
            _dal = new DALbackUp_750VR(cb.ToString(), "ProyectoNailsTime_VR750");
        }

        public void GenerarBackup(string fullBackupPath, Action<string> onInfo = null)
        {
            if (string.IsNullOrWhiteSpace(fullBackupPath))
                throw new ArgumentException(Lenguaje_750VR.ObtenerEtiqueta("Backup.Error.RutaInvalida"), nameof(fullBackupPath));

            var dir = Path.GetDirectoryName(fullBackupPath);
            if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
                throw new DirectoryNotFoundException(Lenguaje_750VR.ObtenerEtiqueta("Backup.Error.CarpetaNoExiste"));

            _dal.DoBackup(fullBackupPath, onInfo);

            var bllBitacora = new BLLbitacora_750VR();
            bllBitacora.BackupEjecutado(fullBackupPath);
        }

    }
}
