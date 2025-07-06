using BE_VR750;
using BLL_VR750;
using DAL_VR750;
using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Proyecto_NailsTime
{
    public partial class FormCobrarServici : Form, Iobserver_750VR
    {
        private int idReserva; 
        public FormCobrarServici(int idReservaRecibido)
        {
            InitializeComponent();
            idReserva = idReservaRecibido;

            
            CargarDatosReserva();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }
     
        private void CargarDatosReserva()
        {
            BLLReserva_750VR bll = new BLLReserva_750VR();
            var reserva = bll.ObtenerReservaPorId(idReserva); 

            if (reserva != null)
            {
                lblimp.Text = $"${reserva.Precio_750VR}";
            }
        }

        private void FormCobrarServicio_750VR_Load(object sender, EventArgs e)
        {
            cmbmet.Items.AddRange(new string[] { "Efectivo", "Débito", "Crédito" });
            cmbmet.SelectedIndex = 0;

            cmbmet.SelectedIndexChanged += cmbmet_SelectedIndexChanged;

            txtnum.Enabled = false;
            txtcuot.Enabled = false;
        }

        private void cmbmet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbmet.SelectedItem == null) return;  

            string metodo = cmbmet.SelectedItem.ToString();

            switch (metodo)
            {
                case "Efectivo":
                    txtnum.Enabled = false;
                    txtcuot.Enabled = false;
                    txtvenc.Enabled = false;
                    txtcvc.Enabled = false;
                    textBox1.Enabled = false;
                    break;

                case "Débito":
                    txtnum.Enabled = true;
                    txtcuot.Enabled = false;
                    txtvenc.Enabled = true;
                    txtcvc.Enabled = true;
                    textBox1.Enabled = true;
                    break;

                case "Crédito":
                    txtnum.Enabled = true;
                    txtcuot.Enabled = true;
                    txtvenc.Enabled = true;
                    txtcvc.Enabled = true;
                    textBox1.Enabled = false;
                    break;
            }

            txtnum.Clear();
            txtcuot.Clear();
        }

        private void btnrealiz_Click(object sender, EventArgs e)
        {
            string metodo = cmbmet.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(metodo))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeSeleccionMetodo"));
                return;
            }

            if (metodo == "Débito" || metodo == "Crédito")
            {
                if (string.IsNullOrWhiteSpace(txtnum.Text) || !Regex.IsMatch(txtnum.Text, @"^\d{13,19}$"))
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeTarjetaInvalida"));
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtcvc.Text) || !Regex.IsMatch(txtcvc.Text, @"^\d{3,4}$"))
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeCVCInvalido"));
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtvenc.Text) || !Regex.IsMatch(txtvenc.Text, @"^(0[1-9]|1[0-2])\/(\d{2}|\d{4})$"))
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeVencimientoInvalido"));
                    return;
                }

                try
                {
                    string[] partes = txtvenc.Text.Split('/');
                    int mes = int.Parse(partes[0]);
                    int año = partes[1].Length == 2 ? 2000 + int.Parse(partes[1]) : int.Parse(partes[1]);

                    DateTime fechaVenc = new DateTime(año, mes, 1).AddMonths(1).AddDays(-1);
                    if (fechaVenc < DateTime.Today)
                    {
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeTarjetaVencida"));
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeVencimientoInvalido"));
                    return;
                }

                // Validar nombre del titular si corresponde (para ambos métodos si visible)
                if (textBox1.Enabled)
                {
                    if (string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeTitularRequerido"));
                        return;
                    }

                    if (!Regex.IsMatch(textBox1.Text, @"^[A-Za-zÁÉÍÓÚÑáéíóúñ\s]+$"))
                    {
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeTitularInvalido"));
                        return;
                    }
                }
            }

            if (metodo == "Crédito")
            {
                if (string.IsNullOrWhiteSpace(txtcuot.Text) || !int.TryParse(txtcuot.Text, out int cuotas) || cuotas <= 0)
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeCuotasInvalidas"));
                    return;
                }
            }

            string textoMonto = lblimp.Text.Replace("$", "").Trim();
            if (!decimal.TryParse(textoMonto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal total))
            {
                MessageBox.Show("Error al interpretar el monto total.");
                return;
            }

            BLLReserva_750VR bll = new BLLReserva_750VR();
            bool exito = bll.MarcarComoCobrado(idReserva);

            if (!exito)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeError"));
                return;
            }

            var nuevaFactura = new BEfactura_750VR(
                codReserva: idReserva,
                fecha: DateTime.Now.Date,
                hora: DateTime.Now.TimeOfDay,
                total: total,
                metodoPago: metodo,
                titular: textBox1.Text
            );

            BLLfactura_750VR bllFactura = new BLLfactura_750VR();
            bool exitoFactura = bllFactura.GenerarFactura(nuevaFactura);

            if (exitoFactura)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.MensajeExito"));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al guardar la factura.");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            cmbmet.SelectedIndex = -1;
            txtcuot.Clear();
            txtnum.Clear();
            textBox1.Clear();
            txtcvc.Clear();
            txtvenc.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormCobrarServicio_750VR.PendienteCobro"));
            MessageBox.Show("la reserva quedo pendiente de cobro.");
            this.Close();

           

        }
    }
}
