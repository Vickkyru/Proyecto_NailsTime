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


        public bool CrearUsuario(Usuario_750VR usuario)
        {
            if (string.IsNullOrEmpty(usuario.nombre) || string.IsNullOrEmpty(usuario.apellido))
                throw new Exception("Nombre y apellido son obligatorios.");

            usuario.user = usuario.dni + usuario.apellido;
            usuario.contraseña = usuario.dni + usuario.nombre;

            return dal.CrearUsuario(usuario);
        }

        public bool ModificarUsuario(Usuario_750VR usuario)
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

        public List<Usuario_750VR> ObtenerUsuarios(bool soloActivos)
        {
            return dal.ObtenerUsuarios(soloActivos);
        }

        public List<Usuario_750VR> BuscarUsuarios(string dni, string nombre, string apellido, string email)
        {
            return dal.BuscarUsuarios(dni, nombre, apellido, email);
        }

        private int intentosFallidos = 0; // contador de intentos fallidos

        public string Login(string usuarioLogin, string contraseñaIngresada)
        {
            //UsuarioDAL dal = new UsuarioDAL();
            //Usuario_VR750 user = dal.ObtenerUsuarioPorLogin(usuarioLogin);

            //if (user == null)
            //{
            //    intentosFallidos++;
            //    return GestionarIntentosFallidos(usuarioLogin);
            //}

            //if (!user.Activo)
            //    return "Cuenta inactiva. Comuníquese con el administrador.";

            //if (user.Bloqueado)
            //    return "Cuenta bloqueada. Contacte al administrador.";

            //string contraseñaHasheadaIngresada = Servicios.Encriptador.HashearContraseña(contraseñaIngresada, user.Salt);

            //if (contraseñaHasheadaIngresada == user.Contra)
            //{
            //    intentosFallidos = 0;
            //    Servicios.SingletonSesion.Instancia.Login(user); // AQUÍ GUARDA la sesión
            //    return "Login exitoso";
            //}
            //else
            //{
            //    intentosFallidos++;
            //    return GestionarIntentosFallidos(usuarioLogin);
            //}

        }

    }
}
