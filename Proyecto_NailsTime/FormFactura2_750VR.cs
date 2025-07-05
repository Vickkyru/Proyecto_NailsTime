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
    public partial class FormFactura2_750VR : Form
    {
        private List<BEfactura_750VR> listaFacturas;
        public FormFactura2_750VR()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccioná una factura.");
                return;
            }

            int codFactura = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CodFactura_750VR"].Value);
            BEfactura_750VR factura = listaFacturas.Find(f => f.CodFactura_750VR == codFactura);

            // Generar el PDF
            Archivo_750VR.GenerarFacturaPDF(factura);
            MessageBox.Show("Factura generada correctamente.");
        }

        private void FormFactura2_750VR_Load(object sender, EventArgs e)
        {
            CargarFacturas();
        }

        private void CargarFacturas()
        {
            BLLfactura_750VR bll = new BLLfactura_750VR();
            listaFacturas = bll.ObtenerFacturas();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaFacturas;

            // Cambiar encabezados del DataGridView
            dataGridView1.Columns["CodFactura_750VR"].HeaderText = "Código";
            dataGridView1.Columns["CodReserva_750VR"].HeaderText = "Reserva";
            dataGridView1.Columns["fecha_750VR"].HeaderText = "Fecha";
            dataGridView1.Columns["horaEmision_750VR"].HeaderText = "Hora";
            dataGridView1.Columns["total_750VR"].HeaderText = "Total";
            dataGridView1.Columns["metodoPago_750VR"].HeaderText = "Método";
            dataGridView1.Columns["titular_750VR"].HeaderText = "Titular";
        }
    }
}
