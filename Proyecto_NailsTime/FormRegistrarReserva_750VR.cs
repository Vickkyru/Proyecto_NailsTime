using BE_VR750;
using BLL_VR750;
using DAL_VR750;
using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace Proyecto_NailsTime
{
    public partial class FormRegistrarReserva : Form, Iobserver_750VR
    {
        private List<BEServicio_750VR> listaServicios;
        private List<BEusuario_750VR> listaUsuarios = new List<BEusuario_750VR>();
        private BLLdisponibilidad_750VR bll = new BLLdisponibilidad_750VR();

        public FormRegistrarReserva()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();

        }

        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }


        private void CargarServicios()
        {
            BLLServicio_750VR bllServicio = new BLLServicio_750VR();
            listaServicios = bllServicio.leerEntidadesActivas_750VR();

            var nombresServicio = listaServicios
                .Select(s => s.nombre_750VR)
                .Distinct()
                .ToList();


            nombresServicio.Insert(0, "");

            cmbserv.DataSource = nombresServicio;
            cmbserv.SelectedIndex = 0;
        }



        private void CargarDisponibilidades()
        {
            BLLdisponibilidad_750VR bllDispo = new BLLdisponibilidad_750VR();
            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();

            // Obtener manicuristas activos
            var manicuristasActivos = bllUsuario.ObtenerManicuristasActivos_750VR();
            var dnisActivos = manicuristasActivos.Select(u => u.dni_750VR).ToList();

            // Obtener todas las disponibilidades
            var listaDispo = bllDispo.LeerDisponibilidades_750VR();

            DataTable tabla = new DataTable();
            tabla.Columns.Add("IdDisponibilidad", typeof(int));
            tabla.Columns.Add("Manicurista", typeof(string));
            tabla.Columns.Add("DNImanicurista", typeof(int));
            tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("Hora Inicio", typeof(string));
            tabla.Columns.Add("Hora Fin", typeof(string));
            tabla.Columns.Add("Estado", typeof(string));

            // Cargar solo las disponibilidades activas y desocupadas de manicuristas activos
            foreach (var dispo in listaDispo.Where(d => d.activo_750VR && d.estado_750VR == false && dnisActivos.Contains(d.DNImanic_750VR)))
            {
                var usu = manicuristasActivos.FirstOrDefault(u => u.dni_750VR == dispo.DNImanic_750VR);
                string nombreCompleto = usu != null ? $"{usu.nombre_750VR} {usu.apellido_750VR}" : "Desconocido";

                tabla.Rows.Add(
                    dispo.CodDisponibilidad_750VR,
                    nombreCompleto,
                    dispo.DNImanic_750VR,
                    dispo.Fecha_750VR.Date,
                    dispo.HoraInicio_750VR.ToString(@"hh\:mm"),
                    dispo.HoraFin_750VR.ToString(@"hh\:mm"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_Disponible")
                );
            }

            dataGridView1.DataSource = tabla;

            // Ocultar columnas internas
            if (dataGridView1.Columns.Contains("IdDisponibilidad"))
                dataGridView1.Columns["IdDisponibilidad"].Visible = false;
            if (dataGridView1.Columns.Contains("DNImanicurista"))
                dataGridView1.Columns["DNImanicurista"].Visible = false;

            // Encabezados traducidos
            dataGridView1.Columns["Manicurista"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_Manicurista");
            dataGridView1.Columns["Fecha"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_Fecha");
            dataGridView1.Columns["Hora Inicio"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_HoraInicio");
            dataGridView1.Columns["Hora Fin"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_HoraFin");
            dataGridView1.Columns["Estado"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_Estado");
        }
        private void CargarManicuristas()
        {

            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();
            var manicuristas = bllUsuario.ObtenerManicuristasActivos_750VR();

            // Agrego opción "--Seleccione--"
            var vacio = new BEusuario_750VR(0, Lenguaje_750VR.ObtenerEtiqueta("ComboBox.Seleccione"), "", "", "", "", "", "manicurista", true, false, "Español");
            manicuristas.Insert(0, vacio);

            cmbmanic.DataSource = manicuristas;
            cmbmanic.DisplayMember = "nombre_750VR";
            cmbmanic.ValueMember = "dni_750VR";
            cmbmanic.SelectedIndex = 0;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        public void CompletarCamposCliente(string dni, string nombre)
        {
            txtdni.Text = dni;
            txtnom.Text = nombre;
        }
        private BECliente_750VR clienteSeleccionado;
        private BECliente_750VR ObtenerClienteDesdeFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtdni.Text))
            {
                //MessageBox.Show("Por favor, ingrese un DNI.");
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIRequerido"));
                return null;
            }

            if (!int.TryParse(txtdni.Text, out int dni))
            {
                //MessageBox.Show("El DNI ingresado no es válido.");
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIinvalido"));
                return null;
            }

            var bllcli = new BLLCliente_750VR();
            var cliente = bllcli.ObtenerClientePorDNI_750VR(dni);

            if (cliente == null)
            {
                //MessageBox.Show("Cliente no encontrado. Puede crearlo desde el botón correspondiente.");
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNInoencontrado"));
                return null;
            }

            return cliente;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbmanic.SelectedItem == null || cmbtec.SelectedItem == null || clienteSeleccionado == null)
                {
                    //MessageBox.Show("Faltan datos obligatorios o no seleccionaste un cliente.");
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIfaltan"));
                    return;
                }

                var manic = cmbmanic.SelectedItem as BEusuario_750VR;
                var servicio = cmbtec.SelectedItem as BEServicio_750VR;

                if (manic == null || servicio == null)
                {
                    //MessageBox.Show("Error al obtener manicurista o servicio.");
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIerror"));
                    return;
                }

                if (!(dataGridView1.CurrentRow?.DataBoundItem is DataRowView row))
                {
                    //MessageBox.Show("Debés seleccionar una disponibilidad.");
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIseleccion"));
                    return;
                }
                var disponibilidadSeleccionada = new BEdisponibilidad_750VR(
                    id: Convert.ToInt32(row["IdDisponibilidad"]),
                    dni: Convert.ToInt32(row["DNImanicurista"]),
                    fecha: Convert.ToDateTime(row["Fecha"]),
                    ini: TimeSpan.Parse(row["Hora Inicio"].ToString()),
                    fin: TimeSpan.Parse(row["Hora Fin"].ToString()),
                    acr: true,
                    est: false
                );

                if (!TimeSpan.TryParse(txthorario.Text, out TimeSpan horaManual))
                {
                    //MessageBox.Show("El formato del horario ingresado es inválido.");
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIformato"));
                    return;
                }

                TimeSpan horaFin = horaManual.Add(TimeSpan.FromMinutes(servicio.duracion_750VR));

                if (horaManual < disponibilidadSeleccionada.HoraInicio_750VR || horaFin > disponibilidadSeleccionada.HoraFin_750VR)
                {
                    //MessageBox.Show("La hora ingresada está fuera del rango disponible seleccionado.");
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIhora"));
                    return;
                }

                BEReserva_750VR nuevaReserva = new BEReserva_750VR(
        dnicli: clienteSeleccionado.dni_750VR,
        cli: clienteSeleccionado,
        dnimanic: manic.dni_750VR,
        manic: manic,
        idserv: servicio.CodServicio_750VR,
        serv: servicio,
        fecha: disponibilidadSeleccionada.Fecha_750VR,
        ini: horaManual,
        fin: horaFin,
        pre: servicio.precio_750VR,
        estado: "Pendiente",
        cobrado: false
    );


                var bllReserva = new BLLReserva_750VR();
                int nuevoID = bllReserva.CrearReserva_750VR(nuevaReserva);
                nuevaReserva.CodReserva_750VR = nuevoID;

                DividirDisponibilidad(disponibilidadSeleccionada, TimeSpan.FromMinutes(servicio.duracion_750VR));

                //MessageBox.Show("Reserva creada correctamente.");
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIcreada"));

                FormCobrarServici frmCobro = new FormCobrarServici(nuevaReserva.CodReserva_750VR);
                var resultado = frmCobro.ShowDialog();

                if (resultado == DialogResult.OK)
                {
                    //MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIcobrada"));
                }
                else
                {
                    // Si cancela el cobro, cambiar el estado y devolver disponibilidad
                    var bllReservaa = new BLLReserva_750VR();
                    bllReservaa.ActualizarEstadoReserva(nuevaReserva.CodReserva_750VR, "Cancelado");

                    var bllDispo = new BLLdisponibilidad_750VR();
                    var nuevaDispo = new BEdisponibilidad_750VR(
                        dni: nuevaReserva.DNImanic_750VR,
                        fecha: nuevaReserva.Fecha_750VR,
                        ini: nuevaReserva.HoraInicio_750VR,
                        fin: nuevaReserva.HoraFin_750VR,
                        acr: true,
                        est: false
                    );
                    bllDispo.CrearDisponibilidad_750VR(nuevaDispo);

                    //MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.MensajeReservaCancelada"));
                }

                CargarReservas();
                LimpiarCamposReserva();
                CargarReservasFiltradas();
                //CargarReservasDispo();
            }
            catch (Exception)
            {
                //MessageBox.Show("Error al crear la reserva: " + ex.Message);
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIerrorr"));
            }
        }


        private void CargarReservas()
        {
            BLLReserva_750VR bll = new BLLReserva_750VR();
            var lista = bll.leerEntidades_750VR();
        }

        private void LimpiarCamposReserva()
        {
            txtdni.Clear();
            txtnom.Clear(); 

            cmbmanic.SelectedIndex = -1;
            cmbserv.SelectedIndex = -1;
            cmbtec.DataSource = null;
            txthorario.Clear();
            txthorest.Clear();
            txtpre.Clear();
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker1.Enabled = true;
            cmbmanic.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void cmbserv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbserv.SelectedItem == null) return;

            string nombre = cmbserv.SelectedItem.ToString();

            var tecnicas = listaServicios
                .Where(s => s.nombre_750VR == nombre)
                .Select(s => new BEServicio_750VR(s.CodServicio_750VR, s.nombre_750VR, s.tecnica_750VR, s.duracion_750VR, s.precio_750VR, s.activo_750VR))
                .ToList();


            tecnicas.Insert(0, new BEServicio_750VR(nombre, "", 0, 0, true));

            cmbtec.DataSource = tecnicas;
            cmbtec.DisplayMember = "tecnica_750VR";
            cmbtec.ValueMember = "CodServicio_750VR";
            cmbtec.SelectedIndex = 0;
        }

        private void txttec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtec.SelectedItem is BEServicio_750VR servicioSeleccionado)
            {
                txtpre.Text = servicioSeleccionado.precio_750VR.ToString("C");
                txthorest.Text = servicioSeleccionado.duracion_750VR + " min";
            }

        }

        private void FormRegistrarReserva_750VR_Load(object sender, EventArgs e)
        {
            dataGridView2.CellContentClick += dataGridView2_CellContentClick;
            cmbmanic.SelectedIndexChanged += cmbmanic_SelectedIndexChanged;
            CargarServicios();
            radioButton1.Checked = true;
            CargarManicuristas();
            //CargarReservasDispo();
            CargarReservasFiltradas();
            CargarDisponibilidades();


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                int dniManic = Convert.ToInt32(fila.Cells["DNImanicurista"].Value);
                DateTime fecha = Convert.ToDateTime(fila.Cells["Fecha"].Value);
                string horaInicioStr = fila.Cells["Hora Inicio"].Value.ToString();

                foreach (var item in cmbmanic.Items)
                {
                    if (item is BEusuario_750VR manic && manic.dni_750VR == dniManic)
                    {
                        cmbmanic.SelectedItem = item;
                        break;
                    }
                }


                try
                {
                    var valorCelda = fila.Cells["Fecha"].Value;

                    if (valorCelda != null && DateTime.TryParse(valorCelda.ToString(), out DateTime fechaSeleccionada))
                    {
                        dateTimePicker1.MinDate = DateTimePicker.MinimumDateTime;
                        dateTimePicker1.MaxDate = DateTimePicker.MaximumDateTime;
                        dateTimePicker1.Value = fechaSeleccionada;
                    }
                    else
                    {
                        MessageBox.Show("La fecha seleccionada no es válida.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al asignar la fecha: " + ex.Message);
                }
                dateTimePicker1.Enabled = false;
                cmbmanic.Enabled = false;

                txthorario.Text = horaInicioStr;
            }
        }

        public void DividirDisponibilidad(BEdisponibilidad_750VR dispo, TimeSpan duracion)
        {
            TimeSpan horaInicioReserva = TimeSpan.Parse(txthorario.Text);
            TimeSpan horaFinReserva = horaInicioReserva.Add(duracion);

            BLLdisponibilidad_750VR blldispo = new BLLdisponibilidad_750VR();


            if (horaInicioReserva > dispo.HoraInicio_750VR)
            {
                var bloqueAnterior = new BEdisponibilidad_750VR(
                    dispo.DNImanic_750VR,
                    dispo.Fecha_750VR,
                    dispo.HoraInicio_750VR,
                    horaInicioReserva,
                    true,
                    false
                );
                blldispo.CrearDisponibilidad_750VR(bloqueAnterior);
            }


            var bloqueReserva = new BEdisponibilidad_750VR(
                dispo.DNImanic_750VR,
                dispo.Fecha_750VR,
                horaInicioReserva,
                horaFinReserva,
                true,
                true
            );
            blldispo.CrearDisponibilidad_750VR(bloqueReserva);


            if (horaFinReserva < dispo.HoraFin_750VR)
            {
                var bloqueRestante = new BEdisponibilidad_750VR(
                    dispo.DNImanic_750VR,
                    dispo.Fecha_750VR,
                    horaFinReserva,
                    dispo.HoraFin_750VR,
                    true,
                    false
                );
                blldispo.CrearDisponibilidad_750VR(bloqueRestante);
            }


            blldispo.CambiarEstado_750VR(dispo.CodDisponibilidad_750VR, false);


            CargarDisponibilidades();
        }

        //public void CargarReservasDispo()
        //{


        //    BLLReserva_750VR bll = new BLLReserva_750VR();
        //    var lista = bll.leerEntidades_750VR(); // debe devolver List<BEReserva_750VR> con cliente, manic, serv

        //    DataTable tabla = new DataTable();
        //    tabla.Columns.Add("ID", typeof(int));
        //    tabla.Columns.Add("Cliente", typeof(string));
        //    tabla.Columns.Add("Manicurista", typeof(string));
        //    tabla.Columns.Add("Servicio", typeof(string));
        //    tabla.Columns.Add("Fecha", typeof(DateTime));
        //    tabla.Columns.Add("Hora Inicio", typeof(string));
        //    tabla.Columns.Add("Hora Fin", typeof(string));
        //    tabla.Columns.Add("Precio", typeof(decimal));
        //    tabla.Columns.Add("Cobrado", typeof(string));

        //    foreach (var r in lista)
        //    {
        //        string cliente = r.cliente != null ? $"{r.cliente.nombre_750VR} {r.cliente.apellido_750VR}" : Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Desconocido");
        //        string manic = r.manic != null ? $"{r.manic.nombre_750VR} {r.manic.apellido_750VR}" : Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Desconocido");
        //        string servicio = r.serv != null ? $"{r.serv.tecnica_750VR}" : Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_SinServicio");

        //        string cobrado = r.Cobrado_750VR
        //            ? Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Si")
        //            : Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_No");

        //        tabla.Rows.Add(
        //            r.CodReserva_750VR,
        //            cliente,
        //            manic,
        //            servicio,
        //            r.Fecha_750VR.Date,
        //            r.HoraInicio_750VR.ToString(@"hh\:mm"),
        //            r.HoraFin_750VR.ToString(@"hh\:mm"),
        //            r.Precio_750VR,
        //            cobrado
        //                );
        //    }

        //    dataGridView2.DataSource = tabla;
        //    dataGridView2.Columns["ID"].Visible = false;
        //    // 🔤 Traducción de encabezados
        //    dataGridView2.Columns["Cliente"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Cliente");
        //    dataGridView2.Columns["Manicurista"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Manicurista");
        //    dataGridView2.Columns["Servicio"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Servicio");
        //    dataGridView2.Columns["Fecha"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Fecha");
        //    dataGridView2.Columns["Hora Inicio"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_HoraInicio");
        //    dataGridView2.Columns["Hora Fin"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_HoraFin");
        //    dataGridView2.Columns["Precio"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Precio");
        //    dataGridView2.Columns["Cobrado"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Cobrado");

        //    foreach (DataGridViewRow fila in dataGridView2.Rows)
        //    {
        //        if (fila.Cells["Cobrado"].Value?.ToString() == Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_No"))
        //        {
        //            fila.DefaultCellStyle.BackColor = Color.LightCoral;
        //            fila.ReadOnly = true; // evita modificación directa
        //        }
        //    }
        //}

        private void cmbmanic_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LimpiarCamposReserva();

            txtdni.Enabled = true;
            txtnom.Enabled = true;
        }

        private void txtdni_Leave(object sender, EventArgs e)
        {
            if (int.TryParse(txtdni.Text.Trim(), out _))
            {
                BuscarOCrearCliente();
            }
        }

        private void BuscarOCrearCliente()
        {
            var cliente = ObtenerClienteDesdeFormulario();

            if (cliente != null)
            {
                clienteSeleccionado = cliente;
                txtnom.Text = cliente.nombre_750VR;
            }
            else
            {
                // Cliente no existe → se abre directamente el formulario para crearlo
                FormABMClientes frm = new FormABMClientes();
                frm.InvocadoDesdeReserva = true;
                frm.FormularioReserva = this;
                frm.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (idReservaSeleccionada == -1)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.MensajeSeleccionaReserva"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            BLLReserva_750VR bllReserva = new BLLReserva_750VR();
            string estadoActual = bllReserva.ObtenerEstadoReserva(idReservaSeleccionada);

            if (!estadoActual.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.MensajeReservaNoCancelable"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            var reserva = bllReserva.ObtenerReservaPorId(idReservaSeleccionada);

            if (reserva == null)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.MensajeReservaNoEncontrada"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            bllReserva.ActualizarEstadoReserva(idReservaSeleccionada, "Cancelado");

            var bllDispo = new BLLdisponibilidad_750VR();

            var nuevaDispo = new BEdisponibilidad_750VR(
                dni: reserva.DNImanic_750VR,
                fecha: reserva.Fecha_750VR,
                ini: reserva.HoraInicio_750VR,
                fin: reserva.HoraFin_750VR,
                acr: true,
                est: false
            );

            bllDispo.CrearDisponibilidad_750VR(nuevaDispo);

            MessageBox.Show(
                Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.MensajeReservaCancelada"),
                Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.TituloConfirmacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            //CargarReservasDispo();
            CargarReservasFiltradas();
            CargarDisponibilidades();
            idReservaSeleccionada = -1;

            txtdni.Enabled = true;
            txtnom.Enabled = true;
        }

        private int idReservaSeleccionada = -1;

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView2.Rows[e.RowIndex];
                idReservaSeleccionada = Convert.ToInt32(fila.Cells["ID"].Value);

                // Obtener cliente
                string nombreCompletoCliente = fila.Cells["Cliente"].Value.ToString();
                txtnom.Text = nombreCompletoCliente;

                // Obtener y setear manicurista
                string nombreManic = fila.Cells["Manicurista"].Value.ToString();
                int indexManic = cmbmanic.FindStringExact(nombreManic);
                if (indexManic >= 0)
                    cmbmanic.SelectedIndex = indexManic;

                // Obtener y setear servicio y técnica
                string tecnica = fila.Cells["Servicio"].Value.ToString();

                // Buscar el servicio en la lista
                var servicioSeleccionado = listaServicios.FirstOrDefault(s => s.tecnica_750VR == tecnica);
                if (servicioSeleccionado != null)
                {
                    // Buscar y setear el nombre general del servicio (para el combo de categoría)
                    string nombreServicio = servicioSeleccionado.nombre_750VR;
                    int indexServicio = cmbserv.FindStringExact(nombreServicio);
                    if (indexServicio >= 0)
                        cmbserv.SelectedIndex = indexServicio;

                    // Luego setear técnica
                    cmbtec.SelectedValue = servicioSeleccionado.CodServicio_750VR;

                    // Cargar duración y precio
                    txtpre.Text = servicioSeleccionado.precio_750VR.ToString("C");
                    txthorest.Text = servicioSeleccionado.duracion_750VR + " min";
                }

                // Fecha y horario
                dateTimePicker1.Value = Convert.ToDateTime(fila.Cells["Fecha"].Value);
                txthorario.Text = fila.Cells["Hora Inicio"].Value.ToString();


                txtdni.Enabled = false;
                txtnom.Enabled = false;
            }


        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null || dataGridView2.CurrentRow.Index < 0)
            {
                MessageBox.Show("Seleccione una reserva para modificar.");
                return;
            }

            DataGridViewRow fila = dataGridView2.CurrentRow;
            int idReserva = Convert.ToInt32(fila.Cells["ID"].Value);


           
            BLLReserva_750VR bllReserva = new BLLReserva_750VR();
            BEReserva_750VR reservaExistente = bllReserva.ObtenerReservaPorId(idReserva);

            if (reservaExistente == null)
            {
                MessageBox.Show("No se encontró la reserva.");
                return;
            }

            var bllCliente = new BLLCliente_750VR();
            var cliente = bllCliente.ObtenerClientePorDNI_750VR(reservaExistente.DNIcli_750VR);

            if (cliente == null)
            {
                MessageBox.Show("No se pudo recuperar el cliente de la reserva.");
                return;
            }
            var manic = cmbmanic.SelectedItem as BEusuario_750VR;
            var servicio = cmbtec.SelectedItem as BEServicio_750VR;

            if (manic == null || servicio == null)
            {
                MessageBox.Show("Complete todos los campos.");
                return;
            }

            if (!TimeSpan.TryParse(txthorario.Text, out TimeSpan horaInicio))
            {
                MessageBox.Show("Hora inválida.");
                return;
            }
            if (reservaExistente.Estado_750VR.Equals("Cancelado", StringComparison.OrdinalIgnoreCase) ||
    reservaExistente.Cobrado_750VR == false)
            {
                MessageBox.Show("No se puede modificar una reserva cancelada o no cobrada.");
                return;
            }


            TimeSpan horaFin = horaInicio.Add(TimeSpan.FromMinutes(servicio.duracion_750VR));

            // 🟢 1. Reactivar disponibilidad anterior
            var bllDispo = new BLLdisponibilidad_750VR();

            var dispoAnterior = new BEdisponibilidad_750VR(
                reservaExistente.DNImanic_750VR,
                reservaExistente.Fecha_750VR,
                reservaExistente.HoraInicio_750VR,
                reservaExistente.HoraFin_750VR,
                true,
                false
            );
            bllDispo.CrearDisponibilidad_750VR(dispoAnterior);

            // 🟢 2. Buscar nueva disponibilidad completa para dividir
            var dispoList = bllDispo.LeerDisponibilidades_750VR();

            var dispoNueva = dispoList.FirstOrDefault(d =>
                d.DNImanic_750VR == manic.dni_750VR &&
                d.Fecha_750VR.Date == dateTimePicker1.Value.Date &&
                d.HoraInicio_750VR <= horaInicio &&
                d.HoraFin_750VR >= horaFin &&
                d.activo_750VR &&
                !d.estado_750VR
            );

            if (dispoNueva == null)
            {
                MessageBox.Show("No se encontró disponibilidad para el nuevo horario.");
                return;
            }


            // 🟢 3. Actualizar la reserva
            BEReserva_750VR nueva = new BEReserva_750VR(
                cod: idReserva,
                dnicli: cliente.dni_750VR,
                cli: cliente,
                dnimanic: manic.dni_750VR,
                manic: manic,
                idserv: servicio.CodServicio_750VR,
                serv: servicio,
                fecha: dateTimePicker1.Value.Date,
                ini: horaInicio,
                fin: horaFin,
                pre: servicio.precio_750VR,
                estado: reservaExistente.Estado_750VR,
                cobrado: reservaExistente.Cobrado_750VR
            );

            bool actualizado = bllReserva.ModificarReserva_750VR(nueva);

            if (actualizado)
            {
                // 🟢 4. Dividir la nueva disponibilidad
                DividirDisponibilidad(dispoNueva, TimeSpan.FromMinutes(servicio.duracion_750VR));
                MessageBox.Show("Reserva modificada correctamente.");
                CargarReservasFiltradas();
                CargarDisponibilidades();
            }
            else
            {
                MessageBox.Show("Error al modificar la reserva.");
            }

            txtdni.Enabled = true;
            txtnom.Enabled = true;
        }

        private void txtdni_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                var fila = dataGridView2.SelectedRows[0];
                var estadoCobro = fila.Cells["Cobrado"].Value?.ToString();
                var estadoReserva = fila.Cells["Estado"]?.Value?.ToString();

                if (estadoCobro == Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_No") ||
                    estadoReserva?.Equals("Cancelado", StringComparison.OrdinalIgnoreCase) == true)
                {
                    dataGridView2.ClearSelection();
                    idReservaSeleccionada = -1;
                    return;
                }

                idReservaSeleccionada = Convert.ToInt32(fila.Cells["ID"].Value);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            CargarReservasFiltradas();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            CargarReservasFiltradas();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            CargarReservasFiltradas();
        }

        private void CargarReservasFiltradas()
        {
            var bll = new BLLReserva_750VR();
            var lista = bll.leerEntidades_750VR(); // trae cliente, manic, serv

            // Filtrar según el radio button
            if (radioButton2.Checked)
            {
                lista = lista.Where(r => !r.Cobrado_750VR).ToList(); // No cobrados
            }
            else if (radioButton3.Checked)
            {
                lista = lista.Where(r => r.Estado_750VR.Equals("Cancelado", StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (radioButton1.Checked)
            {
                lista = lista.Where(r => r.Cobrado_750VR && !r.Estado_750VR.Equals("Cancelado", StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Armar tabla como en CargarReservasDispo()
            DataTable tabla = new DataTable();
            tabla.Columns.Add("ID", typeof(int));
            tabla.Columns.Add("Cliente", typeof(string));
            tabla.Columns.Add("Manicurista", typeof(string));
            tabla.Columns.Add("Servicio", typeof(string));
            tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("Hora Inicio", typeof(string));
            tabla.Columns.Add("Hora Fin", typeof(string));
            tabla.Columns.Add("Precio", typeof(decimal));
            tabla.Columns.Add("Cobrado", typeof(string));
            tabla.Columns.Add("Estado", typeof(string));

            foreach (var r in lista)
            {
                string cliente = r.cliente != null ? $"{r.cliente.nombre_750VR} {r.cliente.apellido_750VR}" : Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Desconocido");
                string manic = r.manic != null ? $"{r.manic.nombre_750VR} {r.manic.apellido_750VR}" : Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Desconocido");
                string servicio = r.serv != null ? $"{r.serv.tecnica_750VR}" : Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_SinServicio");

                string cobrado = r.Cobrado_750VR
                    ? Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Si")
                    : Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_No");

                tabla.Rows.Add(
                    r.CodReserva_750VR,
                    cliente,
                    manic,
                    servicio,
                    r.Fecha_750VR.Date,
                    r.HoraInicio_750VR.ToString(@"hh\:mm"),
                    r.HoraFin_750VR.ToString(@"hh\:mm"),
                    r.Precio_750VR,
                    cobrado,
                    r.Estado_750VR
                );
            }

            dataGridView2.DataSource = tabla;
            dataGridView2.Columns["ID"].Visible = false;

            // Encabezados traducidos
            dataGridView2.Columns["Cliente"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Cliente");
            dataGridView2.Columns["Manicurista"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Manicurista");
            dataGridView2.Columns["Servicio"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Servicio");
            dataGridView2.Columns["Fecha"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Fecha");
            dataGridView2.Columns["Hora Inicio"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_HoraInicio");
            dataGridView2.Columns["Hora Fin"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_HoraFin");
            dataGridView2.Columns["Precio"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Precio");
            dataGridView2.Columns["Cobrado"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Cobrado");
            dataGridView2.Columns["Estado"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_Estado");



            foreach (DataGridViewRow fila in dataGridView2.Rows)
            {
                string cobrado = fila.Cells["Cobrado"].Value?.ToString();
                string estado = fila.Cells["Estado"]?.Value?.ToString();

                if (cobrado == Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid2_No"))
                {
                    fila.DefaultCellStyle.BackColor = Color.LightCoral; // No cobrado → rojo claro
                    fila.ReadOnly = true;
                }
                else if (estado != null && estado.Equals("Cancelado", StringComparison.OrdinalIgnoreCase))
                {
                    fila.DefaultCellStyle.BackColor = Color.LightSalmon; // Cancelado → naranja claro
                    fila.ReadOnly = true;
                }
            }

        }
    }
}
