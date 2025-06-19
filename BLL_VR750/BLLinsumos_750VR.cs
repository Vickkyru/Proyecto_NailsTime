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

        public List<BEinsumos_750VR> ObtenerInsumosActivos()
        {
            var todos = dal.LeerInsumos();
            return todos.Where(i => i.activo_750VR == true).ToList();
        }
    }
}
