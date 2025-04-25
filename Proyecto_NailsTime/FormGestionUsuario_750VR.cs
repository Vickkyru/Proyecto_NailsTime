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
using BE_VR750;

namespace Proyecto_NailsTime
{
    public partial class FormGestionUsuario_750VR : Form
    {
        private string modoActual = "consulta";
        public FormGestionUsuario_750VR()
        {
            InitializeComponent();
            lblcantuser.Text = dataGridView1.Rows.Count.ToString();

            // Cargar solo usuarios activos al iniciar
            rbtnact.Checked = true; // activa el selector "Activos"
            CargarUsuariosActivos();

            // Deshabilitar botones Aplicar y Cancelar
            btnaplicar.Enabled = false;
            btncancelar.Enabled = false;

            // Habilitar grilla solo para selección (no edición)
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            // Iniciar en modo consulta
            modoActual = "consulta";
            lblmensaje.Text = "Modo Consulta";


            // Opcional: limpiar campos
            LimpiarCampos();

        }
        public void LimpiarCampos()
        {
            txtDNI.Clear();
            txtnom.Clear();
            txtape.Clear();
            txtemail.Clear();
            txtuser.Clear();
            cmbrol.SelectedIndex = -1;
            actsi.Checked = false;
            actno.Checked = false;
            bloqsi.Checked = false;
            bloqno.Checked = false;

        }

        private void CargarTodosUsuarios()
        {
            BLLusuario_750VR bll = new BLLusuario_750VR();
            var lista = bll.ObtenerUsuarios(false);
            dataGridView1.DataSource = lista;
            PintarUsuariosInactivos();
        }

        private void CargarUsuariosActivos()
        {
            BLLusuario_750VR bll = new BLLusuario_750VR();
            var lista = bll.ObtenerUsuarios(true);
            dataGridView1.DataSource = lista;
            PintarUsuariosInactivos();
        }

        //hasta aca seria lo q muestra ni bien arranca + lo de los radio btns act y todos









        private void label2_Click(object sender, EventArgs e)
        {

        }

        //boton crear
        private void button1_Click(object sender, EventArgs e)
        {

            modoActual = "añadir";
            ActivarModoEdicion();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea salir? Se perderán los cambios no guardados.",
                               "Confirmar salida",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                this.Close();
        }

