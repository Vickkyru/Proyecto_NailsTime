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
    public partial class Form1_750VR : Form, Iobserver_750VR
    {
        private Form formActivo = null;

        BaseDeDatos_750VR db = new BaseDeDatos_750VR();


        public Form1_750VR()
        {
            InitializeComponent();
            //db.VerificarOCrearBaseDeDatos();
            //db.VerificarYCrearTablaUsuarios_750VR();
            //db.InsertarServiciosIniciales();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            Lenguaje_750VR.ObtenerInstancia().IdiomaActual = "Español";

        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }


        private void AbrirForm(Form nuevoForm)
        {
            // Si ya está abierto el mismo tipo de formulario, no hacemos nada
            if (formActivo != null && formActivo.GetType() == nuevoForm.GetType())
                return;

            // Cerrar y eliminar el anterior si existe
            if (formActivo != null)
            {
                if (!formActivo.IsDisposed)
                {
                    this.Controls.Remove(formActivo);
                    formActivo.Dispose();   // Cierra
                }

                formActivo = null; // Limpia referencia
            }

            // Configurar el nuevo formulario
            formActivo = nuevoForm;
            nuevoForm.TopLevel = false;
            nuevoForm.FormBorderStyle = FormBorderStyle.None;
            nuevoForm.Dock = DockStyle.Fill;

            this.Controls.Add(nuevoForm);
            nuevoForm.Show();
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
        public void Actualizar()
        {
            // Primero verificamos si hay sesión iniciada
            if (!SessionManager_750VR.ObtenerInstancia.EstaLogueado_750VR())
            {
                BloquearTodo();
                return;
            }

            // Ya sabemos que user no es null
            string rol = SessionManager_750VR.ObtenerInstancia.user.rol_750VR.ToLower();
            //MessageBox.Show("Rol detectado: " + rol);



            switch (rol)
            {
                case "manicurista":
                    administradorToolStripMenuItem.Enabled = false;
                    maestrosToolStripMenuItem.Enabled = false;
                    usuarioToolStripMenuItem.Enabled = true;
                    reservaToolStripMenuItem.Enabled = true;
                    insumosToolStripMenuItem.Enabled = false;
                    reportesToolStripMenuItem.Enabled = false;
                    regReservaToolStripMenuItem.Enabled = false;
                    break;

                case "recepcionista":
                    administradorToolStripMenuItem.Enabled = false;
                    maestrosToolStripMenuItem.Enabled = false;
                    usuarioToolStripMenuItem.Enabled = true;
                    reservaToolStripMenuItem.Enabled = true;
                    insumosToolStripMenuItem.Enabled = false;
                    reportesToolStripMenuItem.Enabled = false;
                    actAgendaToolStripMenuItem.Enabled = false;
                    break;

                case "administrador":
                    administradorToolStripMenuItem.Enabled = true;
                    maestrosToolStripMenuItem.Enabled = true;
                    usuarioToolStripMenuItem.Enabled = true;
                    reservaToolStripMenuItem.Enabled = true;
                    insumosToolStripMenuItem.Enabled = true;
                    reportesToolStripMenuItem.Enabled = true;
                    break;

                default:
                    BloquearTodo();
                    break;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ActualizarLabels();
            Actualizar();
          

        }
        private void BloquearTodo()
        {
            administradorToolStripMenuItem.Enabled = false;
            maestrosToolStripMenuItem.Enabled = false;
            usuarioToolStripMenuItem.Enabled = true;
            reservaToolStripMenuItem.Enabled = false;
            insumosToolStripMenuItem.Enabled = false;
            reportesToolStripMenuItem.Enabled = false;
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
            AbrirForm(new FormRegistrarReserva_750VR());
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

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormABMClientes_750VR());
        }

        private void serviciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormABMservicios_750VR());
        }

        private void personalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormABMdisponibilidad());
        }

        private void verTurnosReservadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormActualizarAgenda_750VR());
        }

        private void cambiarIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormCambioIdioma_750VR());
        }
    }
}
