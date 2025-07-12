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
        //public bool EsFamilia => true;

        public GrupoPermiso_750VR(int codigo, string nombre)
        {
            Codigo_750VR = codigo;
            Nombre_750VR = nombre;
        }

        //public List<IComponentePermiso_750VR> Hijos { get; set; } = new List<IComponentePermiso_750VR>();

        // Lista de hijos (permisos simples y/o sub-familias)
        private readonly List<IComponentePermiso_750VR> _hijos = new List<IComponentePermiso_750VR>();
        public List<IComponentePermiso_750VR> Hijos => _hijos;

        public List<IComponentePermiso_750VR> ObtenerHijos() => _hijos;

        public void Agregar(IComponentePermiso_750VR componente) => _hijos.Add(componente);
        public void Quitar(IComponentePermiso_750VR componente) => _hijos.Remove(componente);

        // 🔹 Implementación recursiva de Listar
        public List<string> Listar()
        {
            List<string> resultado = new List<string>();

            foreach (var hijo in _hijos)
                resultado.AddRange(hijo.Listar());   // recursión
            return resultado;
        }
    }
}