        private void btnaplicar_Click(object sender, EventArgs e)
        {

            //después de aplicar el botón aplicar en el datagrid se ponen todos los registros q cumplan con los requerimientos de búsqueda
            if (modoActual == "consulta")
            {
                BLLusuario_750VR bll = new BLLusuario_750VR();
                var resultados = bll.BuscarUsuarios(txtDNI.Text, txtnom.Text, txtape.Text, txtemail.Text);
                dataGridView1.DataSource = resultados;

                // Desactivar los selectores
                rbtnact.Checked = false;
                rbtntodos.Checked = false;

                PintarUsuariosInactivos();
                return;
            }

            if (!ValidarCampos() && modoActual != "eliminar" && modoActual != "desbloquear")
                return;

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

            // Volver al estado de consulta
            modoActual = "consulta";
            ResetearEstadoInterfaz(); // Esto desactiva cancelar y activa el resto

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





            //if (usuarioSeleccionado != null)
            //{
            //    usuarioSeleccionado.Bloqueo = 1; // o false si usás BIT
            //    UsuarioBLL bll = new UsuarioBLL();
            //    bll.DesbloquearUsuario(usuarioSeleccionado.DNI);
            //    MessageBox.Show("Usuario desbloqueado correctamente.");
            //}


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
            try
            {
                Usuario_750VR usuarioMod = new Usuario_750VR
                {
                    dni = int.Parse(txtDNI.Text),
                    nombre = txtnom.Text,
                    apellido = txtape.Text,
                    mail = txtemail.Text,
                    user = txtuser.Text,
                    rol = cmbrol.SelectedItem.ToString(),
                    estado = actsi.Checked ? "Activo" : "Inactivo"
                };

                BLLusuario_750VR bll = new BLLusuario_750VR();
                bool modificado = bll.ModificarUsuario(usuarioMod);

                if (modificado)
                {
                    MessageBox.Show("Usuario modificado correctamente.");
                    CargarUsuariosActivos();
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el usuario.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar usuario: " + ex.Message);
            }

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                txtDNI.Text = row.Cells["DNI"].Value.ToString();
                txtnom.Text = row.Cells["Nombre"].Value.ToString();
                txtape.Text = row.Cells["Apellido"].Value.ToString();
                txtemail.Text = row.Cells["Email"].Value.ToString();
                txtuser.Text = row.Cells["UsuarioLogin"].Value.ToString();
                cmbrol.SelectedItem = row.Cells["Rol"].Value.ToString();
                actsi.Checked = row.Cells["Activo"].Value.ToString() == "1";

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
                Usuario_750VR nuevoUsuario = new Usuario_750VR
                {
                    dni = int.Parse(txtDNI.Text),
                    nombre = txtnom.Text,
                    apellido = txtape.Text,
                    mail = txtemail.Text,
                    rol = cmbrol.SelectedItem.ToString(),
                    estado = actsi.Checked ? "Activo" : "Inactivo",
                    user = txtDNI.Text + txtape.Text,
                    contraseña = txtDNI.Text + txtnom.Text
                };

                BLLusuario_750VR bll = new BLLusuario_750VR();
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
       string.IsNullOrWhiteSpace(txtnom.Text) ||
       string.IsNullOrWhiteSpace(txtape.Text) ||
       string.IsNullOrWhiteSpace(txtemail.Text) ||
       string.IsNullOrWhiteSpace(cmbrol.Text))
            //falta el resto
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.");
                return false;
            }
            return true;
        }

        private void btndesb_Click(object sender, EventArgs e)
        {
            modoActual = "desbloquear";
            // Bloquear otras acciones
            btncrear.Enabled = false;
            btnmod.Enabled = false;
            btnelim.Enabled = false;

            rbtnact.Enabled = false;
            rbtntodos.Enabled = false;

            // Habilitar botones
            btncancelar.Enabled = true;
            btnaplicar.Enabled = true;

            // Activar selección de usuario en el DataGridView
            dataGridView1.Enabled = true;

            LimpiarCampos(); // opcional
            ActivarModoEdicion();
        }

        private void btnelim_Click(object sender, EventArgs e)
        {
            modoActual = "eliminar";
            ActivarModoEdicion();
        }

        private void rbtntodos_CheckedChanged(object sender, EventArgs e)
        {

            if (rbtntodos.Checked)
                CargarTodosUsuarios();
        }




