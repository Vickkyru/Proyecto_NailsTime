using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE_VR750;
using BLL_VR750;
using SERVICIOS_VR750;

namespace Proyecto_NailsTime
{
    public partial class FormCrearPerfiles : Form, Iobserver_750VR
    {
        public FormCrearPerfiles()
        {
            InitializeComponent();
        }
        private BEperfil_750VR perfilSeleccionado;
        private GrupoPermiso_750VR perfilCompuesto;
        private IComponentePermiso_750VR componenteSeleccionado;



        private BLLperfil_750VR bllPerfil = new BLLperfil_750VR();
        private void FormCrearPerfiles_750VR_Load(object sender, EventArgs e)
        {
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
           

            CargarPerfilesEnComboBox();
            CargarPermisosSimples();
            CargarFamilias();

            MostrarTreeViewInicialPerfil();
            MostrarTreeViewInicialFamilia();
        }


        private void CargarFamilias()
        {
            var familias1 = bllPerfil.ObtenerFamilias();
            var familias2 = bllPerfil.ObtenerFamilias();
            var familias3 = bllPerfil.ObtenerFamilias();

            cmbfam.DataSource = familias1;
            cmbfam.DisplayMember = "Nombre_750VR";
            cmbfam.ValueMember = "Codigo_750VR";
            cmbfam.SelectedIndex = -1;

            cmbfamperf.DataSource = familias2;
            cmbfamperf.DisplayMember = "Nombre_750VR";
            cmbfamperf.ValueMember = "Codigo_750VR";
            cmbfamperf.SelectedIndex = -1;

            cmbfamfam.DataSource = familias3;
            cmbfamfam.DisplayMember = "Nombre_750VR";
            cmbfamfam.ValueMember = "Codigo_750VR";
            cmbfamfam.SelectedIndex = -1;
        }



        private void CargarPerfilesEnComboBox()
        {
            cmbperf.DataSource = bllPerfil.ObtenerPerfiles();
            cmbperf.DisplayMember = "NombrePerfil_750VR";
            cmbperf.ValueMember = "CodPerfil_750VR";
            cmbperf.SelectedIndex = -1;
        }
        private void CargarPermisosSimples()
        {
            cmbpermperf.DataSource = bllPerfil.ObtenerPermisosSimples();
            cmbpermperf.DisplayMember = "Nombre_750VR";
            cmbpermperf.ValueMember = "Codigo_750VR";
            cmbpermperf.SelectedIndex = -1;

            cmbpermfam.DataSource = bllPerfil.ObtenerPermisosSimples();
            cmbpermfam.DisplayMember = "Nombre_750VR";
            cmbpermfam.ValueMember = "Codigo_750VR";
            cmbpermfam.SelectedIndex = -1;
        }
        private void MostrarTreeViewInicialPerfil()
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(new TreeNode("Seleccione un perfil"));
        }

