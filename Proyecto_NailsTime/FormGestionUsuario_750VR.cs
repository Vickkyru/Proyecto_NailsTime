using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE_VR750;
using BLL_VR750;
using SERVICIOS_VR750;



namespace Proyecto_NailsTime
{
    public partial class FormGestionUsuario_750VR : Form
    {
        private string modoActual = "consulta";
        public FormGestionUsuario_750VR()
        {
            InitializeComponent();
 
            

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
            txtuser.Visible = false;   // textbox donde normalmente aparece el user
            bloqno.Visible = false;
            bloqsi.Visible = false;
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
                var resultados = bll.BuscarUsuarios(
            string.IsNullOrWhiteSpace(txtDNI.Text) ? null : txtDNI.Text,
            string.IsNullOrWhiteSpace(txtnom.Text) ? null : txtnom.Text,
            string.IsNullOrWhiteSpace(txtape.Text) ? null : txtape.Text,
            string.IsNullOrWhiteSpace(txtemail.Text) ? null : txtemail.Text,
            string.IsNullOrWhiteSpace(txtuser.Text) ? null : txtuser.Text,
            string.IsNullOrWhiteSpace(cmbrol.Text) ? null : cmbrol.Text
        );

                dataGridView1.DataSource = resultados;

                // Desmarcar los radio buttons
                rbtnact.Checked = false;
                rbtntodos.Checked = false;

                PintarUsuariosInactivos();
                LimpiarCampos();
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
                case "Activar/Desactivar":
                    AplicarActivarDesactivar();
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
            MostrarCantidadUsuarios();
            LimpiarCampos();


        }

        private void AplicarDesbloqueo() //falta
        {
            if (dataGridView1.CurrentRow != null)
            {
                int dni = Convert.ToInt32(dataGridView1.CurrentRow.Cells["dni"].Value);
                bool bloqueado = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["bloqueado"].Value);

                if (!bloqueado)
                {
                    MessageBox.Show("El usuario ya está desbloqueado. Por favor seleccione uno que esté bloqueado.");
                    return;
                }

                BLLusuario_750VR bll = new BLLusuario_750VR();
                bll.DesbloquearUsuario(dni);

                MessageBox.Show("Usuario desbloqueado correctamente.", "Éxito");
                CargarUsuariosActivos();// o el método que refresca el DataGrid
                LimpiarCampos();
            }

        }

