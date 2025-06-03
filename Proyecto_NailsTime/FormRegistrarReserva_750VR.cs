using BE_VR750;
using BLL_VR750;
using DAL_VR750;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_NailsTime
{
    public partial class FormRegistrarReserva_750VR : Form
    {
        private List<BEServicio_750VR> listaServicios;
        private List<BEusuario_750VR> listaUsuarios = new List<BEusuario_750VR>();

        public FormRegistrarReserva_750VR()
        {
            InitializeComponent();
        }

        private void CargarServicios()
        {
            BLLServicio_750VR bllServicio = new BLLServicio_750VR();
            listaServicios = bllServicio.leerEntidadesActivas_750VR_750VR(); // Te lo muestro abajo

            var nombresServicio = listaServicios
                .Select(s => s.nombre_750VR)
                .Distinct()
                .ToList();


            nombresServicio.Insert(0, ""); // ← esto agrega el valor vacío al principio

            cmbserv.DataSource = nombresServicio;
            cmbserv.SelectedIndex = 0; // ← se muestra en blanco
        }

       

        private void CargarDisponibilidadesConNombre()
        {
            BLLdisponibilidad_750VR bllDispo = new BLLdisponibilidad_750VR();
            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();

            var listaDispo = bllDispo.LeerDisponibilidades_750VR();
            var listaUsuarios = bllUsuario.leerEntidades_750VR();

            DataTable tabla = new DataTable();
            tabla.Columns.Add("IdDisponibilidad", typeof(int));
            tabla.Columns.Add("Manicurista", typeof(string));
            tabla.Columns.Add("DNImanicurista", typeof(int));
            tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("Hora Inicio", typeof(string));
            tabla.Columns.Add("Hora Fin", typeof(string));
            tabla.Columns.Add("Estado", typeof(string));

            // Filtra por activas y disponibles
            foreach (var dispo in listaDispo.Where(d => d.activo_750VR && d.estado_750VR == false))
            {
                var usu = listaUsuarios.FirstOrDefault(u => u.dni_750VR == dispo.DNImanic_750VR);
                string nombreCompleto = usu != null ? $"{usu.nombre_750VR} {usu.apellido_750VR}" : "Desconocido";

                tabla.Rows.Add(
                    dispo.IdDisponibilidad_750VR,
                    nombreCompleto,
                    dispo.DNImanic_750VR,
                    dispo.Fecha_750VR.Date,
                    dispo.HoraInicio_750VR.ToString(@"hh\:mm"),
                    dispo.HoraFin_750VR.ToString(@"hh\:mm"),
                    "Disponible" // Ya estás filtrando por eso, no hace falta preguntar
                );
            }

            dataGridView1.DataSource = tabla;

            if (dataGridView1.Columns.Contains("IdDisponibilidad"))
                dataGridView1.Columns["IdDisponibilidad"].Visible = false;
        }
        private void CargarManicuristas()
        {
            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();
            var manicuristas = bllUsuario.ObtenerManicuristasActivos_750VR();

            cmbmanic.DataSource = manicuristas;
            cmbmanic.DisplayMember = "nombre_750VR";   // o $"{nombre} {apellido}" si querés mostrar ambos
            cmbmanic.ValueMember = "dni_750VR";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        //boton buscar cliente
        private void button3_Click(object sender, EventArgs e)
        {
            string dni = txtdni.Text;
            var bllcliente = new BLLCliente_750VR();
            var cliente = bllcliente.ObtenerClientePorDNI_750VR(Convert.ToInt32(dni));

            if (cliente != null)
            {
                txtnom.Text = cliente.nombre_750VR;
            }
            else
            {
                MessageBox.Show("Cliente no encontrado. Puede crearlo desde el botón correspondiente.");
            }
        }


        //crea cliente
        private void button5_Click(object sender, EventArgs e)
        {

            FormABMClientes_750VR frm = new FormABMClientes_750VR();
            frm.InvocadoDesdeReserva = true;
            frm.FormularioReserva = this;
            frm.ShowDialog(); 
        }
        public void CompletarCamposCliente(string dni, string nombre)
        {
            txtdni.Text = dni;
            txtnom.Text = nombre;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones mínimas
                if (cmbmanic.SelectedItem == null || cmbtec.SelectedItem == null || string.IsNullOrEmpty(txtdni.Text))
                {
                    MessageBox.Show("Faltan datos obligatorios.");
                    return;
                }

                BLLCliente_750VR bllcli = new BLLCliente_750VR();
                var cliente = bllcli.ObtenerClientePorDNI_750VR(Convert.ToInt32(txtdni.Text));
                if (cliente == null)
                {
                    MessageBox.Show("No se encontró el cliente.");
                    return;
                }
                var manic = cmbmanic.SelectedItem as BEusuario_750VR;

                var servicio = cmbtec.SelectedItem as BEServicio_750VR;

                if (cliente == null || manic == null || servicio == null)
                {
                    MessageBox.Show("Error al obtener los datos del cliente, manicurista o servicio.");
                    return;
                }

                // Obtener disponibilidad seleccionada del DataGridView
                if (dataGridView1.CurrentRow?.DataBoundItem is DataRowView row)
                {
                    // Acá armás la disponibilidad original a partir del DataRow
                    var disponibilidadSeleccionada = new BEdisponibilidad_750VR
                    {
                        IdDisponibilidad_750VR = Convert.ToInt32(row["IdDisponibilidad"]),
                        DNImanic_750VR = Convert.ToInt32(row["DNImanicurista"]),
                        Fecha_750VR = Convert.ToDateTime(row["Fecha"]),
                        HoraInicio_750VR = TimeSpan.Parse(row["Hora Inicio"].ToString()),
                        HoraFin_750VR = TimeSpan.Parse(row["Hora Fin"].ToString()),
                        activo_750VR = true,
                        estado_750VR = false
                    };

                    // Armar la reserva
                    TimeSpan horaInicio = disponibilidadSeleccionada.HoraInicio_750VR;
                    TimeSpan horaFin = horaInicio.Add(TimeSpan.FromMinutes(servicio.duracion_750VR));

                    BEReserva_750VR nuevaReserva = new BEReserva_750VR
                    {
                        DNIcli_750VR = cliente.dni_750VR,
                        cliente = cliente,
                        DNImanic_750VR = manic.dni_750VR,
                        manic = manic,
                        IdServicio_750VR = servicio.idServicio_750VR,
                        serv = servicio,
                        Fecha_750VR = disponibilidadSeleccionada.Fecha_750VR,
                        HoraInicio_750VR = horaInicio,
                        HoraFin_750VR = horaFin,
                        Precio_750VR = servicio.precio_750VR,
                        Estado_750VR = "Pendiente",
                        Cobrado_750VR = false
                    };

                    // Guardar la reserva
                    BLLReserva_750VR bllReserva = new BLLReserva_750VR();
                    //bllReserva.CrearReserva_750VR(nuevaReserva);
                    int nuevoID = bllReserva.CrearReserva_750VR(nuevaReserva);
                    nuevaReserva.IdReserva_750VR = nuevoID;

                    // 🔥 ACÁ VA LA DIVISIÓN DE LA DISPONIBILIDAD
                    DividirDisponibilidad(disponibilidadSeleccionada, TimeSpan.FromMinutes(servicio.duracion_750VR));

                    MessageBox.Show("Reserva creada correctamente.");
                    CargarReservas();
                    LimpiarCamposReserva();
                    CargarReservasDispo();

                    // Llamamos directamente al form de cobro
                    FormCobrarServicio_750VR frmCobro = new FormCobrarServicio_750VR(nuevaReserva.IdReserva_750VR);
                    var resultado = frmCobro.ShowDialog();

                    if (resultado == DialogResult.OK)
                    {
                        MessageBox.Show("Reserva cobrada correctamente.");
                    }
                    else
                    {
                        
                        MessageBox.Show("La reserva quedó pendiente de cobro.");
                    }

                    CargarReservas();
                    LimpiarCamposReserva();
                    CargarReservasDispo();
                }
                else
                {
                    MessageBox.Show("Debés seleccionar una disponibilidad.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear la reserva: " + ex.Message);
            }
        }


        private void CargarReservas()
        {
            BLLReserva_750VR bll = new BLLReserva_750VR();
            var lista = bll.leerEntidades();
        }

        private void LimpiarCamposReserva()
        {
            txtdni.Clear();
            txtnom.Clear(); // si tenés
            
            cmbmanic.SelectedIndex = -1;
            cmbserv.SelectedIndex = -1;
            cmbtec.DataSource = null;
            txthorario.Clear();
            txthorest.Clear();
            txtpre.Clear();
            dateTimePicker1.Value = DateTime.Today;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //FormCobrarServicio_750VR frm = new FormCobrarServicio_750VR();
            //frm.ShowDialog();
        }

        private void cmbserv_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombre = cmbserv.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(nombre)) return;

            var tecnicas = listaServicios
                .Where(s => s.nombre_750VR == nombre)
                .ToList();

            // Insertar un objeto vacío al principio
            tecnicas.Insert(0, new BEServicio_750VR { tecnica_750VR = "" });

            cmbtec.DataSource = tecnicas;
            cmbtec.DisplayMember = "tecnica_750VR";
            cmbtec.SelectedIndex = 0;
        }

        private void txttec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtec.SelectedItem is BEServicio_750VR servicioSeleccionado)
            {
                txtpre.Text = servicioSeleccionado.precio_750VR.ToString("C"); // con símbolo moneda
                txthorest.Text = servicioSeleccionado.duracion_750VR + " min";
            }
       
        }

        private void FormRegistrarReserva_750VR_Load(object sender, EventArgs e)
        {
            CargarServicios();
            CargarReservasDispo();
        
            CargarManicuristas();
            CargarDisponibilidadesConNombre();
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

                // Buscamos el usuario
                var manic = listaUsuarios.FirstOrDefault(u => u.dni_750VR == dniManic);
                if (manic != null)
                {
                    cmbmanic.SelectedItem = manic;
                }

                dateTimePicker1.Value = fecha;
                txthorario.Text = fila.Cells["Hora Inicio"]?.Value?.ToString(); // o txtHoraInicio según cómo lo tengas
            }
        }

        public void DividirDisponibilidad(BEdisponibilidad_750VR dispo, TimeSpan duracion)
        {
            TimeSpan inicioReserva = dispo.HoraInicio_750VR;
            TimeSpan finReserva = inicioReserva.Add(duracion);

            // 1. Marcamos la franja usada como ocupada
            BEdisponibilidad_750VR ocupado = new BEdisponibilidad_750VR
            {
                DNImanic_750VR = dispo.DNImanic_750VR,
                Fecha_750VR = dispo.Fecha_750VR,
                HoraInicio_750VR = inicioReserva,
                HoraFin_750VR = finReserva,
                activo_750VR = true,
                estado_750VR = true // ocupado
            };
            BLLdisponibilidad_750VR blldispo = new BLLdisponibilidad_750VR();
            blldispo.CrearDisponibilidad_750VR(ocupado);

            // 2. Creamos la parte restante si existe
            if (finReserva < dispo.HoraFin_750VR)
            {
                BEdisponibilidad_750VR restante = new BEdisponibilidad_750VR
                {
                    DNImanic_750VR = dispo.DNImanic_750VR,
                    Fecha_750VR = dispo.Fecha_750VR,
                    HoraInicio_750VR = finReserva,
                    HoraFin_750VR = dispo.HoraFin_750VR,
                    activo_750VR = true,
                    estado_750VR = false // disponible
                };
                blldispo.CrearDisponibilidad_750VR(restante);
            }

            // 3. Eliminamos o marcamos como inactiva la original
            blldispo.CambiarEstado_750VR(dispo.IdDisponibilidad_750VR, false); // o inactivar lógica
            CargarDisponibilidadesConNombre();
        }

        public void CargarReservasDispo()
        {
         

            BLLReserva_750VR bll = new BLLReserva_750VR();
            var lista = bll.leerEntidades(); // debe devolver List<BEReserva_750VR> con cliente, manic, serv

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

            foreach (var r in lista)
            {
                string cliente = r.cliente != null ? $"{r.cliente.nombre_750VR} {r.cliente.apellido_750VR}" : "Desconocido";
                string manic = r.manic != null ? $"{r.manic.nombre_750VR} {r.manic.apellido_750VR}" : "Desconocido";
                string servicio = r.serv != null ? $"{r.serv.tecnica_750VR}" : "Sin servicio";

                tabla.Rows.Add(
                    r.IdReserva_750VR,
                    cliente,
                    manic,
                    servicio,
                    r.Fecha_750VR.Date,
                    r.HoraInicio_750VR.ToString(@"hh\:mm"),
                    r.HoraFin_750VR.ToString(@"hh\:mm"),
                    r.Precio_750VR,
                    r.Cobrado_750VR ? "Sí" : "No"
                );
            }

            dataGridView2.DataSource = tabla;
            dataGridView2.Columns["ID"].Visible = false; // opcional
        }

        private void cmbmanic_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
