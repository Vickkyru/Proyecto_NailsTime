using BE_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_VR750;

namespace BLL_VR750
{
    public class BLLperfil_750VR
    {

        private DALperfil_750VR dal = new DALperfil_750VR();

        public List<BEperfil_750VR> ObtenerPerfiles()
        {
            return dal.ObtenerPerfiles();
        }

        public void AgregarPerfil(BEperfil_750VR perfil)
        {
            dal.InsertarPerfil(perfil);
        }

        public void EliminarPerfil(int id)
        {
            dal.EliminarPerfil(id);
        }

        public void AsignarPermiso(int idPerfil, int idPermiso)
        {
            dal.AsignarPermiso(idPerfil, idPermiso);
        }

        public void AsignarFamilia(int idPerfil, int idFamilia)
        {
            dal.AsignarFamilia(idPerfil, idFamilia);
        }

        public List<IComponentePermiso_750VR> ObtenerPermisosDePerfil(int idPerfil)
        {
            return dal.ObtenerPermisosDePerfil(idPerfil);
        }
    }
}
