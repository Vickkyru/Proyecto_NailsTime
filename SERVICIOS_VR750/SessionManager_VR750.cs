using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_VR750;

namespace SERVICIOS_VR750
{
    public sealed class SessionManager_VR750
    {
        private static SessionManager_VR750 instancia = null;
        private static readonly object _lock = new object();
        private Usuario_750VR _usuario = null;

        private SessionManager_VR750() { }

        public static SessionManager_VR750 Instancia
        {
            get
            {
                lock (_lock)
                {
                    if (instancia == null)
                        instancia = new SessionManager_VR750();
                    return instancia;
                }
            }
        }

        public void IniciarSesion(Usuario_750VR usuario)
        {
            _usuario = usuario;
        }

        public void CerrarSesion()
        {
            _usuario = null;
        }

        public bool EstaLogueado()
        {
            return _usuario != null;
        }

        public Usuario_750VR Usuario => _usuario;

        public int DNIUsuario => _usuario?.dni ?? 0;
    }
}
