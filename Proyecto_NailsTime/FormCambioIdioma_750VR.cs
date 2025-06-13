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
    public partial class FormCambioIdioma_750VR : Form, Iobserver_750VR
    {
        public FormCambioIdioma_750VR()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string idiomaSeleccionado = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(idiomaSeleccionado))
            {
                MessageBox.Show("Por favor seleccioná un idioma.");
                return;
            }

            // Setear el idioma y notificar a todos los formularios suscriptos
            Lenguaje_750VR.ObtenerInstancia().IdiomaActual = idiomaSeleccionado;

            // Mostrar confirmación o cerrar formulario
            MessageBox.Show("Idioma cambiado correctamente.");
            this.Close(); // o dejalo abierto si querés
        }

        private void FormCambioIdioma_750VR_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            //comboBox1.SelectedIndex = 0; // idioma por defecto
        }
    }
}
