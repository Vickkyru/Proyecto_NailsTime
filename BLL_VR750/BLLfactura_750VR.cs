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
        private DALfactura_750VR dal = new DALfactura_750VR();

        public bool GenerarFactura(BEfactura_750VR factura)
        {
            return dal.InsertarFactura(factura);
        }

        public List<BEfactura_750VR> ObtenerFacturas()
        {
            return dal.LeerFacturas();
        }
    }
}
