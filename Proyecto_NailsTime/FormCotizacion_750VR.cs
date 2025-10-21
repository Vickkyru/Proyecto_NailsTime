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
    public partial class FormCotizacion_750VR : Form
    {
        private readonly BLLinsumos_750VR bllInsumos = new BLLinsumos_750VR();

        private class ItemSolicitud
        {
            public int CodInsumo { get; set; }
            public string Insumo { get; set; }
            public int Cantidad { get; set; }
            public string Unidad { get; set; }
        }
        private readonly List<ItemSolicitud> _detalle = new List<ItemSolicitud>();
        private readonly List<string> _proveedores = new List<string>();

        public FormCotizacion_750VR()
        {
            InitializeComponent();
        }

        private void FormCotizacion_750VR_Load(object sender, EventArgs e)
        {
            CargarCombos();
            CargarStock();
            RefrescarDetalle();
            RefrescarProveedores();
        }

        // ====== STOCK ======
        private void CargarStock()
        {
            var activos = bllInsumos.LeerInsumosActivos_750VR();

            var dt = new DataTable();
            dt.Columns.Add("Cod", typeof(int));
            dt.Columns.Add("Insumo", typeof(string));
            dt.Columns.Add("Unidad", typeof(string));
            dt.Columns.Add("Actual", typeof(int));
            dt.Columns.Add("Mínimo", typeof(int));
            dt.Columns.Add("Faltante", typeof(int));
            dt.Columns.Add("Sugerido", typeof(int));

            var bajos = bllInsumos.ObtenerBajoMinimo(); // (insumo, faltante, sugerido)

            foreach (var i in activos)
            {
                var low = bajos.FirstOrDefault(b => b.insumo.CodInsumo_750VR == i.CodInsumo_750VR);
                int faltante = low.insumo != null ? low.faltante : 0;
                int sugerido = low.insumo != null ? low.sugerido : 0;

                dt.Rows.Add(
                    i.CodInsumo_750VR,
                    i.nombre_750VR,
                    i.unidadMedida_750VR,
                    i.cantidadActual_750VR,
                    i.stockMinimo_750VR,
                    faltante,
                    sugerido
                );
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns["Cod"].Visible = false;

            // pintar filas con bajo stock
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int falt = Convert.ToInt32(row.Cells["Faltante"].Value);
                if (falt > 0) row.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;
            }
        }

        private void CargarCombos()
        {
            var lista = bllInsumos.LeerInsumosActivos_750VR();
            comboBox1.DataSource = lista;
            comboBox1.DisplayMember = "nombre_750VR";
            comboBox1.ValueMember = "CodInsumo_750VR";
            comboBox1.SelectedIndex = -1;

            // si usás NumericUpDown en el diseñador:
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 1_000_000;
            numericUpDown1.Value = 1;
        }

        // ====== DETALLE (carrito de cotización) ======
        private void RefrescarDetalle()
        {
            var dt = new DataTable();
            dt.Columns.Add("Cod", typeof(int));
            dt.Columns.Add("Insumo", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Unidad", typeof(string));

            foreach (var it in _detalle)
                dt.Rows.Add(it.CodInsumo, it.Insumo, it.Cantidad, it.Unidad);

            dataGridView3.DataSource = dt;
            if (dataGridView3.Columns.Contains("Cod"))
                dataGridView3.Columns["Cod"].Visible = false;
        }

        private void RefrescarProveedores()
        {
            var dt = new DataTable();
            dt.Columns.Add("Proveedor", typeof(string));
            foreach (var p in _proveedores) dt.Rows.Add(p);
            dataGridView2.DataSource = dt;
        }

        private void btnrealiz_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Elegí un insumo.");
                return;
            }

            int cant = (int)numericUpDown1.Value;
            if (cant <= 0)
            {
                MessageBox.Show("Cantidad inválida.");
                return;
            }

            var ins = (BEinsumos_750VR)comboBox1.SelectedItem;

            var existente = _detalle.FirstOrDefault(x => x.CodInsumo == ins.CodInsumo_750VR);
            if (existente != null) existente.Cantidad += cant;
            else _detalle.Add(new ItemSolicitud
            {
                CodInsumo = ins.CodInsumo_750VR,
                Insumo = ins.nombre_750VR,
                Cantidad = cant,
                Unidad = ins.unidadMedida_750VR
            });

            RefrescarDetalle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count == 0) return;
            int cod = Convert.ToInt32(dataGridView3.SelectedRows[0].Cells["Cod"].Value);
            _detalle.RemoveAll(x => x.CodInsumo == cod);
            RefrescarDetalle();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string proveedor = txtproveedor.Text?.Trim();
            if (string.IsNullOrEmpty(proveedor))
            {
                MessageBox.Show("Ingresá un proveedor.");
                return;
            }
            if (_proveedores.Contains(proveedor, StringComparer.OrdinalIgnoreCase))
            {
                MessageBox.Show("Ya está en la lista.");
                return;
            }
            _proveedores.Add(proveedor);
            txtproveedor.Clear();
            RefrescarProveedores();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0) return;
            string prov = dataGridView2.SelectedRows[0].Cells["Proveedor"].Value.ToString();
            _proveedores.RemoveAll(p => string.Equals(p, prov, StringComparison.OrdinalIgnoreCase));
            RefrescarProveedores();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var bajos = bllInsumos.ObtenerBajoMinimo();
            if (bajos.Count == 0)
            {
                MessageBox.Show("No hay insumos bajo mínimo.");
                return;
            }

            foreach (var (ins, _, sugerido) in bajos)
            {
                var ya = _detalle.FirstOrDefault(x => x.CodInsumo == ins.CodInsumo_750VR);
                if (ya != null) ya.Cantidad = Math.Max(ya.Cantidad, sugerido);
                else _detalle.Add(new ItemSolicitud
                {
                    CodInsumo = ins.CodInsumo_750VR,
                    Insumo = ins.nombre_750VR,
                    Cantidad = sugerido,
                    Unidad = ins.unidadMedida_750VR
                });
            }
            RefrescarDetalle();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (_detalle.Count == 0)
            {
                MessageBox.Show("Agregá al menos un insumo a la solicitud.");
                return;
            }
            if (_proveedores.Count == 0)
            {
                MessageBox.Show("Agregá al menos un proveedor.");
                return;
            }

            // Acá persiste la “Solicitud de Cotización” si ya tenés entidades/tablas.
            // Por ahora dejamos un mensaje y refresco de stock:
            MessageBox.Show("Solicitud de cotización generada. Podés exportarla o enviarla a los proveedores.");
            CargarStock();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _detalle.Clear();
            _proveedores.Clear();
            RefrescarDetalle();
            RefrescarProveedores();
        }

        private void button7_Click(object sender, EventArgs e) => Close();

        private void cmbcant_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
