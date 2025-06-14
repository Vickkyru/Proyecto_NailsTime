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

namespace Proyecto_NailsTime
{
    public partial class FormRegistrarReserva_750VR : Form, Iobserver_750VR
    {
        private List<BEServicio_750VR> listaServicios;
        private List<BEusuario_750VR> listaUsuarios = new List<BEusuario_750VR>();

        public FormRegistrarReserva_750VR()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }
        public void ActualizarIdioma()
        {
            //MessageBox.Show("Me estoy traduciendo...");
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
                    "Disponible" 
                );
            }

            dataGridView1.DataSource = tabla;

            if (dataGridView1.Columns.Contains("IdDisponibilidad"))
                dataGridView1.Columns["IdDisponibilidad"].Visible = false;
            if (dataGridView1.Columns.Contains("DNImanicurista"))
                dataGridView1.Columns["DNImanicurista"].Visible = false;

        }
        private void CargarManicuristas()
        {
            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();
            listaUsuarios = bllUsuario.ObtenerManicuristasActivos_750VR();

            cmbmanic.DataSource = listaUsuarios;
            cmbmanic.DisplayMember = "nombre_750VR"; 
            cmbmanic.ValueMember = "dni_750VR";
            cmbmanic.SelectedIndex = -1;
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
            var cliente = ObtenerClienteDesdeFormulario();
            if (cliente != null)
            {
                clienteSeleccionado = cliente;
                txtnom.Text = cliente.nombre_750VR;
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
                    dni: Convert.ToInt32(row["DNImanicurista"]),
                    fecha: Convert.ToDateTime(row["Fecha"]),
                    ini: TimeSpan.Parse(row["Hora Inicio"].ToString()),
                    fin: TimeSpan.Parse(row["Hora Fin"].ToString()),
                    acr: true,
                    est: false
                );

                disponibilidadSeleccionada.IdDisponibilidad_750VR = Convert.ToInt32(row["IdDisponibilidad"]);

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

                BEReserva_750VR nuevaReserva = new BEReserva_750VR
                {
                    DNIcli_750VR = clienteSeleccionado.dni_750VR,
                    cliente = clienteSeleccionado,
                    DNImanic_750VR = manic.dni_750VR,
                    manic = manic,
                    IdServicio_750VR = servicio.idServicio_750VR,
                    serv = servicio,
                    Fecha_750VR = disponibilidadSeleccionada.Fecha_750VR,
                    HoraInicio_750VR = horaManual,
                    HoraFin_750VR = horaFin,
                    Precio_750VR = servicio.precio_750VR,
                    Estado_750VR = "Pendiente",
                    Cobrado_750VR = false
                };

                var bllReserva = new BLLReserva_750VR();
                int nuevoID = bllReserva.CrearReserva_750VR(nuevaReserva);
                nuevaReserva.IdReserva_750VR = nuevoID;

                DividirDisponibilidad(disponibilidadSeleccionada, TimeSpan.FromMinutes(servicio.duracion_750VR));

                //MessageBox.Show("Reserva creada correctamente.");
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIcreada"));

                FormCobrarServicio_750VR frmCobro = new FormCobrarServicio_750VR(nuevaReserva.IdReserva_750VR);
                var resultado = frmCobro.ShowDialog();
                if (resultado == DialogResult.OK)
                {
                    //MessageBox.Show("Reserva cobrada correctamente.");
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIcobrada"));
                }
                else
                {
                    //MessageBox.Show("La reserva quedó pendiente de cobro.");
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Mensaje_DNIpendiente"));
                }

                CargarReservas();
                LimpiarCamposReserva();
                CargarReservasDispo();
            }
            catch (Exception ex)
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
            txtnom.Clear(); // si tenés
            
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
                .Select(s => new BEServicio_750VR(s.idServicio_750VR, s.nombre_750VR, s.tecnica_750VR, s.duracion_750VR, s.precio_750VR, s.activo_750VR))
                .ToList();

            
            tecnicas.Insert(0, new BEServicio_750VR(nombre, "", 0, 0, true));

            cmbtec.DataSource = tecnicas;
            cmbtec.DisplayMember = "tecnica_750VR";
            cmbtec.ValueMember = "idServicio_750VR";
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
                string horaInicioStr = fila.Cells["Hora Inicio"].Value.ToString();

                
                var manicSeleccionado = listaUsuarios.FirstOrDefault(u => u.dni_750VR == dniManic);
                if (manicSeleccionado != null)
                {
                    cmbmanic.SelectedValue = manicSeleccionado.dni_750VR;
                }

                
                dateTimePicker1.Value = fecha;
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

            
            blldispo.CambiarEstado_750VR(dispo.IdDisponibilidad_750VR, false);

           
            CargarDisponibilidadesConNombre();
        }

        public void CargarReservasDispo()
        {
         

            BLLReserva_750VR bll = new BLLReserva_750VR();
            var lista = bll.leerEntidades_750VR(); // debe devolver List<BEReserva_750VR> con cliente, manic, serv

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
            dataGridView2.Columns["ID"].Visible = false; 
        }

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
        }
    }
}
