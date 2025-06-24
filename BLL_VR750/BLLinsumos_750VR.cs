using BE_VR750;
using DAL_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLinsumos_750VR
    {

        private DALinsumos_750VR dal = new DALinsumos_750VR();

        public void CrearInsumo_750VR(BEinsumos_750VR insumo)
        {
            dal.CrearInsumo_750VR(insumo);
        }

        public bool ModificarInsumo_750VR(BEinsumos_750VR insumo)
        {
            return dal.ModificarInsumo_750VR(insumo);
        }

        public bool CambiarEstado_750VR(int codInsumo, bool nuevoEstado)
        {
            return dal.CambiarEstado_750VR(codInsumo, nuevoEstado);
        }

        public List<BEinsumos_750VR> LeerInsumos_750VR()
        {
            return dal.LeerInsumos_750VR();
        }

        public List<BEinsumos_750VR> LeerInsumosActivos_750VR()
        {
            return dal.LeerInsumosActivos_750VR();
        }
    }
}
