using BE_VR750;
using BLL_VR750;
using SERVICIOS_VR750;
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
    public partial class FormABMservicios_750VR : Form, Iobserver_750VR
    {
        private string modoActual = "consulta";
        public FormABMservicios_750VR()
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
            string clave = "FormABMdisponibilidad.Mensaje." + modoActual;
            lblmensaje.Text = Lenguaje_750VR.ObtenerEtiqueta(clave);
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
         
        }

        private void btnañadir_Click(object sender, EventArgs e)
        {
            //lblmensaje.Text = "Modo Añadir";
            modoActual = "añadir";
            ActualizarMensajeModo();
            ActivarModoEdicion();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            var mensaje = Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.ConfirmarSalidaMensaje");
            var titulo = Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.ConfirmarSalidaTitulo");

            var result = MessageBox.Show(mensaje, titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
                this.Close();
        }

        private void btnapli_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos() && modoActual != "Activar/Desactivar")
                return;

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

            modoActual = "consulta";
            //lblmensaje.Text = "Modo Consulta";
            ActualizarMensajeModo();
            ResetearEstadoInterfaz();
            CargarServicios();
            LimpiarCampos();
        }

        private void CargarServicios()
        {
            var bll = new BLLServicio_750VR();
            var lista = bll.LeerEntidades_750VR();

            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = lista;


            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "nombre_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Servicio.Nombre"),
                Name = "colNombre"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "tecnica_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Servicio.Tecnica"),
                Name = "colTecnica"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "duracion_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Servicio.Duracion"),
                Name = "colDuracion"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "precio_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Servicio.Precio"),
                Name = "colPrecio"
            });

            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "activo_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Servicio.Estado"),
                Name = "colEstado"
            });

            PintarServiciosInactivos();
        }



        private void PintarServiciosInactivos()
        {
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.DataBoundItem is BEServicio_750VR usuario && !usuario.activo_750VR)
                {
                    fila.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void AplicarActivarDesactivar()
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.SeleccioneServicio"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            BEServicio_750VR servicio = dataGridView1.CurrentRow.DataBoundItem as BEServicio_750VR;
            bool nuevoEstado = !servicio.activo_750VR;

            BLLServicio_750VR bll = new BLLServicio_750VR();
            bll.CambiarEstadoServicio_750VR(servicio.idServicio_750VR, nuevoEstado);

            string mensaje = nuevoEstado
                ? Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.ServicioActivado")
                : Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.ServicioDesactivado");

            MessageBox.Show(mensaje);
            CargarServicios();
            ResetearEstadoInterfaz();
            LimpiarCampos();
        }


        private void AplicarModificacion()
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.SeleccioneServicio"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!ValidarCampos())
                return;

            var servicioSeleccionado = dataGridView1.CurrentRow.DataBoundItem as BEServicio_750VR;
            if (servicioSeleccionado == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.ErrorObtenerServicio"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloError")
                );
                return;
            }

            string nombre = txtnombre.Text.Trim();
            string tecnica = txttec.Text.Trim();
            int duracion;
            decimal precio;

            if (!int.TryParse(txtduracion.Text.Trim(), out duracion))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.DuracionInvalida"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloError")
                );
                return;
            }

            if (!decimal.TryParse(txtprecio.Text.Trim(), out precio))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.PrecioInvalido"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloError")
                );
                return;
            }

            BLLServicio_750VR bll = new BLLServicio_750VR();
            bool exito = bll.ModificarServicio_750VR(servicioSeleccionado.idServicio_750VR, nombre, tecnica, duracion, precio);

            if (exito)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.ServicioModificado")
                );
                CargarServicios();
                ResetearEstadoInterfaz();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.ErrorModificarServicio"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloError")
                );
            }
        }


        private void AplicarAlta()
        {
            try
            {
                if (!ValidarCampos()) return;

                string nombre = txtnombre.Text.Trim();
                string tecnica = txttec.Text.Trim();

                var bll = new BLLServicio_750VR();
                var existentes = bll.LeerEntidades_750VR();
                bool yaExiste = existentes.Any(s =>
                    s.nombre_750VR.Equals(nombre, StringComparison.OrdinalIgnoreCase) &&
                    s.tecnica_750VR.Equals(tecnica, StringComparison.OrdinalIgnoreCase));

                if (yaExiste)
                {
                    MessageBox.Show(
                        Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.ServicioDuplicado"),
                        Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloAdvertencia"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                int duracion = int.Parse(txtduracion.Text);
                decimal precio = decimal.Parse(txtprecio.Text);

                BEServicio_750VR nuevo = new BEServicio_750VR(nombre, tecnica, duracion, precio, true);
                bll.CrearServicio_750VR(nuevo);

                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.ServicioCreado"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloInfo"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                LimpiarCampos();
                CargarServicios();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.ErrorCrearServicio") + ": " + ex.Message,
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtnombre.Text) ||
                string.IsNullOrWhiteSpace(txttec.Text) ||
                string.IsNullOrWhiteSpace(txtduracion.Text) ||
                string.IsNullOrWhiteSpace(txtprecio.Text))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.CamposObligatorios"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloAdvertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            if (!int.TryParse(txtduracion.Text, out _))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.DuracionInvalida"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloAdvertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            if (!decimal.TryParse(txtprecio.Text, out _))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.PrecioInvalido"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormABMservicios_750VR.TituloAdvertencia"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return false;
            }

            return true;
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

            ActualizarMensajeModo();
        }

        private void btncance_Click(object sender, EventArgs e)
        {
            //lblmensaje.Text = "Modo Consulta";
            modoActual = "consulta";
            ActualizarMensajeModo();

            // Limpiar todos los campos
            LimpiarCampos();

            ResetearEstadoInterfaz();
            CargarServicios();   // Mostrar todos
        }

        private void ActivarModoEdicion()
        {
            if (modoActual == "añadir")
            {
                dataGridView1.Enabled = false;
            }

            if (modoActual == "añadir" || modoActual == "modificar")
            {
                txtnombre.Enabled = true;
                txttec.Enabled = true;
                txtprecio.Enabled = true;
                txtduracion.Enabled = true;
            }
            else if (modoActual == "desbloquear" || modoActual == "Activar/Desactivar")
            {
                dataGridView1.Enabled = true;

                // Mostrar datos sin habilitar edición
                txtnombre.Enabled = false;
                txttec.Enabled = false;
                txtprecio.Enabled = false;
                txtduracion.Enabled = false;
             

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

            // Verifica si al menos un campo está completo
            bool hayDatos = !string.IsNullOrWhiteSpace(txtnombre.Text)
                         || !string.IsNullOrWhiteSpace(txttec.Text)
                         || !string.IsNullOrWhiteSpace(txtduracion.Text)
                         || !string.IsNullOrWhiteSpace(txtprecio.Text)
            //|| !string.IsNullOrWhiteSpace(txtcel.Text)
            //|| !string.IsNullOrWhiteSpace(txtdire.Text)


            ;

            btnapli.Enabled = hayDatos;
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
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            CargarServicios();

            // Deshabilitar botones Aplicar y Cancelar
            btnapli.Enabled = false;
            btncance.Enabled = false;

            // Habilitar grilla solo para selección (no edición)
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;


            // Iniciar en modo consulta
            modoActual = "consulta";
            //lblmensaje.Text = "Modo Consulta";
            ActualizarMensajeModo();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var servicio = dataGridView1.SelectedRows[0].DataBoundItem as BEServicio_750VR;
                if (servicio != null)
                {
                    txtnombre.Text = servicio.nombre_750VR;
                    txttec.Text = servicio.tecnica_750VR;
                    txtduracion.Text = servicio.duracion_750VR.ToString();
                    txtprecio.Text = servicio.precio_750VR.ToString("0.00");
                }
            }
        }
    }
}
