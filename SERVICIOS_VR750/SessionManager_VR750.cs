using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE_VR750;

namespace SERVICIOS_VR750
{
    public sealed class SessionManager_VR750
    {
      
        private static SessionManager_VR750 Instancia;
        public Usuario_750VR user;

        private SessionManager_VR750() { }

        public static SessionManager_VR750 ObtenerInstancia
        {
            get
            {
                
                //if (Instancia == null) throw new Exception("Sesion no iniciada");
                return Instancia;
            }
        }

        public bool EstaLogueado()
        {
            return user != null;
        }

        public static void IniciarSesion(Usuario_750VR userNuevo)
        {
          

            if (Instancia == null)
            {
                Instancia = new SessionManager_VR750();
                Instancia.user = userNuevo;
   
                MessageBox.Show($"Se inicio la sesión de {Instancia.user.nombre}");
            }
            else
            {
                throw new Exception("Sesion ya iniciada");
            }
        }

        public static void CerrarSesion()
        {
            if (Instancia != null)
            {

                Instancia = null;
                MessageBox.Show($" Se cerro sesión de {Instancia.user.nombre}");
            }
            else
            {
                throw new Exception("Sesion no iniciada");
            }
        }

        public Usuario_750VR UsuarioActual => user;
    }
}
