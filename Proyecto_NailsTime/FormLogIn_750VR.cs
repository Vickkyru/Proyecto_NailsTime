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
        private Dictionary<string, int> intentosFallidosPorUsuario = new Dictionary<string, int>();

        private Form1_750VR formPrincipal;
        

        public FormLogIn_750VR(Form1_750VR principal)
        {
            InitializeComponent();
            formPrincipal = principal;

        }
        //private int intentosFallidos = 0;


        private void button1_Click(object sender, EventArgs e)
        {
            string login = txtuser.Text.Trim();
            string password = txtcontra.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Complete los campos.");
                return;
            }
            try
            {
                BEusuario_750VR usuario = bll.recuperarUsuario_750VR(login, password);

                if (!SessionManager_750VR.ObtenerInstancia.IniciarSesion_750VR(usuario))
                {
                    MessageBox.Show("Ya hay una sesión activa.");
                    this.Close();
                }

                formPrincipal.MostrarDatosUsuarioLogueado();
                //MessageBox.Show("Login exitoso.");
                if (intentosFallidosPorUsuario.ContainsKey(login))
                    intentosFallidosPorUsuario.Remove(login);
                formPrincipal.Actualizar();
                this.Close();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

                if (mensaje.Contains("Contraseña incorrecta"))
                {
                    if (!intentosFallidosPorUsuario.ContainsKey(login))
                        intentosFallidosPorUsuario[login] = 0;

                    intentosFallidosPorUsuario[login]++;

                    if (intentosFallidosPorUsuario[login] >= 3)
                    {
                        bll.BloquearUsuario_750VR(login);
                        MessageBox.Show("Cuenta bloqueada tras 3 intentos fallidos.");
                    }
                    else
                    {
                        MessageBox.Show($"Contraseña incorrecta. Intentos fallidos: {intentosFallidosPorUsuario[login]}");
                    }
                }
                else
                {
                    MessageBox.Show(mensaje);
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
