using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class PermisoSimple_750VR :IComponentePermiso_750VR
    {
        public int Codigo_750VR { get; set; }
        public string Nombre_750VR { get; set; }

        public List<IComponentePermiso_750VR> ObtenerHijos()
        {
            return new List<IComponentePermiso_750VR>(); // No tiene hijos
        }
    }
}
