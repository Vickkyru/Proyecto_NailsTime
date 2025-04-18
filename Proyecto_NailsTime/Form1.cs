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
    public partial class Form1 : Form
    {
        private static Form formactivo = null;

        public Form1()
        {
            InitializeComponent();

        }

        private void AbrirForm(Form formu)
        {
            if (formactivo != null)
            {
                formactivo.Close();
            }
            formactivo = formu;
            formu.TopLevel = false;
            formu.FormBorderStyle = FormBorderStyle.None;
            formu.Dock = DockStyle.Fill;

            this.Controls.Add(formu);
            formu.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {


        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormGestionUsuario_750VR());
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormLogIn_750VR());
        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormCambiarClave_750VR());
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormLogOut_750VR());
        }

        private void administradorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void verTurnosDisponiblesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
