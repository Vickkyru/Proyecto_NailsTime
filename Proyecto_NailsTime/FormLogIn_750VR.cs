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
            string usuarioLogin = txtuser.Text.Trim();
            string contraseña = txtcontra.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuarioLogin) || string.IsNullOrWhiteSpace(contraseña))
            {
                MessageBox.Show("Debe completar todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Usamos BLL
                BLLusuario_750VR usuarioBLL = new BLLusuario_750VR();
                var resultado = usuarioBLL.recuperarUsuario(usuarioLogin, contraseña);

                if (resultado.resultado) // Login exitoso
                {
                    SERVICIOS_VR750.SessionManager_VR750.Instancia
                        .IniciarSesion(resultado.entidad);

                    MessageBox.Show("Bienvenido/a", "Inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                else
                {
                    MessageBox.Show(resultado.mensaje, "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
