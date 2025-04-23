using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL_VR750;

namespace Proyecto_NailsTime
{
    public partial class FormGestionUsuario_750VR : Form
    {

        public FormGestionUsuario_750VR()
        {
            InitializeComponent();
        }

        private string modoActual = "consulta";

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


            modoActual = "añadir";
            ActivarModoEdicion();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void btnaplicar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            switch (modoActual)
            {
                case "añadir":
                    AplicarAlta();
                    break;
                case "modificar":
                    AplicarModificacion();
                    break;
                case "eliminar":
                    AplicarBorradoLogico();
                    break;
                case "desbloquear":
                    AplicarDesbloqueo();
                    break;
            }

            modoActual = "consulta";
            ResetearEstadoInterfaz();
        }

        private void AplicarDesbloqueo()
        {
            try
            {
                string dni = txtDNI.Text;
                BLLusuario_750VR bll = new BLLusuario_750VR();
                bll.DesbloquearUsuario(dni);

                MessageBox.Show("Usuario desbloqueado correctamente.");
                CargarUsuariosActivos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al desbloquear usuario: " + ex.Message);
            }





            if (usuarioSeleccionado != null)
            {
                usuarioSeleccionado.Bloqueo = 1; // o false si usás BIT
                UsuarioBLL bll = new UsuarioBLL();
                bll.DesbloquearUsuario(usuarioSeleccionado.DNI);
                MessageBox.Show("Usuario desbloqueado correctamente.");
            }


        }

