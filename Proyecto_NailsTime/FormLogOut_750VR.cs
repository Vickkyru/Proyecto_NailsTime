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
    public partial class FormLogOut_750VR : Form, Iobserver_750VR
    {
        public FormLogOut_750VR()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
        }

        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SERVICIOS_VR750.SessionManager_750VR.ObtenerInstancia.CerrarSesion_750VR();
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