        private void rbtnact_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnact.Checked)
                CargarUsuariosActivos();
        }


        private void PintarUsuariosInactivos()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
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

            // Limpiar todos los campos
            LimpiarCampos();

            // Restaurar interfaz al estado inicial
            btnaplicar.Enabled = false;
            btncancelar.Enabled = false;

            btncrear.Enabled = true;
            btnmod.Enabled = true;
            btnelim.Enabled = true;
            btndesb.Enabled = true;

            rbtnact.Enabled = true;
            rbtntodos.Enabled = true;

            dataGridView1.Enabled = true;

            ResetearEstadoInterfaz();
        }


        private void ActivarModoEdicion()
        {
            if (modoActual == "añadir" || modoActual == "modificar")
            {
                txtDNI.Enabled = modoActual == "añadir";
                txtnom.Enabled = true;
                txtape.Enabled = true;
                txtemail.Enabled = true;
                cmbrol.Enabled = true;
                actsi.Enabled = true;
                actno.Enabled = true;
            }
            else if (modoActual == "desbloquear")
            {
                // Mostrar datos sin habilitar edición
                txtDNI.Enabled = false;
                txtnom.Enabled = false;
                txtape.Enabled = false;
                txtemail.Enabled = false;
                cmbrol.Enabled = false;
                actsi.Enabled = false;
                actno.Enabled = false;
            }
       

            // Habilitar botones Aplicar y Cancelar
            btnaplicar.Enabled = true;
            btncancelar.Enabled = true;

            // Deshabilitar botones de navegación
            btncrear.Enabled = false;
            btnmod.Enabled = false;
            btnmod.Enabled = false;
            btndesb.Enabled = false;
            rbtnact.Enabled = false;
            rbtntodos.Enabled = false;

            // Deshabilitar DataGridView si estás añadiendo o desbloqueando
            dataGridView1.Enabled = modoActual == "modificar" || modoActual == "eliminar";

        
        }


        private void ResetearEstadoInterfaz()
        {
            txtDNI.Enabled = false;
            txtnom.Enabled = false;
            txtape.Enabled = false;
            txtemail.Enabled = false;
            cmbrol.Enabled = false;
            actsi.Enabled = false;
            actno.Enabled = false;

            btnaplicar.Enabled = false;
            btncancelar.Enabled = false;

            btncrear.Enabled = true;
            btnmod.Enabled = true;
            btnelim.Enabled = true;
            btndesb.Enabled = true;
            rbtnact.Enabled = true;
            rbtntodos.Enabled = true;

            dataGridView1.Enabled = true;
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void txtDNI_TextChanged(object sender, EventArgs e) //hasta q no se implementen los campos no se aplica
        {
            VerificarCamposBusqueda();
        }

        private void txtape_TextChanged(object sender, EventArgs e) //hasta q no se implementen los campos no se aplica
        {
            VerificarCamposBusqueda();
        }

        private void txtnom_TextChanged(object sender, EventArgs e) //hasta q no se implementen los campos no se aplica
        {
            VerificarCamposBusqueda();
        }

        private void cmbrol_TextChanged(object sender, EventArgs e) //hasta q no se implementen los campos no se aplica
        {
            VerificarCamposBusqueda();
        }

        private void txtuser_TextChanged(object sender, EventArgs e) //hasta q no se implementen los campos no se aplica
        {
            VerificarCamposBusqueda();
        }
        private void VerificarCamposBusqueda() //hasta q no se implementen los campos no se aplica
        {
            if (modoActual != "consulta") return;

            // Verifica si al menos un campo está completo
            bool hayDatos = !string.IsNullOrWhiteSpace(txtDNI.Text)
                         || !string.IsNullOrWhiteSpace(txtnom.Text)
                         || !string.IsNullOrWhiteSpace(txtape.Text)
                         || !string.IsNullOrWhiteSpace(txtemail.Text)
            || !string.IsNullOrWhiteSpace(cmbrol.Text)
            || !string.IsNullOrWhiteSpace(txtuser.Text);

            btnaplicar.Enabled = hayDatos;
        }

        private void txtemail_TextChanged(object sender, EventArgs e) //hasta q no se implementen los campos no se aplica
        {
            VerificarCamposBusqueda();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //detecta seleccion del desbl
            if (modoActual == "desbloquear" && dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                txtDNI.Text = row.Cells["DNI"].Value.ToString();
                txtnom.Text = row.Cells["Nombre"].Value.ToString();
                txtape.Text = row.Cells["Apellido"].Value.ToString();
                txtuser.Text = row.Cells["UsuarioLogin"].Value.ToString();
                txtemail.Text = row.Cells["Email"].Value.ToString();
                cmbrol.SelectedItem = row.Cells["Rol"].Value.ToString();

                actsi.Checked = row.Cells["Activo"].Value.ToString() == "1";
                actno.Checked = row.Cells["Activo"].Value.ToString() == "0";
            }



            //private void textBox5_TextChanged(object sender, EventArgs e)
            //{

            //}
        }
    }
}
