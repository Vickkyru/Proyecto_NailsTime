using BE_VR750;
using DAL_VR750;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLdesserializar_750VR
    {
        private readonly DALdesserializar_750VR _dal = new DALdesserializar_750VR();

        /// <summary> Valida y delega la deserialización. </summary>
        public List<BECliente_750VR> ImportarClientes(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Debe seleccionar un archivo XML.", nameof(filePath));
            if (!File.Exists(filePath))
                throw new FileNotFoundException("No se encontró el archivo XML.", filePath);

            return _dal.LeerXml(filePath);
        }
    }
}
