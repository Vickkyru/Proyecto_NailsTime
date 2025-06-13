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
            if (this.user != null)
                return false; // Ya hay sesión iniciada

            this.user = userNuevo;

            MessageBox.Show($"Sesión iniciada para: {user.nombre_750VR} {user.apellido_750VR}");

            // ✅ Setear el idioma del usuario
            if (!string.IsNullOrEmpty(userNuevo.idioma_750VR))
            {
                Lenguaje_750VR.ObtenerInstancia().IdiomaActual = userNuevo.idioma_750VR;
            }
            else
            {
                Lenguaje_750VR.ObtenerInstancia().IdiomaActual = "Español";
            }

            return true;
        }

        // Cerrar sesión
        public void CerrarSesion_750VR()
        {
            if (this.user != null)
            {
                MessageBox.Show($"Sesión cerrada para: {user.nombre_750VR} {user.apellido_750VR}");
                user = null;
            }
            else
            {
                MessageBox.Show("No hay sesión activa para cerrar.");
            }
        }

        public bool EstaLogueado_750VR()
        {
            return user != null;
        }

        public BEusuario_750VR UsuarioActual => user;
    }
}