        private void AplicarBorradoLogico()
        {
            try
            {
                string dni = txtDNI.Text;
                BLLusuario_750VR bll = new BLLusuario_750VR();
                bool eliminado = bll.BorrarUsuarioLogico(dni);

                if (eliminado)
                {
                    MessageBox.Show("Usuario marcado como inactivo.");
                    CargarUsuariosActivos();
                }
                else
                {
                    MessageBox.Show("No se pudo borrar el usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al borrar usuario: " + ex.Message);
            }
        }

        private void AplicarModificacion()
        {
            //try
            //{
            //    UsuarioBE usuarioMod = new UsuarioBE
            //    {
            //        dni = int.Parse(txtDNI.Text),
            //        nombre = txtNombre.Text,
            //        apellido = txtApellido.Text,
            //        telefono = int.Parse(txtTelefono.Text),
            //        mail = txtEmail.Text,
            //        user = txtUsuario.Text,
            //        contraseña = txtContraseña.Text,
            //        rol = cmbRol.SelectedItem.ToString(),
            //        estado = rbtnActivoSi.Checked ? "Activo" : "Inactivo"
            //    };

            //    UsuarioBLL bll = new UsuarioBLL();
            //    bool modificado = bll.ModificarUsuario(usuarioMod);

            //    if (modificado)
            //    {
            //        MessageBox.Show("Usuario modificado correctamente.");
            //        CargarUsuariosActivos();
            //    }
            //    else
            //    {
            //        MessageBox.Show("No se pudo modificar el usuario.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error al modificar usuario: " + ex.Message);
            //}

            if (dgvUsuarios.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvUsuarios.SelectedRows[0];
                txtDNI.Text = row.Cells["DNI"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtApellido.Text = row.Cells["Apellido"].Value.ToString();
                txtTelefono.Text = row.Cells["Telefono"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtUsuario.Text = row.Cells["UsuarioLogin"].Value.ToString();
                txtContraseña.Text = row.Cells["Contrasenia"].Value.ToString();
                cmbRol.SelectedItem = row.Cells["Rol"].Value.ToString();
                rbtnActivoSi.Checked = row.Cells["Activo"].Value.ToString() == "1";

                modoActual = "modificar";
                ActivarModoEdicion();
            }
            else
            {
                MessageBox.Show("Seleccione un usuario de la lista.");
            }


        }

        private void AplicarAlta()
        {
            try
            {
                UsuarioBE nuevoUsuario = new UsuarioBE
                {
                    dni = int.Parse(txtDNI.Text),
                    nombre = txtNombre.Text,
                    apellido = txtApellido.Text,
                    telefono = int.Parse(txtTelefono.Text),
                    mail = txtEmail.Text,
                    rol = cmbRol.SelectedItem.ToString(),
                    estado = rbtnActivoSi.Checked ? "Activo" : "Inactivo",
                    user = txtDNI.Text + txtApellido.Text,
                    contraseña = txtDNI.Text + txtNombre.Text
                };

                UsuarioBLL bll = new UsuarioBLL();
                bool creado = bll.CrearUsuario(nuevoUsuario);

                if (creado)
                {
                    MessageBox.Show("Usuario creado correctamente.");
                    CargarUsuariosActivos();
                }
                else
                {
                    MessageBox.Show("No se pudo crear el usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear usuario: " + ex.Message);
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text) ||
       string.IsNullOrWhiteSpace(txtNombre.Text) ||
       string.IsNullOrWhiteSpace(txtApellido.Text) ||
       string.IsNullOrWhiteSpace(txtTelefono.Text) ||
       string.IsNullOrWhiteSpace(txtEmail.Text) ||
       string.IsNullOrWhiteSpace(cmbRol.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.");
                return false;
            }
            return true;
        }

        private void btndesb_Click(object sender, EventArgs e)
        {
            modoActual = "desbloquear";
            ActivarModoEdicion();
        }

        private void btnelim_Click(object sender, EventArgs e)
        {
            modoActual = "eliminar";
            ActivarModoEdicion();
        }

        private void rbtntodos_CheckedChanged(object sender, EventArgs e)
        {

            if (rbtnTodos.Checked)
                CargarTodosUsuarios();
        }

        private void CargarTodosUsuarios()
        {
            UsuarioBLL bll = new UsuarioBLL();
            var lista = bll.ObtenerUsuarios(false);
            dgvUsuarios.DataSource = lista;
            PintarUsuariosInactivos();
        }

        private void rbtnact_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnActivos.Checked)
                CargarUsuariosActivos();
        }

        private void CargarUsuariosActivos()
        {
            UsuarioBLL bll = new UsuarioBLL();
            var lista = bll.ObtenerUsuarios(true);
            dgvUsuarios.DataSource = lista;
            PintarUsuariosInactivos();
        }

        private void PintarUsuariosInactivos()
        {
            foreach (DataGridViewRow row in dgvUsuarios.Rows)
            {
                if (row.Cells["Activo"].Value != null && row.Cells["Activo"].Value.ToString() == "0")
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            modoActual = "consulta";
            ResetearEstadoInterfaz();
        }


        private void ActivarModoEdicion()
        {
            // Habilitar campos de edición según corresponda
            txtDNI.Enabled = modoActual == "añadir";
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;
            txtTelefono.Enabled = true;
            txtEmail.Enabled = true;
            cmbRol.Enabled = true;
            rbtnActivoSi.Enabled = true;
            rbtnActivoNo.Enabled = true;

            // Habilitar botones Aplicar y Cancelar
            btnAplicar.Enabled = true;
            btnCancelar.Enabled = true;

            // Deshabilitar botones de navegación
            btnAñadir.Enabled = false;
            btnModificar.Enabled = false;
            btnEliminar.Enabled = false;
            btnDesbloquear.Enabled = false;
            rbtnActivos.Enabled = false;
            rbtnTodos.Enabled = false;

            // Deshabilitar DataGridView si estás añadiendo o desbloqueando
            dgvUsuarios.Enabled = modoActual == "modificar" || modoActual == "eliminar";
        }


        private void ResetearEstadoInterfaz()
        {
            txtDNI.Enabled = false;
            txtNombre.Enabled = false;
            txtApellido.Enabled = false;
            txtTelefono.Enabled = false;
            txtEmail.Enabled = false;
            cmbRol.Enabled = false;
            rbtnActivoSi.Enabled = false;
            rbtnActivoNo.Enabled = false;

            btnAplicar.Enabled = false;
            btnCancelar.Enabled = false;

            btnAñadir.Enabled = true;
            btnModificar.Enabled = true;
            btnEliminar.Enabled = true;
            btnDesbloquear.Enabled = true;
            rbtnActivos.Enabled = true;
            rbtnTodos.Enabled = true;

            dgvUsuarios.Enabled = true;
        }

        //private void textBox5_TextChanged(object sender, EventArgs e)
        //{

        //}
    }
}
