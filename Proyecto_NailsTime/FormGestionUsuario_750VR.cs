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

 

        private void CargarUsuarios(bool soloActivos)
        {
            var bll = new BLLusuario_750VR();
            var lista = bll.leerEntidades_750VR();
            if (soloActivos)
                lista = lista.Where(u => u.activo_750VR).ToList();
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = lista;
            dataGridView1.Columns["contraseña_750VR"].Visible = false;
            dataGridView1.Columns["salt_750VR"].Visible = false;
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
                var resultados = bll.BuscarUsuarios_750VR(
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
            if (!ValidarCampos() && modoActual != "Activar/Desactivar" && modoActual != "desbloquear" )
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
            CargarUsuarios(true); // Refrescar grilla general
            MostrarCantidadUsuarios();
            LimpiarCampos();


        }

        private void AplicarDesbloqueo() //falta
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEusuario_750VR usuario)
            {
                if (!usuario.bloqueado_750VR)
                {
                    MessageBox.Show("El usuario ya está desbloqueado. Por favor seleccione uno que esté bloqueado.");
                    return;
                }

                BLLusuario_750VR bll = new BLLusuario_750VR();
                bll.DesbloquearUsuario_750VR(usuario.dni_750VR);

                MessageBox.Show("Usuario desbloqueado correctamente.", "Éxito");
                CargarUsuarios(true); // refresca la lista
                ResetearEstadoInterfaz();
                LimpiarCampos();
            }

        }

        private void AplicarActivarDesactivar()
        {
            var item = dataGridView1.CurrentRow?.DataBoundItem as BEusuario_750VR;
            if (item == null)
            {
                MessageBox.Show("Seleccione un usuario válido de la lista.");
                return;
            }

            bool nuevoEstado = !item.activo_750VR;

            BLLusuario_750VR bll = new BLLusuario_750VR();
            bool exito = bll.CambiarEstadoUsuario_750VR(item.dni_750VR, nuevoEstado);

            if (exito)
            {
                string mensaje = nuevoEstado ? "Usuario activado correctamente." : "Usuario desactivado correctamente.";
                MessageBox.Show(mensaje);

                if (!nuevoEstado)
                {
                    MessageBox.Show("Recuerde que el usuario no podrá iniciar sesión.");
                }

                CargarUsuarios(true);
                ResetearEstadoInterfaz();
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

            if (!ValidarCampos())
                return;

            int dni = int.Parse(txtDNI.Text);
            string nombre = txtnom.Text;
            string apellido = txtape.Text;
            string mail = txtemail.Text;
            string rol = cmbrol.Text;
            string usuario = $"{nombre}{apellido}";

            BLLusuario_750VR bll = new BLLusuario_750VR();

            // ⚠️ Obtener usuario original
            BEusuario_750VR original = bll.ObtenerUsuarioPorLogin_750VR(mail);

            bool seModificoApellido = original.apellido_750VR != apellido;

            bool exito = bll.ModificarUsuario_750VR(dni, nombre, apellido, mail, rol, usuario);

            if (exito)
            {
                MessageBox.Show("Usuario modificado correctamente.");

                // 🔔 Mostrar mensaje solo si se modificó el user
                if (seModificoApellido)
                {
                    MessageBox.Show($"Recuerde que ahora su nombre de usuario es {usuario}.");
                }

                CargarUsuarios(true);
                ResetearEstadoInterfaz();
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
                int dni = Convert.ToInt32(txtDNI.Text);
                string nombre = txtnom.Text.Trim();
                string apellido = txtape.Text.Trim();
                string mail = txtemail.Text.Trim();
                string rol = cmbrol.SelectedItem?.ToString();
                string user = $"{nombre}{apellido}";

                BLLusuario_750VR bll = new BLLusuario_750VR();

                // Verificar existencia por DNI
                if (bll.ObtenerUsuarioPorLogin_750VR(mail) != null)
                {
                    MessageBox.Show("Ya existe un usuario con ese mail.");
                    return;
                }

                // Verificar existencia por mail/login
                if (bll.ObtenerUsuarioPorDNI_750VR(dni) != null)
                {
                    MessageBox.Show("Ya existe un usuario con ese DNI.");
                    return;
                   
                }

                Encriptador_750VR encriptador = new Encriptador_750VR();
                string contraseña = $"{dni}{nombre}";
                string salt = encriptador.GenerarSalt_750VR();

                BEusuario_750VR nuevo = new BEusuario_750VR
                {
                    dni_750VR = dni,
                    nombre_750VR = nombre,
                    apellido_750VR = apellido,
                    mail_750VR = mail,
                    user_750VR = user,
                    salt_750VR = salt,
                    contraseña_750VR = encriptador.HashearConSalt_750VR(contraseña, salt),
                    rol_750VR = rol,
                    activo_750VR = true,
                    bloqueado_750VR = false
                };

                bll.CrearUsuario_750VR(nuevo);
                MessageBox.Show("Usuario creado correctamente.");
                MessageBox.Show("Recuerde que usuario=nombre+apellido, contraseña=dni+nombre");


                LimpiarCampos();
                rbtnact.Checked = true;
                CargarUsuarios(true);
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
            
        }

        private void rbtntodos_CheckedChanged(object sender, EventArgs e)
        {

            if (rbtntodos.Checked)
            {
                CargarUsuarios(false);

            }
                
        }



        private void rbtnact_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnact.Checked)
            {
                CargarUsuarios(true);
            }
                
        }


        private void PintarUsuariosInactivos()
        {
            
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.DataBoundItem is BEusuario_750VR usuario && !usuario.activo_750VR)
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

            ResetearEstadoInterfaz();
            rbtnact.Checked = true; // Marcar por defecto
            CargarUsuarios(true);   // Mostrar todos
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
                actsi.Enabled = false;
                actno.Enabled = false;
                bloqno.Enabled = false;
                bloqsi.Enabled = false;
                txtuser.Enabled = false;
            }
            else if (modoActual == "desbloquear" || modoActual == "Activar/Desactivar")
            {
                // Mostrar datos sin habilitar edición
                txtDNI.Enabled = false;
                txtnom.Enabled = false;
                txtape.Enabled = false;
                txtemail.Enabled = false;
                cmbrol.Enabled = false;
                txtuser.Enabled=false;
                bloqno .Enabled = false;
                bloqsi .Enabled = false;
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
            txtuser.Enabled = true;

            //el boton de activos activado
            rbtnact.Checked = true;

            //deshabilito bloq y act
            bloqno.Enabled = false;
            bloqsi.Enabled = false;
            actno.Enabled = false;
            actsi.Enabled = false;

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
            || !string.IsNullOrWhiteSpace(txtuser.Text)

           
            ;

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
            
        }

        private void MostrarCantidadUsuarios()
        {
            BLLusuario_750VR bll = new BLLusuario_750VR();
            var listaUsuarios = bll.leerEntidades_750VR();
            lblcantuser.Text = $"{listaUsuarios.Count}";
        }
        private void FormGestionUsuario_750VR_Load(object sender, EventArgs e)
        {
            rbtnact.Checked = true; // Marcar por defecto
            CargarUsuarios(true);   // Mostrar activos

            MostrarCantidadUsuarios(); //muetsra en el label cant users


            // Deshabilitar botones Aplicar y Cancelar
            btnaplicar.Enabled = false;
            btncancelar.Enabled = false;

            // Habilitar grilla solo para selección (no edición)
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            //deshabilito bloq y act
            bloqno.Enabled = false;
            bloqsi .Enabled = false;
            actno .Enabled = false;
            actsi .Enabled = false;

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

        private void actno_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void actsi_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void bloqsi_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void bloqno_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEusuario_750VR usuario)
            {
                txtDNI.Text = usuario.dni_750VR.ToString();
                txtnom.Text = usuario.nombre_750VR;
                txtape.Text = usuario.apellido_750VR;
                txtemail.Text = usuario.mail_750VR;
                cmbrol.Text = usuario.rol_750VR;
                txtuser.Text = usuario.user_750VR;

                bloqsi.Checked = usuario.bloqueado_750VR;
                bloqno.Checked = !usuario.bloqueado_750VR;
                actsi.Checked = usuario.activo_750VR;
                actno.Checked = !usuario.activo_750VR;

                btncancelar.Enabled = true;
                btncrear.Enabled = false;

                if (modoActual == "modificar" || modoActual == "desbloquear")
                {
                    txtDNI.Text = usuario.dni_750VR.ToString();
                    txtnom.Text = usuario.nombre_750VR;
                    txtape.Text = usuario.apellido_750VR;
                    txtemail.Text = usuario.mail_750VR;
                    cmbrol.Text = usuario.rol_750VR;
                    txtuser.Text = usuario.user_750VR;

                    bloqsi.Checked = usuario.bloqueado_750VR;
                    bloqno.Checked = !usuario.bloqueado_750VR;
                    actsi.Checked = usuario.activo_750VR;
                    actno.Checked = !usuario.activo_750VR;

                    btncancelar.Enabled = true;
                    btncrear.Enabled = false;
                }
            }
        }
    }
}
