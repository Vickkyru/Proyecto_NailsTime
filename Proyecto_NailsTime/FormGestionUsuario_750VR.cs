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
    public partial class FormGestionUsuario_750VR : Form
    {
        public FormGestionUsuario_750VR()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //boton crear
        private void button1_Click(object sender, EventArgs e)
        {
            //UsuarioBE nuevoUsuario = new UsuarioBE();

            //nuevoUsuario.dni = int.Parse(txtDNI.Text);
            //nuevoUsuario.nombre = txtNombre.Text;
            //nuevoUsuario.apellido = txtApellido.Text;
            //nuevoUsuario.telefono = int.Parse(txtTelefono.Text);
            //nuevoUsuario.mail = txtEmail.Text;

            //// Usuario y contraseña automáticos
            //nuevoUsuario.user = txtDNI.Text + txtApellido.Text;
            //nuevoUsuario.contraseña = txtDNI.Text + txtNombre.Text;

            //nuevoUsuario.rol = cmbRol.SelectedItem.ToString();

            //// Estado activo
            //nuevoUsuario.estado = rbtnActivoSi.Checked ? "Activo" : "Inactivo";

            //// Podés también guardar si está bloqueado o no como campo extra si querés

            //UsuarioBLL usuarioBLL = new UsuarioBLL();
            //bool creado = usuarioBLL.CrearUsuario(nuevoUsuario);

            //if (creado)
            //{
            //    MessageBox.Show("Usuario creado correctamente.");
            //}
            //else
            //{
            //    MessageBox.Show("Error al crear el usuario.");
            //}
        }
    }
}
