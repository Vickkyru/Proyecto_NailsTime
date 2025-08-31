using BE_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_VR750;

namespace BLL_VR750
{
    public class BLLCliente_750VR
    {
        private readonly DALcliente_750VR dal;
        private readonly BLLbitacora_750VR log;

        public BLLCliente_750VR()
        {
            dal = new DALcliente_750VR();
            log = new BLLbitacora_750VR();
        }
        public BECliente_750VR ObtenerClientePorDNI_750VR(int dni)
        {
            return dal.ObtenerClientePorDNI_750VR(dni);
        }


        public void CrearCliente_750VR(BECliente_750VR usuario)
        {


            dal.CrearCliente_750VR(usuario);
            log.CrearCliente(usuario.dni_750VR);
        }

        public bool ModificarCliente_750VR(int dni, string nombre, string apellido, string mail, string dire,int celu)
        {
            var ok = dal.ModificarCliente_750VR(dni, nombre, apellido, mail, dire, celu);
            if (ok) log.ModificarCliente(dni);  // Criticidad 2
            return ok;
        }

        public bool CambiarEstadoCliente_750VR(int dni, bool nuevoEstado)
        {
            var ok = dal.CambiarEstadoCliente_750VR(dni, nuevoEstado);
            if (ok)
            {
                if (nuevoEstado)
                    // No había helper específico: usamos libre (Criticidad 2)
                    log.RegistrarLibre($"Activar cliente DNI={dni}", "Administrador", Criticidad_750VR.C2);
                else
                    log.RegistrarLibre($"Desactivar cliente DNI={dni}", "Administrador", Criticidad_750VR.C2);
            }
            return ok;
        }

        public List<BECliente_750VR> BuscarClientes_750VR(string dni, string nombre, string apellido, string email, string dire, string celu)
        {
            return dal.BuscarClientes_750VR(dni, nombre, apellido, email, dire, celu);
        }

        public List<BECliente_750VR> leerEntidades_750VR()
        {
            return dal.leerEntidades_750VR();
        }

    }
}
