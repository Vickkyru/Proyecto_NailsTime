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
        BLLusuario_750VR bll = new BLLusuario_750VR();

        public FormLogIn_750VR()
        {
            InitializeComponent();
        }
        private int intentosFallidos = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            string login = txtuser.Text.Trim();
            string password = txtcontra.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Complete los campos.");
                return;
            }

            BLLusuario_750VR bll = new BLLusuario_750VR();
            Resultado_VR750<Usuario_750VR> resultado = bll.Login(login, password);

            if (resultado.resultado)
            {
                intentosFallidos = 0; // ✅ se resetean

                if (!SessionManager_VR750.ObtenerInstancia.IniciarSesion(resultado.entidad))
                {
                    MessageBox.Show("Ya hay una sesión activa.");
                    return;
                }

                MessageBox.Show("Login exitoso.");
                this.Close();
            }
            else
            {
                intentosFallidos++;

                if (intentosFallidos >= 3)
                {
                    bll.BloquearUsuario(login);
                    MessageBox.Show("Cuenta bloqueada tras 3 intentos fallidos.");
                }
                else
                {
                    MessageBox.Show(resultado.mensaje + $" Intentos fallidos: {intentosFallidos}");
                }
            }

        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormLogIn_750VR_Load(object sender, EventArgs e)
        {

        }

        private void txtcontra_TextChanged(object sender, EventArgs e)
        {
            txtcontra.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtcontra.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}
