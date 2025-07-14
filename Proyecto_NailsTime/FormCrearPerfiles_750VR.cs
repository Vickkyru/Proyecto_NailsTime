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
        private GrupoPermiso_750VR perfilCompuesto; // Opcional si usás la propiedad interna
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



        private TreeNode CrearNodoPermiso(IComponentePermiso_750VR comp)
        {
            TreeNode nodo = new TreeNode(comp.Nombre_750VR)
            {
                Tag = comp        // ⬅️ SIEMPRE asignamos el componente
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
            MostrarTreeViewInicial();
            
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

            if (bllPerfil.PerfilTieneUsuariosAsociados(perfilSeleccionado.CodPerfil_750VR))
            {
                MessageBox.Show("No se puede eliminar el perfil porque está asignado a uno o más usuarios.");
                return;
            }


            var confirm = MessageBox.Show($"¿Está seguro de eliminar el perfil '{perfilSeleccionado.NombrePerfil_750VR}'?",
                                          "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                bllPerfil.EliminarPerfil(perfilSeleccionado.CodPerfil_750VR);

                MessageBox.Show("Perfil eliminado correctamente.");
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
            // 1) Obtener el perfil seleccionado del árbol
            if (treeView1.SelectedNode == null || !(treeView1.SelectedNode.Tag is BEperfil_750VR perfil))
            {
                MessageBox.Show("Debe seleccionar un perfil desde el árbol.");
                return;
            }

            // 2) Validar que haya un permiso seleccionado del combo
            if (!(cmbpermperf.SelectedItem is PermisoSimple_750VR permiso))
            {
                MessageBox.Show("Debe seleccionar un permiso simple.");
                return;
            }

            // 3) Verificar que el permiso no sea una familia (por seguridad)
            if (permiso is GrupoPermiso_750VR)
            {
                MessageBox.Show("No se puede asignar una familia desde aquí. Use la sección de asignación de familias.");
                return;
            }

            // 4) Verificar que no esté ya en los permisos del perfil (en memoria)
            bool yaExiste = PermisoYaExisteEnLista(perfil.Permisos_750VR, permiso.Codigo_750VR);
            if (yaExiste)
            {
                MessageBox.Show("Este permiso ya está asignado al perfil (directa o indirectamente).");
                return;
            }

            // 5) Verificar si ya está en la BD
            var permisosActuales = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);
            bool yaAsignado = permisosActuales
                .OfType<PermisoSimple_750VR>()
                .Any(p => p.Codigo_750VR == permiso.Codigo_750VR);

            if (yaAsignado)
            {
                MessageBox.Show("Este permiso ya está asignado al perfil.");
                RefrescarTreeViewPerfil();
                return;
            }

            // 6) Asignar el permiso
            bllPerfil.AsignarPermiso(perfil.CodPerfil_750VR, permiso.Codigo_750VR);
            MessageBox.Show("Permiso asignado correctamente.");

            // 7) Refrescar permisos
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
                MessageBox.Show("No se puede eliminar un permiso de una familia desde aca.");
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
                perfilSeleccionado = perfil; // <-- actualizar la referencia
                perfil.Permisos_750VR = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR); // <- actualizar lista
                RefrescarTreeViewPerfil();
               
            }
        }

        private void btnagfamperf_Click(object sender, EventArgs e)
        {
            // 1. Validar selección en el árbol
            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            if (nodoSeleccionado == null)
            {
                MessageBox.Show("Seleccione un nodo en el árbol.");
                return;
            }

            // 1a) Impedir asignar sobre un permiso simple
            if (nodoSeleccionado.Tag is PermisoSimple_750VR)
            {
                MessageBox.Show("No se puede asignar una familia a un permiso simple. Seleccione el perfil raíz en el árbol.");
                return;
            }

            // 1b) Impedir asignar sobre otra familia
            if (nodoSeleccionado.Tag is GrupoPermiso_750VR)
            {
                MessageBox.Show("No se puede asignar una familia dentro de otra familia desde esta opción. Seleccione el perfil raíz en el árbol.");
                return;
            }

            // 1c) Verificar que sea un perfil
            if (!(nodoSeleccionado.Tag is BEperfil_750VR perfil))
            {
                MessageBox.Show("Seleccione un perfil válido en el árbol.");
                return;
            }

            // 2. Validar familia seleccionada en el combo
            if (!(cmbfamperf.SelectedItem is GrupoPermiso_750VR familia))
            {
                MessageBox.Show("Seleccione una familia para asignar al perfil.");
                return;
            }

            perfilSeleccionado = perfil;

            int idFamilia = familia.Codigo_750VR;

            var familiaCompleta = bllPerfil.ObtenerFamiliaCompletaPorId(idFamilia);
            if (familiaCompleta == null)
            {
                MessageBox.Show("No se encontró la familia.");
                return;
            }

            if (bllPerfil.FamiliaYaAsignada(perfil.CodPerfil_750VR, idFamilia))
            {
                MessageBox.Show("Esa familia ya está asignada al perfil.");
                return;
            }

            // Obtener todos los permisos actuales del perfil
            var permisosActuales = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);

            // Obtener todos los permisos que tiene la familia (de forma recursiva)
            var permisosDeLaFamilia = familiaCompleta.ObtenerTodosLosPermisos();

            // Validar si hay alguno ya asignado
            bool yaExisten = permisosDeLaFamilia
                .Any(pNuevo => permisosActuales.Any(pExistente => pExistente.Codigo_750VR == pNuevo.Codigo_750VR));

            if (yaExisten)
            {
                MessageBox.Show("No se puede asignar esta familia porque ya contiene permisos que el perfil ya tiene.");
                return;
            }


            // Asignación
            perfil.Agregar(familiaCompleta);
            bllPerfil.AsignarFamiliaAlPerfil(perfil.CodPerfil_750VR, idFamilia);
            MessageBox.Show("Familia agregada correctamente.");

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

            // Validar si ya existe una familia con ese nombre (ignora mayúsculas)
            var familiasExistentes = bllPerfil.ObtenerFamilias(); 
            bool yaExiste = familiasExistentes.Any(f =>
                f.Nombre_750VR.Trim().Equals(nombreFamilia, StringComparison.OrdinalIgnoreCase));

            if (yaExiste)
            {
                MessageBox.Show("Ya existe una familia con ese nombre.");
                return;
            }

            // Si no existe, se crea
            bllPerfil.AgregarFamilia(nombreFamilia);
            MessageBox.Show("Se ha agregado una familia nueva.");

            RefrescarPantalla();
            CargarFamilias();
            MostrarTreeViewInicialFamilia();
            txtfam.Clear();
        }
        


        //TreeNode CrearNodoDesdeComponente(IComponentePermiso_750VR componente)
        //{
        //    TreeNode nodo = new TreeNode(componente.Nombre_750VR);
        //    nodo.Tag = componente;

        //    foreach (var hijo in componente.ObtenerHijos())
        //    {
        //        nodo.Nodes.Add(CrearNodoDesdeComponente(hijo));
        //    }

        //    return nodo;
        //}

        private void btnelimfam_Click(object sender, EventArgs e)
        {
            if (treeView2.SelectedNode == null || !(treeView2.SelectedNode.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaEliminar"));
                return;
            }

            // Validar si la familia está asignada a algún perfil
            bool estaAsignada = bllPerfil.FamiliaAsignadaAAlgunPerfil(familia.Codigo_750VR);
            if (estaAsignada)
            {
                MessageBox.Show("No se puede eliminar esta familia porque está asignada a uno o más perfiles. Primero desasígnela.");
                return;
            }

            // Confirmación
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
                // Opción 2 – explícita
                var hijos = bllPerfil.ObtenerPermisosDeFamilia(familia.Codigo_750VR);
                foreach (var h in hijos)
                    familiaCompleta.Agregar(h);   // usa el método Agregar del Composite


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
            /*----------------------------------------------------------
        * 1) Debe haber un nodo seleccionado EN EL TREE DE FAMILIAS
        *    (treeView2). Si no, avisamos y salimos.
        *---------------------------------------------------------*/
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

            /*----------------------------------------------------------
             * 2) El padre del nodo (el nivel inmediatamente anterior)
             *    DEBE ser la familia raíz. Si el padre no existe o a su
             *    vez tiene padre, significa que el usuario eligió algo
             *    más profundo y no lo permitimos.
             *---------------------------------------------------------*/
            TreeNode nodoPadre = nodoSeleccionado.Parent;
            if (nodoPadre == null || nodoPadre.Parent != null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SoloQuitarAsignadosADirectoRaiz"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            /*----------------------------------------------------------
             * 3) Verificamos que el PADRE sea efectivamente una familia
             *---------------------------------------------------------*/
            GrupoPermiso_750VR familia = nodoPadre.Tag as GrupoPermiso_750VR;
            if (familia == null)
            {
                // Si Tag no contiene la familia, algo está mal cargado.
                MessageBox.Show("Nodo padre inválido (no es una familia).");
                return;
            }

            /*----------------------------------------------------------
             * 4) El nodo a eliminar debe ser un PermisoSimple.
             *---------------------------------------------------------*/
            PermisoSimple_750VR permiso = nodoSeleccionado.Tag as PermisoSimple_750VR;
            if (permiso == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SoloPermisosSimplesEliminar"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.Advertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            /*----------------------------------------------------------
             * 5) Confirmación
             *---------------------------------------------------------*/
            DialogResult confirm = MessageBox.Show(
                $"¿Desea quitar el permiso '{permiso.Nombre_750VR}' de la familia '{familia.Nombre_750VR}'?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            /*----------------------------------------------------------
             * 6) BLL → eliminar y refrescar UI
             *---------------------------------------------------------*/
            bllPerfil.QuitarPermisoDeFamilia(familia.Codigo_750VR, permiso.Codigo_750VR);

            // Recargar la familia en el árbol para que se vea el cambio
            RefrescarYMostrarFamilia(familia.Codigo_750VR);
        }

        private void btnagfamfam_Click(object sender, EventArgs e)
        {
            // ──────────────────── 1) Validar selección en el árbol
            TreeNode nodoSeleccionado = treeView2.SelectedNode;

            if (nodoSeleccionado == null || !(nodoSeleccionado.Tag is GrupoPermiso_750VR familiaPadre))
            {
                MessageBox.Show("Seleccione la familia PADRE en el árbol.");
                return;
            }

            // ──────────────────── 2) Validar selección en el ComboBox
            if (!(cmbfamfam.SelectedItem is GrupoPermiso_750VR familiaHija))
            {
                MessageBox.Show("Seleccione la familia HIJA en el combo.");
                return;
            }

            // ──────────────────── 3) Mismo ID  (padre == hija)
            if (familiaPadre.Codigo_750VR == familiaHija.Codigo_750VR)
            {
                MessageBox.Show("No se puede agregar una familia dentro de sí misma.");
                return;
            }

            // ──────────────────── 4) Ciclo indirecto  (hija contiene al padre)
            if (bllPerfil.FamiliaContieneAFamilia(familiaHija.Codigo_750VR, familiaPadre.Codigo_750VR))
            {
                MessageBox.Show("No se puede crear una relación circular entre familias.");
                return;
            }

            // ──────────────────── 5) Permisos duplicados
            //    a) Obtener permisos de la familia padre
            var permisosPadre = familiaPadre.ObtenerTodosLosPermisos();           // método recursivo del Composite
                                                                                  //    b) Obtener permisos de la familia hija COMPLETA
            var hijaCompleta = bllPerfil.ObtenerFamiliaCompletaPorId(familiaHija.Codigo_750VR);
            var permisosDeHija = hijaCompleta.ObtenerTodosLosPermisos();

            bool hayDuplicados = permisosDeHija
                .Any(pHija => permisosPadre.Any(pPadre => pPadre.Codigo_750VR == pHija.Codigo_750VR));

            if (hayDuplicados)
            {
                MessageBox.Show("La familia hija contiene permisos que la familia padre ya posee.");
                return;
            }

            // ──────────────────── 6) Insertar si todo es válido
            bool agregado = bllPerfil.AsignarFamiliaAFamilia(familiaPadre.Codigo_750VR, familiaHija.Codigo_750VR);

            if (agregado)
            {
                MessageBox.Show("Familia agregada correctamente.");
                var familiaActualizada = bllPerfil.ObtenerFamiliaPorId(familiaPadre.Codigo_750VR);
                MostrarPermisosDeFamilia(familiaActualizada);   // refresca el árbol
            }
            else
            {
                MessageBox.Show("Esa familia ya estaba asignada.");
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

                // Cargar los permisos del perfil desde la BLL
                perfilSeleccionado.Permisos_750VR = bllPerfil.ObtenerPermisosDePerfil(perfil.CodPerfil_750VR);

  

                // Refrescar visualmente el TreeView
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
                    familiaActualizada.Agregar(hijo); // ✅ Agrega usando el método del patrón Composite
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

                // Obtener familia completa (con hijos y permisos)
                var familiaCompleta = bllPerfil.ObtenerFamiliaPorId(familiaSeleccionada.Codigo_750VR);
                if (familiaCompleta == null) return;

                // Crear nodo raíz y agregarlo al TreeView
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
            //if (e.Node?.Tag is BEperfil_750VR perfil)
            //{
            //    perfilSeleccionado = perfil;

            //    // Si tenés cargado el árbol con hijos en PermisosCompuestos_750VR:
            //    RefrescarTreeViewPerfil();
            //}

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
                MessageBox.Show("Seleccione un perfil y una familia a quitar.");
                return;
            }

            TreeNode nodoSeleccionado = treeView1.SelectedNode;

            // Solo permitir quitar familias, no permisos simples
            if (nodoSeleccionado.Tag is GrupoPermiso_750VR familia)
            {
                // Eliminar de la base de datos
                bllPerfil.EliminarFamiliaDePerfil(perfilSeleccionado.CodPerfil_750VR, familia.Codigo_750VR);

                // Eliminar del objeto en memoria
                perfilSeleccionado.Hijos.RemoveAll(h => h.Codigo_750VR == familia.Codigo_750VR);
                MessageBox.Show("Se elimino la familia correctamente");


                // Refrescar TreeView
                perfilSeleccionado.Permisos_750VR = bllPerfil.ObtenerPermisosDePerfil(perfilSeleccionado.CodPerfil_750VR);
                RefrescarTreeViewPerfil();
                
            }
            else
            {
                MessageBox.Show("Solo se pueden quitar familias, no permisos individuales.");
            }
        }

        private void cmbpermperf_SelectedIndexChanged(object sender, EventArgs e)
        {
            //List<IComponentePermiso_750VR> todosLosPermisos = bllPerfil.ObtenerTodosLosPermisos();
            //List<PermisoSimple_750VR> permisosSimples = todosLosPermisos
            //    .OfType<PermisoSimple_750VR>()
            //    .ToList();

            //cmbpermperf.DataSource = permisosSimples;
            //cmbpermperf.DisplayMember = "Nombre_750VR";
            //cmbpermperf.ValueMember = "Codigo_750VR";
           
        }

        private void btnmodperf_Click(object sender, EventArgs e)
        {
            var perfil = cmbperf.SelectedItem as BEperfil_750VR;
            if (perfil == null)
            {
                MessageBox.Show("Debe seleccionar un perfil para modificar.");
                return;
            }

            string nuevoNombre = txtnomperf.Text.Trim();
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show("Debe ingresar un nuevo nombre para el perfil.");
                return;
            }

            // Validar que no exista otro perfil con el mismo nombre
            var todos = bllPerfil.ObtenerPerfiles(); // Asumimos que este método existe
            if (todos.Any(p => p.NombrePerfil_750VR.Equals(nuevoNombre, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Ya existe un perfil con ese nombre.");
                return;
            }

            // Modificar en BD
            bllPerfil.ModificarNombrePerfil(perfil.CodPerfil_750VR, nuevoNombre);

            MessageBox.Show("Perfil modificado correctamente.");

            // Recargar combos y TreeView
            CargarPerfilesEnComboBox();
            CargarTreeViewPerfiles();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Validar selección de familia
            if (treeView2.SelectedNode == null || !(treeView2.SelectedNode.Tag is GrupoPermiso_750VR familia))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.SeleccioneFamiliaModificar"));
                return;
            }

            // 2. Validar que se haya ingresado un nuevo nombre
            string nuevoNombre = txtfam.Text.Trim();
            if (string.IsNullOrWhiteSpace(nuevoNombre))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCrearPerfiles.IngreseNombreFamilia"));
                return;
            }

            // 3. Verificar que no exista otra familia con ese nombre (excepto la actual)
            if (bllPerfil.ExisteFamiliaConNombre(nuevoNombre, familia.Codigo_750VR))
            {
                MessageBox.Show("Ya existe otra familia con ese nombre.");
                return;
            }

            // 4. Realizar modificación
            bllPerfil.ModificarNombreFamilia(familia.Codigo_750VR, nuevoNombre);
            MessageBox.Show("Familia modificada correctamente.");

            // 5. Refrescar vista
            txtfam.Clear();
            CargarTreeViewFamilias();
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
