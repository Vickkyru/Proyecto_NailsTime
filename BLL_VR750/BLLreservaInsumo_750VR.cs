using DAL_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLreservaInsumo_750VR
    {
        private DALReservaInsumo_750VR dal = new DALReservaInsumo_750VR();

        public void RegistrarInsumoUsado(int idReserva, int idInsumo, int cantidad)
        {
            dal.InsertarInsumoReserva(idReserva, idInsumo, cantidad);
        }

        public bool InsumoYaAgregado(int idReserva, int idInsumo)
        {
            return dal.YaExisteInsumoParaReserva(idReserva, idInsumo);
        }


    }
}
