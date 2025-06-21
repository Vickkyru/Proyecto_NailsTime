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
    public partial class FormGestionUsuario_750VR : Form, Iobserver_750VR
    {
        private string modoActual = "consulta";
        public FormGestionUsuario_750VR()
        {
            InitializeComponent();
            //Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            //ActualizarIdioma();
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
            ActualizarMensajeModo();
        }
        private void ActualizarMensajeModo()
        {
            string clave = "FormABMdisponibilidad.Mensaje." + modoActual;
            lblmensaje.Text = Lenguaje_750VR.ObtenerEtiqueta(clave);
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
            // Traducción de encabezados
            dataGridView1.Columns["dni_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Usuario.DNI");
            dataGridView1.Columns["nombre_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Usuario.Nombre");
            dataGridView1.Columns["apellido_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Usuario.Apellido");
            dataGridView1.Columns["mail_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Usuario.Email");
            dataGridView1.Columns["user_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Usuario.Login");
            dataGridView1.Columns["rol_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Usuario.Rol");
            dataGridView1.Columns["activo_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Usuario.Activo");
            dataGridView1.Columns["bloqueado_750VR"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Usuario.Bloqueado");
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
            string mensaje = Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.ConfirmarSalidaMensaje");
            string titulo = Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.ConfirmarSalidaTitulo");

            var result = MessageBox.Show(
                mensaje,
                titulo,
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

                btncancelar.Enabled = false;
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
            ActualizarMensajeModo();
            ResetearEstadoInterfaz();
            CargarUsuarios(true); 
            MostrarCantidadUsuarios();
            LimpiarCampos();


        }

        private void AplicarDesbloqueo()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEusuario_750VR usuario)
            {
                if (!usuario.bloqueado_750VR)
                {
                    MessageBox.Show(
                        Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.UsuarioYaDesbloqueado"),
                        Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloAdvertencia")
                    );
                    return;
                }

                BLLusuario_750VR bll = new BLLusuario_750VR();
                bll.DesbloquearUsuario_750VR(usuario.dni_750VR);

                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.UsuarioDesbloqueado"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloExito")
                );

                CargarUsuarios(true);
                ResetearEstadoInterfaz();
                LimpiarCampos();
            }
        }


        private void AplicarActivarDesactivar()
        {
            var item = dataGridView1.CurrentRow?.DataBoundItem as BEusuario_750VR;
            if (item == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.SeleccioneUsuario"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloAdvertencia")
                );
                return;
            }

            bool nuevoEstado = !item.activo_750VR;

            BLLusuario_750VR bll = new BLLusuario_750VR();
            bool exito = bll.CambiarEstadoUsuario_750VR(item.dni_750VR, nuevoEstado);

            if (exito)
            {
                string mensaje = nuevoEstado
                    ? Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.UsuarioActivado")
                    : Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.UsuarioDesactivado");

                MessageBox.Show(mensaje, Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloExito"));

                if (!nuevoEstado)
                {
                    MessageBox.Show(
                        Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.AdvertenciaDesactivado"),
                        Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloAdvertencia")
                    );
                }

                CargarUsuarios(true);
                ResetearEstadoInterfaz();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.ErrorCambioEstado"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloError")
                );
            }
        }


        private void AplicarModificacion()
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.SeleccioneUsuario"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloAdvertencia")
                );
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
            BEusuario_750VR original = bll.ObtenerUsuarioPorDNI_750VR(dni);
            bool seModificoApellido = original.apellido_750VR != apellido;

            bool exito = bll.ModificarUsuario_750VR(dni, nombre, apellido, mail, rol, usuario);

            if (exito)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.ModificacionExitosa"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloExito")
                );

                if (seModificoApellido)
                {
                    MessageBox.Show(
                        string.Format(
                            Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.NuevoUsuario"),
                            usuario
                        ),
                        Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloInfo")
                    );
                }

                CargarUsuarios(true);
                ResetearEstadoInterfaz();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.ErrorModificacion"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloError")
                );
            }
        }




        private void AplicarAlta()
        {
            try
            {
                if (!ValidarCampos())
                    return;

                int dni = Convert.ToInt32(txtDNI.Text);
                string nombre = txtnom.Text.Trim();
                string apellido = txtape.Text.Trim();
                string mail = txtemail.Text.Trim();
                string rol = cmbrol.SelectedItem?.ToString();
                string user = $"{nombre}{apellido}";

               

                BLLusuario_750VR bll = new BLLusuario_750VR();

                if (bll.ExisteUsuarioConLoginODNI(user, dni))
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.UsuarioODNIExistente"));
                    return;
                }


                Encriptador_750VR encriptador = new Encriptador_750VR();
                string contraseña = $"{dni}{nombre}";
                string salt = encriptador.GenerarSalt_750VR();
                string contraseñaHasheada = encriptador.HashearConSalt_750VR(contraseña, salt);

                BEusuario_750VR nuevo = new BEusuario_750VR(
                    dni,
                    nombre,
                    apellido,
                    mail,
                    user,
                    contraseñaHasheada,
                    salt,
                    rol,
                    true,
                    false,
                    idiom: Lenguaje_750VR.ObtenerInstancia().IdiomaActual
                );

                bll.CrearUsuario_750VR(nuevo);

                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.AltaExitosa"));
                MessageBox.Show(string.Format(
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.CredencialesGeneradas"),
                    user, contraseña
                ));

                LimpiarCampos();
                rbtnact.Checked = true;
                CargarUsuarios(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.ErrorAlta") + ": " + ex.Message,
                    Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtDNI.Text) ||
                string.IsNullOrWhiteSpace(txtnom.Text) ||
                string.IsNullOrWhiteSpace(txtape.Text) ||
                string.IsNullOrWhiteSpace(txtemail.Text) ||
                string.IsNullOrWhiteSpace(cmbrol.Text))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.CamposObligatorios"));
                return false;
            }

            string dniPattern = @"^(\d{7,8}|\d{2}\.\d{3}\.\d{3})$";
            if (!Regex.IsMatch(txtDNI.Text.Trim(), dniPattern))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.DNIInvalido"));
                return false;
            }

            if (!EsEmailValido(txtemail.Text.Trim()))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.EmailInvalido"));
                return false;
            }

            return true;
        }

        
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
            ActualizarMensajeModo();

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
            
            modoActual = "consulta";
            ActualizarMensajeModo();

            // Limpiar todos los campos
            LimpiarCampos();

            ResetearEstadoInterfaz();
            rbtnact.Checked = true; // Marcar por defecto
            CargarUsuarios(true);   // Mostrar todos
        }


        private void ActivarModoEdicion()
        {
            if (modoActual == "añadir")
            {
                dataGridView1.Enabled = false;
            }

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
                dataGridView1.Enabled = true;

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
            groupBox1.Text = Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.groupBox1");
            groupBox2.Text = Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.groupBox2");
            actsi.Text = Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.actsi");
            actno.Text = Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.actno");
            bloqsi.Text = Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.bloqsi");
            bloqno.Text = Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.bloqno");
            rbtnact.Checked = true; // Marcar por defecto
            //CargarUsuarios(true);   // Mostrar activos

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
            ActualizarMensajeModo();

        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            modoActual = "modificar";
            ActivarModoEdicion();
            ActualizarMensajeModo();

        }

        private void btnact_Click(object sender, EventArgs e)
        {
            
            modoActual = "Activar/Desactivar";
            ActualizarMensajeModo();
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
          
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
          
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Obtener DNI del usuario seleccionado
                string dniSeleccionado = dataGridView1.SelectedRows[0].Cells["dni_750VR"].Value.ToString();

                // Llamar al BLL para recuperar el usuario por DNI
                BLLusuario_750VR bll = new BLLusuario_750VR();
                var resultado = bll.ObtenerUsuarioPorDNI_750VR(int.Parse(dniSeleccionado));

                if (resultado != null)
                {
                    // Cargar los campos con los datos del usuario
                    txtDNI.Text = resultado.dni_750VR.ToString();
                    txtnom.Text = resultado.nombre_750VR;
                    txtape.Text = resultado.apellido_750VR;
                    txtemail.Text = resultado.mail_750VR;
                    cmbrol.Text = resultado.rol_750VR;
                    txtuser.Text = resultado.user_750VR;

                    bloqsi.Checked = resultado.bloqueado_750VR;
                    bloqno.Checked = !resultado.bloqueado_750VR;
                    actsi.Checked = resultado.activo_750VR;
                    actno.Checked = !resultado.activo_750VR;

                    btncancelar.Enabled = true;
                    btncrear.Enabled = false;
                }
                else
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormGestionUsuario_750VR.ErrorRecuperarDatos"));
                }
            }
        }
    }
}
