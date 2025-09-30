using BE_VR750;
using DAL_VR750;
using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLserializar_750VR
    {
        private readonly DALserializar_750VR _dal = new DALserializar_750VR();

        public void ExportarClientes(string filePath, List<BECliente_750VR> clientes)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(Lenguaje_750VR.ObtenerEtiqueta("Ser.Error.RutaArchivo"), nameof(filePath));

            if (!filePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                filePath += ".xml";

            if (clientes == null || clientes.Count == 0)
                throw new InvalidOperationException(Lenguaje_750VR.ObtenerEtiqueta("Ser.Error.SinClientes"));

            _dal.GuardarXml(filePath, clientes);

            // Bitácora
            var bllBitacora = new BLLbitacora_750VR();
            bllBitacora.Serializar(filePath);
        }
    }
}
