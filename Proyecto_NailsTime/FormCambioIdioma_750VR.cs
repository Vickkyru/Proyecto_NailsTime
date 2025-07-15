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
    public partial class FormCambioIdioma : Form, Iobserver_750VR
    {
        public FormCambioIdioma()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
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
            
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormCambioIdioma_750VR.MensajeDebeSeleccionarIdioma"),
                    "Idioma",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }


            SessionManager_750VR.ObtenerInstancia.IdiomaActual = comboBox1.SelectedItem.ToString();



            MessageBox.Show(
                Lenguaje_750VR.ObtenerEtiqueta("FormCambioIdioma_750VR.MensajeIdiomaActualizado"),
                "Idioma",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        

        private void FormCambioIdioma_750VR_Load(object sender, EventArgs e)
        {
            //comboBox1.Items.Clear();

            //comboBox1.SelectedIndex = 0; // idioma por defecto
        }
    }
}
