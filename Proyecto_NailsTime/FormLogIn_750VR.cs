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

        private void button1_Click(object sender, EventArgs e)
        {
            BLLusuario_750VR bll = new BLLusuario_750VR();

            if (string.IsNullOrWhiteSpace(txtuser.Text) || string.IsNullOrWhiteSpace(txtcontra.Text))
            {
                MessageBox.Show("Complete los campos.");
                return;
            }

            // Validar login con BLL
            string resultado = bll.Login(txtuser.Text.Trim(), txtcontra.Text.Trim());

            if (resultado == "Login exitoso.")
            {
                MessageBox.Show(resultado);
                this.Close(); // cerrar login si fue exitoso
            }
            else
            {
                MessageBox.Show(resultado);
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
