using BE_VR750;
using DAL_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLfactura_750VR
    {
        private readonly DALfactura_750VR dal = new DALfactura_750VR();
        private readonly BLLbitacora_750VR log = new BLLbitacora_750VR();


        public int GenerarFactura(BEfactura_750VR factura)
        {
            return dal.InsertarFactura(factura); // devuelve CodFactura_VR750
        }

        // mantener:
        public List<BEfactura_750VR> ObtenerFacturas() => dal.LeerFacturas();
        public BEfactura_750VR ObtenerFacturaPorCodigo(int codFactura) => dal.ObtenerFacturaPorCodigo(codFactura);

        public void ImprimirFactura(int codFactura)
        {
            log.ImprimirFactura(codFactura); // sólo log (la impresión real la hace el Form)
        }

    }
}
