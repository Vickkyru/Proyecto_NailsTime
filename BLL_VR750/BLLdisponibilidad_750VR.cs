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

        public void CrearDisponibilidad(BEdisponibilidad_750VR disp)
        {
            dal.CrearDisponibilidad(disp);
        }

        public bool ModificarDisponibilidad (BEdisponibilidad_750VR dispo)
        {
            return dal.ModificarDisponibilidad(dispo);
        }

        public bool CambiarEstadoActivo(int id, bool nuevoEstado)
        {
            return CambiarEstadoActivo(id, nuevoEstado);
        }

        public List<BEdisponibilidad_750VR> LeerDisponibilidades()
        {
            return dal.LeerDisponibilidades();
        }
    }
}
