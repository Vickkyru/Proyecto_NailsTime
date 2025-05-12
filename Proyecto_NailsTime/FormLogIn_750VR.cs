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
using BE_VR750;
using BLL_VR750;
using SERVICIOS_VR750;


namespace Proyecto_NailsTime
{
    public partial class FormLogIn_750VR : Form
    {
        public BLLusuario_750VR usuarioBLL = new BLLusuario_750VR();

        public FormLogIn_750VR()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            BLLusuario_750VR bll = new BLLusuario_750VR();
            if (txtuser.Text == "" || txtcontra.Text == "")
            {
                MessageBox.Show("Complete los campos");
                return;
            }

            if (SessionManager_VR750.ObtenerInstancia.UsuarioActual != null)
            {
                MessageBox.Show("Ya hay una sesión activa.");
                this.Close();
                return;
            }

            string resultado = bll.Login(txtuser.Text.Trim(), txtcontra.Text.Trim());

            MessageBox.Show(resultado);

            if (resultado == "Login exitoso.")
            {
               this.Close();
            }
            //this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormLogIn_750VR_Load(object sender, EventArgs e)
        {

        }
    }
}
