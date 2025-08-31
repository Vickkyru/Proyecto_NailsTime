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
        private readonly DALdisponibilidad_750VR dal = new DALdisponibilidad_750VR();
        private readonly BLLbitacora_750VR log = new BLLbitacora_750VR();

        public void CrearDisponibilidad_750VR(BEdisponibilidad_750VR disp)
        {
            dal.CrearDisponibilidad_750VR(disp);
            log.CrearDisponibilidad(disp.CodDisponibilidad_750VR, disp.DNImanic_750VR);
        }
        public List<BEdisponibilidad_750VR> ObtenerDisponibilidadesPorManicurista(int dniManicurista)
        {
            return dal.ObtenerDisponibilidadesPorManicurista(dniManicurista);

        }

        public bool ModificarDisponibilidad_750VR (BEdisponibilidad_750VR dispo)
        {
            var ok = dal.ModificarDisponibilidad_750VR(dispo);
            if (ok) log.ModificarDisponibilidad(dispo.CodDisponibilidad_750VR);
            return ok;
        }

        public bool CambiarEstado_750VR(int id, bool nuevoEstado)
        {
            var ok = dal.CambiarEstado_750VR(id, nuevoEstado);
            if (ok)
            {
                if (nuevoEstado)
                    log.RegistrarLibre($"Activar disponibilidad Id={id}", "Administrador", Criticidad_750VR.C2);
                else
                    log.RegistrarLibre($"Desactivar disponibilidad Id={id}", "Administrador", Criticidad_750VR.C2);
            }
            return ok;
        }

        public List<BEdisponibilidad_750VR> LeerDisponibilidades_750VR()
        {
            return dal.LeerDisponibilidades_750VR();
        }


      
    }
}
