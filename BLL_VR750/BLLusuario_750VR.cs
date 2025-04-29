using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_VR750;
using BE_VR750;
using SERVICIOS_VR750;




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


        public void CrearUsuario(Usuario_750VR usuario)
        {
           
            dal.CrearUsuario(usuario);
        }

        public void ModificarUsuario(Usuario_750VR usuario)
        {
           
            dal.ModificarUsuario(usuario);
        }

        public bool BorrarUsuarioLogico(string dni)
        {
            return dal.BorrarUsuarioLogico(dni);
        }

        public void DesbloquearUsuario(int dni)
        {
            
            dal.DesbloquearUsuario(dni); // le pasa el dni
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
           
            Usuario_750VR user = dal.ObtenerUsuarioPorLogin(usuarioLogin);

            if (user == null)
            {
                intentosFallidos++;
                return GestionarIntentosFallidos(usuarioLogin);
            }

            if (!user.activo)
                return "Cuenta inactiva. Comuníquese con el administrador.";

            if (user.bloqueado)
                return "Cuenta bloqueada. Contacte al administrador.";

            string contraseñaHasheadaIngresada = SERVICIOS_VR750.Encriptador_VR750.HashearConSalt(contraseñaIngresada, user.salt);

            if (contraseñaHasheadaIngresada == user.contraseña)
            {
                intentosFallidos = 0;
                SERVICIOS_VR750.Singleton.SingletonSesion_VR750.Instancia.Login(user); // AQUÍ GUARDA la sesión
                return "Login exitoso";
            }
            else
            {
                intentosFallidos++;
                return GestionarIntentosFallidos(usuarioLogin);
            }

        }

        private string GestionarIntentosFallidos(string usuarioLogin)
        {
           

            if (intentosFallidos >= 3)
            {
                
                dal.BloquearUsuario(usuarioLogin); // Bloquea en base de datos

                intentosFallidos = 0; // Resetear intentos después de bloquear
                return "Usuario bloqueado por múltiples intentos fallidos.";
            }

            return $"Usuario o contraseña incorrectos. Intentos fallidos: {intentosFallidos}/3";
        }

        public Usuario_750VR ObtenerUsuarioPorDNI(int dni)
        {
            return dal.ObtenerUsuarioPorDNI(dni);
        }
    }
}
