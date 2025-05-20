using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using System.Windows.Forms;
using SERVICIOS_VR750;
using DAL_VR750;


namespace Proyecto_NailsTime
{
    public partial class Form1_750VR : Form
    {
        private static Form formactivo = null;

        BaseDeDatos_750VR db = new BaseDeDatos_750VR();
        public Form1_750VR()
        {
            InitializeComponent();
            db.VerificarOCrearBaseDeDatos();
            db.VerificarYCrearTablaUsuarios_750VR();

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
            //AbrirForm(new FormLogIn_750VR());

            FormLogIn_750VR login = new FormLogIn_750VR(this); // PASÁS EL PRINCIPAL
            AbrirForm(login);

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
            ActualizarLabels();
            //DeshabilitarMenus();
            //MostrarDatosUsuario();

            //.VerificarYCrearTablaUsuarios();
        }

        private void DeshabilitarMenus()
        {
            administradorToolStripMenuItem.Enabled = false;
            maestrosToolStripMenuItem.Enabled = false;
            reservaToolStripMenuItem.Enabled = false;
            insumosToolStripMenuItem.Enabled = false;
            reportesToolStripMenuItem.Enabled = false;
            ayudaToolStripMenuItem.Enabled = false;
            logoutToolStripMenuItem.Enabled = false;

            loginToolStripMenuItem.Enabled = true;
            usuariosToolStripMenuItem.Enabled = true;
        }

    
        public void ActualizarLabels()
        {
            var usuario = SessionManager_750VR.ObtenerInstancia.user;

            if (usuario != null)
            {
                lblbienvenido.Text = usuario.nombre_750VR;
                lblrol.Text = usuario.rol_750VR;
            }
        }
        private void verTurnosDisponiblesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        public void MostrarDatosUsuarioLogueado()
        {
            var usuario = SessionManager_750VR.ObtenerInstancia.user;

            if (usuario != null)
            {
                lblbienvenido.Text = $"{usuario.nombre_750VR}";
                lblrol.Text = $"{usuario.rol_750VR}";
            }
            else
            {
                lblbienvenido.Text = "";
                lblrol.Text = "";
            }
        }
    }
}
