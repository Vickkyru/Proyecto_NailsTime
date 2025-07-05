using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class GrupoPermiso_750VR : IComponentePermiso_750VR
    {

        public int Codigo_750VR { get; set; }
        public string Nombre_750VR { get; set; }

        public List<IComponentePermiso_750VR> Hijos { get; set; } = new List<IComponentePermiso_750VR>();

        public List<IComponentePermiso_750VR> ObtenerHijos()
        {
            return Hijos;
        }

        public void Agregar(IComponentePermiso_750VR componente)
        {
            Hijos.Add(componente);
        }

        public void Quitar(IComponentePermiso_750VR componente)
        {
            Hijos.Remove(componente);
        }
    }
}
