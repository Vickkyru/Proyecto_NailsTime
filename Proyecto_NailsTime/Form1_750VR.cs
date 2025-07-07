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
    public partial class FormPrincipal : Form, Iobserver_750VR
    {
        private Form formActivo = null;

        BaseDeDatos_750VR db = new BaseDeDatos_750VR();


        public FormPrincipal()
        {
            InitializeComponent();
            db.VerificarOCrearBaseDeDatos();
            db.VerificarYCrearTablaUsuarios_750VR();
            db.InsertarServiciosIniciales();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            Lenguaje_750VR.ObtenerInstancia().IdiomaActual = "Español";

        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }


        private void AbrirForm(Form nuevoForm)
        {

            if (formActivo != null && formActivo.GetType() == nuevoForm.GetType())
                return;


            if (formActivo != null)
            {
                if (!formActivo.IsDisposed)
                {
                    this.Controls.Remove(formActivo);
                    formActivo.Dispose();
                }

                formActivo = null;
            }


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
            AbrirForm(new FormGestionUsuario());
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AbrirForm(new FormLogIn_750VR());

            FormLogIn login = new FormLogIn(this);
            AbrirForm(login);

        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormCambiarClave());
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormLogOut());
        }

        private void administradorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        public void Actualizar()
        {

            if (!SessionManager_750VR.ObtenerInstancia.EstaLogueado_750VR())
            {
                BloquearTodo();
                return;
            }


            string rol = SessionManager_750VR.ObtenerInstancia.user.rol_750VR.ToLower();




            //switch (rol)
            //{
            //    case "manicurista":
            //        administradorToolStripMenuItem.Enabled = false;
            //        maestrosToolStripMenuItem.Enabled = false;
            //        usuarioToolStripMenuItem.Enabled = true;
            //        reservaToolStripMenuItem.Enabled = true;
            //        insumosToolStripMenuItem.Enabled = false;
            //        reportesToolStripMenuItem.Enabled = false;
            //        regReservaToolStripMenuItem.Enabled = false;
            //        cambiarClaveToolStripMenuItem.Enabled = true;
            //        cambiarIdiomaToolStripMenuItem.Enabled = true;
            //        break;

            //    case "recepcionista":
            //        administradorToolStripMenuItem.Enabled = false;
            //        maestrosToolStripMenuItem.Enabled = false;
            //        usuarioToolStripMenuItem.Enabled = true;
            //        reservaToolStripMenuItem.Enabled = true;
            //        insumosToolStripMenuItem.Enabled = false;
            //        reportesToolStripMenuItem.Enabled = false;
            //        actAgendaToolStripMenuItem.Enabled = false;
            //        cambiarClaveToolStripMenuItem.Enabled = true;
            //        cambiarIdiomaToolStripMenuItem.Enabled = true;
            //        break;

            //    case "administrador":
            //        administradorToolStripMenuItem.Enabled = true;
            //        maestrosToolStripMenuItem.Enabled = true;
            //        usuarioToolStripMenuItem.Enabled = true;
            //        reservaToolStripMenuItem.Enabled = true;
            //        insumosToolStripMenuItem.Enabled = true;
            //        reportesToolStripMenuItem.Enabled = true;
            //        cambiarClaveToolStripMenuItem.Enabled = true;
            //        cambiarIdiomaToolStripMenuItem.Enabled = true;
            //        break;

            //    default:
            //        BloquearTodo();
            //        break;
        }

    


        private void Form1_Load(object sender, EventArgs e)
        {
            //AplicarPermisos();
            ActualizarLabels();
            Actualizar();
          

        }

        public void AplicarPermisos()
        {
            var permisos = SessionManager_750VR.ObtenerInstancia.PermisosDelUsuario;

            MessageBox.Show("Permisos cargados:\n" + string.Join("\n", SessionManager_750VR.ObtenerInstancia.PermisosDelUsuario));


            GestorPermisos_750VR.AplicarPermisosAlMenuCompleto(menuStrip1, permisos);
        }


        private void BloquearTodo()
        {
            pestañaAdmin.Enabled = false;
            pestañaMaestros.Enabled = false;
            pestañaUsuarios.Enabled = true;
            inicioSesion.Enabled = true;
            cerrarSesion.Enabled = true;
            pestañaReserva.Enabled = false;
            pestañaInsumos.Enabled = false;
            pestañaReportes.Enabled = false;
            cambiarClave.Enabled = false;
            cambiarIdioma.Enabled = false;
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
            AbrirForm(new FormRegistrarReserva());
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
            AbrirForm(new FormABMClientes());
        }

        private void serviciosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormABMservicios());
        }

        private void personalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormABMdisponibilidad());
        }

        private void verTurnosReservadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormActualizarAgenda());
        }

        private void cambiarIdiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormCambioIdioma());
        }

        private void insumosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormABMinsumos());
        }

        private void facturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormFactura());
        }

        private void gestionDePerfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormCrearPerfiles());
        }
    }
}
