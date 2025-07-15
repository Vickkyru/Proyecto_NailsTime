using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
        private static string _idiomaActual = "Español";
        public static string IdiomaActual
        {
            get => _idiomaActual;
            set
            {
                _idiomaActual = string.IsNullOrWhiteSpace(value) ? "Español" : value;
                Lenguaje_750VR.ObtenerInstancia().IdiomaActual = _idiomaActual;
            }
        }


        public static SessionManager_750VR ObtenerInstancia
        => Instancia ?? (Instancia = new SessionManager_750VR());

        public void EstablecerPermisos(List<string> permisos)
        {
            PermisosDelUsuario = permisos;
        }


        private SessionManager_750VR() {  }

       


        public bool IniciarSesion_750VR(BEusuario_750VR usuario)
        {
            if (user != null) return false;

            user = usuario;

            
            IdiomaActual = usuario.idioma_750VR ?? "Español";


            MessageBox.Show($"Sesión iniciada para: {usuario.nombre_750VR} {usuario.apellido_750VR}");
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

    }
}
