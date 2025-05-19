using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE_VR750;

namespace SERVICIOS_VR750
{
    public sealed class SessionManager_750VR
    {
      
        private static SessionManager_750VR Instancia;
        public BEusuario_750VR user { get; private set; }

        private SessionManager_750VR() { }

        public static SessionManager_750VR ObtenerInstancia
        {
            get
            {
               
                
                    if (Instancia == null)
                        Instancia = new SessionManager_750VR();
                    return Instancia;
                
            }
        }

     

        // Iniciar sesión
        public bool IniciarSesion_750VR(BEusuario_750VR userNuevo)
        {
            if (user != null)
                return false; // Ya hay sesión iniciada

            user = userNuevo;
            MessageBox.Show($"Sesión iniciada para: {user.nombre_750VR} {user.apellido_750VR}");
            return true;
        }

        // Cerrar sesión
        public void CerrarSesion_750VR()
        {
            if (user != null)
            {
                MessageBox.Show($"Sesión cerrada para: {user.nombre_750VR} {user.apellido_750VR}");
                user = null;
            }
            else
            {
                MessageBox.Show("No hay sesión activa para cerrar.");
            }
        }

        // ¿Está logueado?
        //public bool EstaLogueado_750VR()
        //{
        //    return user != null;
        //}

        //public BEusuario_750VR UsuarioActual => user;
    }
}
