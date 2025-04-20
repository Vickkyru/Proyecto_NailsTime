using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_VR750;
using BE_VR750;

namespace BLL_VR750
{
    public class BLLusuario_750VR
    {
        DALusuario_750VR dal;

        public BLLusuario_750VR()
        {
          dal = new DALusuario_750VR();
        }


        public bool CrearUsuario(Usuario_750VR usuario)
        {
            // Validación básica (podés expandir esto)
            if (string.IsNullOrEmpty(usuario.nombre) || string.IsNullOrEmpty(usuario.apellido))
                throw new Exception("Faltan datos obligatorios.");

            // Generar user y contraseña por defecto
            usuario.user = usuario.dni.ToString() + usuario.apellido.ToLower();
            usuario.contraseña = usuario.dni.ToString() + usuario.nombre.ToLower();

            return dal.CrearUsuario(usuario);
        }
    }
}
