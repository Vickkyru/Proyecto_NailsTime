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
    public partial class FormReparacionDV_750VR : Form
    {
        public FormReparacionDV_750VR()
        {
            InitializeComponent();
        }

        private void btnrec_Click(object sender, EventArgs e)
        {
            try
            {
                DVService_750VR.RecalcularYGuardarDV_BD();
                MessageBox.Show("DV recalculado y guardado. Volverás al Login.",
                                "Reparación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // vuelve al Login
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al recalcular: " + ex.Message,
                                "Reparación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnres_Click(object sender, EventArgs e)
        {
            try
            {
                using (var f = new FormBackupRestore_750VR())
                {
                    f.ShowDialog(this);
                }
                MessageBox.Show("Restore finalizado. Volverás al Login.",
                                "Reparación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en restore: " + ex.Message,
                                "Reparación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsali_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
