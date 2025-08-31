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
    public partial class FormFactura : Form, Iobserver_750VR
    {
        private readonly BLLfactura_750VR _bll = new BLLfactura_750VR();
        private List<BEfactura_750VR> listaFacturas;

        public FormFactura()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
        }

        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
            this.Text = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Titulo");
            button1.Text = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.btnGenerar");

            dataGridView1.Columns["CodFactura_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Codigo");
            dataGridView1.Columns["CodReserva_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Reserva");
            dataGridView1.Columns["fecha_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Fecha");
            dataGridView1.Columns["horaEmision_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Hora");
            dataGridView1.Columns["total_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Total");
            dataGridView1.Columns["metodoPago_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.MetodoPago");
            dataGridView1.Columns["titular_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormFactura.Titular");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormFactura.MensajeSeleccion"));
                return;
            }

            try
            {
                int codFactura = Convert.ToInt32(
                    dataGridView1.SelectedRows[0].Cells["CodFactura_750VR"].Value
                );

                var factura = listaFacturas?.Find(f => f.CodFactura_750VR == codFactura);
                if (factura == null)
                {
                    MessageBox.Show("No se encontró la factura seleccionada.");
                    return;
                }

                // 1) Generar/mostrar PDF (tu lógica actual)
                Archivo_750VR.GenerarFacturaPDF(factura);

                // 2) Registrar en bitácora la impresión (Criticidad 4) desde la BLL
                _bll.ImprimirFactura(codFactura);

                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormFactura.MensajeExito"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al imprimir la factura: " + ex.Message);
            }
        }

        private void FormFactura2_750VR_Load(object sender, EventArgs e)
        {
            CargarFacturas();
            ActualizarIdioma();
        }

        private void CargarFacturas()
        {
            listaFacturas = _bll.ObtenerFacturas();
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaFacturas;

            // formatos
            if (dataGridView1.Columns.Contains("horaEmision_750VR"))
                dataGridView1.Columns["horaEmision_750VR"].DefaultCellStyle.Format = @"hh\:mm";
            if (dataGridView1.Columns.Contains("total_750VR"))
                dataGridView1.Columns["total_750VR"].DefaultCellStyle.Format = "N2";
        }
    }
}
