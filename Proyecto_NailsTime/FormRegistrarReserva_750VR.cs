using BE_VR750;
using BLL_VR750;
using DAL_VR750;
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
    public partial class FormRegistrarReserva_750VR : Form
    {
        private List<BEServicio_750VR> listaServicios;
       
        public FormRegistrarReserva_750VR()
        {
            InitializeComponent();
        }

        private void CargarServicios()
        {
            BLLServicio_750VR bllServicio = new BLLServicio_750VR();
            listaServicios = bllServicio.leerEntidadesActivas_750VR_750VR(); // Te lo muestro abajo

            var nombresServicio = listaServicios
                .Select(s => s.nombre_750VR)
                .Distinct()
                .ToList();


            nombresServicio.Insert(0, ""); // ← esto agrega el valor vacío al principio

            cmbserv.DataSource = nombresServicio;
            cmbserv.SelectedIndex = 0; // ← se muestra en blanco
        }

        private void CargarDisponibilidad()
        {
            BLLdisponibilidad_750VR bll = new BLLdisponibilidad_750VR();
            var lista = bll.leerDisponibilidadesActivas_750VR();
            dataGridView1.DataSource = lista;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        //boton buscar cliente
        private void button3_Click(object sender, EventArgs e)
        {
            string dni = txtdni.Text;
            var bllcliente = new BLLCliente_750VR();
            var cliente = bllcliente.ObtenerClientePorDNI_750VR(Convert.ToInt32(dni));

            if (cliente != null)
            {
                txtnom.Text = cliente.nombre_750VR;
            }
            else
            {
                MessageBox.Show("Cliente no encontrado. Puede crearlo desde el botón correspondiente.");
            }
        }


        //crea cliente
        private void button5_Click(object sender, EventArgs e)
        {

            FormABMClientes_750VR frm = new FormABMClientes_750VR();
            frm.InvocadoDesdeReserva = true;
            frm.FormularioReserva = this;
            frm.ShowDialog(); 
        }
        public void CompletarCamposCliente(string dni, string nombre)
        {
            txtdni.Text = dni;
            txtnom.Text = nombre;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormCobrarServicio_750VR frm = new FormCobrarServicio_750VR();
            frm.ShowDialog();
        }

        private void cmbserv_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombre = cmbserv.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(nombre)) return;

            var tecnicas = listaServicios
                .Where(s => s.nombre_750VR == nombre)
                .ToList();

            // Insertar un objeto vacío al principio
            tecnicas.Insert(0, new BEServicio_750VR { tecnica_750VR = "" });

            cmbtec.DataSource = tecnicas;
            cmbtec.DisplayMember = "tecnica_750VR";
            cmbtec.SelectedIndex = 0;
        }

        private void txttec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtec.SelectedItem is BEServicio_750VR servicioSeleccionado)
            {
                txtpre.Text = servicioSeleccionado.precio_750VR.ToString("C"); // con símbolo moneda
                txthorest.Text = servicioSeleccionado.duracion_750VR + " min";
            }
       
        }

        private void FormRegistrarReserva_750VR_Load(object sender, EventArgs e)
        {
            CargarServicios();
            CargarDisponibilidad();
        }
    }
}
