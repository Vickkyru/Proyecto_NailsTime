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
    public partial class FormLogOut_750VR : Form
    {
        public FormLogOut_750VR()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SERVICIOS_VR750.SessionManager_VR750.ObtenerInstancia.CerrarSesion();
                Application.Restart(); // reinicia la app para forzar el login de nuevo
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cerrar sesión");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