        private void MostrarTreeViewInicialFamilia()
        {
            treeView2.Nodes.Clear();
            treeView2.Nodes.Add(new TreeNode("Seleccione una familia"));
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
            this.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Titulo");
        }
        private void MostrarTreeViewInicial()
        {
            treeView1.Nodes.Clear();
            TreeNode nodoRaiz = new TreeNode(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfil"));

            treeView1.Nodes.Add(nodoRaiz);
        }
        private void MostrarTreeViewInicial2()
        {
            treeView2.Nodes.Clear();
            TreeNode nodoRaiz = new TreeNode(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamilia"));
            treeView2.Nodes.Add(nodoRaiz);
        }

        private void RefrescarPantalla()
        {
           
            CargarComboBoxPerfiles();
            
            CargarComboBoxFamilias();
           
            CargarComboPermisos();
           
        }
        private void CargarComboBoxPerfiles()
        {
            cmbperf.SelectedIndexChanged -= cmbperf_SelectedIndexChanged; 

            var perfiles = bllPerfil.ObtenerPerfiles();
            cmbperf.DataSource = null;
            cmbperf.DataSource = perfiles;
            cmbperf.DisplayMember = "NombrePerfil_750VR";
            cmbperf.ValueMember = "CodPerfil_750VR";
            cmbperf.SelectedIndex = -1;

            cmbperf.SelectedIndexChanged += cmbperf_SelectedIndexChanged; 
        }

        private void CargarTreeViewPerfiles()
        {
            treeView1.Nodes.Clear();
            var perfiles = bllPerfil.ObtenerPerfiles();

            foreach (var perfil in perfiles)
            {
                TreeNode nodoPerfil = new TreeNode(perfil.NombrePerfil_750VR);
                nodoPerfil.Tag = perfil;
                nodoPerfil.ForeColor = Color.DarkBlue;
                nodoPerfil.NodeFont = new Font("Segoe UI", 9, FontStyle.Bold);

                treeView1.Nodes.Add(nodoPerfil);
            }

            treeView1.ExpandAll();
        }
        private TreeNode CrearNodoPermisoRecursivo(IComponentePermiso_750VR permiso)
        {
            TreeNode nodo = new TreeNode(permiso.Nombre_750VR) { Tag = permiso };

            if (permiso is GrupoPermiso_750VR grupo)
            {
                foreach (var hijo in grupo.ObtenerHijos())
                {
                    nodo.Nodes.Add(CrearNodoPermisoRecursivo(hijo));
                }
            }

            return nodo;
        }

        private TreeNode CrearNodoPermiso(IComponentePermiso_750VR comp)
        {
            TreeNode nodo = new TreeNode(comp.Nombre_750VR)
            {
                Tag = comp    
            };

            if (comp is GrupoPermiso_750VR grupo)
            {
                foreach (var hijo in grupo.ObtenerHijos())
                    nodo.Nodes.Add(CrearNodoPermiso(hijo));
            }

            return nodo;
        }

        private void CargarComboBoxFamilias()
        {
            var familias = bllPerfil.ObtenerFamilias();

            cmbfamperf.DataSource = familias;
            cmbfamperf.DisplayMember = "Nombre_750VR";
            cmbfamperf.ValueMember = "Codigo_750VR";
            cmbfamperf.SelectedIndex = -1;

        }
  

        private void AgregarNodoPermiso(TreeNode nodoPadre, IComponentePermiso_750VR permiso)
        {
            TreeNode nodo = new TreeNode(permiso.Nombre_750VR)
            {
                Tag = permiso
            };
            nodoPadre.Nodes.Add(nodo);

            foreach (var hijo in permiso.ObtenerHijos())
            {
                AgregarNodoPermiso(nodo, hijo);
            }
        }

        private void agperf_Click(object sender, EventArgs e)
        {
            string nombrePerfil = txtnomperf.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombrePerfil))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.IngreseNombrePerfil"));

                return;
            }
            var perfilesExistentes = bllPerfil.ObtenerPerfiles();
            bool yaExiste = perfilesExistentes.Any(p => p.NombrePerfil_750VR.Equals(nombrePerfil, StringComparison.OrdinalIgnoreCase));

            if (yaExiste)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PerfilYaExiste"));

