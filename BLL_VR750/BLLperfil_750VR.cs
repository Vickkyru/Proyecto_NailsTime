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

     
        public bool PerfilTieneUsuariosAsociados(int codPerfil)
        {
            return dal.ExisteUsuarioConPerfil(codPerfil);
        }

        public void ModificarNombrePerfil(int codPerfil, string nuevoNombre)
        {
            dal.ActualizarNombrePerfil(codPerfil, nuevoNombre);
        }
        public bool FamiliaContieneAFamilia(int idFamiliaOrigen, int idFamiliaBuscada)
        {
            GrupoPermiso_750VR origen = ObtenerFamiliaPorId(idFamiliaOrigen);
            return BuscarFamiliaRecursivamente(origen, idFamiliaBuscada);
        }

        private bool BuscarFamiliaRecursivamente(GrupoPermiso_750VR grupo, int idBuscado)
        {
            foreach (var hijo in grupo.Hijos)
            {
                if (hijo is GrupoPermiso_750VR subFamilia)
                {
                    if (subFamilia.Codigo_750VR == idBuscado)
                        return true;

                    if (BuscarFamiliaRecursivamente(subFamilia, idBuscado))
                        return true;
                }
            }
            return false;
        }

        public void ModificarNombreFamilia(int codFamilia, string nuevoNombre)
        {
            dal.ModificarNombreFamilia(codFamilia, nuevoNombre);
        }

        public bool ExisteFamiliaConNombre(string nombre, int idExcluir = 0)
        {
            return dal.ExisteFamiliaConNombre(nombre, idExcluir);
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
            return dal.ObtenerFamiliaPorIdRecursiva(idFamilia, new HashSet<int>());
        }

        public List<PermisoSimple_750VR> ObtenerPermisosSimples()
        {
           return  dal.ObtenerPermisosSimples();
        }

        public List<GrupoPermiso_750VR> ObtenerFamilias()
        {
           return dal.ObtenerFamilias();
        }
        public GrupoPermiso_750VR ObtenerFamiliaCompletaPorId(int idFamilia)
        {
            GrupoPermiso_750VR familia = dal.ObtenerFamiliaPorId(idFamilia);
            if (familia == null) return null;

            // Permisos simples de la familia
            var permisosSimples = dal.ObtenerPermisosSimplesPorFamilia(idFamilia);
            foreach (var permiso in permisosSimples)
                familia.Agregar(permiso);

            // Subfamilias
            var subfamilias = dal.ObtenerFamiliasHijas(idFamilia);
            foreach (var sub in subfamilias)
            {
                var subCompleta = ObtenerFamiliaCompletaPorId(sub.Codigo_750VR);
                if (subCompleta != null)
                    familia.Agregar(subCompleta);
            }

            return familia;
        }

        public void AsignarFamiliaAlPerfil(int idPerfil, int idFamilia)
        {
            dal.AsignarFamiliaAlPerfil(idPerfil, idFamilia);
        }


        public bool FamiliaYaAsignada(int idPerfil, int idFamilia)
        {
            return dal.FamiliaYaAsignada(idPerfil, idFamilia);
        }


        // ---- BLL ----
        public List<IComponentePermiso_750VR> ObtenerPermisosDePerfil(int idPerfil)
        {
            var permisos = new List<IComponentePermiso_750VR>();

            // 1. Obtener permisos simples directos
            var simples = dal.ObtenerPermisosSimplesDePerfil(idPerfil);
            permisos.AddRange(simples);

            // 2. Obtener familias del perfil
            var familias = dal.ObtenerFamiliasDePerfil(idPerfil);

            foreach (var familia in familias)
            {
                // 🔁 Obtener hijos de cada familia
                var hijos = ObtenerPermisosDeFamilia(familia.Codigo_750VR);
                foreach (var hijo in hijos)
                {
                    familia.Agregar(hijo);  // Importante: llenar recursivamente
                }

                permisos.Add(familia);  // Cargar la familia completa
            }

            return permisos;
        }



        public List<IComponentePermiso_750VR> ObtenerPermisosDePerfilPorNombre(string nombrePerfil)
        {
            int codPerfil = dal.ObtenerCodPerfilPorNombre(nombrePerfil);
            return ObtenerPermisosDePerfil(codPerfil);
        }
        public void EliminarFamiliaDePerfil(int idPerfil, int idFamilia)
        {
            dal.EliminarFamiliaDePerfil(idPerfil, idFamilia);
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

        public void ObtenerPermisosRecursivos(IComponentePermiso_750VR componente, List<IComponentePermiso_750VR> lista)
        {
            lista.Add(componente);
            foreach (var hijo in componente.ObtenerHijos())
            {
                ObtenerPermisosRecursivos(hijo, lista);
            }
        }


        public bool FamiliaAsignadaAAlgunPerfil(int codFamilia)
        {
            return dal.FamiliaAsignadaAAlgunPerfil(codFamilia); // consulta SQL
        }


    }
}
