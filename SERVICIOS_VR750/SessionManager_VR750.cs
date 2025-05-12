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
        public Usuario_750VR user { get; private set; }

        private SessionManager_VR750() { }

        public static SessionManager_VR750 ObtenerInstancia
        {
            get
            {
               
                
                    if (Instancia == null)
                        Instancia = new SessionManager_VR750();
                    return Instancia;
                
            }
        }

     

        // Iniciar sesión
        public bool IniciarSesion(Usuario_750VR userNuevo)
        {
            if (user != null)
                return false; // Ya hay sesión iniciada

            user = userNuevo;
            MessageBox.Show($"Sesión iniciada para: {user.nombre} {user.apellido}");
            return true;
        }

        // Cerrar sesión
        public void CerrarSesion()
        {
            if (user != null)
            {
                MessageBox.Show($"Sesión cerrada para: {user.nombre} {user.apellido}");
                user = null;
            }
            else
            {
                MessageBox.Show("No hay sesión activa para cerrar.");
            }
        }

        // ¿Está logueado?
        public bool EstaLogueado()
        {
            return user != null;
        }

        public Usuario_750VR UsuarioActual => user;
    }
}
