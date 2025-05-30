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
    public partial class FormRegistrarReserva_750VR : Form
    {
        public FormRegistrarReserva_750VR()
        {
            InitializeComponent();
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
            FormABMClientes_750VR formCrearCliente = new FormABMClientes_750VR();
            formCrearCliente.StartPosition = FormStartPosition.CenterScreen;
            formCrearCliente.ShowDialog(); // bloquea el formulario actual hasta que se cierre el otro
        }
    }
}
