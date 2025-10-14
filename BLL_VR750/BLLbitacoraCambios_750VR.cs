using BE_VR750;
using DAL_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLbitacoraCambios_750VR
    {
        private readonly DALbitacoraCambios_750VR dal = new DALbitacoraCambios_750VR();

        public List<BEinsumoCambios_750VR> ObtenerCambios(int? codInsumo, string nombre, DateTime fechaInicio, DateTime fechaFin)
        {
            return dal.FiltrarCambios(codInsumo, nombre, fechaInicio, fechaFin);
        }

        public void ActivarVersion(int codInsumo, DateTime fecha, TimeSpan hora)
        {
            dal.ActivarRegistro(codInsumo, fecha, hora);
        }
    }
}
