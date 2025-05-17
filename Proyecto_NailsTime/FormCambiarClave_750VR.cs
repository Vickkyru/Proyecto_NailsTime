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
    public partial class FormCambiarClave_750VR : Form
    {
        public FormCambiarClave_750VR()
        {
            InitializeComponent();
            
        }
        Encriptador_VR750 encriptador = new Encriptador_VR750();


        private void FormCambiarClave_750VR_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var usuario = SessionManager_VR750.ObtenerInstancia.user;

            if (usuario == null)
            {
                MessageBox.Show("No hay sesión activa.");
                return;
            }

            string actual = textBox2.Text;
            string nueva = textBox3.Text;
            string confirmar = textBox4.Text;

            string hashActual = encriptador.HashearConSalt(actual, usuario.salt);
            if (hashActual != usuario.contraseña)
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

            if (nueva == usuario.contraseña)
            {
                MessageBox.Show("Las contraseña nueva no puede ser la contraseña actual.");
                return;
            }

            //string hashNueva = encriptador.HashearConSalt(nueva, usuario.salt);
            //if (hashNueva == usuario.contraseña)
            //{
            //    MessageBox.Show("La nueva contraseña no puede ser igual a la anterior.");
            //    return;
            //}

            //// Si todo está bien, generar nuevo salt y guardar
            //string nuevoSalt = encriptador.GenerarSalt();
            //string nuevoHash = encriptador.HashearConSalt(nueva, nuevoSalt);

            BLLusuario_750VR bll = new BLLusuario_750VR();
            bll.CambiarContraseña(usuario,nueva);

            // Actualizar en sesión
            //usuario.salt = nuevoSalt;
            //usuario.contraseña = nuevoHash;

            MessageBox.Show("Contraseña actualizada exitosamente.");

            

            SERVICIOS_VR750.SessionManager_VR750.ObtenerInstancia.CerrarSesion();
            MessageBox.Show("Contraseña actualizada exitosamente. Se cerro la sesion, vuelva a inciar sesion con su nueva contraseña");
            Application.Restart();

        }
    }
}
