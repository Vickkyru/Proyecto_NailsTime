using BE_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL_VR750;

namespace BLL_VR750
{
    public class BLLReserva_750VR
    {
        DALreserva_750VR dal;
        public List<BEReserva_750VR> ObtenerReservasPorManicurista(int dniManicurista)
        {
            return dal.ObtenerReservasPorManicurista(dniManicurista);
        }
    }
}
