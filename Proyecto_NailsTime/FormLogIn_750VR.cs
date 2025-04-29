using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            try
            {
                // Validar campos
                if (string.IsNullOrWhiteSpace(txtuser.Text) || string.IsNullOrWhiteSpace(txtcontra.Text))
                {
                    MessageBox.Show("Debe completar Usuario y Contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string usuarioLogin = txtuser.Text.Trim();
                string contraseñaIngresada = txtcontra.Text.Trim();

                //BLLusuario_750VR usuarioBLL = new BLLusuario_750VR();
                string resultadoLogin = usuarioBLL.Login(usuarioLogin, contraseñaIngresada);

                if (resultadoLogin == "Login exitoso")
                {
                    MessageBox.Show("¡Bienvenido/a!", "Login Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Abrir Form Principal
                    this.Hide();
                    //Form1 formPrincipal = new form();
                    //formPrincipal.Show();
                }
                else
                {
                    MessageBox.Show(resultadoLogin, "Error de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en el login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
