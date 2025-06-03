using BE_VR750;
using BLL_VR750;
using SERVICIOS_VR750;
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

namespace Proyecto_NailsTime
{
    public partial class FormABMClientes_750VR : Form
    {
        private string modoActual = "consulta";
 
        public FormABMClientes_750VR()
        {
            InitializeComponent();
  
        }
        // Propiedades para comunicación con el formulario de reserva
        public bool InvocadoDesdeReserva { get; set; } = false;
        public FormRegistrarReserva_750VR FormularioReserva { get; set; } = null;
        public void LimpiarCampos()
        {
            txtdni.Clear();
            txtnom.Clear();
            txtape.Clear();
            txtemail.Clear();
            txtdire.Clear();
            txtcel.Clear();

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string dniSeleccionado = dataGridView1.SelectedRows[0].Cells["dni_750VR"].Value.ToString();

                BLLCliente_750VR bll = new BLLCliente_750VR();
                var cliente = bll.ObtenerClientePorDNI_750VR(Convert.ToInt32(dniSeleccionado));

                if (cliente != null)
                {
                    txtdni.Text = cliente.dni_750VR.ToString();
                    txtnom.Text = cliente.nombre_750VR;
                    txtape.Text = cliente.apellido_750VR;
                    txtemail.Text = cliente.gmail_750VR;
                    txtcel.Text = cliente.celular_750VR.ToString();
                    txtdire.Text = cliente.direccion_750VR;
                }
                else
                {
                    MessageBox.Show("No se encontró el cliente.");
                }
            }
        }

