using BE_VR750;
using DAL_VR750;
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

        /// <summary> Valida y delega la serialización. </summary>
        public void ExportarClientes(string filePath, List<BECliente_750VR> clientes)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Debe indicar una ruta de archivo.", nameof(filePath));

            if (!filePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                filePath += ".xml";

            if (clientes == null || clientes.Count == 0)
                throw new InvalidOperationException("No hay clientes para exportar.");

            _dal.GuardarXml(filePath, clientes);
        }
    }
}
