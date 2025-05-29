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
        DALcliente_750VR dal;

        public BLLCliente_750VR()
        {
                dal = new DALcliente_750VR();
        }

        public BECliente_750VR ObtenerClientePorDNI_750VR(int dni)
        {
            return dal.ObtenerClientePorDNI_750VR(dni);
        }


        public void CrearCliente_750VR(BECliente_750VR usuario)
        {


            dal.CrearCliente_750VR(usuario);
        }

        public bool ModificarCliente_750VR(int dni, string nombre, string apellido, string mail, string dire,int celu)
        {

            return dal.ModificarCliente_750VR(dni, nombre, apellido, mail, dire,celu);
        }

        public bool CambiarEstadoCliente_750VR(int dni, bool nuevoEstado)
        {
            return dal.CambiarEstadoCliente_750VR(dni, nuevoEstado);
        }

        public List<BECliente_750VR> BuscarClientes_750VR(string dni, string nombre, string apellido, string email, string dire, string celu)
        {
            return dal.BuscarClientes_750VR(dni, nombre, apellido, email, dire, celu);
        }
    }
}
