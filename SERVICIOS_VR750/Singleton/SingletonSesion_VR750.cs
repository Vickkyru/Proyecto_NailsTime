using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS_VR750.Singleton
{
    public static class SingletonSesion_VR750
    {
        private static Sesion_VR750 instancia;
        private static readonly object lockObj = new object();

        public static Sesion_VR750 Instancia
        {
            get
            {
                lock (lockObj)
                {
                    if (instancia == null)
                        instancia = new Sesion_VR750();
                    return instancia;
                }
            }
        }

    }
}
