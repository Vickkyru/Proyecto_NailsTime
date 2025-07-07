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

namespace Proyecto_NailsTime
{
    public partial class FormCrearPerfiles : Form
    {
        public FormCrearPerfiles()
        {
            InitializeComponent();
        }


        private BLLperfil_750VR bllPerfil = new BLLperfil_750VR();
        private void FormCrearPerfiles_750VR_Load(object sender, EventArgs e)
        {
            RefrescarPantalla();
            RefrescarPantallaFamilias();
        }

        private void RefrescarPantallaFamilias()
        {
            // Limpiar y volver a cargar el ComboBox con familias creadas
            CargarComboBoxFamiliasCreadas(); // este método lo explico abajo si no lo tenés

            // Limpiar y mostrar solo el nodo raíz vacío en el árbol de familias
            treeView2.Nodes.Clear(); // asegurate de tener ese TreeView
            TreeNode nodoRaiz = new TreeNode("Seleccione una familia")
            {
                Name = "Raiz"
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
        }

        private void RefrescarPantalla()
        {
            // Refrescar ComboBox de perfiles
            CargarComboBoxPerfiles();

            // Refrescar árbol con nodo raíz vacío
            treeView1.Nodes.Clear();
            TreeNode nodoRaiz = new TreeNode("Seleccione un perfil") { Name = "Raiz" };
            treeView1.Nodes.Add(nodoRaiz);
            treeView1.ExpandAll();

            // Refrescar combos de familias y permisos
            CargarComboBoxFamilias();
           
            CargarComboPermisos();
           
        }
        private void CargarComboBoxPerfiles()
        {
            var perfiles = bllPerfil.ObtenerPerfiles();

            cmbperf.DataSource = null;
            cmbperf.DataSource = perfiles;
            cmbperf.DisplayMember = "NombrePerfil_750VR";
            cmbperf.ValueMember = "CodPerfil_750VR";

            cmbperf.SelectedIndex = -1;

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
        private void MostrarPerfilEnTreeview(BEperfil_750VR perfil)
        {
            treeView1.Nodes.Clear();

            TreeNode raiz = new TreeNode(perfil.NombrePerfil_750VR);
            raiz.Tag = perfil;

            var permisos = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
            perfil.Permisos_750VR = permisos;

            foreach (var permiso in permisos)
            {
                TreeNode nodoPermiso = CrearNodoPermiso(permiso);
                raiz.Nodes.Add(nodoPermiso);
            }

            treeView1.Nodes.Add(raiz);
            treeView1.ExpandAll(); // Mostrar todos los nodos
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
                MessageBox.Show("Debe ingresar un nombre de perfil.");
                return;
            }

            var nuevoPerfil = new BEperfil_750VR { NombrePerfil_750VR = nombrePerfil };

            try
            {
                bllPerfil.AgregarPerfil(nuevoPerfil);
                MessageBox.Show("Perfil agregado correctamente.");
                txtnomperf.Clear();
                CargarTreeViewPerfiles();
                RefrescarPantalla();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar perfil: " + ex.Message);
            }
        }

        private void btnelimperf_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Seleccione un perfil a eliminar.");
                return;
            }

            if (treeView1.SelectedNode.Tag is BEperfil_750VR perfilSeleccionado)
            {
                var confirm = MessageBox.Show("¿Está seguro que desea eliminar este perfil?", "Confirmar", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        bllPerfil.EliminarPerfil(perfilSeleccionado.CodPerfil_750VR);
                        MessageBox.Show("Perfil eliminado correctamente.");
                        CargarTreeViewPerfiles();
                        RefrescarPantalla();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar perfil: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un nodo de tipo perfil.");
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
                MessageBox.Show("Debes seleccionar el PERFIL (nodo raíz) para agregar un permiso.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Seleccioná un perfil del árbol.");
                return;
            }

            if (cmbpermperf.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un permiso.");
                return;
            }

            var perfil = treeView1.SelectedNode.Tag as BEperfil_750VR;
            var permiso = cmbpermperf.SelectedItem as PermisoSimple_750VR;

            bllPerfil.AsignarPermiso(perfil.CodPerfil_750VR, permiso.Codigo_750VR);
            MessageBox.Show("Permiso agregado.");
            MostrarPerfilEnTreeview(perfil);
            RefrescarPantalla();
        }

