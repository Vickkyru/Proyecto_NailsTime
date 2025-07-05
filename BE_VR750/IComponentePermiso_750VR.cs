using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public interface IComponentePermiso_750VR
    {
        int Codigo_750VR { get; set; }
        string Nombre_750VR { get; set; }
        List<IComponentePermiso_750VR> ObtenerHijos();
    }
}
