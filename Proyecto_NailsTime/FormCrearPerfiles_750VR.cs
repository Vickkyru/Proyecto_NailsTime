using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private bool estaCargando = true;

        private BLLperfil_750VR bllPerfil = new BLLperfil_750VR();
        private void FormCrearPerfiles_750VR_Load(object sender, EventArgs e)
        {
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
            estaCargando = true;
            MostrarTreeViewInicial();
            CargarComboBoxPerfiles();
            CargarComboBoxFamilias();
            CargarComboPermisos();
            RefrescarPantallaFamilias();
            estaCargando = false;
            
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
            this.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Titulo");
            agperf.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.AgregarPerfil");
            btnelimperf.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.EliminarPerfil");
            btnagpermperf.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.AgregarPermiso");
            btnelimpermperf.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.QuitarPermiso");
            btnagfamperf.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.AgregarFamilia");
            //btnelimfamperf.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.QuitarFamilia");
            btnagfam.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.CrearFamilia");
            btnelimfam.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.EliminarFamilia");
            agpermfam.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.AgregarPermisoAFamilia");
            elimpermfam.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.QuitarPermisoDeFamilia");
            btnagfamfam.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.AgregarFamiliaAFamilia");
            btnelimfamfam.Text = Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.QuitarFamiliaDeFamilia");

            MostrarTreeViewInicial();
            MostrarTreeViewInicial2();
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
            TreeNode nodoRaiz = new TreeNode(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfil"));
            treeView2.Nodes.Add(nodoRaiz);
        }

        private void RefrescarPantallaFamilias()
        {
            // Limpiar y volver a cargar el ComboBox con familias creadas
            CargarComboBoxFamiliasCreadas(); // este método lo explico abajo si no lo tenés

            // Limpiar y mostrar solo el nodo raíz vacío en el árbol de familias
            treeView2.Nodes.Clear(); // asegurate de tener ese TreeView
            TreeNode nodoRaiz = new TreeNode(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamilia"));

            {
                Name = "Raiz";
            };
            treeView2.Nodes.Add(nodoRaiz);
            treeView2.ExpandAll();

            // Recargar combos de familia y permisos (lado derecho)
            CargarComboBoxFamilias2();
            CargarComboPermisos2();
        }
        private void CargarComboBoxFamiliasCreadas()
        {
            var familias = bllPerfil.ObtenerFamilias();
            cmbfam.DataSource = familias;
            cmbfam.DisplayMember = "Nombre_750VR";
            cmbfam.ValueMember = "Codigo_750VR";
            cmbfam.SelectedIndex = -1;
        }

        private void RefrescarPantalla()
        {
            // Refrescar ComboBox de perfiles
            CargarComboBoxPerfiles();

         
            // Refrescar combos de familias y permisos
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

            cmbperf.SelectedIndexChanged += cmbperf_SelectedIndexChanged; // ✅ Vuelve a vincular
        }



        private void CargarTreeViewPerfiles()
        {
            treeView1.Nodes.Clear(); // el TreeView blanco del lado izquierdo

            var perfiles = bllPerfil.ObtenerPerfiles();
            foreach (var perfil in perfiles)
            {
                TreeNode nodoPerfil = new TreeNode(perfil.NombrePerfil_750VR)
                {
                    Tag = perfil
                };

                var permisos = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
                foreach (var permiso in permisos)
                {
                    AgregarNodoPermiso(nodoPerfil, permiso);
                }

                treeView1.Nodes.Add(nodoPerfil);
            }

            treeView1.ExpandAll();
        }
        private void ActualizarPerfilEnTreeview(BEperfil_750VR perfilActualizado)
        {
            foreach (TreeNode nodo in treeView1.Nodes)
            {
                if (nodo.Tag is BEperfil_750VR perfil && perfil.CodPerfil_750VR == perfilActualizado.CodPerfil_750VR)
                {
                    nodo.Nodes.Clear();

                    var permisos = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
                    perfil.Permisos_750VR = permisos;

                    foreach (var permiso in permisos)
                    {
                        TreeNode nodoPermiso = CrearNodoPermiso(permiso);
                        nodo.Nodes.Add(nodoPermiso);
                    }

                    treeView1.ExpandAll();
                    break;
                }
            }
        }

        private TreeNode CrearNodoPermiso(IComponentePermiso_750VR permiso)
        {
            TreeNode nodo = new TreeNode(permiso.Nombre_750VR);
            nodo.Tag = permiso;

            foreach (var hijo in permiso.ObtenerHijos())
            {
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
        private void CargarComboBoxFamilias2()
        {
            var familias = bllPerfil.ObtenerFamilias();

            cmbfamfam.DataSource = familias;
            cmbfamfam.DisplayMember = "Nombre_750VR";
            cmbfamfam.ValueMember = "Codigo_750VR";
            cmbfamfam.SelectedIndex = -1;

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
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ErrorNombrePerfil"));

                return;
            }

            var nuevoPerfil = new BEperfil_750VR { NombrePerfil_750VR = nombrePerfil };

            try
            {
                bllPerfil.AgregarPerfil(nuevoPerfil);
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PerfilAgregado"));

                txtnomperf.Clear();
                ActualizarPerfilEnTreeview(nuevoPerfil);

                RefrescarPantalla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ErrorAgregarPerfil") + ex.Message);

            }
        }

        private void btnelimperf_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfilEliminar"));

                return;
            }

            if (treeView1.SelectedNode.Tag is BEperfil_750VR perfilSeleccionado)
            {
                var confirm = MessageBox.Show(
      Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ConfirmarEliminarPerfil"),
      Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Confirmar"),
      MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        bllPerfil.EliminarPerfil(perfilSeleccionado.CodPerfil_750VR);
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PerfilEliminado"));

                        CargarTreeViewPerfiles();
                        RefrescarPantalla();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.ErrorEliminarPerfil") + ex.Message);

                    }
                }
            }
            else
            {
              MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneNodoPerfil"));

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
        private void CargarComboPermisos2()
        {
            var listaPermisos = bllPerfil.ObtenerPermisosSimples();
            cmbpermfam.DataSource = listaPermisos;
            cmbpermfam.DisplayMember = "Nombre_750VR";
            cmbpermfam.ValueMember = "Codigo_750VR";
            cmbpermfam.SelectedIndex = -1;
        }

        private void btnagpermperf_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            if (nodoSeleccionado == null || nodoSeleccionado.Parent != null)
            {
                MessageBox.Show("Debes seleccionar el PERFIL (nodo raíz) para agregar un permiso.");
                return;
            }

            if (cmbpermperf.SelectedItem == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePermiso"));

                return;
            }

            var perfil = treeView1.SelectedNode.Tag as BEperfil_750VR;
            var permiso = cmbpermperf.SelectedItem as PermisoSimple_750VR;

            bool resultado = bllPerfil.AsignarPermiso(perfil.CodPerfil_750VR, permiso.Codigo_750VR);

            if (resultado)
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoAgregado"));

            else
                MessageBox.Show(
    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoYaAsignado"),
    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Aviso"),
    MessageBoxButtons.OK,
    MessageBoxIcon.Information);


            ActualizarPerfilEnTreeview(perfil);

            RefrescarPantalla();
        }

        private void btnelimpermperf_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            // Validar selección
            if (nodoSeleccionado == null || nodoSeleccionado.Parent == null)
            {
                MessageBox.Show(
            Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionPermisoQuitar"),
            Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
                return;
            }

            TreeNode nodoPadre = nodoSeleccionado.Parent;

            // Validar que el padre sea un perfil (es decir, que el nodo seleccionado esté a 1 nivel de profundidad)
            if (!(nodoPadre.Tag is BEperfil_750VR perfil))
            {
                MessageBox.Show(
            Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionInvalidaPermiso"),
            Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
                return;
            }

            // Validar que el nodo tenga un permiso asociado
            if (!(nodoSeleccionado.Tag is IComponentePermiso_750VR permiso))
            {
                MessageBox.Show(
            Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NodoInvalido"),
            Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Error"),
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
                return;
            }

            // Realizar la eliminación
            bllPerfil.QuitarPermiso(perfil.CodPerfil_750VR, permiso.Codigo_750VR);
            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.PermisoQuitadoCorrectamente"));

            // Refrescar árbol
            ActualizarPerfilEnTreeview(perfil);


            RefrescarPantalla();
        }

        private void btnagfamperf_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            if (nodoSeleccionado == null || nodoSeleccionado.Parent != null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfilNodoRaiz"));
                return;
            }

            if (!(nodoSeleccionado.Tag is BEperfil_750VR perfil))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfil"));
                return;
            }

            if (!(cmbfamperf.SelectedItem is GrupoPermiso_750VR familiaSeleccionada))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaValida"));
                return;
            }

            // ✅ Traer todos los permisos del perfil
            var permisosPerfil = new List<IComponentePermiso_750VR>();
            foreach (var permiso in bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR))
            {
                bllPerfil.ObtenerPermisosRecursivos(permiso, permisosPerfil);
            }

            // ✅ Traer todos los permisos de la familia que se quiere agregar
            var permisosFamilia = new List<IComponentePermiso_750VR>();
            bllPerfil.ObtenerPermisosRecursivos(familiaSeleccionada, permisosFamilia);

            // ✅ Comparar por código
            bool hayDuplicados = permisosFamilia.Any(pf =>
                permisosPerfil.Any(pp => pp.Codigo_750VR == pf.Codigo_750VR));

            if (hayDuplicados)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaYaAsignada"));
                return;
            }

            // Agregar familia
            bool resultado = bllPerfil.AsignarPermiso(perfil.CodPerfil_750VR, familiaSeleccionada.Codigo_750VR);

            if (resultado)
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaAsignadaOk"));
            else
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaYaEstabaAsignada"));

            CargarTreeViewPerfiles();
            ActualizarPerfilEnTreeview(perfil);
            RefrescarPantalla();
        }

        //private void btnelimfamperf_Click(object sender, EventArgs e)
        //{
        //    if (treeView1.SelectedNode == null || !(treeView1.SelectedNode.Tag is BEperfil_750VR perfil))
        //    {
        //        MessageBox.Show("Seleccioná un perfil del árbol para quitarle una familia.");
        //        return;
        //    }

        //    if (cmbfamperf.SelectedItem is GrupoPermiso_750VR familiaSeleccionada)
        //    {
        //        bllPerfil.QuitarFamilia(perfil.CodPerfil_750VR, familiaSeleccionada.Codigo_750VR);
        //        MessageBox.Show("Familia quitada correctamente.");

        //        CargarTreeViewPerfiles(); // Refrescar árbol
        //        RefrescarPantalla();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Seleccioná una familia válida.");
        //    }
        //}

        private void btnagfam_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtfam.Text))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.IngreseNombreFamilia"));
                return;
            }

            bllPerfil.AgregarFamilia(txtfam.Text.Trim());
            CargarTreeViewFamilias(); // Refresca el árbol
            RefrescarPantalla();
            txtfam.Clear();
        }

        private void btnelimfam_Click(object sender, EventArgs e)
        {
            if (treeView2.SelectedNode == null || !(treeView2.SelectedNode.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaEliminar"));
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
                CargarTreeViewFamilias();
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


        private void CargarTreeViewFamilias()
        {
            treeView2.Nodes.Clear();

            var familias = bllPerfil.ObtenerFamilias();

            foreach (var familia in familias)
            {
                TreeNode nodoRaiz = new TreeNode(familia.Nombre_750VR) { Tag = familia };

                foreach (var hijo in familia.ObtenerHijos())
                {
                    TreeNode nodoHijo = CrearNodoPermiso(hijo);
                    nodoRaiz.Nodes.Add(nodoHijo);
                }

                treeView2.Nodes.Add(nodoRaiz);
            }

            treeView2.ExpandAll();
        }

        //private void MostrarPermisosDeFamilia(GrupoPermiso_750VR familia)
        //{
        //    treeView2.Nodes.Clear();

        //    TreeNode nodoFamilia = new TreeNode(familia.Nombre_750VR)
        //    {
        //        Tag = familia
        //    };

        //    AgregarNodoPermiso(nodoFamilia, familia);

        //    treeView2.Nodes.Add(nodoFamilia);
        //    treeView2.ExpandAll();
        //}
        private void agpermfam_Click(object sender, EventArgs e)
        {
            //TreeNode nodoSeleccionado2 = treeView1.SelectedNode;

            //if (nodoSeleccionado2 == null || nodoSeleccionado2.Parent != null)
            //{
            //    MessageBox.Show("Debes seleccionar la FAMILIA raíz (nodo raíz) para agregar un permiso simple.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            var nodoSeleccionado = treeView2.SelectedNode;

            if (nodoSeleccionado == null || !(nodoSeleccionado.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaAsignarPermiso"));
                return;
            }

            if (cmbpermfam.SelectedItem is PermisoSimple_750VR permiso)
            {
                bool resultado = bllPerfil.AgregarPermisoAFamilia(familia.Codigo_750VR, permiso.Codigo_750VR);

                if (resultado)
                {
                    RefrescarYMostrarFamilia(familia.Codigo_750VR);
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
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePermiso"));
            }
        }

        private void elimpermfam_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            if (nodoSeleccionado == null || nodoSeleccionado.Parent == null)
            {
                MessageBox.Show(
          Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionarPermisoFamiliaEnRaiz"),
          Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
          MessageBoxButtons.OK,
          MessageBoxIcon.Warning);
                return;
            }

            // El padre debe ser la familia raíz
            TreeNode nodoPadre = nodoSeleccionado.Parent;
            if (nodoPadre.Parent != null)
            {
                MessageBox.Show(
           Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SoloQuitarAsignadosADirectoRaiz"),
           Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
           MessageBoxButtons.OK,
           MessageBoxIcon.Warning);
                return;
            }

            var nodoSeleccionado2 = treeView2.SelectedNode;

            if (nodoSeleccionado2 == null || !(nodoSeleccionado2.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaParaQuitarPermiso"));
                return;
            }

            if (cmbpermfam.SelectedItem is PermisoSimple_750VR permiso)
            {
                bllPerfil.QuitarPermisoDeFamilia(familia.Codigo_750VR, permiso.Codigo_750VR);
                RefrescarYMostrarFamilia(familia.Codigo_750VR);
            }
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePermiso"));
            }
        }

        private void btnagfamfam_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView2.SelectedNode;

            if (nodoSeleccionado == null || !(nodoSeleccionado.Tag is GrupoPermiso_750VR familiaPadre))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaAgregarOtra"));
                return;
            }

            if (cmbfamfam.SelectedItem is GrupoPermiso_750VR familiaHija)
            {
                if (familiaPadre.Codigo_750VR == familiaHija.Codigo_750VR)
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.NoAgregarMismaFamilia"));
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
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.FamiliaYaAsignada"));
                }

            }
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaAgregar"));
            }
        }

        private void btnelimfamfam_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView2.SelectedNode;

            if (nodoSeleccionado == null || !(nodoSeleccionado.Tag is GrupoPermiso_750VR familiaPadre))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaParaQuitarOtra"));
                return;
            }

            if (cmbfamfam.SelectedItem is GrupoPermiso_750VR familiaHija)
            {
                bllPerfil.QuitarFamiliaDeFamilia(familiaPadre.Codigo_750VR, familiaHija.Codigo_750VR);
                RefrescarYMostrarFamilia(familiaPadre.Codigo_750VR);
            }
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaAQuitar"));
            }
        }

        private void cmbperf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbperf.SelectedItem == null)
                return;

          
            var perfilSeleccionado = (BEperfil_750VR)cmbperf.SelectedItem;

           
            MostrarPermisosDelPerfil(perfilSeleccionado);
        }

        private void MostrarPermisosDelPerfil(BEperfil_750VR perfil)
        {
            treeView1.Nodes.Clear();

            TreeNode nodoPerfil = new TreeNode(perfil.NombrePerfil_750VR)
            {
                Tag = perfil
            };

            var permisos = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
            foreach (var permiso in permisos)
            {
                AgregarNodoPermiso(nodoPerfil, permiso);
            }

            treeView1.Nodes.Add(nodoPerfil);
            treeView1.ExpandAll();
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
                familiaActualizada.Hijos = hijos;

                MostrarPermisosDeFamilia(familiaActualizada);
            }
        }


        private void cmbfam_SelectedIndexChanged(object sender, EventArgs e)
        {
         
            if (estaCargando || cmbfam.SelectedIndex == -1)
                return;

           
            if (cmbfam.SelectedItem is GrupoPermiso_750VR familia)
            {
              
                var hijos = bllPerfil.ObtenerPermisosDeFamilia(familia.Codigo_750VR);
                familia.Hijos = hijos;

               
                MostrarPermisosDeFamilia(familia);
            }
        }
    }
}
