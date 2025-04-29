using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_VR750;

namespace SERVICIOS_VR750.Singleton
{
    public class Sesion_VR750
    {
        private Usuario_750VR _usuario;

        public Usuario_750VR Usuario => _usuario;


        public void Login(Usuario_750VR usuario)
        {
            _usuario = usuario;
        }

        public void Logout()
        {
            _usuario = null;
        }

        public bool EstaLogueado()
        {
            return _usuario != null;
        }
    }
}
