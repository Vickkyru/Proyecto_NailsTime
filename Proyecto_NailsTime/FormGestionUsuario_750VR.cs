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
            dataGridView1.AutoGenerateColumns = false; 
            dataGridView1.DataSource = lista;
            PintarUsuariosInactivos();
        }

        private void CargarUsuariosActivos()
        {
            BLLusuario_750VR bll = new BLLusuario_750VR();
            var lista = bll.ObtenerUsuarios(true);
            dataGridView1.AutoGenerateColumns = false; 
            dataGridView1.DataSource = lista;
            PintarUsuariosInactivos();
        }

  









        private void label2_Click(object sender, EventArgs e)
        {

        }

        //boton crear
        private void button1_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Añadir";
            modoActual = "añadir";
            ActivarModoEdicion();
        }

        //boton salir
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
            // Si estamos en modo consulta, se hace la búsqueda
            if (modoActual == "consulta")
            {
                BLLusuario_750VR bll = new BLLusuario_750VR();
                var resultados = bll.BuscarUsuarios(txtDNI.Text, txtnom.Text, txtape.Text, txtemail.Text);
                dataGridView1.DataSource = resultados;

                // Desmarcar los radio buttons
                rbtnact.Checked = false;
                rbtntodos.Checked = false;

                PintarUsuariosInactivos();
                return;
            }

            // Validar campos solo si no estamos eliminando ni desbloqueando
            if (!ValidarCampos() && modoActual != "eliminar" && modoActual != "desbloquear")
                return;

            // Ejecutar la acción según el modo
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
            lblmensaje.Text = "Modo Consulta";
            ResetearEstadoInterfaz();
            CargarTodosUsuarios(); // Refrescar grilla general

        }

        private void AplicarDesbloqueo() //falta
        {
            int dni = int.Parse(txtDNI.Text);

            BLLusuario_750VR bll = new BLLusuario_750VR();
            bool ok = bll.DesbloquearUsuario(dni);

            if (ok)
            {
                MessageBox.Show("Usuario desbloqueado correctamente.");
            }
            else
            {
                MessageBox.Show("No se pudo desbloquear el usuario.");
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
            try
            {
                if (string.IsNullOrWhiteSpace(txtDNI.Text) ||
                    string.IsNullOrWhiteSpace(txtnom.Text) ||
                    string.IsNullOrWhiteSpace(txtape.Text) ||
                    string.IsNullOrWhiteSpace(txtemail.Text) ||
                    cmbrol.SelectedItem == null)
                {
                    MessageBox.Show("Debe completar todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Armar usuario modificado
                Usuario_750VR usuarioModificado = new Usuario_750VR();

                usuarioModificado.dni = int.Parse(txtDNI.Text.Trim()); // No cambia
                usuarioModificado.nombre = txtnom.Text.Trim();
                usuarioModificado.apellido = txtape.Text.Trim();
                usuarioModificado.mail = txtemail.Text.Trim();
                usuarioModificado.rol = cmbrol.SelectedItem.ToString();
                usuarioModificado.activo = true; //no cambia

                // Importante: usuarioModificado.Usuario, Contra y Salt no se modifican aquí.
                // Los recuperamos para no perderlos

                BLLusuario_750VR usuarioBLL = new BLLusuario_750VR();
                Usuario_750VR datosExistente = usuarioBLL.ObtenerUsuarioPorDNI(usuarioModificado.dni);

                usuarioModificado.user = datosExistente.user;
                usuarioModificado.contraseña = datosExistente.contraseña;
                usuarioModificado.salt = datosExistente.salt;
                usuarioModificado.bloqueado = datosExistente.bloqueado; // respetamos si estaba bloqueado o no

                // Llamar a BLL para actualizar
                usuarioBLL.ModificarUsuario(usuarioModificado);

                MessageBox.Show("Usuario modificado exitosamente.", "Modificación de Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refrescar grilla o limpiar pantalla
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void AplicarAlta()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDNI.Text) ||
                    string.IsNullOrWhiteSpace(txtnom.Text) ||
                    string.IsNullOrWhiteSpace(txtape.Text) ||
                    string.IsNullOrWhiteSpace(txtemail.Text) ||
                    cmbrol.SelectedItem == null)
                {
                    MessageBox.Show("Debe completar todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crear nuevo usuario
                Usuario_750VR nuevoUsuario = new Usuario_750VR();

                nuevoUsuario.dni = int.Parse(txtDNI.Text.Trim());
                nuevoUsuario.nombre = txtnom.Text.Trim();
                nuevoUsuario.apellido = txtape.Text.Trim();
                nuevoUsuario.mail = txtemail.Text.Trim();
                nuevoUsuario.user = nuevoUsuario.dni.ToString() + nuevoUsuario.apellido; // User predeterminado

                string contraseñaPredeterminada = nuevoUsuario.dni.ToString() + nuevoUsuario.nombre; // Contraseña predeterminada

                // Generar Salt y encriptar contraseña
                nuevoUsuario.salt = SERVICIOS_VR750.Encriptador_VR750.GenerarSalt();
                nuevoUsuario.contraseña = SERVICIOS_VR750.Encriptador_VR750.HashearConSalt(contraseñaPredeterminada, nuevoUsuario.salt);

                nuevoUsuario.rol = cmbrol.SelectedItem.ToString();
                nuevoUsuario.activo = true; // al dar de alta -> Activo
                nuevoUsuario.bloqueado = false; // Alta de usuario ➔ no bloqueado

                // Llamar a BLL para guardar
                BLLusuario_750VR usuarioBLL = new BLLusuario_750VR();
                usuarioBLL.CrearUsuario(nuevoUsuario);

                MessageBox.Show("Usuario creado exitosamente.", "Alta de Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refrescar la grilla o limpiar campos
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (dataGridView1.CurrentRow != null)
            {
                // Cambiar modo
                modoActual = "desbloquear";
                lblmensaje.Text = "Modo Desbloquear";

                // Activar la lógica de edición para este modo
                ActivarModoEdicion();

            }
            else
            {
                MessageBox.Show("Seleccione un usuario bloqueado para desbloquear.");
            }
            LimpiarCampos();
        }

        private void btnelim_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Eliminar";
            modoActual = "eliminar";
            ActivarModoEdicion();
        }

        private void rbtntodos_CheckedChanged(object sender, EventArgs e)
        {

            if (rbtntodos.Checked)
            {
                CargarTodosUsuarios();

            }
                
        }




        private void rbtnact_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnact.Checked)
            {
                CargarUsuariosActivos();
            }
                
        }


        private void PintarUsuariosInactivos()
        {
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells["activo"].Value != null && !(bool)fila.Cells["activo"].Value)
                {
                    fila.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        //boton cancelar
        private void btncancelar_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Consulta";
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
            rbtntodos.Checked = true; // Marcar por defecto
            CargarTodosUsuarios();   // Mostrar todos
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
                //txtDNI.Enabled = false;
                //txtnom.Enabled = false;
                //txtape.Enabled = false;
                //txtemail.Enabled = false;
                //cmbrol.Enabled = false;
                //actsi.Enabled = false;
                //actno.Enabled = false;
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
            //dataGridView1.Enabled = modoActual == "modificar" || modoActual == "eliminar";

        
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
            if (dataGridView1.CurrentRow != null)
            {
                txtDNI.Text = dataGridView1.CurrentRow.Cells["dni"].Value.ToString();
                txtnom.Text = dataGridView1.CurrentRow.Cells["nombre"].Value.ToString();
                txtape.Text = dataGridView1.CurrentRow.Cells["apellido"].Value.ToString();
                txtemail.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();
                cmbrol.Text = dataGridView1.CurrentRow.Cells["rol"].Value.ToString();

                // Asignar radio button según valor booleano o entero
                bool activo = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["activo"].Value);
                actsi.Checked = activo;
                actno.Checked = !activo;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //detecta seleccion del desbl
            //if (modoActual == "desbloquear" && dataGridView1.SelectedRows.Count > 0)
            //{
            //    DataGridViewRow row = dataGridView1.SelectedRows[0];

            //    txtDNI.Text = row.Cells["DNI"].Value.ToString();
            //    txtnom.Text = row.Cells["Nombre"].Value.ToString();
            //    txtape.Text = row.Cells["Apellido"].Value.ToString();
            //    txtuser.Text = row.Cells["UsuarioLogin"].Value.ToString();
            //    txtemail.Text = row.Cells["Email"].Value.ToString();
            //    cmbrol.SelectedItem = row.Cells["Rol"].Value.ToString();

            //    actsi.Checked = row.Cells["Activo"].Value.ToString() == "1";
            //    actno.Checked = row.Cells["Activo"].Value.ToString() == "0";
            //}

         

            //private void textBox5_TextChanged(object sender, EventArgs e)
            //{

            //}
        }

        private void FormGestionUsuario_750VR_Load(object sender, EventArgs e)
        {
            rbtntodos.Checked = true; // Marcar por defecto
            CargarTodosUsuarios();   // Mostrar todos

            lblcantuser.Text = dataGridView1.Rows.Count.ToString();


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



        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Modificar";
        }
    }
}
