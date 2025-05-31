using BE_VR750;
using DAL_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLdisponibilidad_750VR
    {
        DALdisponibilidad_750VR dal = new DALdisponibilidad_750VR();

        public void CrearDisponibilidad_750VR(BEdisponibilidad_750VR disp)
        {
            dal.CrearDisponibilidad_750VR(disp);
        }

        public bool ModificarDisponibilidad_750VR (BEdisponibilidad_750VR dispo)
        {
            return dal.ModificarDisponibilidad_750VR(dispo);
        }

        public bool CambiarEstado_750VR(int id, bool nuevoEstado)
        {
            return CambiarEstado_750VR(id, nuevoEstado);
        }

        public List<BEdisponibilidad_750VR> LeerDisponibilidades_750VR()
        {
            return dal.LeerDisponibilidades_750VR();
        }
    }
}
