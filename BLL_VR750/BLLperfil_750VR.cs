using BE_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_VR750;
using System.Data.SqlClient;

namespace BLL_VR750
{
    public class BLLperfil_750VR
    {

        private DALperfil_750VR dal = new DALperfil_750VR();

        public List<BEperfil_750VR> ObtenerPerfiles() => dal.ObtenerPerfiles();

        public void AgregarPerfil(BEperfil_750VR perfil)
        {
            perfil.CodPerfil_750VR = dal.InsertarPerfilYDevolverID(perfil);
        }

        public List<BEperfil_750VR> ObtenerTodosLosPerfiles()
        {
           return dal.ObtenerTodosLosPerfiles();
        }

        public void EliminarPerfil(int id)
        {
            dal.EliminarPerfil(id);
        }

        public bool AsignarPermiso(int idPerfil, int idPermiso)
        {
           return dal.AsignarPermiso(idPerfil, idPermiso);
        }

        public void AsignarFamilia(int idPerfil, int idFamilia)
        {
            dal.AsignarFamilia(idPerfil, idFamilia);
        }

        public void QuitarFamilia(int idPerfil, int idFamilia)
        {
            dal.QuitarFamilia(idPerfil, idFamilia);
        }

        public void QuitarPermiso(int idPerfil, int idPermiso)
        {
            dal.QuitarPermiso(idPerfil, idPermiso);
        }

        public void AgregarFamilia(string nombre)
        {
            dal.InsertarFamilia(nombre);
        }

        public void EliminarFamilia(int codFamilia)
        {
            dal.EliminarFamilia(codFamilia);
        }

        public bool AgregarPermisoAFamilia(int idFamilia, int idPermiso)
        {
           return dal.AgregarPermisoAFamilia(idFamilia, idPermiso);
        }

        public void QuitarPermisoDeFamilia(int idFamilia, int idPermiso)
        {
            dal.QuitarPermisoDeFamilia(idFamilia, idPermiso);
        }

        public bool AsignarFamiliaAFamilia(int idPadre, int idHija)
        {
            try
            {
                dal.AsignarFamiliaAFamilia(idPadre, idHija);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public void QuitarFamiliaDeFamilia(int idPadre, int idHija)
        {
            dal.QuitarFamiliaDeFamilia(idPadre, idHija);
        }

        public GrupoPermiso_750VR ObtenerFamiliaPorId(int idFamilia)
        {
           return dal.ObtenerFamiliaPorId(idFamilia);
        }

        public List<PermisoSimple_750VR> ObtenerPermisosSimples()
        {
           return  dal.ObtenerPermisosSimples();
        }

        public List<GrupoPermiso_750VR> ObtenerFamilias()
        {
           return dal.ObtenerFamilias();
        }

        public List<IComponentePermiso_750VR> ObtenerPermisosDePerfil(int idPerfil)
        {
            var permisos = dal.ObtenerPermisosDePerfil(idPerfil);

            foreach (var permiso in permisos.OfType<GrupoPermiso_750VR>())
            {
                permiso.ObtenerHijos().AddRange(ObtenerPermisosDeFamilia(permiso.Codigo_750VR));
            }

            return permisos;
        }

        public List<IComponentePermiso_750VR> ObtenerPermisosDePerfilPorNombre(string nombrePerfil)
        {
            return dal.ObtenerPermisosDePerfilPorNombre(nombrePerfil);
        }

        public List<IComponentePermiso_750VR> ObtenerPermisosDeFamilia(int codFamilia)
        {
            var hijos = dal.ObtenerHijosDeFamilia(codFamilia);

            foreach (var familia in hijos.OfType<GrupoPermiso_750VR>())
            {
                familia.ObtenerHijos().AddRange(ObtenerPermisosDeFamilia(familia.Codigo_750VR));
            }

            return hijos;
        }

        public void ObtenerPermisosRecursivos(IComponentePermiso_750VR componente, List<IComponentePermiso_750VR> acumulador)
        {
            if (!acumulador.Any(p => p.Codigo_750VR == componente.Codigo_750VR))
                acumulador.Add(componente);

            foreach (var hijo in componente.ObtenerHijos())
            {
                ObtenerPermisosRecursivos(hijo, acumulador);
            }
        }

    }
}
