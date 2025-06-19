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
    public partial class FormABMClientes_750VR : Form, Iobserver_750VR
    {
        private string modoActual = "consulta";
 
        public FormABMClientes_750VR()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
            
        }

        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
            ActualizarMensajeModo();
        }

        private void ActualizarMensajeModo()
        {
            string clave = "FormABMClientes_750VR.Mensaje." + modoActual;
            lblmensaje.Text = Lenguaje_750VR.ObtenerEtiqueta(clave);
        }


        
        public bool InvocadoDesdeReserva { get; set; } = false;
        public FormRegistrarReserva_750VR FormularioReserva { get; set; }
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

                    emailCifradoActual = cliente.gmail_750VR;
                    txtemail.Text = checkBox1.Checked ? DesencriptarEmail(emailCifradoActual) : "[Email protegido]";

                    txtcel.Text = cliente.celular_750VR.ToString();
                    txtdire.Text = cliente.direccion_750VR;
                }
                else
                {
                    //MessageBox.Show("No se encontró el cliente.");
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.NoClienteEncontrado");
                }
            }
        }

        private void btnañadir_Click(object sender, EventArgs e)
        {
            
            modoActual = "añadir";
            ActualizarMensajeModo();
            ActivarModoEdicion();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
    Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.ConfirmarSalidaMensaje"),
    Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.ConfirmarSalidaTitulo"),
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Warning
);


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
            ActualizarMensajeModo();
            ActivarModoEdicion();
            
            //lblmensaje.Text = "Modo Consulta";
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


            PintarUsuariosInactivos();
            TraducirEncabezadosDataGrid();

        }
        private void TraducirEncabezadosDataGrid()
        {
            Dictionary<string, string> columnasTraducidas = new Dictionary<string, string>
    {
        { "dni_750VR", Lenguaje_750VR.ObtenerEtiqueta("Grid.Cliente.DNI") },
        { "nombre_750VR", Lenguaje_750VR.ObtenerEtiqueta("Grid.Cliente.Nombre") },
        { "apellido_750VR", Lenguaje_750VR.ObtenerEtiqueta("Grid.Cliente.Apellido") },
        { "gmail_750VR", Lenguaje_750VR.ObtenerEtiqueta("Grid.Cliente.Email") },
        { "direccion_750VR", Lenguaje_750VR.ObtenerEtiqueta("Grid.Cliente.Direccion") },
        { "celular_750VR", Lenguaje_750VR.ObtenerEtiqueta("Grid.Cliente.Celular") },
        { "activo_750VR", Lenguaje_750VR.ObtenerEtiqueta("Grid.Cliente.Estado") }
    };

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (columnasTraducidas.ContainsKey(col.DataPropertyName))
                {
                    col.HeaderText = columnasTraducidas[col.DataPropertyName];
                }
            }
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
            var item = dataGridView1.CurrentRow?.DataBoundItem as BECliente_750VR;
            if (item == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.SeleccioneClienteValido"));
                return;
            }

            bool nuevoEstado = !item.activo_750VR;

            BLLCliente_750VR bll = new BLLCliente_750VR();
            bool exito = bll.CambiarEstadoCliente_750VR(item.dni_750VR, nuevoEstado);

            if (exito)
            {
                string clave = nuevoEstado
                    ? "FormABMClientes_750VR.ClienteActivado"
                    : "FormABMClientes_750VR.ClienteDesactivado";

                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta(clave));

                CargarUsuarios();
                ResetearEstadoInterfaz();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.ErrorCambiarEstado"));
            }
        }

        private void AplicarModificacion()
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.SeleccioneCliente"));
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

            bool exito = bll.ModificarCliente_750VR(dni, nombre, apellido, mail, dire, cel);

            if (exito)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.ClienteModificado"));

                CargarUsuarios();
                ResetearEstadoInterfaz();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.ErrorModificarCliente"));
            }
        }

        private void AplicarAlta()
        {
            try
            {
                if (!ValidarCampos())
                    return;

                int dni = Convert.ToInt32(txtdni.Text);
                string nombre = txtnom.Text.Trim();
                string apellido = txtape.Text.Trim();
                string dire = txtdire.Text.Trim();
                string cel = txtcel.Text.Trim();
                string mail = txtemail.Text.Trim();

                if (string.IsNullOrWhiteSpace(mail) || mail.StartsWith("["))
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.EmailInvalido"));
                    return;
                }

                BLLCliente_750VR bll = new BLLCliente_750VR();

                if (bll.ObtenerClientePorDNI_750VR(dni) != null)
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.ClienteYaExiste"));
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

                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.ClienteCreado"));
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.ErrorCrearCliente") + ex.Message,
                                Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.TituloError"),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.CamposObligatorios"));
                return false;
            }

            string dniPattern = @"^(\d{7,8}|\d{2}\.\d{3}\.\d{3})$";
            if (!Regex.IsMatch(txtdni.Text.Trim(), dniPattern))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.DNIInvalido"));
                return false;
            }

            if (!EsEmailValido(txtemail.Text.Trim()))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.EmailInvalido"));
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

        private void btnelim_Click(object sender, EventArgs e)
        {
           
            //lblmensaje.Text = "Modo Activar/Desactivar";
            modoActual = "Activar/Desactivar";
            ActualizarMensajeModo();
            ActivarModoEdicion();
        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            modoActual = "modificar";
            ActivarModoEdicion();
            //lblmensaje.Text = "Modo Modificar";
        }

        private void btncance_Click(object sender, EventArgs e)
        {
           
            modoActual = "consulta";
            ActualizarMensajeModo();
            ActivarModoEdicion();
            

            LimpiarCampos();

            ResetearEstadoInterfaz();
            CargarUsuarios();  
        }

        private void ActivarModoEdicion()
        {
            if (modoActual == "añadir")
            {
                dataGridView1.Enabled = false;
                btnapli.Enabled = true;
                btncance.Enabled = true;
            }

            if (modoActual == "añadir" || modoActual == "modificar")
            {
                txtdni.Enabled = modoActual == "añadir";
                txtnom.Enabled = true;
                txtape.Enabled = true;
                txtemail.Enabled = true;
                txtcel.Enabled = true;
                txtdire.Enabled = true;

                btnapli.Enabled = true;
                btncance.Enabled = true;
            }
            else if (modoActual == "desbloquear" || modoActual == "Activar/Desactivar")
            {
                dataGridView1.Enabled = true;

                txtdni.Enabled = false;
                txtnom.Enabled = false;
                txtape.Enabled = false;
                txtemail.Enabled = false;
                txtdire.Enabled = false;
                txtcel.Enabled = false;

                btnapli.Enabled = true;
                btncance.Enabled = true;

            }

           
            //btnapli.Enabled = false;
            //btncance.Enabled = false;

            
            btnañadir.Enabled = true;
            btnmod.Enabled = true;
            btnelim.Enabled = true;

        }

        private void ResetearEstadoInterfaz()
        {
            
            txtdni.Enabled = txtnom.Enabled = txtape.Enabled = txtemail.Enabled = true;
            txtdire.Enabled = txtcel.Enabled = true;

            
            btnañadir.Enabled = btnmod.Enabled = /*btnelim.Enabled =*/ true;
 
            btnelim.Enabled = true;

           
            btnapli.Enabled = false;
            btncance.Enabled = false;

           
            dataGridView1.Enabled = true;

        }

        private void VerificarCamposBusqueda() 
        {
            if (modoActual != "consulta") return;

            
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
           

            if (InvocadoDesdeReserva)
            {
                btnmod.Enabled = false;
                btnelim.Enabled = false;
                btnañadir.Enabled = true;
                //btn.Enabled = false;
                //btndesb.Enabled = false;
                
                dataGridView1.Enabled = false;

                modoActual = "añadir";
                //lblmensaje.Text = "Alta desde Reserva";
                ActualizarMensajeModo();
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.MensajeAltaDesdeReserva"));

                ActivarModoEdicion();
                btncance.Enabled = false;
            }
            else
            {
                
                btnapli.Enabled = false;
                btncance.Enabled = false;

               
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;



               
                    modoActual = "consulta";           // 🔁 PRIMERO asignás el modo correcto
                    ActualizarMensajeModo();           // ✅ Luego actualizás el label según ese modo
                    ActivarModoEdicion();
                


            }

            CargarUsuarios();
            ActualizarIdioma();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtemail.Text = checkBox1.Checked ? DesencriptarEmail(emailCifradoActual) : "[Email protegido]";
        }

        private string emailCifradoActual = "";
        private string DesencriptarEmail(string texto)
        {
            try
            {
                var encriptador = new Encriptador_750VR();
                return encriptador.DesencriptarAES_750VR(texto);
            }
            catch
            {
                return "[ERROR AL DESENCRIPTAR]";
            }
        }

    }
}
