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
        private BLLusuario_750VR usuarioBLL = new BLLusuario_750VR();

        public FormLogIn_750VR()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = textBox1.Text.Trim();
            string contraseña = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
            {
                MessageBox.Show("Debe completar usuario y contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string resultadoLogin = usuarioBLL.(usuario, contraseña);

            if (resultadoLogin == "Login exitoso")
            {
                this.Hide(); // Ocultamos el login
                Form1 formPrincipal = new Form1();
                formPrincipal.Show();
            }
            else
            {
                MessageBox.Show(resultadoLogin, "Error de Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Clear();
                textBox2.Focus();
            }
        }
    }
}
