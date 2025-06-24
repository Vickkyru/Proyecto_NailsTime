using BE_VR750;
using BLL_VR750;
using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_NailsTime
{
    public partial class FormABMinsumos_750VR : Form, Iobserver_750VR
    {
        private string modoActual = "consulta";
        BLLinsumos_750VR bll = new BLLinsumos_750VR();

        public FormABMinsumos_750VR()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
            ActualizarMensajeModo();
        }

        private void ActualizarMensajeModo()
        {
            string clave = "FormABMinsumos.Mensaje." + modoActual;
            lblmensaje.Text = Lenguaje_750VR.ObtenerEtiqueta(clave);
        }

        private void FormABMinsumos_750VR_Load(object sender, EventArgs e)
        {
            modoActual = "consulta";
            btnapli.Enabled = false;
            btncance.Enabled = false;
            rbnActivos.Checked = true;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            CargarInsumos(true);
        }

        private void CargarInsumos(bool soloActivos)
        {
            var lista = bll.LeerInsumos_750VR();
            if (soloActivos)
                lista = lista.Where(i => i.activo_750VR).ToList();

            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CodInsumo_750VR", HeaderText = "Código", Visible = false });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "nombre_750VR", HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Insumo.Nombre") });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "descripcion_750VR", HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Insumo.Descripcion") });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "cantidadActual_750VR", HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Insumo.CantidadActual") });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "stockMinimo_750VR", HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Insumo.StockMinimo") });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "unidadMedida_750VR", HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Insumo.UnidadMedida") });
            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { DataPropertyName = "activo_750VR", HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Insumo.Activo") });

            dataGridView1.DataSource = lista;
            PintarInactivos();
        }

        private void PintarInactivos()
        {
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.DataBoundItem is BEinsumos_750VR insumo && !insumo.activo_750VR)
                {
                    fila.DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void btnañadir_Click(object sender, EventArgs e)
        {
            modoActual = "añadir";
            ActualizarMensajeModo();
            ActivarModoEdicion();
        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            modoActual = "modificar";
            ActualizarMensajeModo();
            ActivarModoEdicion();
        }

        private void btnelim_Click(object sender, EventArgs e)
        {
            modoActual = "cambiarEstado";
            ActualizarMensajeModo();
            ActivarModoEdicion();
        }

        private void btnapli_Click(object sender, EventArgs e)
        {
            switch (modoActual)
            {
                case "añadir": AplicarAlta(); break;
                case "modificar": AplicarModificacion(); break;
                case "cambiarEstado": AplicarCambioEstado(); break;
            }

            modoActual = "consulta";
            ActualizarMensajeModo();
            ResetearInterfaz();
            CargarInsumos(true);
            LimpiarCampos();
        }


        private void AplicarAlta()
        {
            try
            {
                if (!ValidarCampos()) return;

                // Validar duplicados
                var existentes = bll.LeerInsumos_750VR();
                bool yaExiste = existentes.Any(i =>
                    i.nombre_750VR.Trim().ToLower() == txtnombre.Text.Trim().ToLower() &&
                    i.unidadMedida_750VR.Trim().ToLower() == txtunidad.Text.Trim().ToLower() &&
                    i.activo_750VR
                );

                if (yaExiste)
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMinsumos_750VR.InsumoDuplicado"));
                    return;
                }

                var nuevo = new BEinsumos_750VR
                {
                    nombre_750VR = txtnombre.Text.Trim(),
                    descripcion_750VR = txtdesc.Text.Trim(),
                    cantidadActual_750VR = int.Parse(txtcant.Text),
                    stockMinimo_750VR = int.Parse(txtstock.Text),
                    unidadMedida_750VR = txtunidad.Text.Trim(),
                    activo_750VR = true
                };

                bll.CrearInsumo_750VR(nuevo);
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMinsumos_750VR.InsumoCreado"));

                CargarInsumos(true); // refresca la grilla
                LimpiarCampos();
                ResetearInterfaz();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMinsumos_750VR.ErrorCrearInsumo") + ": " + ex.Message);
            }
        }

        private void AplicarModificacion()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEinsumos_750VR insumo)
            {
                if (!ValidarCampos()) return;

                insumo.nombre_750VR = txtnombre.Text.Trim();
                insumo.descripcion_750VR = txtdesc.Text.Trim();
                insumo.cantidadActual_750VR = int.Parse(txtcant.Text);
                insumo.stockMinimo_750VR = int.Parse(txtstock.Text);
                insumo.unidadMedida_750VR = txtunidad.Text.Trim();

                bool modificado = bll.ModificarInsumo_750VR(insumo);

                if (modificado)
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMinsumos_750VR.InsumoModificado"));
                else
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMinsumos_750VR.ModificacionFallida"));
            }
        }

        private void AplicarCambioEstado()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEinsumos_750VR insumo)
            {
                bll.CambiarEstado_750VR(insumo.CodInsumo_750VR, !insumo.activo_750VR);
                string clave = insumo.activo_750VR
                    ? "FormABMinsumos_750VR.InsumoDesactivado"
                    : "FormABMinsumos_750VR.InsumoActivado";

                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta(clave));
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtnombre.Text) || string.IsNullOrWhiteSpace(txtcant.Text)
                || string.IsNullOrWhiteSpace(txtstock.Text) || string.IsNullOrWhiteSpace(txtunidad.Text))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMinsumos_750VR.CamposIncompletos"));
                return false;
            }

            if (!int.TryParse(txtcant.Text, out _) || !int.TryParse(txtstock.Text, out _))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMinsumos_750VR.CantidadInvalida"));
                return false;
            }

            return true;
        }

        private void btncance_Click(object sender, EventArgs e)
        {
            modoActual = "consulta";
            ActualizarMensajeModo();
            ResetearInterfaz();
            LimpiarCampos();
        }

        private void ActivarModoEdicion()
        {
            //txtnombre.Enabled = txtdesc.Enabled = txtcant.Enabled = txtstock.Enabled = txtunidad.Enabled = true;
            //dataGridView1.Enabled = modoActual != "añadir";
            //btnapli.Enabled = true;
            //btncance.Enabled = true;

            if (modoActual == "añadir")
            {
                dataGridView1.Enabled = false;
                btnapli.Enabled = true;
                btncance.Enabled = true;
                btnelim.Enabled = false;
                btnmod.Enabled = false;

                txtnombre.Enabled = txtdesc.Enabled = txtcant.Enabled = txtstock.Enabled = txtunidad.Enabled = true;
            }

            if (modoActual == "modificar")
            {
                dataGridView1.Enabled = false;
                btnapli.Enabled = true;
                btncance.Enabled = true;
                btnelim.Enabled = false;
                btnañadir.Enabled = false;

                txtnombre.Enabled = txtdesc.Enabled = txtcant.Enabled = txtstock.Enabled = txtunidad.Enabled = true;

            }

            else if (modoActual == "cambiarEstado")
            {
                dataGridView1.Enabled = true;

                txtnombre.Enabled = txtdesc.Enabled = txtcant.Enabled = txtstock.Enabled = txtunidad.Enabled = false;

                btnapli.Enabled = true;
                btncance.Enabled = true;
                btnañadir.Enabled = false;
                btnmod.Enabled = false;
            }


        }

        private void ResetearInterfaz()
        {
            txtnombre.Enabled = txtdesc.Enabled = txtcant.Enabled = txtstock.Enabled = txtunidad.Enabled = false;
            dataGridView1.Enabled = true;
            btnapli.Enabled = false;
            btncance.Enabled = false;
        }

        private void LimpiarCampos()
        {
            txtnombre.Clear();
            txtdesc.Clear();
            txtcant.Clear();
            txtstock.Clear();
            txtunidad.Clear();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 &&
               dataGridView1.SelectedRows[0].DataBoundItem is BEinsumos_750VR insumo)
            {
                txtnombre.Text = insumo.nombre_750VR;
                txtdesc.Text = insumo.descripcion_750VR;
                txtcant.Text = insumo.cantidadActual_750VR.ToString();
                txtstock.Text = insumo.stockMinimo_750VR.ToString();
                txtunidad.Text = insumo.unidadMedida_750VR;
            }
        }

        private void rbnActivos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnActivos.Checked) CargarInsumos(true);
        }

        private void rbnTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnTodos.Checked) CargarInsumos(false);
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
