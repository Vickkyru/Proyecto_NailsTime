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
    public partial class FormABMClientes : Form, Iobserver_750VR
    {
        private string modoActual = "consulta";
        private string emailCifradoActual = "";

        public bool InvocadoDesdeReserva { get; set; } = false;
        public FormRegistrarReserva FormularioReserva { get; set; }

        public FormABMClientes()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
            ActivarModoEdicion();

        }
        
        private enum VistaDatos { BD, XML }
        private VistaDatos vistaActual = VistaDatos.BD;

        private readonly BLLserializar_750VR _bllSerializar = new BLLserializar_750VR();
        private readonly BLLdesserializar_750VR _bllDeserializar = new BLLdesserializar_750VR();

        
        private static readonly string CarpetaSerializacion =
            @"C:\Users\mavru\OneDrive\Escritorio\hoy\Proyecto_NailsTime\Proyecto_NailsTime\bin\Debug\Serializacion";

        private static void EnsureCarpetaSerializacion()
        {
            if (!System.IO.Directory.Exists(CarpetaSerializacion))
                System.IO.Directory.CreateDirectory(CarpetaSerializacion);
        }

        private static string RutaXmlPorDefecto()
        {
            EnsureCarpetaSerializacion();
            return System.IO.Path.Combine(CarpetaSerializacion, "Clientes_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xml");
        }
       
        private void MostrarListaEnGrilla(List<BECliente_750VR> lista, VistaDatos vista)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = lista;

            TraducirEncabezadosDataGrid();
            PintarUsuariosInactivos();

            vistaActual = vista;
            modoActual = "consulta";
            ResetearEstadoInterfaz();
            btnapli.Enabled = false;
            btncance.Enabled = false;
        }

        
        private List<BECliente_750VR> ObtenerClientesDeLaGrilla()
        {
            
            var tipada = dataGridView1.DataSource as IEnumerable<BECliente_750VR>;
            if (tipada != null) return tipada.ToList();

            
            var dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                var l = new List<BECliente_750VR>();
                foreach (DataRow r in dt.Rows)
                {
                    var cli = new BECliente_750VR
                    {
                        dni_750VR = Convert.ToInt32(r["dni_750VR"]),
                        nombre_750VR = Convert.ToString(r["nombre_750VR"]),
                        apellido_750VR = Convert.ToString(r["apellido_750VR"]),
                        gmail_750VR = Convert.ToString(r["gmail_750VR"]),
                        direccion_750VR = Convert.ToString(r["direccion_750VR"]),
                        celular_750VR = Convert.ToString(r["celular_750VR"]),
                        activo_750VR = Convert.ToBoolean(r["activo_750VR"])
                    };
                    l.Add(cli);
                }
                return l;
            }

            
            var listaGrid = new List<BECliente_750VR>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;
                var cli = new BECliente_750VR
                {
                    dni_750VR = Convert.ToInt32(row.Cells["dni_750VR"].Value),
                    nombre_750VR = Convert.ToString(row.Cells["nombre_750VR"].Value),
                    apellido_750VR = Convert.ToString(row.Cells["apellido_750VR"].Value),
                    gmail_750VR = Convert.ToString(row.Cells["gmail_750VR"].Value),
                    direccion_750VR = Convert.ToString(row.Cells["direccion_750VR"].Value),
                    celular_750VR = Convert.ToString(row.Cells["celular_750VR"].Value),
                    activo_750VR = Convert.ToBoolean(row.Cells["activo_750VR"].Value)
                };
                listaGrid.Add(cli);
            }
            return listaGrid;
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
            if (dataGridView1.SelectedRows.Count == 0) return;

            if (vistaActual == VistaDatos.XML)
            {
                
                var cli = dataGridView1.SelectedRows[0].DataBoundItem as BECliente_750VR;
                if (cli == null) return;

                txtdni.Text = cli.dni_750VR.ToString();
                txtnom.Text = cli.nombre_750VR;
                txtape.Text = cli.apellido_750VR;

                emailCifradoActual = cli.gmail_750VR;
                txtemail.Text = emailCifradoActual;
                checkBox1.Checked = false;

                txtcel.Text = cli.celular_750VR?.ToString();
                txtdire.Text = cli.direccion_750VR;
                return;
            }

          
            string dniSeleccionado = dataGridView1.SelectedRows[0].Cells["dni_750VR"].Value.ToString();
            BLLCliente_750VR bll = new BLLCliente_750VR();
            var cliente = bll.ObtenerClientePorDNI_750VR(Convert.ToInt32(dniSeleccionado));

            if (cliente != null)
            {
                txtdni.Text = cliente.dni_750VR.ToString();
                txtnom.Text = cliente.nombre_750VR;
                txtape.Text = cliente.apellido_750VR;

                emailCifradoActual = cliente.gmail_750VR;
                txtemail.Text = emailCifradoActual;
                checkBox1.Checked = false;

                txtcel.Text = cliente.celular_750VR?.ToString();
                txtdire.Text = cliente.direccion_750VR;
            }
            else
            {
                MessageBox.Show(
       Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.NoClienteEncontrado"),
       Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.TituloError"),
       MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnañadir_Click(object sender, EventArgs e)
        {
            btnelim.Enabled = false;
            btnmod.Enabled = false;
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

                PintarUsuariosInactivos();
                LimpiarCampos();
                return;
            }

           
            if (!ValidarCampos() && modoActual != "Activar/Desactivar" )
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

            // Volver al estado de consulta
            modoActual = "consulta";
            ActualizarMensajeModo();
            ActivarModoEdicion();
            
            //lblmensaje.Text = "Modo Consulta";
            ResetearEstadoInterfaz();
            bool mostrarSoloActivos = rbnActivos.Checked;
            CargarUsuarios(mostrarSoloActivos);
            LimpiarCampos();

        }

        private void CargarUsuarios(bool soloActivos)
        {
            var bll = new BLLCliente_750VR();
            var lista = soloActivos ? bll.leerEntidades_750VR().Where(c => c.activo_750VR).ToList() : bll.leerEntidades_750VR();

            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = lista;

            TraducirEncabezadosDataGrid();
            PintarUsuariosInactivos();

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
                    fila.DefaultCellStyle.BackColor = Color.LightCoral;
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

                bool mostrarSoloActivos = rbnActivos.Checked;
                CargarUsuarios(mostrarSoloActivos);
                vistaActual = VistaDatos.BD;
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

                bool mostrarSoloActivos = rbnActivos.Checked;
                CargarUsuarios(mostrarSoloActivos);
                vistaActual = VistaDatos.BD;
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
                vistaActual = VistaDatos.BD;
                LimpiarCampos();
                bool mostrarSoloActivos = rbnActivos.Checked;
                CargarUsuarios(mostrarSoloActivos);
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
            btnañadir.Enabled = false;
            btnmod.Enabled = false;
            //lblmensaje.Text = "Modo Activar/Desactivar";
            modoActual = "Activar/Desactivar";
            ActualizarMensajeModo();
            ActivarModoEdicion();
        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            btnelim.Enabled = false;
            btnañadir.Enabled = false;
            modoActual = "modificar";
            ActualizarMensajeModo();
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
            bool mostrarSoloActivos = rbnActivos.Checked;
            CargarUsuarios(mostrarSoloActivos);
        }

        private void ActivarModoEdicion()
        {
            if (modoActual == "añadir")
            {
                dataGridView1.Enabled = false;
                btnapli.Enabled = true;
                btncance.Enabled = true;
               

                txtdni.Enabled = modoActual == "añadir";
                txtnom.Enabled = true;
                txtape.Enabled = true;
                txtemail.Enabled = true;
                txtcel.Enabled = true;
                txtdire.Enabled = true;

            }
            if (modoActual == "modificar")
            {
                dataGridView1.Enabled = false;
                btnapli.Enabled = true;
                btncance.Enabled = true;
                btnelim.Enabled = false;
                btnañadir.Enabled = false;

                txtdni.Enabled = modoActual == "añadir";
                txtnom.Enabled = true;
                txtape.Enabled = true;
                txtemail.Enabled = true;
                txtcel.Enabled = true;
                txtdire.Enabled = true;

            }


            if (modoActual == "Activar/Desactivar")
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
                btnañadir.Enabled = false;
                btnmod.Enabled = false;
            }

            if(modoActual == "desserializar")
            {

            }

            if (modoActual == "actualizar")
            {

            }


            //btnapli.Enabled = false;
            //btncance.Enabled = false;


        }

        private void ResetearEstadoInterfaz()
        {
            
            txtdni.Enabled = txtnom.Enabled = txtape.Enabled = txtemail.Enabled = true;
            txtdire.Enabled = txtcel.Enabled = true;

            
            btnañadir.Enabled = btnmod.Enabled = true;
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
                modoActual = "añadir";
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMClientes_750VR.MensajeAltaDesdeReserva"));
                rbnActivos.Checked = true;
                CargarUsuarios(true);
                ActivarModoEdicion();
                btnmod.Enabled = btnelim.Enabled = false;
                dataGridView1.Enabled = false;
                btncance.Enabled = false;
            }
            else
            {
                modoActual = "consulta";
                rbnActivos.Checked = true;
                CargarUsuarios(true);
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
            }

            // Serialización: carpeta y ruta por defecto
            EnsureCarpetaSerializacion();
            var ctrl = this.Controls.Find("txtRutaExport", true);
            if (ctrl != null && ctrl.Length > 0 && ctrl[0] is TextBox)
                ((TextBox)ctrl[0]).Text = RutaXmlPorDefecto();

            vistaActual = VistaDatos.BD;
            ActualizarIdioma();
            ActivarModoEdicion();
            PintarUsuariosInactivos();
        }

 
    

      
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txtemail.Text = DesencriptarEmail(emailCifradoActual); // Mostrar desencriptado
            }
            else
            {
                txtemail.Text = emailCifradoActual; // Mostrar cifrado
            }
        }

       
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

        private void rbnActivos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnActivos.Checked)
                CargarUsuarios(true);
        }

        private void rbnTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnTodos.Checked)
                CargarUsuarios(false);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnExportar_Click_1(object sender, EventArgs e)
        {
            try
            {
                EnsureCarpetaSerializacion();
                var txt = this.Controls.Find("txtRutaExport", true).FirstOrDefault() as TextBox;
                string destino = (txt != null && !string.IsNullOrWhiteSpace(txt.Text)) ? txt.Text : RutaXmlPorDefecto();

                var lista = ObtenerClientesDeLaGrilla();
                _bllSerializar.ExportarClientes(destino, lista);

                if (txt != null) txt.Text = destino;
                MessageBox.Show(
    Lenguaje_750VR.ObtenerEtiqueta("Clientes.Export.Ok"),
    Lenguaje_750VR.ObtenerEtiqueta("Dialogo.XML.Titulo"),
    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
     Lenguaje_750VR.ObtenerEtiqueta("Clientes.Export.Error") + ex.Message,
     Lenguaje_750VR.ObtenerEtiqueta("Dialogo.XML.Titulo"),
     MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportar_Click_1(object sender, EventArgs e)
        {
            try
            {
                var txt = this.Controls.Find("txtRutaImport", true).FirstOrDefault() as TextBox;
                if (txt == null || string.IsNullOrWhiteSpace(txt.Text))
                {
                    MessageBox.Show(
       Lenguaje_750VR.ObtenerEtiqueta("Clientes.Import.SeleccioneXML"),
       Lenguaje_750VR.ObtenerEtiqueta("Dialogo.XML.Titulo"),
       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var clientes = _bllDeserializar.ImportarClientes(txt.Text);

                // Mostrar XML en grilla y dejar todo en modo consulta (sin bloquear)
                MostrarListaEnGrilla(clientes, VistaDatos.XML);

                MessageBox.Show(
    Lenguaje_750VR.ObtenerEtiqueta("Clientes.Import.ViendoXML"),
    Lenguaje_750VR.ObtenerEtiqueta("Dialogo.XML.Titulo"),
    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
     Lenguaje_750VR.ObtenerEtiqueta("Clientes.Import.Error") + ex.Message,
     Lenguaje_750VR.ObtenerEtiqueta("Dialogo.XML.Titulo"),
     MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            modoActual = "desserializar";
            ActivarModoEdicion();
        }

        private void btnBuscarExport_Click_1(object sender, EventArgs e)
        {
            EnsureCarpetaSerializacion();
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = Lenguaje_750VR.ObtenerEtiqueta("Dialogo.XML.Filtro");
                sfd.InitialDirectory = CarpetaSerializacion;
                sfd.FileName = "Clientes_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xml";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var txt = this.Controls.Find("txtRutaExport", true).FirstOrDefault() as TextBox;
                    if (txt != null) txt.Text = sfd.FileName;
                }
            }
        }

        private void btnBuscarImport_Click_1(object sender, EventArgs e)
        {
            EnsureCarpetaSerializacion();
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = Lenguaje_750VR.ObtenerEtiqueta("Dialogo.XML.Filtro");
                ofd.InitialDirectory = CarpetaSerializacion;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var txt = this.Controls.Find("txtRutaImport", true).FirstOrDefault() as TextBox;
                    if (txt != null) txt.Text = ofd.FileName;
                }
            }
        }

        private void btnactualizar_Click(object sender, EventArgs e)
        {
            vistaActual = VistaDatos.BD;
            CargarUsuarios(rbnActivos.Checked);
            modoActual = "consulta";
            ResetearEstadoInterfaz();
            modoActual = "actualizar";
            ActivarModoEdicion();

            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("Clientes.Grid.Actualizada"));
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            txtRutaExport.Clear();
            txtRutaImport.Clear();
        }
    }
}
