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

        private Form1_750VR formPrincipal;

        public FormLogIn_750VR(Form1_750VR principal)
        {
            InitializeComponent();
            formPrincipal = principal;
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

            try
            {
                BLLusuario_750VR bll = new BLLusuario_750VR();
                BEusuario_750VR usuario = bll.ObtenerUsuarioPorLogin_750VR(login); // No valida aún

                if (usuario == null)
                    throw new Exception("Usuario no encontrado");

                // Validación de contraseña
                Encriptador_750VR encriptador = new Encriptador_750VR();

                if (string.IsNullOrEmpty(usuario.salt_750VR))
                {
                    // Usuario de prueba sin salt
                    if (usuario.contraseña_750VR != password)
                        throw new Exception("Contraseña incorrecta (usuario prueba)");
                }
                else
                {
                    // Usuario real con hash + salt
                    string hashCalculado = encriptador.HashearConSalt_750VR(password, usuario.salt_750VR);

                    if (usuario.contraseña_750VR != hashCalculado)
                        throw new Exception("Contraseña incorrecta");
                }

                if (!usuario.activo_750VR)
                    throw new Exception("Usuario inactivo");

                if (usuario.bloqueado_750VR)
                    throw new Exception("Usuario bloqueado");

                intentosFallidos = 0;

                if (!SessionManager_750VR.ObtenerInstancia.IniciarSesion_750VR(usuario))
                {
                    MessageBox.Show("Ya hay una sesión activa.");
                    this.Close();
                }

                formPrincipal.MostrarDatosUsuarioLogueado();
                MessageBox.Show("Login exitoso.");
                this.Close();
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

                if (mensaje.Contains("Contraseña incorrecta"))
                {
                    intentosFallidos++;

                    if (intentosFallidos >= 3)
                    {
                        BLLusuario_750VR bll = new BLLusuario_750VR();
                        bll.BloquearUsuario_750VR(txtuser.Text.Trim());
                        MessageBox.Show("Cuenta bloqueada tras 3 intentos fallidos.");
                    }
                    else
                    {
                        MessageBox.Show($"{mensaje}. Intentos fallidos: {intentosFallidos}");
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
