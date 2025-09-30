using BE_VR750;
using DAL_VR750;
using SERVICIOS_VR750;
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

        public List<BECliente_750VR> ImportarClientes(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(Lenguaje_750VR.ObtenerEtiqueta("Deser.Error.SeleccionarXML"), nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException(Lenguaje_750VR.ObtenerEtiqueta("Deser.Error.XMLNoEncontrado"), filePath);

            var clientes = _dal.LeerXml(filePath);

            // Bitácora
            var bllBitacora = new BLLbitacora_750VR();
            bllBitacora.DesSerializar(filePath);

            return clientes;
        }
    }
}
