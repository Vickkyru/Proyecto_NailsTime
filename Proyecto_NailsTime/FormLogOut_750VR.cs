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
    public partial class FormLogOut : Form, Iobserver_750VR
    {
        public FormLogOut()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
        }

        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SessionManager_750VR.ObtenerInstancia.EstaLogueado_750VR())
            {
                try
                {
                    var idiomaFinal = SessionManager_750VR.IdiomaActual;
                    string login = SessionManager_750VR.ObtenerInstancia.user.user_750VR;

                    // Guardar idioma en BD
                    var bllUsuario = new BLLusuario_750VR();
                    bllUsuario.ModificarIdiomaUsuario_750VR(login, idiomaFinal);

                    // 👉 ahora hacemos logout con bitácora
                    bllUsuario.Logout_750VR();

                    Application.Restart(); // opcional
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cerrar sesión: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No hay sesión activa para cerrar.");
            }
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormLogOut_750VR_Load(object sender, EventArgs e)
        {

        }
    }
}
