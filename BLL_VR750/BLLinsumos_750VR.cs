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

        private readonly DALinsumos_750VR dal = new DALinsumos_750VR();
        private readonly BLLbitacora_750VR log = new BLLbitacora_750VR();

        public void CrearInsumo_750VR(BEinsumos_750VR insumo)
        {
            dal.CrearInsumo_750VR(insumo);
            log.CrearInsumo(insumo.CodInsumo_750VR);
        }

        public bool ModificarInsumo_750VR(BEinsumos_750VR insumo)
        {
            var ok = dal.ModificarInsumo_750VR(insumo);
            if (ok) log.ModificarInsumo(insumo.CodInsumo_750VR);
            return ok;
        }

        public bool CambiarEstado_750VR(int codInsumo, bool nuevoEstado)
        {
            var ok = dal.CambiarEstado_750VR(codInsumo, nuevoEstado);
            if (ok)
            {
                if (nuevoEstado)
                    log.RegistrarLibre($"Activar insumo Cod={codInsumo}", "Administrador", Criticidad_750VR.C2);
                else
                    log.RegistrarLibre($"Desactivar insumo Cod={codInsumo}", "Administrador", Criticidad_750VR.C2);
            }
            return ok;
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
