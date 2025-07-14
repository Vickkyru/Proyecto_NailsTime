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
        public List<string> PermisosDelUsuario { get; set; } = new List<string>();

        public void EstablecerPermisos(List<string> permisos)
        {
            PermisosDelUsuario = permisos;
        }


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

        public string IdiomaActual
        {
            get => _idiomaActual;
            set
            {
                _idiomaActual = value;
                // Propaga el cambio al sistema de traducción
                Lenguaje_750VR.ObtenerInstancia().IdiomaActual = _idiomaActual;
            }
        }
        private string _idiomaActual = "Español";   // valor por defecto



      
        public bool IniciarSesion_750VR(BEusuario_750VR userNuevo)
        {
            if (this.user != null)
                return false; // Ya hay sesión iniciada

            this.user = userNuevo;

            MessageBox.Show($"Sesión iniciada para: {user.nombre_750VR} {user.apellido_750VR}");
     

            // ✅ Setear el idioma por defecto o el que ya tenía en SessionManager
            if (string.IsNullOrEmpty(SessionManager_750VR.ObtenerInstancia.IdiomaActual))
                SessionManager_750VR.ObtenerInstancia.IdiomaActual = "Español";

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
