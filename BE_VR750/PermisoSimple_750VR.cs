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
        //public bool EsFamilia => false;


        public List<IComponentePermiso_750VR> ObtenerHijos()
        {
            return new List<IComponentePermiso_750VR>(); 
        }

        public void Agregar(IComponentePermiso_750VR componente)
        {
            throw new NotImplementedException("Un permiso simple no puede tener hijos.");
        }

        public void Quitar(IComponentePermiso_750VR componente)
        {
            throw new NotImplementedException("Un permiso simple no puede tener hijos.");
        }

        
        public PermisoSimple_750VR(int codigo, string nombre)
        {
            Codigo_750VR = codigo;
            Nombre_750VR = nombre;
        }

 
        public List<string> Listar() => new List<string> { Nombre_750VR };
    }
}