        private void btnañadir_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Añadir";
            modoActual = "añadir";
            ActivarModoEdicion();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea salir? Se perderán los cambios no guardados.",
                              "Confirmar salida",
                              MessageBoxButtons.YesNo,
                              MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                this.Close();
        }

        private void btnapli_Click(object sender, EventArgs e)
        {
            // Si estamos en modo consulta, se hace la búsqueda
            if (modoActual == "consulta")
            {
                BLLCliente_750VR bll = new BLLCliente_750VR();
                var resultados = bll.BuscarClientes_750VR(
            string.IsNullOrWhiteSpace(txtdni.Text) ? null : txtdni.Text,
            string.IsNullOrWhiteSpace(txtnom.Text) ? null : txtnom.Text,
            string.IsNullOrWhiteSpace(txtape.Text) ? null : txtape.Text,
            string.IsNullOrWhiteSpace(txtemail.Text) ? null : txtemail.Text,
            string.IsNullOrWhiteSpace(txtdire.Text) ? null : txtdire.Text,
            string.IsNullOrWhiteSpace(txtcel.Text) ? null : txtcel.Text
        );

                dataGridView1.DataSource = resultados;

                btncance.Enabled = false;
 
                //PintarUsuariosInactivos();
                LimpiarCampos();
                return;
            }

            // Validar campos solo si no estamos eliminando ni desbloqueando
            if (!ValidarCampos() && modoActual != "Activar/Desactivar" && modoActual != "desbloquear")
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
 
            }

            // Volver al estado de consulta
            modoActual = "consulta";
            lblmensaje.Text = "Modo Consulta";
            ResetearEstadoInterfaz();
            CargarUsuarios(); // Refrescar grilla general
            LimpiarCampos();

        }

        private void CargarUsuarios()
        {
            var bll = new BLLCliente_750VR();
            var lista = bll.leerEntidades_750VR();

            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = lista;


            PintarUsuariosInactivos(); // si querés seguir resaltando inactivos en rojo
        }

        private void PintarUsuariosInactivos()
        {
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.DataBoundItem is BECliente_750VR usuario && !usuario.activo_750VR)
                {
                    fila.DefaultCellStyle.BackColor = Color.Red;
                }
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

                CargarUsuarios();
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

            int dni = int.Parse(txtdni.Text);
            string nombre = txtnom.Text;
            string apellido = txtape.Text;
            string mail = txtemail.Text;
            int cel = int.Parse(txtcel.Text);
            string dire = txtdire.Text;

            BLLCliente_750VR bll = new BLLCliente_750VR();


            //BEusuario_750VR original = bll.ObtenerUsuarioPorLogin_750VR(mail);

           

            bool exito = bll.ModificarCliente_750VR(dni, nombre, apellido, mail, dire, cel);

            if (exito)
            {
                MessageBox.Show("Usuario modificado correctamente.");

                CargarUsuarios();
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
                int dni = Convert.ToInt32(txtdni.Text);
                string nombre = txtnom.Text.Trim();
                string apellido = txtape.Text.Trim();
                string mail = txtemail.Text.Trim();
                int cel = int.Parse(txtcel.Text);
                string dire = txtdire.Text;

                BLLCliente_750VR bll = new BLLCliente_750VR();

              //falta validar el resto
                if (bll.ObtenerClientePorDNI_750VR(dni) != null)
                {
                    MessageBox.Show("Ya existe un cliente con ese DNI.");
                    return;

                }


                BECliente_750VR nuevo = new BECliente_750VR(
       dni: dni,
       nom: nombre,
       ape: apellido,
       gmail: mail,
       dire: dire,
       celu: cel,
       act: true
   );

                bll.CrearCliente_750VR(nuevo);

                if (InvocadoDesdeReserva && FormularioReserva != null)
                {
                    FormularioReserva.CompletarCamposCliente(txtdni.Text, txtnom.Text);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cliente creado correctamente.");
                    LimpiarCampos();
                    CargarUsuarios(); 
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtdni.Text) ||
       string.IsNullOrWhiteSpace(txtnom.Text) ||
       string.IsNullOrWhiteSpace(txtape.Text) ||
       string.IsNullOrWhiteSpace(txtemail.Text) ||
       string.IsNullOrWhiteSpace(txtdire.Text) ||
     string.IsNullOrWhiteSpace(txtcel.Text))

            //falta el resto
            {
                MessageBox.Show("Por favor, complete todos los campos obligatorios.");
                return false;
            }
            // 2.2) DNI: 7–8 dígitos, con o sin puntos (12.345.678 o 12345678)
            string dniPattern = @"^(\d{7,8}|\d{2}\.\d{3}\.\d{3})$";
            if (!Regex.IsMatch(txtdni.Text.Trim(), dniPattern))
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

        private void btnelim_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Activar/Desactivar";
            modoActual = "Activar/Desactivar";
            ActivarModoEdicion();
        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            modoActual = "modificar";
            ActivarModoEdicion();
            lblmensaje.Text = "Modo Modificar";
        }

        private void btncance_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Consulta";
            modoActual = "consulta";

            // Limpiar todos los campos
            LimpiarCampos();

            ResetearEstadoInterfaz();
            CargarUsuarios();   // Mostrar todos
        }

        private void ActivarModoEdicion()
        {
            if (modoActual == "añadir")
            {
                dataGridView1.Enabled = false;
            }

            if (modoActual == "añadir" || modoActual == "modificar")
            {
                txtdni.Enabled = modoActual == "añadir";
                txtnom.Enabled = true;
                txtape.Enabled = true;
                txtemail.Enabled = true;
                txtcel.Enabled = true;
                txtdire.Enabled = true;
            }
            else if (modoActual == "desbloquear" || modoActual == "Activar/Desactivar")
            {
                dataGridView1.Enabled = true;

                // Mostrar datos sin habilitar edición
                txtdni.Enabled = false;
                txtnom.Enabled = false;
                txtape.Enabled = false;
                txtemail.Enabled = false;
                txtdire.Enabled = false;
                txtcel.Enabled = false;
     
            }

            // Habilitar botones Aplicar y Cancelar
            btnapli.Enabled = true;
            btncance.Enabled = true;

            // Deshabilitar botones de navegación
            btnañadir.Enabled = false;
            btnmod.Enabled = false;
            //btnelim.Enabled = false;

            btnelim.Enabled = false;

        }

        private void ResetearEstadoInterfaz()
        {
            // Campos de texto deshabilitados
            txtdni.Enabled = txtnom.Enabled = txtape.Enabled = txtemail.Enabled = true;
            txtdire.Enabled = txtcel.Enabled = true;

            // CRUD y filtros habilitados
            btnañadir.Enabled = btnmod.Enabled = /*btnelim.Enabled =*/ true;
 
            btnelim.Enabled = true;

            // Aplicar/Cancelar deshabilitados
            btnapli.Enabled = false;
            btncance.Enabled = false;

            // Grilla habilitada para seleccionar
            dataGridView1.Enabled = true;

        }

        private void VerificarCamposBusqueda() //hasta q no se implementen los campos no se aplica
        {
            if (modoActual != "consulta") return;

            // Verifica si al menos un campo está completo
            bool hayDatos = !string.IsNullOrWhiteSpace(txtdni.Text)
                         || !string.IsNullOrWhiteSpace(txtnom.Text)
                         || !string.IsNullOrWhiteSpace(txtape.Text)
                         || !string.IsNullOrWhiteSpace(txtemail.Text)
            || !string.IsNullOrWhiteSpace(txtcel.Text)
            || !string.IsNullOrWhiteSpace(txtdire.Text)


            ;

            btnapli.Enabled = hayDatos;
        }

        private void txtdni_TextChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void txtape_TextChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void txtnom_TextChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void txtemail_TextChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void txtcel_TextChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void txtdire_TextChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void lblmensaje_Click(object sender, EventArgs e)
        {

        }

        private void FormABMClientes_750VR_Load(object sender, EventArgs e)
        {

            CargarUsuarios();

            // Deshabilitar botones Aplicar y Cancelar
            btnapli.Enabled = false;
            btncance.Enabled = false;

            // Habilitar grilla solo para selección (no edición)
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;


            // Iniciar en modo consulta
            modoActual = "consulta";
            lblmensaje.Text = "Modo Consulta";
        }
    }
}
