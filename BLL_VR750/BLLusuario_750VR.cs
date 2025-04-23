using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_VR750;
using BE_VR750;




//hacer mejor con la interfaz
namespace BLL_VR750
{
    public class BLLusuario_750VR
    {
        DALusuario_750VR dal;

        public BLLusuario_750VR()
        {
          dal = new DALusuario_750VR();
        }


        public bool CrearUsuario(UsuarioBE usuario)
        {
            if (string.IsNullOrEmpty(usuario.nombre) || string.IsNullOrEmpty(usuario.apellido))
                throw new Exception("Nombre y apellido son obligatorios.");

            usuario.user = usuario.dni + usuario.apellido;
            usuario.contraseña = usuario.dni + usuario.nombre;

            return dal.CrearUsuario(usuario);
        }

        public bool ModificarUsuario(UsuarioBE usuario)
        {
            return dal.ModificarUsuario(usuario);
        }

        public bool BorrarUsuarioLogico(string dni)
        {
            return dal.BorrarUsuarioLogico(dni);
        }

        public void DesbloquearUsuario(string dni)
        {
            dal.DesbloquearUsuario(dni);
        }

        public List<UsuarioBE> ObtenerUsuarios(bool soloActivos)
        {
            return dal.ObtenerUsuarios(soloActivos);
        }

    }
}
