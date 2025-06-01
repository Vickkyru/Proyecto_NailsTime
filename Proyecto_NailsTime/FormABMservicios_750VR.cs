using BE_VR750;
using BLL_VR750;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_NailsTime
{
    public partial class FormABMservicios_750VR : Form
    {
        private string modoActual = "consulta";
        public FormABMservicios_750VR()
        {
            InitializeComponent();
        }


        public void LimpiarCampos()
        {
            txtnombre.Clear();
            txtduracion.Clear();
            txtprecio.Clear();
            txttec.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count > 0)
            //{
            //    string dniSeleccionado = dataGridView1.SelectedRows[0].Cells["dni_750VR"].Value.ToString();

            //    BLLServicio_750VR bll = new BLLServicio_750VR();
            //    var cliente = bll.ObtenerClientePorDNI_750VR(Convert.ToInt32(dniSeleccionado));

            //    if (cliente != null)
            //    {
            //        txtprecio.Text = cliente.precio_750VR.ToString();
            //        txtnombre.Text = cliente.nombre_750VR;
            //        txtduracion.Text = cliente.apellido_750VR;
            //        cmbtec.Text = cliente.gmail_750VR;
            //    }
            //    else
            //    {
            //        MessageBox.Show("No se encontró el cliente.");
            //    }
            //}
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
        //    // Si estamos en modo consulta, se hace la búsqueda
        //    if (modoActual == "consulta")
        //    {
        //        BLLCliente_750VR bll = new BLLCliente_750VR();
        //        var resultados = bll.BuscarClientes_750VR(
        //    string.IsNullOrWhiteSpace(txtdni.Text) ? null : txtdni.Text,
        //    string.IsNullOrWhiteSpace(txtnom.Text) ? null : txtnom.Text,
        //    string.IsNullOrWhiteSpace(txtape.Text) ? null : txtape.Text,
        //    string.IsNullOrWhiteSpace(txtemail.Text) ? null : txtemail.Text,
        //    string.IsNullOrWhiteSpace(txtdire.Text) ? null : txtdire.Text,
        //    string.IsNullOrWhiteSpace(txtcel.Text) ? null : txtcel.Text
        //);

        //        dataGridView1.DataSource = resultados;

        //        btncance.Enabled = false;

        //        //PintarUsuariosInactivos();
        //        LimpiarCampos();
        //        return;
        //    }

        //    // Validar campos solo si no estamos eliminando ni desbloqueando
        //    if (!ValidarCampos() && modoActual != "Activar/Desactivar" && modoActual != "desbloquear")
        //        return;

        //    // Ejecutar la acción según el modo
        //    switch (modoActual)
        //    {
        //        case "añadir":
        //            AplicarAlta();
        //            break;
        //        case "modificar":
        //            AplicarModificacion();
        //            break;
        //        case "Activar/Desactivar":
        //            AplicarActivarDesactivar();
        //            break;

        //    }

        //    // Volver al estado de consulta
        //    modoActual = "consulta";
        //    lblmensaje.Text = "Modo Consulta";
        //    ResetearEstadoInterfaz();
        //    CargarUsuarios(); // Refrescar grilla general
        //    LimpiarCampos();

        }

        private void CargarUsuarios()
        {
            var bll = new BLLServicio_750VR();
            var lista = bll.LeerEntidades_750VR();

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

            int precio = int.Parse(txtprecio.Text);
            string nombre = txtnombre.Text;
        
            int cel = int.Parse(txtduracion.Text);
            string tecnica = txttec.Text;

            BLLCliente_750VR bll = new BLLCliente_750VR();


            //BEusuario_750VR original = bll.ObtenerUsuarioPorLogin_750VR(mail);



            //bool exito = bll.ModificarCliente_750VR(precio, nombre, tecnica, cel);

            //if (exito)
            //{
            //    MessageBox.Show("Usuario modificado correctamente.");

            //    CargarUsuarios();
            //    ResetearEstadoInterfaz();
            //    LimpiarCampos();
            //}
            //else
            //{
            //    MessageBox.Show("Error al modificar el usuario.");
            //}
        }

        private void AplicarAlta()
        {
            try
            {
                //// Validamos primero
                //if (!ValidarCampos())
                //    return;
                //int dni = Convert.ToInt32(txtdni.Text);
                //string nombre = txtnom.Text.Trim();
                //string apellido = txtape.Text.Trim();
                //string mail = txtemail.Text.Trim();
                //int cel = int.Parse(txtcel.Text);
                //string dire = txtdire.Text;

                //BLLCliente_750VR bll = new BLLCliente_750VR();

                // Verificar existencia por DNI
                //if (bll.ObtenerClientePorLogin_750VR(mail) != null)
                //{
                //    MessageBox.Show("Ya existe un usuario con ese mail.");
                //    return;
                //}

                // Verificar existencia por mail/login
                //if (bll.ObtenerClientePorDNI_750VR(dni) != null)
                //{
                //    MessageBox.Show("Ya existe un cliente con ese DNI.");
                //    return;

                //}


                //BECliente_750VR nuevo = new BECliente_750VR
                //{
                //    dni_750VR = dni,
                //    nombre_750VR = nombre,
                //    apellido_750VR = apellido,
                //    gmail_750VR = mail,
                //    celular_750VR = cel,
                //    direccion_750VR = dire,
                //    activo_750VR = true,
                //};

                //bll.CrearCliente_750VR(nuevo);
                //MessageBox.Show("Cliente creado correctamente.");


                LimpiarCampos();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtnombre.Text) ||
       string.IsNullOrWhiteSpace(txtduracion.Text) ||
       string.IsNullOrWhiteSpace(txtprecio.Text) ||
       string.IsNullOrWhiteSpace(txttec.Text))
                return true; return false;
      
  

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

            //if (modoActual == "añadir" || modoActual == "modificar")
            //{
            //    txtdni.Enabled = modoActual == "añadir";
            //    txtnom.Enabled = true;
            //    txtape.Enabled = true;
            //    txtemail.Enabled = true;
            //    txtcel.Enabled = true;
            //    txtdire.Enabled = true;
            //}
            //else if (modoActual == "desbloquear" || modoActual == "Activar/Desactivar")
            //{
            //    dataGridView1.Enabled = true;

            //    // Mostrar datos sin habilitar edición
            //    txtdni.Enabled = false;
            //    txtnom.Enabled = false;
            //    txtape.Enabled = false;
            //    txtemail.Enabled = false;
            //    txtdire.Enabled = false;
            //    txtcel.Enabled = false;

            //}

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
            //// Campos de texto deshabilitados
            //txtdni.Enabled = txtnom.Enabled = txtape.Enabled = txtemail.Enabled = true;
            //txtdire.Enabled = txtcel.Enabled = true;

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

            //// Verifica si al menos un campo está completo
            //bool hayDatos = !string.IsNullOrWhiteSpace(txtdni.Text)
            //             || !string.IsNullOrWhiteSpace(txtnom.Text)
            //             || !string.IsNullOrWhiteSpace(txtape.Text)
            //             || !string.IsNullOrWhiteSpace(txtemail.Text)
            //|| !string.IsNullOrWhiteSpace(txtcel.Text)
            //|| !string.IsNullOrWhiteSpace(txtdire.Text)


            ;

            //btnapli.Enabled = hayDatos;
        }

        private void txtnombre_TextChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void cmbtec_SelectedIndexChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void txtduracion_TextChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void txtprecio_TextChanged(object sender, EventArgs e)
        {
            VerificarCamposBusqueda();
        }

        private void lblmensaje_Click(object sender, EventArgs e)
        {

        }

        private void FormABMservicios_750VR_Load(object sender, EventArgs e)
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