        private void btnelimpermperf_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            if (nodoSeleccionado == null || nodoSeleccionado.Parent == null)
            {
                MessageBox.Show("Debes seleccionar un PERMISO o FAMILIA que esté directamente asignado al PERFIL.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // El padre debe ser el perfil (nodo raíz)
            TreeNode nodoPadre = nodoSeleccionado.Parent;
            if (nodoPadre.Parent != null)
            {
                MessageBox.Show("Solo se pueden quitar permisos o familias asignados directamente al perfil, no a subniveles.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("Seleccioná un perfil del árbol.");
                return;
            }

            if (cmbpermperf.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un permiso.");
                return;
            }

            var perfil = treeView1.SelectedNode.Tag as BEperfil_750VR;
            var permiso = cmbpermperf.SelectedItem as PermisoSimple_750VR;

            bllPerfil.QuitarPermiso(perfil.CodPerfil_750VR, permiso.Codigo_750VR);
            MessageBox.Show("Permiso quitado.");
            MostrarPerfilEnTreeview(perfil);
            RefrescarPantalla();
        }

        private void btnagfamperf_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            if (nodoSeleccionado == null || nodoSeleccionado.Parent != null)
            {
                MessageBox.Show("Debes seleccionar el PERFIL (nodo raíz) para agregar una familia.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (treeView1.SelectedNode == null || !(treeView1.SelectedNode.Tag is BEperfil_750VR perfil))
            {
                MessageBox.Show("Seleccioná un perfil del árbol para asignarle una familia.");
                return;
            }

            if (cmbfamperf.SelectedItem is GrupoPermiso_750VR familiaSeleccionada)
            {
                bllPerfil.AsignarFamilia(perfil.CodPerfil_750VR, familiaSeleccionada.Codigo_750VR);
                MessageBox.Show("Familia asignada correctamente.");

                CargarTreeViewPerfiles(); // Refrescar árbol
                RefrescarPantalla();
            }
            else
            {
                MessageBox.Show("Seleccioná una familia válida.");
            }
        }

        private void btnelimfamperf_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null || !(treeView1.SelectedNode.Tag is BEperfil_750VR perfil))
            {
                MessageBox.Show("Seleccioná un perfil del árbol para quitarle una familia.");
                return;
            }

            if (cmbfamperf.SelectedItem is GrupoPermiso_750VR familiaSeleccionada)
            {
                bllPerfil.QuitarFamilia(perfil.CodPerfil_750VR, familiaSeleccionada.Codigo_750VR);
                MessageBox.Show("Familia quitada correctamente.");

                CargarTreeViewPerfiles(); // Refrescar árbol
                RefrescarPantalla();
            }
            else
            {
                MessageBox.Show("Seleccioná una familia válida.");
            }
        }

        private void btnagfam_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtfam.Text))
            {
                MessageBox.Show("Ingrese un nombre para la familia.");
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
                MessageBox.Show("Seleccione una familia del árbol para eliminar.");
                return;
            }