                return;
            }

            var nuevoPerfil = new BEperfil_750VR
            {
                NombrePerfil_750VR = nombrePerfil
            };

            bllPerfil.AgregarPerfil(nuevoPerfil);

            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PerfilAgregadoOk"));


            CargarPerfilesEnComboBox();
            MostrarTreeViewInicial();
            
        }

        private void btnelimperf_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Parent != null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfilEliminar"));

                return;
            }

            var perfilSeleccionado = treeView1.SelectedNode.Tag as BEperfil_750VR;
            if (perfilSeleccionado == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ErrorObtenerPerfil"));

                return;
            }

            if (bllPerfil.PerfilTieneUsuariosAsociados(perfilSeleccionado.NombrePerfil_750VR))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PerfilConUsuarios"));

                return;
            }

            var confirm = MessageBox.Show(
                string.Format(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ConfirmarEliminarPerfil"), perfilSeleccionado.NombrePerfil_750VR),
                Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Confirmar"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);


            if (confirm == DialogResult.Yes)
            {
                bllPerfil.EliminarPerfil(perfilSeleccionado.CodPerfil_750VR);

                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PerfilEliminadoOk"));

                CargarPerfilesEnComboBox();
                MostrarTreeViewInicial();
                
            }
        }

        private void CargarComboPermisos()
        {
            var listaPermisos = bllPerfil.ObtenerPermisosSimples();
            cmbpermperf.DataSource = listaPermisos;
            cmbpermperf.DisplayMember = "Nombre_750VR";
            cmbpermperf.ValueMember = "Codigo_750VR";
            cmbpermperf.SelectedIndex = -1;
        }
   
        private void btnagpermperf_Click(object sender, EventArgs e)
        {
            
            if (treeView1.SelectedNode == null || !(treeView1.SelectedNode.Tag is BEperfil_750VR perfil))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfilPermiso"));
                return;
            }

           
            if (!(cmbpermperf.SelectedItem is PermisoSimple_750VR permiso))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePermisoSimple"));

                return;
            }

           
            if (permiso is GrupoPermiso_750VR)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NoAsignarFamiliaDesdeAqui"));

                return;
            }

         
            bool yaExiste = PermisoYaExisteEnLista(perfil.Permisos_750VR, permiso.Codigo_750VR);
            if (yaExiste)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoYaEnMemoria"));
                return;
            }

         
            var permisosActuales = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
            bool yaAsignado = permisosActuales
                .OfType<PermisoSimple_750VR>()
                .Any(p => p.Codigo_750VR == permiso.Codigo_750VR);

            if (yaAsignado)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoYaAsignadoBD"));
                RefrescarTreeViewPerfil();
                return;
            }

            
            bllPerfil.AsignarPermiso(perfil.CodPerfil_750VR, permiso.Codigo_750VR);
            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoAsignadoOk"));

            
            perfil.Permisos_750VR = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
            RefrescarTreeViewPerfil();
           
        }

        private bool PermisoYaExisteEnLista(List<IComponentePermiso_750VR> lista, int codPermiso)
        {
            foreach (var comp in lista)
            {
                if (comp is PermisoSimple_750VR simple && simple.Codigo_750VR == codPermiso)
                    return true;

                if (comp is GrupoPermiso_750VR grupo && PermisoYaExisteEnLista(grupo.ObtenerHijos(), codPermiso))
                    return true;
            }

            return false;
        }


        private void btnelimpermperf_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePermisoSimplePerfil"));

                return;
            }

            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            
            if (nodoSeleccionado.Parent == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NoSeleccionarPerfil"));
                return;
            }

        
            TreeNode nodoPerfil = nodoSeleccionado.Parent;
            var perfil = nodoPerfil.Tag as BEperfil_750VR;

            if (perfil == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NoEliminarDesdeFamilia"));

                return;
            }

            
            var permiso = nodoSeleccionado.Tag as PermisoSimple_750VR;
            if (permiso == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SoloPermisosSimplesEliminarPerfil"));

                return;
            }

       
            var confirm = MessageBox.Show(string.Format(
    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ConfirmarEliminarPermisoPerfil"),
    permiso.Nombre_750VR,
    perfil.NombrePerfil_750VR),
    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Confirmar"),
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                bllPerfil.QuitarPermiso(perfil.CodPerfil_750VR, permiso.Codigo_750VR);
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoEliminado"));
                perfilSeleccionado = perfil; 
                perfil.Permisos_750VR = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
                RefrescarTreeViewPerfil();
               
            }
        }

        private void btnagfamperf_Click(object sender, EventArgs e)
        {
           
            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            if (nodoSeleccionado == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneNodoArbol"));
                return;
            }

           
            if (nodoSeleccionado.Tag is PermisoSimple_750VR)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NoAsignarAFamiliaAPermiso"));
                return;
            }

            
            if (nodoSeleccionado.Tag is GrupoPermiso_750VR)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NoAsignarAFamiliaDesdePerfil"));

                return;
            }

           
            if (!(nodoSeleccionado.Tag is BEperfil_750VR perfil))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfilValido"));

                return;
            }

           
            if (!(cmbfamperf.SelectedItem is GrupoPermiso_750VR familia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaAsignar"));

                return;
            }

            perfilSeleccionado = perfil;

            int idFamilia = familia.Codigo_750VR;

            var familiaCompleta = bllPerfil.ObtenerFamiliaCompletaPorId(idFamilia);
            if (familiaCompleta == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaNoEncontrada"));

                return;
            }

            if (bllPerfil.FamiliaYaAsignada(perfil.CodPerfil_750VR, idFamilia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaYaAsignada"));

                return;
            }

          
            var permisosActuales = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);

            var permisosDeLaFamilia = familiaCompleta.ObtenerTodosLosPermisos();

           
            bool yaExisten = permisosDeLaFamilia
                .Any(pNuevo => permisosActuales.Any(pExistente => pExistente.Codigo_750VR == pNuevo.Codigo_750VR));

            if (yaExisten)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaContienePermisosYaAsignados"));
                return;
            }


           
            perfil.Agregar(familiaCompleta);
            bllPerfil.AsignarFamiliaAlPerfil(perfil.CodPerfil_750VR, idFamilia);
            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaAgregadaCorrectamente"));

            perfil.Permisos_750VR = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
            RefrescarTreeViewPerfil();
           


        }


        private void RefrescarTreeViewPerfil()
        {
            treeView1.Nodes.Clear();

            TreeNode nodoRaiz = new TreeNode(perfilSeleccionado.NombrePerfil_750VR)
            {
                Tag = perfilSeleccionado
            };

            foreach (var comp in perfilSeleccionado.Permisos_750VR)
            {
                TreeNode nodoHijo = CrearNodoDesdeComponente(comp);
                nodoRaiz.Nodes.Add(nodoHijo);
            }

            treeView1.Nodes.Add(nodoRaiz);
            treeView1.ExpandAll();
        }

        private TreeNode CrearNodoDesdeComponente(IComponentePermiso_750VR comp)
        {
            TreeNode nodo = new TreeNode(comp.Nombre_750VR)
            {
                Tag = comp
            };

            if (comp is GrupoPermiso_750VR grupo)
            {
                foreach (var hijo in grupo.ObtenerHijos())
                {
                    nodo.Nodes.Add(CrearNodoDesdeComponente(hijo));
                }
            }

            return nodo;
        }

        private void btnagfam_Click(object sender, EventArgs e)
        {
            string nombreFamilia = txtfam.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombreFamilia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.IngreseNombreFamilia"));
                return;
            }

         
            var familiasExistentes = bllPerfil.ObtenerFamilias(); 
            bool yaExiste = familiasExistentes.Any(f =>
                f.Nombre_750VR.Trim().Equals(nombreFamilia, StringComparison.OrdinalIgnoreCase));

            if (yaExiste)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaYaExiste"));
                return;
            }

            bllPerfil.AgregarFamilia(nombreFamilia);
            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaAgregadaNueva"));

            RefrescarPantalla();
            CargarFamilias();
            MostrarTreeViewInicialFamilia();
            txtfam.Clear();
        }
        

        private void btnelimfam_Click(object sender, EventArgs e)
        {
            if (treeView2.SelectedNode == null || !(treeView2.SelectedNode.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaEliminar"));
                return;
            }

            
            bool estaAsignada = bllPerfil.FamiliaAsignadaAAlgunPerfil(familia.Codigo_750VR);
            if (estaAsignada)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaAsignadaNoEliminar"));
                return;
            }


            DialogResult result = MessageBox.Show(
                string.Format(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ConfirmarEliminarFamilia"), familia.Nombre_750VR),
                Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Confirmar"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                bllPerfil.EliminarFamilia(familia.Codigo_750VR);
                //CargarTreeViewFamilias();
                RefrescarPantalla();
            }
        }
        private void MostrarPermisosDeFamilia(GrupoPermiso_750VR familia)
        {
            treeView2.Nodes.Clear();

            TreeNode nodoFamilia = new TreeNode(familia.Nombre_750VR)
            {
                Tag = familia
            };

            foreach (var hijo in familia.ObtenerHijos())
            {
                TreeNode nodoHijo = CrearNodoPermiso(hijo);
                nodoFamilia.Nodes.Add(nodoHijo);
            }

            treeView2.Nodes.Add(nodoFamilia);
            treeView2.ExpandAll();
        }


      
 
        private void agpermfam_Click(object sender, EventArgs e)
        {  
            TreeNode nodoSeleccionado = treeView2.SelectedNode;
            if (nodoSeleccionado == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaAsignarPermiso"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }


            if (!(nodoSeleccionado.Tag is GrupoPermiso_750VR familiaSeleccionada))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.DebeSeleccionarNodoFamilia"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!(cmbpermfam.SelectedItem is PermisoSimple_750VR permisoSeleccionado))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePermiso"));
                return;
            }


            if (ContienePermiso(familiaSeleccionada, permisoSeleccionado.Codigo_750VR))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoYaPresenteEnFamiliaOSubfamilia"));
                return;
            }

          
            bool resultado = bllPerfil.AgregarPermisoAFamilia(familiaSeleccionada.Codigo_750VR, permisoSeleccionado.Codigo_750VR);
            if (resultado)
            {

                //CargarTreeViewFamilias();      
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoAgregadoCorrectamente"));

            }
            else
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoYaAsignadoAFamilia"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Aviso"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }
        private bool ContienePermiso(IComponentePermiso_750VR componente, int codPermiso)
        {
            if (componente is PermisoSimple_750VR permiso)
            {
                return permiso.Codigo_750VR == codPermiso;
            }
            else if (componente is GrupoPermiso_750VR grupo)
            {
                foreach (var hijo in grupo.Hijos)
                {
                    if (ContienePermiso(hijo, codPermiso))
                        return true;
                }
            }

            return false;
        }

        private void elimpermfam_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView2.SelectedNode;
            if (nodoSeleccionado == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePermiso"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            TreeNode nodoPadre = nodoSeleccionado.Parent;
            if (nodoPadre == null || !(nodoPadre.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoDebeEstarDirectamenteEnFamilia"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!(nodoSeleccionado.Tag is PermisoSimple_750VR permiso))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SoloPermisosSimplesEliminar"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
    string.Format(
        Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ConfirmarEliminarPermisoDeFamilia"),
        permiso.Nombre_750VR,
        familia.Nombre_750VR),
    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Confirmar"),
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            bool eliminado = bllPerfil.QuitarPermisoDeFamilia(familia.Codigo_750VR, permiso.Codigo_750VR);

            if (eliminado)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoEliminadoCorrectamente"));
                RefrescarYMostrarFamilia(familia.Codigo_750VR); // Solo refresca esa familia
            }
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoNoSePudoEliminar"));
            }
        }

        private void btnagfamfam_Click(object sender, EventArgs e)
        {
           
            TreeNode nodoSeleccionado = treeView2.SelectedNode;

            if (nodoSeleccionado == null || !(nodoSeleccionado.Tag is GrupoPermiso_750VR familiaPadre))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaPadre"));
                return;
                
            }

          
            if (!(cmbfamfam.SelectedItem is GrupoPermiso_750VR familiaHija))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaHija"));
                return;
            }

          
            if (familiaPadre.Codigo_750VR == familiaHija.Codigo_750VR)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NoAgregarMismaFamilia"));
                return;
            }

            if (bllPerfil.FamiliaContieneAFamilia(familiaHija.Codigo_750VR, familiaPadre.Codigo_750VR))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.RelacionCircularFamilias"));
                return;
            }

      
            var permisosPadre = familiaPadre.ObtenerTodosLosPermisos();          
                                                                                  
            var hijaCompleta = bllPerfil.ObtenerFamiliaCompletaPorId(familiaHija.Codigo_750VR);
            var permisosDeHija = hijaCompleta.ObtenerTodosLosPermisos();

            bool hayDuplicados = permisosDeHija
                .Any(pHija => permisosPadre.Any(pPadre => pPadre.Codigo_750VR == pHija.Codigo_750VR));

            if (hayDuplicados)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisosDuplicadosEntreFamilias"));
                return;
            }

            
            bool agregado = bllPerfil.AsignarFamiliaAFamilia(familiaPadre.Codigo_750VR, familiaHija.Codigo_750VR);

            if (agregado)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaAgregadaCorrectamente"));
                var familiaActualizada = bllPerfil.ObtenerFamiliaPorId(familiaPadre.Codigo_750VR);
                MostrarPermisosDeFamilia(familiaActualizada);   
            }
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaYaEstabaAsignada"));
            }
        }

        private void btnelimfamfam_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView2.SelectedNode;

            if (nodoSeleccionado == null || !(nodoSeleccionado.Tag is GrupoPermiso_750VR familiaHija))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaHijaEliminar"));
                return;
            }

            if (nodoSeleccionado.Parent == null || !(nodoSeleccionado.Parent.Tag is GrupoPermiso_750VR familiaPadre))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaSinPadreValido"));

                return;
            }

            string mensaje = string.Format(
        Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ConfirmarQuitarFamilia"),
        familiaHija.Nombre_750VR,
        familiaPadre.Nombre_750VR
    );

            DialogResult confirmacion = MessageBox.Show(mensaje,
                Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Confirmar"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion == DialogResult.Yes)
            {
                bool eliminada = bllPerfil.QuitarFamiliaDeFamilia(familiaPadre.Codigo_750VR, familiaHija.Codigo_750VR);

                if (eliminada)
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaEliminadaOk"));


                    RefrescarYMostrarFamilia(familiaPadre.Codigo_750VR);
                }
                else
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NoSePudoEliminarFamilia"));

                }
            }
        }

        private void cmbperf_SelectedIndexChanged(object sender, EventArgs e)
        {
        

            if (cmbperf.SelectedItem is BEperfil_750VR perfil)
            {
                perfilSeleccionado = perfil;

                perfilSeleccionado.Permisos_750VR = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);

                RefrescarTreeViewPerfil();
            }

            var perfilSeleccionado1 = cmbperf.SelectedItem as BEperfil_750VR;
            if (perfilSeleccionado1 != null)
            {
                txtnomperf.Text = perfilSeleccionado1.NombrePerfil_750VR;
                RefrescarTreeViewPerfil();
            }

        }



        

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtnomperf.Clear();
            cmbperf.SelectedIndex = -1;
            cmbpermperf.SelectedIndex = -1;
            cmbfamperf.SelectedIndex = -1;
            MostrarTreeViewInicial(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtfam.Clear();
            cmbfam.SelectedIndex = -1;
            cmbfamfam.SelectedIndex = -1;
            cmbfamperf.SelectedIndex = -1;
            MostrarTreeViewInicial2(); 
        }


        private void RefrescarYMostrarFamilia(int codFamilia)
        {
           
            var familiaActualizada = bllPerfil.ObtenerFamilias().FirstOrDefault(f => f.Codigo_750VR == codFamilia);

            if (familiaActualizada != null)
            {
                var hijos = bllPerfil.ObtenerPermisosDeFamilia(codFamilia);
                foreach (var hijo in hijos)
                {
                    familiaActualizada.Agregar(hijo); 
                }

                MostrarPermisosDeFamilia(familiaActualizada);
            }
        }


        private void cmbfam_SelectedIndexChanged(object sender, EventArgs e)
        {
            var familiaSeleccionada = cmbfam.SelectedItem as GrupoPermiso_750VR;
            if (familiaSeleccionada != null)
            {
                txtfam.Text = familiaSeleccionada.Nombre_750VR;
                treeView2.Nodes.Clear();

                var familiaCompleta = bllPerfil.ObtenerFamiliaPorId(familiaSeleccionada.Codigo_750VR);
                if (familiaCompleta == null) return;

                TreeNode nodoRaiz = new TreeNode(familiaCompleta.Nombre_750VR) { Tag = familiaCompleta };
                AgregarHijosAlTreeView(familiaCompleta, nodoRaiz);

                treeView2.Nodes.Add(nodoRaiz);
                treeView2.ExpandAll();
            }
            if (familiaSeleccionada == null) return;

        

           
         
        }

        private void AgregarHijosAlTreeView(GrupoPermiso_750VR familia, TreeNode padre)
        {
            foreach (var hijo in familia.Hijos)
            {
                TreeNode n = new TreeNode(hijo.Nombre_750VR) { Tag = hijo };  // ⬅️
                if (hijo is GrupoPermiso_750VR sub)
                    AgregarHijosAlTreeView(sub, n);

                padre.Nodes.Add(n);
            }
        }



        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
 

            if (e.Node?.Tag is IComponentePermiso_750VR comp)
            {
                componenteSeleccionado = comp;
            }
            else
            {
                componenteSeleccionado = null;
            }
        }

        private void txtnomperf_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (perfilSeleccionado == null || treeView1.SelectedNode == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfilYFamiliaQuitar"));

                return;
            }

            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            if (nodoSeleccionado.Tag is GrupoPermiso_750VR familia)
            {
                bllPerfil.EliminarFamiliaDePerfil(perfilSeleccionado.CodPerfil_750VR, familia.Codigo_750VR);

           
                perfilSeleccionado.Hijos.RemoveAll(h => h.Codigo_750VR == familia.Codigo_750VR);
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaEliminadaCorrectamente"));


                perfilSeleccionado.Permisos_750VR = bllPerfil.ObtenerPermisosDePerfil(perfilSeleccionado.CodPerfil_750VR);
                RefrescarTreeViewPerfil();
                
            }
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SoloEliminarFamiliasNoPermisos"));
            }
        }

        private void cmbpermperf_SelectedIndexChanged(object sender, EventArgs e)
        {
        
           
        }

        private void btnmodperf_Click(object sender, EventArgs e)
        {
            var perfil = cmbperf.SelectedItem as BEperfil_750VR;
            if (perfil == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfilModificar"));
                return;
            }

            string nuevoNombre = txtnomperf.Text.Trim();
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.IngreseNuevoNombrePerfil"));

                return;
            }

   
            var todos = bllPerfil.ObtenerPerfiles(); 
            if (todos.Any(p => p.NombrePerfil_750VR.Equals(nuevoNombre, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NombrePerfilYaExiste"));

                return;
            }

         
            bllPerfil.ModificarNombrePerfil(perfil.CodPerfil_750VR, nuevoNombre);

            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PerfilModificadoOK"));


            CargarPerfilesEnComboBox();
            CargarTreeViewPerfiles();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            if (treeView2.SelectedNode == null || !(treeView2.SelectedNode.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaModificar"));
                return;
            }

            string nuevoNombre = txtfam.Text.Trim();
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.IngreseNombreFamilia"));
                return;
            }

            if (bllPerfil.ExisteFamiliaConNombre(nuevoNombre, familia.Codigo_750VR))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaYaExiste"));
                return;
            }

            bllPerfil.ModificarNombreFamilia(familia.Codigo_750VR, nuevoNombre);
            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaModificadaOK"));

            txtfam.Clear();
            //CargarTreeViewFamilias();
            RefrescarPantalla();
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void cmbfamfam_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbfamperf_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
