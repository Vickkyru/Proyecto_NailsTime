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
    public partial class FormCobrarServicio_750VR : Form, Iobserver_750VR
    {
        private int idReserva; // este campo almacena el ID recibido
        public FormCobrarServicio_750VR(int idReservaRecibido)
        {
            InitializeComponent();
            idReserva = idReservaRecibido;

            // Podés cargar la reserva si querés mostrar el importe:
            CargarDatosReserva();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private void CargarDatosReserva()
        {
            BLLReserva_750VR bll = new BLLReserva_750VR();
            var reserva = bll.ObtenerReservaPorId(idReserva); // tenés que tener este método

            if (reserva != null)
            {
                lblimp.Text = $"${reserva.Precio_750VR}";
            }
        }

        private void FormCobrarServicio_750VR_Load(object sender, EventArgs e)
        {
            cmbmet.Items.AddRange(new string[] { "Efectivo", "Débito", "Crédito" });
            cmbmet.SelectedIndex = 0; // predeterminado

            txtnum.Enabled = false;
            txtcuot.Enabled = false;
        }

        private void cmbmet_SelectedIndexChanged(object sender, EventArgs e)
        {
            string metodo = cmbmet.SelectedItem.ToString();

            switch (metodo)
            {
                case "Efectivo":
                    txtnum.Enabled = false;
                    txtcuot.Enabled = false;
                    txtvenc.Enabled = false;
                    txtcvc.Enabled = false;
                    break;

                case "Débito":
                    txtnum.Enabled = true;
                    txtcuot.Enabled = false;
                    txtvenc.Enabled = true;
                    txtcvc.Enabled = true;
                    break;

                case "Crédito":
                    txtnum.Enabled = true;
                    txtcuot.Enabled = true;
                    txtvenc.Enabled = true;
                    txtcvc.Enabled = true;
                    break;
            }

            txtnum.Clear();
            txtcuot.Clear();
        }

        private void btnrealiz_Click(object sender, EventArgs e)
        {
            // Validación simple (podés mejorarla)
            if (cmbmet.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un método de pago.");
                return;
            }

            if ((cmbmet.Text == "Débito" || cmbmet.Text == "Crédito") && string.IsNullOrWhiteSpace(txtnum.Text))
            {
                MessageBox.Show("Ingresá el número de tarjeta.");
                return;
            }

            if ((txtcvc.Text == "Débito" || cmbmet.Text == "Crédito") && string.IsNullOrWhiteSpace(txtnum.Text))
            {
                MessageBox.Show("Ingresá el número de cvc.");
                return;
            }

            if ((txtvenc.Text == "Débito" || cmbmet.Text == "Crédito") && string.IsNullOrWhiteSpace(txtnum.Text))
            {
                MessageBox.Show("Ingresá el número de vencimiento.");
                return;
            }

            if (cmbmet.Text == "Crédito" && string.IsNullOrWhiteSpace(txtcuot.Text))
            {
                MessageBox.Show("Ingresá la cantidad de cuotas.");
                return;
            }
            MessageBox.Show("ID que llega para cobrar: " + idReserva);
            // Marcar como cobrada
            BLLReserva_750VR bll = new BLLReserva_750VR();
            bool exito = bll.MarcarComoCobrado(idReserva); // método que ya deberías tener

            if (exito)
            {
                MessageBox.Show("Pago registrado correctamente.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al registrar el pago.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cmbmet.SelectedIndex = -1;
            txtcuot.Clear();
            txtnum.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; // o Cancel según corresponda
            this.Close();
        }
    }
}
