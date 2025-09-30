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


        }

    


        private void Form1_Load(object sender, EventArgs e)
        {
            MostrarDatosUsuarioLogueado(); 
            Actualizar(); 


        }

 
        public void AplicarPermisos()
        {
            var permisos = SessionManager_750VR.ObtenerInstancia.PermisosDelUsuario ?? new List<string>();
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
                string mensaje = string.Format(
                    Lenguaje_750VR.ObtenerEtiqueta("FormPrincipal.MensajeUsuario"),
                    usuario.nombre_750VR,
                    usuario.rol_750VR
                );

                label1.Text = mensaje;
                label1.Visible = true; // Lo muestro
            }
            else
            {
                label1.Text = "";
                label1.Visible = false; // Lo oculto
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

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormBitacora_750VR());
        }

        private void Respaldos_Click(object sender, EventArgs e)
        {
            AbrirForm(new FormBackupRestore_750VR());
        }
    }
}