        private void AplicarActivarDesactivar()
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario de la lista.");
                return;
            }

            int dni = Convert.ToInt32(dataGridView1.CurrentRow.Cells["dni"].Value);
            bool activoActual = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["activo"].Value);

            bool nuevoEstado = !activoActual; // invertimos el estado

            BLLusuario_750VR bll = new BLLusuario_750VR();
            bool exito = bll.CambiarEstadoUsuario(dni, nuevoEstado);

            if (exito)
            {
                string mensaje = nuevoEstado ? "Usuario activado correctamente." : "Usuario desactivado correctamente.";
                MessageBox.Show(mensaje);
                CargarTodosUsuarios();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al cambiar estado del usuario.");
            }
        }

        private void AplicarModificacion()
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario de la lista.");
                return;
            }

            // Validamos primero
            if (!ValidarCampos())
                return;

            int dni = int.Parse(txtDNI.Text);
            string nombre = txtnom.Text;
            string apellido = txtape.Text;
            string mail = txtemail.Text;
            string rol = cmbrol.Text;
            bool activo = actsi.Checked;

            BLLusuario_750VR bll = new BLLusuario_750VR();
            bool exito = bll.ModificarUsuario(dni, nombre, apellido, mail, rol, activo);

            if (exito)
            {
                MessageBox.Show("Usuario modificado correctamente.");
                CargarUsuariosActivos();// o el método que refresca el DataGrid
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al modificar el usuario.");
            }


        }
       

        private void AplicarAlta()
        {
            try
            {
                // Validamos primero
                if (!ValidarCampos())
                    return;

                // Llamar a la BLL con los valores de la GUI (datos crudos)
                BLLusuario_750VR usuarioBLL = new BLLusuario_750VR();


                int dni = Convert.ToInt32(txtDNI.Text);
                string nombre = txtnom.Text.Trim();
                string apellido = txtape.Text.Trim();
                string mail = txtemail.Text.Trim();
                string rol = cmbrol.SelectedItem?.ToString();


                ValidarCampos();


                BLLusuario_750VR bll = new BLLusuario_750VR();

                // Verificar existencia por DNI
                if (bll.ObtenerUsuarioPorLogin(mail) != null)
                {
                    MessageBox.Show("Ya existe un usuario con ese mail.");
                    return;
                }

                // Verificar existencia por mail/login
                if (bll.ObtenerUsuarioPorDNI(dni) != null)
                {
                    MessageBox.Show("Ya existe un usuario con ese DNI.");
                    return;
                   
                }

                Encriptador_VR750 encriptador = new Encriptador_VR750();
                string contraseña = $"{dni}{nombre}";
                string salt = encriptador.GenerarSalt();

                Usuario_750VR nuevo = new Usuario_750VR
                {
                    dni = dni,
                    nombre = nombre,
                    apellido = apellido,
                    mail = mail,
                    user = mail,
                    salt = salt,
                    contraseña = encriptador.HashearConSalt(contraseña, salt),
                    rol = rol,
                    activo = true,
                    bloqueado = false
                };

                bll.CrearUsuario(nuevo);
                MessageBox.Show("Usuario creado correctamente.");
                this.Close();

                MessageBox.Show("Usuario creado exitosamente.");
                LimpiarCampos();
                CargarUsuariosActivos();
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
            // 2.2) DNI: 7–8 dígitos, con o sin puntos (12.345.678 o 12345678)
            string dniPattern = @"^(\d{7,8}|\d{2}\.\d{3}\.\d{3})$";
            if (!Regex.IsMatch(txtDNI.Text.Trim(), dniPattern))
            {
                MessageBox.Show("Debe ingresar un DNI válido (7–8 dígitos, con o sin puntos).");
                return false;
            }

            // 2.3) E-mail
            if (!EsEmailValido(txtemail.Text.Trim()))
            {
                MessageBox.Show("Debe ingresar un e-mail válido.");
                return false;
            }

            return true;
        }
        // 1) Valida formato de e-mail
        private bool EsEmailValido(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btndesb_Click(object sender, EventArgs e)
        {
            modoActual = "desbloquear";
            ActivarModoEdicion();
            lblmensaje.Text = "Modo desbloqueo";
        }

        private void btnelim_Click(object sender, EventArgs e)
        {
            //lblmensaje.Text = "Modo Eliminar";
            //modoActual = "eliminar";
            //ActivarModoEdicion();
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
            //btnelim.Enabled = true;
            btndesb.Enabled = true;
            btnact.Enabled = true;

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
                bloqno.Enabled = true;
                bloqsi.Enabled = true;
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
            //btnelim.Enabled = false;
            btndesb.Enabled = false;
            btnact.Enabled = false;
            rbtnact.Enabled = false;
            rbtntodos.Enabled = false;

            // Permitir selección solo en modificar y desbloquear
            dataGridView1.Enabled = modoActual == "modificar" || modoActual == "desbloquear";


        }


        private void ResetearEstadoInterfaz()
        {
            // Campos de texto deshabilitados
            txtDNI.Enabled = txtnom.Enabled = txtape.Enabled = txtemail.Enabled = true;
            cmbrol.Enabled = actsi.Enabled = actno.Enabled = true;

            // CRUD y filtros habilitados
            btncrear.Enabled = btnmod.Enabled = /*btnelim.Enabled =*/ btndesb.Enabled = true;
            rbtnact.Enabled = rbtntodos.Enabled = true;
            btnact.Enabled = true; 

            // Aplicar/Cancelar deshabilitados
            btnaplicar.Enabled = false;
            btncancelar.Enabled = false;

            // Grilla habilitada para seleccionar
            dataGridView1.Enabled = true;

            // (si ocultaste txtUser en Crear, mostrala de nuevo)
            txtuser.Visible = txtuser.Visible = true;
            bloqno.Visible = true;
            bloqsi.Visible = true;

            if(txtDNI.Text != null)
            {
                btncancelar.Enabled = true;
            }
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
                txtuser.Text = dataGridView1.CurrentRow.Cells["usuario"].Value.ToString();

                // Asignar radio button según valor booleano o entero
                bool activo = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["activo"].Value);
                bool bloq = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["bloqueado"].Value);
                MessageBox.Show("Bloqueado: " + dataGridView1.CurrentRow.Cells["bloqueado"].Value.ToString());

                bloqsi.Checked = bloq;
                bloqno.Checked = !bloq;
                actsi.Checked = activo;
                actno.Checked = !activo;

            }

            if (dataGridView1.CurrentRow != null && (modoActual == "modificar" || modoActual == "desbloquear"))
            {
                txtDNI.Text = dataGridView1.CurrentRow.Cells["dni"].Value.ToString();
                txtnom.Text = dataGridView1.CurrentRow.Cells["nombre"].Value.ToString();
                txtape.Text = dataGridView1.CurrentRow.Cells["apellido"].Value.ToString();
                txtemail.Text = dataGridView1.CurrentRow.Cells["mail"].Value.ToString();
                cmbrol.Text = dataGridView1.CurrentRow.Cells["rol"].Value.ToString();
                txtuser.Text = dataGridView1.CurrentRow.Cells["usuario"].Value.ToString();

                bool activo = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["activo"].Value);
                bool bloq = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["bloqueado"].Value);
                actsi.Checked = activo;
                actno.Checked = !activo;
                bloqno.Checked = bloq;
                bloqsi.Checked = !bloq;

            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void MostrarCantidadUsuarios()
        {
            BLLusuario_750VR bll = new BLLusuario_750VR();
            var listaUsuarios = bll.leerEntidades();
            lblcantuser.Text = $"{listaUsuarios.Count}";
        }
        private void FormGestionUsuario_750VR_Load(object sender, EventArgs e)
        {
            rbtnact.Checked = true; // Marcar por defecto
            CargarUsuariosActivos();   // Mostrar activos

            MostrarCantidadUsuarios(); //muetsra en el label cant users


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
            modoActual = "modificar";
            ActivarModoEdicion();
            lblmensaje.Text = "Modo Modificar";
        }

        private void btnact_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Activar/Desactivar";
            modoActual = "Activar/Desactivar";
            ActivarModoEdicion();
        }
    }
}
