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
    public partial class FormCambiarClave_750VR : Form, Iobserver_750VR
    {
        public FormCambiarClave_750VR()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);

        }

        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }
        Encriptador_750VR encriptador = new Encriptador_750VR();


        private void FormCambiarClave_750VR_Load(object sender, EventArgs e)
        {

        }

        //hacer lo de los intentos 

        private void button1_Click(object sender, EventArgs e)
        {
            var usuario = SessionManager_750VR.ObtenerInstancia.user;

            if (usuario == null)
            {
                MessageBox.Show("No hay sesión activa.");
                return;
            }

            string actual = textBox2.Text;
            string nueva = textBox3.Text;
            string confirmar = textBox4.Text;

            string hashActual = encriptador.HashearConSalt_750VR(actual, usuario.salt_750VR);
            if (hashActual != usuario.contraseña_750VR)
            {
                MessageBox.Show("La contraseña actual es incorrecta.");
                return;
            }

            if (nueva != confirmar)
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                return;
            }

            if (nueva.Length < 10 || !nueva.Any(char.IsUpper) || !nueva.Any(char.IsDigit))
            {
                MessageBox.Show("La contraseña debe tener al menos 10 caracteres, una mayúscula y un número.");
                return;
            }

            if (nueva == usuario.contraseña_750VR)
            {
                MessageBox.Show("Las contraseña nueva no puede ser la contraseña actual.");
                return;
            }

 
            BLLusuario_750VR bll = new BLLusuario_750VR();
            bll.CambiarContraseña_750VR(usuario,nueva);



            SERVICIOS_VR750.SessionManager_750VR.ObtenerInstancia.CerrarSesion_750VR();
            MessageBox.Show("Contraseña actualizada exitosamente. Se cerro la sesion, vuelva a inciar sesion con su nueva contraseña");
            Application.Restart();

        }
    }
}
