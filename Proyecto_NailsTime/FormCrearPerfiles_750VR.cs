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
        private BEperfil_750VR perfilSeleccionado;
        private GrupoPermiso_750VR perfilCompuesto; // Opcional si usás la propiedad interna


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
            var familias = bllPerfil.ObtenerFamilias();

            cmbfam.DataSource = familias;
            cmbfam.DisplayMember = "Nombre_750VR";
            cmbfam.ValueMember = "Codigo_750VR";
            cmbfam.SelectedIndex = -1;

            cmbfamperf.DataSource = familias;
            cmbfamperf.DisplayMember = "Nombre_750VR";
            cmbfamperf.ValueMember = "Codigo_750VR";
            cmbfamperf.SelectedIndex = -1;

            cmbfamfam.DataSource = familias;
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
            TreeNode nodoRaiz = new TreeNode(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccionePerfil"));
            treeView2.Nodes.Add(nodoRaiz);
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
            treeView1.Nodes.Clear();
            var perfiles = bllPerfil.ObtenerPerfiles();

            foreach (var perfil in perfiles)
            {
                TreeNode nodoPerfil = new TreeNode(perfil.NombrePerfil_750VR);
                nodoPerfil.Tag = perfil;
                nodoPerfil.ForeColor = Color.DarkBlue;
                nodoPerfil.NodeFont = new Font("Segoe UI", 9, FontStyle.Bold);

                // (Opcional) podrías cargarle hijos después
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

      

        private TreeNode CrearNodoPermiso(IComponentePermiso_750VR permiso)
        {
            TreeNode nodo = new TreeNode(permiso.Nombre_750VR)
            {
                Tag = permiso
            };

            if (permiso is GrupoPermiso_750VR grupo)
            {
                foreach (var hijo in grupo.ObtenerHijos())
                {
                    nodo.Nodes.Add(CrearNodoPermiso(hijo));
                }
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
                MessageBox.Show("Debe ingresar un nombre para el perfil.");
                return;
            }

            // Verificar si ya existe un perfil con ese nombre
            var perfilesExistentes = bllPerfil.ObtenerPerfiles();
            bool yaExiste = perfilesExistentes.Any(p => p.NombrePerfil_750VR.Equals(nombrePerfil, StringComparison.OrdinalIgnoreCase));

            if (yaExiste)
            {
                MessageBox.Show("Ya existe un perfil con ese nombre.");
                return;
            }

            // Crear e insertar
            var nuevoPerfil = new BEperfil_750VR
            {
                NombrePerfil_750VR = nombrePerfil
            };

            bllPerfil.AgregarPerfil(nuevoPerfil);

            MessageBox.Show("Perfil agregado correctamente.");

            CargarPerfilesEnComboBox();
            txtnomperf.Clear();
        }

        private void btnelimperf_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || treeView1.SelectedNode.Parent != null)
            {
                MessageBox.Show("Debe seleccionar un perfil (nodo raíz) para eliminar.");
                return;
            }

            var perfilSeleccionado = treeView1.SelectedNode.Tag as BEperfil_750VR;
            if (perfilSeleccionado == null)
            {
                MessageBox.Show("Error al obtener el perfil seleccionado.");
                return;
            }

            var confirm = MessageBox.Show($"¿Está seguro de eliminar el perfil '{perfilSeleccionado.NombrePerfil_750VR}'?",
                                          "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                bllPerfil.EliminarPerfil(perfilSeleccionado.CodPerfil_750VR);

                MessageBox.Show("Perfil eliminado correctamente.");
                CargarPerfilesEnComboBox();
                CargarTreeViewPerfiles();
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
            var perfil = cmbperf.SelectedItem as BEperfil_750VR;
            if (perfil == null)
            {
                MessageBox.Show("Debe seleccionar un perfil.");
                return;
            }

            var permiso = cmbpermperf.SelectedItem as PermisoSimple_750VR;
            if (permiso == null)
            {
                MessageBox.Show("Debe seleccionar un permiso.");
                return;
            }

            var permisosActuales = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
            bool yaAsignado = permisosActuales.OfType<PermisoSimple_750VR>()
                .Any(p => p.Codigo_750VR == permiso.Codigo_750VR);

            if (yaAsignado)
            {
                MessageBox.Show("Este permiso ya está asignado al perfil.");
                CargarTreeViewPerfiles();
                return;
            }
        }

        private void btnelimpermperf_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Seleccione un permiso simple dentro de un perfil.");
                return;
            }

            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            // Verificamos que no sea un nodo raíz (perfil)
            if (nodoSeleccionado.Parent == null)
            {
                MessageBox.Show("Debe seleccionar un permiso, no un perfil.");
                return;
            }

            // Nodo raíz (perfil)
            TreeNode nodoPerfil = nodoSeleccionado.Parent;
            var perfil = nodoPerfil.Tag as BEperfil_750VR;

            if (perfil == null)
            {
                MessageBox.Show("Error al obtener el perfil.");
                return;
            }

            // Verificar que el nodo seleccionado sea un permiso simple
            var permiso = nodoSeleccionado.Tag as PermisoSimple_750VR;
            if (permiso == null)
            {
                MessageBox.Show("Solo puede eliminar permisos simples desde este botón.");
                return;
            }

            // Confirmación
            var confirm = MessageBox.Show($"¿Desea quitar el permiso '{permiso.Nombre_750VR}' del perfil '{perfil.NombrePerfil_750VR}'?",
                                          "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                bllPerfil.QuitarPermiso(perfil.CodPerfil_750VR, permiso.Codigo_750VR);
                MessageBox.Show("Permiso eliminado.");
                CargarTreeViewPerfiles();
            }
        }

        private void btnagfamperf_Click(object sender, EventArgs e)
        {
            if (cmbfam.SelectedItem == null || perfilSeleccionado == null)
            {
                MessageBox.Show("Seleccione una familia y un perfil.");
                return;
            }

            int idFamilia = (int)cmbfam.SelectedValue;

            // 1. Obtener familia completa
            GrupoPermiso_750VR familia = bllPerfil.ObtenerFamiliaCompletaPorId(idFamilia);
            if (familia == null)
            {
                MessageBox.Show("No se encontró la familia.");
                return;
            }

            // 2. Verificar si ya fue agregada al perfil
            if (perfilSeleccionado.Hijos.Any(h => h.Codigo_750VR == familia.Codigo_750VR))
            {
                MessageBox.Show("Esa familia ya fue asignada al perfil.");
                return;
            }

            // 3. Agregar familia al perfil
            perfilSeleccionado.Agregar(familia);

            // 👉 4. Refrescar el TreeView
            RefrescarTreeViewPerfil();

        }
 
        private void RefrescarTreeViewPerfil()
        {
            if (perfilSeleccionado == null) return;

            treeView1.Nodes.Clear();

            TreeNode root = new TreeNode(perfilSeleccionado.NombrePerfil_750VR) { Tag = perfilSeleccionado };

            foreach (var comp in perfilSeleccionado.PermisosCompuestos_750VR.ObtenerHijos())
                root.Nodes.Add(ConstruirNodoDesdeComponente(comp));

            treeView1.Nodes.Add(root);
            treeView1.ExpandAll();
        }
        private TreeNode ConstruirNodoDesdeComponente(IComponentePermiso_750VR comp)
        {
            TreeNode nodo = new TreeNode(comp.Nombre_750VR) { Tag = comp };

            foreach (var hijo in comp.ObtenerHijos())
                nodo.Nodes.Add(ConstruirNodoDesdeComponente(hijo));

            return nodo;
        }

        private void AgregarNodosRecursivos(TreeNode padre, IComponentePermiso_750VR componente)
        {
            TreeNode nodo = new TreeNode(componente.Nombre_750VR);
            nodo.Tag = componente;

            if (padre == null)
                treeView1.Nodes.Add(nodo); // nodo raíz
            else
                padre.Nodes.Add(nodo);     // nodo hijo

            if (componente is GrupoPermiso_750VR grupo)
            {
                foreach (var hijo in grupo.ObtenerHijos())
                {
                    AgregarNodosRecursivos(nodo, hijo);
                }
            }
        }
        private void MostrarPermisosDePerfil(BEperfil_750VR perfil)
        {
            treeView1.Nodes.Clear(); // limpia

            var permisos = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);

            foreach (var p in permisos)
            {
                AgregarNodosRecursivos(null, p); // << usa la función recursiva
            }

            treeView1.ExpandAll(); // muestra todo desplegado
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
                // 🔁 IMPORTANTE: traer hijos desde la base
                var familiaCompleta = bllPerfil.ObtenerFamiliaPorId(familia.Codigo_750VR);
                familiaCompleta.Hijos = bllPerfil.ObtenerPermisosDeFamilia(familia.Codigo_750VR);

                TreeNode nodoRaiz = new TreeNode(familiaCompleta.Nombre_750VR) { Tag = familiaCompleta };

                foreach (var hijo in familiaCompleta.ObtenerHijos())
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
            if (cmbperf.SelectedItem is BEperfil_750VR perfil)
            {
                perfilSeleccionado = perfil;
                perfilSeleccionado.Permisos_750VR = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
                RefrescarTreeViewPerfil();
            }
        }



        private void MostrarPermisosDelPerfil(BEperfil_750VR perfil)
        {
            treeView1.Nodes.Clear();

            TreeNode nodoPerfil = new TreeNode(perfil.NombrePerfil_750VR)
            {
                Tag = perfil
            };

            // Trae los permisos asignados
            var permisos = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);

            foreach (var permiso in permisos)
            {
                // Si es familia, traigo sus hijos
                if (permiso is GrupoPermiso_750VR grupo)
                {
                    grupo.Hijos = bllPerfil.ObtenerPermisosDeFamilia(grupo.Codigo_750VR);
                }

                TreeNode nodoPermiso = CrearNodoPermisoRecursivo(permiso);
                nodoPerfil.Nodes.Add(nodoPermiso);
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
         
            //if (estaCargando || cmbfam.SelectedIndex == -1)
            //    return;

           
            //if (cmbfam.SelectedItem is GrupoPermiso_750VR familia)
            //{
              
            //    var hijos = bllPerfil.ObtenerPermisosDeFamilia(familia.Codigo_750VR);
            //    familia.Hijos = hijos;

               
            //    MostrarPermisosDeFamilia(familia);
            //}
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is BEperfil_750VR perfil)
            {
                perfilSeleccionado = perfil;

                // Si tenés cargado el árbol con hijos en PermisosCompuestos_750VR:
                RefrescarTreeViewPerfil();
            }
        }

        private void txtnomperf_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
