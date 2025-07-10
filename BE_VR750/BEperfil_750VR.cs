using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class BEperfil_750VR
    {

        //public int CodPerfil_750VR { get; set; }             // ID en base de datos
        //public string NombrePerfil_750VR { get; set; }      // Nombre visible del perfil

        //// Permisos asociados al perfil (componente raíz del patrón Composite)
        //public List<IComponentePermiso_750VR> Permisos_750VR { get; set; }

        //public BEperfil_750VR()
        //{
        //    Permisos_750VR = new List<IComponentePermiso_750VR>();
        //}

        //// Método auxiliar para obtener todos los permisos simples
        //public List<PermisoSimple_750VR> ObtenerPermisosSimples()
        //{
        //    List<PermisoSimple_750VR> resultado = new List<PermisoSimple_750VR>();
        //    foreach (var p in Permisos_750VR)
        //        ObtenerPermisosSimplesRecursivo(p, resultado);

        //    return resultado;
        //}

        //private void ObtenerPermisosSimplesRecursivo(IComponentePermiso_750VR comp, List<PermisoSimple_750VR> resultado)
        //{
        //    if (comp is PermisoSimple_750VR simple)
        //    {
        //        resultado.Add(simple);
        //    }
        //    else if (comp is GrupoPermiso_750VR familia)
        //    {
        //        foreach (var hijo in familia.ObtenerHijos())
        //        {
        //            ObtenerPermisosSimplesRecursivo(hijo, resultado);
        //        }
        //    }
        //}

        public int CodPerfil_750VR { get; set; }
        public string NombrePerfil_750VR { get; set; }

        //public List<IComponentePermiso_750VR> Permisos_750VR { get; } = new();
        public List<IComponentePermiso_750VR> Permisos_750VR { get; set; }

        public List<PermisoSimple_750VR> ObtenerPermisosSimples()
        {
            var resultado = new List<PermisoSimple_750VR>();
            foreach (var p in Permisos_750VR)
                Recorrer(p, resultado);
            return resultado;

            void Recorrer(IComponentePermiso_750VR comp, List<PermisoSimple_750VR> lista)
            {
                switch (comp)
                {
                    case PermisoSimple_750VR simple:
                        lista.Add(simple);
                        break;
                    case GrupoPermiso_750VR fam:
                        foreach (var hijo in fam.ObtenerHijos())
                            Recorrer(hijo, lista);
                        break;
                }
            }
        }
    }
}