            DialogResult result = MessageBox.Show($"¿Está seguro de eliminar la familia '{familia.Nombre_750VR}'?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                bllPerfil.EliminarFamilia(familia.Codigo_750VR);
                CargarTreeViewFamilias();
                RefrescarPantalla();
            }
        }

        private void CargarTreeViewFamilias()
        {
            treeView2.Nodes.Clear();

            var familias = bllPerfil.ObtenerFamilias();

            foreach (var familia in familias)
            {
                TreeNode nodo = new TreeNode(familia.Nombre_750VR) { Tag = familia };
                AgregarNodoPermiso(nodo, familia);
                treeView2.Nodes.Add(nodo);
            }

            treeView2.ExpandAll();
        }

        private void agpermfam_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado2 = treeView1.SelectedNode;

            if (nodoSeleccionado2 == null || nodoSeleccionado2.Parent != null)
            {
                MessageBox.Show("Debes seleccionar la FAMILIA raíz (nodo raíz) para agregar un permiso simple.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nodoSeleccionado = treeView2.SelectedNode;

            if (nodoSeleccionado == null || !(nodoSeleccionado.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show("Seleccione una familia del árbol para asignarle un permiso.");
                return;
            }

            if (cmbpermfam.SelectedItem is PermisoSimple_750VR permiso)
            {
                bllPerfil.AgregarPermisoAFamilia(familia.Codigo_750VR, permiso.Codigo_750VR);
                CargarTreeViewFamilias();
                RefrescarPantalla();
            }
            else
            {
                MessageBox.Show("Seleccione un permiso.");
            }
        }

        private void elimpermfam_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            if (nodoSeleccionado == null || nodoSeleccionado.Parent == null)
            {
                MessageBox.Show("Debes seleccionar un PERMISO o FAMILIA que esté directamente asignado a la FAMILIA raíz.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // El padre debe ser la familia raíz
            TreeNode nodoPadre = nodoSeleccionado.Parent;
            if (nodoPadre.Parent != null)
            {
                MessageBox.Show("Solo se pueden quitar permisos o familias asignados directamente a la familia raíz, no a subniveles.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nodoSeleccionado2 = treeView2.SelectedNode;

            if (nodoSeleccionado2 == null || !(nodoSeleccionado2.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show("Seleccione una familia del árbol para quitarle un permiso.");
                return;
            }

            if (cmbpermfam.SelectedItem is PermisoSimple_750VR permiso)
            {
                bllPerfil.QuitarPermisoDeFamilia(familia.Codigo_750VR, permiso.Codigo_750VR);
                CargarTreeViewFamilias();
                RefrescarPantalla();
            }
            else
            {
                MessageBox.Show("Seleccione un permiso.");
            }
        }

        private void btnagfamfam_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView2.SelectedNode;

            if (nodoSeleccionado == null || !(nodoSeleccionado.Tag is GrupoPermiso_750VR familiaPadre))
            {
                MessageBox.Show("Seleccione una familia en el árbol para agregarle otra familia.");
                return;
            }

            if (cmbfamfam.SelectedItem is GrupoPermiso_750VR familiaHija)
            {
                if (familiaPadre.Codigo_750VR == familiaHija.Codigo_750VR)
                {
                    MessageBox.Show("No se puede agregar una familia a sí misma.");
                    return;
                }

                bllPerfil.AsignarFamiliaAFamilia(familiaPadre.Codigo_750VR, familiaHija.Codigo_750VR);
                CargarTreeViewFamilias();
                RefrescarPantalla();
            }
            else
            {
                MessageBox.Show("Seleccione una familia a agregar.");
            }
        }

        private void btnelimfamfam_Click(object sender, EventArgs e)
        {
            TreeNode nodoSeleccionado = treeView2.SelectedNode;

            if (nodoSeleccionado == null || !(nodoSeleccionado.Tag is GrupoPermiso_750VR familiaPadre))
            {
                MessageBox.Show("Seleccione una familia en el árbol para quitarle otra familia.");
                return;
            }

            if (cmbfamfam.SelectedItem is GrupoPermiso_750VR familiaHija)
            {
                bllPerfil.QuitarFamiliaDeFamilia(familiaPadre.Codigo_750VR, familiaHija.Codigo_750VR);
                CargarTreeViewFamilias();
                RefrescarPantalla();
            }
            else
            {
                MessageBox.Show("Seleccione una familia a quitar.");
            }
        }

        private void cmbperf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbperf.SelectedItem == null)
                return;

            // Obtenés el perfil seleccionado
            var perfilSeleccionado = (BEperfil_750VR)cmbperf.SelectedItem;

            // Cargás el árbol con los permisos del perfil
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
    }
}
