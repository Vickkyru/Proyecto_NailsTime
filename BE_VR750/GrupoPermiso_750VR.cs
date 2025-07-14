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

        public List<PermisoSimple_750VR> ObtenerTodosLosPermisos()
        {
            var lista = new List<PermisoSimple_750VR>();

            foreach (var hijo in this.Hijos)        // Recorre todos los hijos de la familia
            {
                if (hijo is PermisoSimple_750VR permisoSimple)      // ⬅️ Es un permiso simple
                {
                    lista.Add(permisoSimple);
                }
                else if (hijo is GrupoPermiso_750VR grupo)          // ⬅️ Es otra familia
                {
                    // 🔁 Llama recursivamente para acumular los permisos de la sub-familia
                    lista.AddRange(grupo.ObtenerTodosLosPermisos());
                }
            }

            return lista;   // Devuelve la lista plana de permisos simples
        }
    }
}
