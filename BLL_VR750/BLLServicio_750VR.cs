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
    public class BLLServicio_750VR
    {
        DALservicio_750VR dal;
        public BLLServicio_750VR()
        {
            dal = new DALservicio_750VR();
        }
        public void CrearServicio_750VR(BEServicio_750VR servicio)
        {

            dal.CrearServicio_750VR(servicio);
        }

        //no se si va el id
        public bool ModificarServicio_750VR(int id, string nombre, string tecnica, int duracion, decimal precio)
        {

            return dal.ModificarServicio_750VR(id, nombre, tecnica, duracion, precio);
        }

        public bool CambiarEstadoServicio_750VR(int idServicio, bool nuevoEstado)
        {
            return dal.CambiarEstadoServicio_750VR(idServicio, nuevoEstado);
        }

        public List<BEServicio_750VR> LeerEntidades_750VR()
        {
            return dal.LeerEntidades_750VR();
        }

        public List<BEServicio_750VR> leerEntidadesActivas_750VR()
        {
           
            return dal.leerEntidadesActivas_750VR();
        }

    }
}
