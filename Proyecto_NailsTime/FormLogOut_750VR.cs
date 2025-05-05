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
    public partial class FormLogOut_750VR : Form
    {
        public FormLogOut_750VR()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SERVICIOS_VR750.SessionManager_VR750.ObtenerInstancia().CerrarSesion();
            MessageBox.Show("Sesión cerrada correctamente.");

            // Cerrar formulario principal y volver al Login
            
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
