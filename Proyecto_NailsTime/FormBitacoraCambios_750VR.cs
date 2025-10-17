using BE_VR750;
using BLL_VR750;
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
    public partial class FormBitacoraCambios_750VR : Form
    {
        private readonly BLLbitacoraCambios_750VR bllCambios = new BLLbitacoraCambios_750VR();
        public FormBitacoraCambios_750VR()
        {
            InitializeComponent();
        }

        private void FormBitacoraCambios_750VR_Load(object sender, EventArgs e)
        {
            CargarComboInsumos();
            dateTimePicker1.Value = DateTime.Now.AddDays(-3);
            dateTimePicker2.Value = DateTime.Now;
            CargarTodosLosCambios();
        }

        private void CargarTodosLosCambios()
        {
            // Muestra los últimos 30 días por defecto
            DateTime fechaInicio = DateTime.Now.AddDays(-30);
            DateTime fechaFin = DateTime.Now;

            var lista = bllCambios.ObtenerCambios(null, null, fechaInicio, fechaFin);
            dataGridView1.DataSource = lista;

            FormatearGrilla();
        }

        private void CargarComboInsumos()
        {
            var bllInsumo = new BLLinsumos_750VR();
            var lista = bllInsumo.LeerInsumos_750VR();

            cmbInsumo.DataSource = lista;
            cmbInsumo.DisplayMember = "nombre_750VR";
            cmbInsumo.ValueMember = "CodInsumo_750VR";
            cmbInsumo.SelectedIndex = -1;
        }

        private void btnapli_Click(object sender, EventArgs e)
        {
            int? codInsumo = (cmbInsumo.SelectedIndex >= 0) ? (int?)cmbInsumo.SelectedValue : null;
            string nombre = txtNombre.Text.Trim();
            DateTime fechaInicio = dateTimePicker1.Value.Date;
            DateTime fechaFin = dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1);

            var lista = bllCambios.ObtenerCambios(codInsumo, nombre, fechaInicio, fechaFin);
            dataGridView1.DataSource = lista;
            FormatearGrilla();
        }

        private void btnlimp_Click(object sender, EventArgs e)
        {
            cmbInsumo.SelectedIndex = -1;
            txtNombre.Clear();
            dateTimePicker1.Value = DateTime.Now.AddDays(-3);
            dateTimePicker2.Value = DateTime.Now;
            dataGridView1.DataSource = null;
            CargarTodosLosCambios();
        }
        private void FormatearGrilla()
        {
            if (dataGridView1.Columns.Contains("Act"))
            {
                // Mostrar claramente los activos/inactivos
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    bool activo = Convert.ToBoolean(row.Cells["Act"].Value);
                    row.DefaultCellStyle.BackColor = activo ? System.Drawing.Color.LightGreen : System.Drawing.Color.MistyRose;
                }
            }

            // Ajustar columnas visuales
            dataGridView1.Columns["CodInsumo_750VR"].HeaderText = "Código";
            dataGridView1.Columns["nombre_750VR"].HeaderText = "Nombre";
            dataGridView1.Columns["descripcion_750VR"].HeaderText = "Descripción";
            dataGridView1.Columns["cantidadActual_750VR"].HeaderText = "Cantidad";
            dataGridView1.Columns["stockMinimo_750VR"].HeaderText = "Stock Mínimo";
            dataGridView1.Columns["unidadMedida_750VR"].HeaderText = "Unidad";
            //dataGridView1.Columns["activo_750VR"].HeaderText = "Activo BD";
            dataGridView1.Columns["Act"].HeaderText = "Versión Activa";
        }

        private void btnimp_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro para activar.");
                return;
            }

            var fila = (BEinsumoCambios_750VR)dataGridView1.CurrentRow.DataBoundItem;
            bllCambios.ActivarVersion(fila.CodInsumo_750VR, fila.Fecha, fila.Hora);

            MessageBox.Show("Versión activada correctamente.");
            btnapli_Click(sender, e); // refrescar
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
