using BE_VR750;
using BLL_VR750;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SERVICIOS_VR750;
using DAL_VR750;

namespace Proyecto_NailsTime
{
    public partial class FormActualizarAgenda : Form, Iobserver_750VR
    {
        // ====== Buffer temporal de insumos (no toca BD hasta Aplicar) ======
        private class InsumoTmp
        {
            public int IdInsumo { get; set; }
            public string Nombre { get; set; }
            public int Cantidad { get; set; }
        }
        private readonly List<InsumoTmp> _insumosTmp = new List<InsumoTmp>();

        private int idReservaSeleccionada = -1;
        private bool _aplicando = false;
        public FormActualizarAgenda()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
        }
        private bool insumosPendientes = false;

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormActualizarAgenda_750VR_Load(object sender, EventArgs e)
        {
            CargarReservas();
            Disponibilidad();
            CargarInsumos();
            RefrescarEstadoBotones();
            RefrescarGridInsumosTmp();
        }
        private void CargarInsumos()
        {
            var bllInsumo = new BLLinsumos_750VR();
            var listaInsumos = bllInsumo.LeerInsumosActivos_750VR();

            comboBox1.DataSource = listaInsumos;
            comboBox1.DisplayMember = "nombre_750VR";
            comboBox1.ValueMember = "codinsumo_750VR";
            comboBox1.SelectedIndex = -1;
        }

        private void Disponibilidad()
        {
            var bllDispo = new BLLdisponibilidad_750VR();
            var bllUsuario = new BLLusuario_750VR();
            var sesion = SessionManager_750VR.ObtenerInstancia;
            int dniManicurista = sesion.user.dni_750VR;

            var listaDispo = bllDispo.ObtenerDisponibilidadesPorManicurista(dniManicurista);
            var listaUsuarios = bllUsuario.leerEntidades_750VR();

            var tabla = new DataTable();
            tabla.Columns.Add("IdDisponibilidad", typeof(int));
            tabla.Columns.Add("Manicurista", typeof(string));
            tabla.Columns.Add("DNImanicurista", typeof(int));
            tabla.Columns.Add("Fecha", typeof(DateTime));
            tabla.Columns.Add("Hora Inicio", typeof(string));
            tabla.Columns.Add("Hora Fin", typeof(string));
            tabla.Columns.Add("Estado", typeof(string));

            foreach (var d in listaDispo.Where(x => x.activo_750VR && x.estado_750VR == false))
            {
                var u = listaUsuarios.FirstOrDefault(z => z.dni_750VR == d.DNImanic_750VR);
                string nom = u != null ? $"{u.nombre_750VR} {u.apellido_750VR}" : "Desconocido";
                tabla.Rows.Add(
                    d.CodDisponibilidad_750VR,
                    nom,
                    d.DNImanic_750VR,
                    d.Fecha_750VR.Date,
                    d.HoraInicio_750VR.ToString(@"hh\:mm"),
                    d.HoraFin_750VR.ToString(@"hh\:mm"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_Disponible")
                );
            }

            dataGridView2.DataSource = tabla;
            if (dataGridView2.Columns.Contains("IdDisponibilidad")) dataGridView2.Columns["IdDisponibilidad"].Visible = false;
            if (dataGridView2.Columns.Contains("DNImanicurista")) dataGridView2.Columns["DNImanicurista"].Visible = false;

            dataGridView2.Columns["Manicurista"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_Manicurista");
            dataGridView2.Columns["Fecha"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_Fecha");
            dataGridView2.Columns["Hora Inicio"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_HoraInicio");
            dataGridView2.Columns["Hora Fin"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_HoraFin");
            dataGridView2.Columns["Estado"].HeaderText = Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_Estado");
        }
        private void CargarReservas()
        {
            var sesion = SessionManager_750VR.ObtenerInstancia;
            int dniManicurista = sesion.user.dni_750VR;
            var bll = new BLLReserva_750VR();

            var reservas = bll.ObtenerReservasPorManicurista(dniManicurista)
                              .Where(r => r.Cobrado_750VR)
                              .ToList();

            var tabla = new DataTable();
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.DNICliente"), typeof(int));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.NombreCliente"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.NombreManic"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.Servicio"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.Tecnica"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.FechaReserva"), typeof(DateTime));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.HoraInicio"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.HoraFin"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.Estado"), typeof(string));
            tabla.Columns.Add("IdReserva", typeof(int));

            foreach (var r in reservas)
            {
                tabla.Rows.Add(
                    r.DNIcli_750VR,
                    $"{r.cliente?.nombre_750VR} {r.cliente?.apellido_750VR}",
                    $"{r.manic?.nombre_750VR} {r.manic?.apellido_750VR}",
                    r.serv?.nombre_750VR,
                    r.serv?.tecnica_750VR,
                    r.Fecha_750VR.Date,
                    r.HoraInicio_750VR.ToString(@"hh\:mm"),
                    r.HoraFin_750VR.ToString(@"hh\:mm"),
                    r.Estado_750VR,
                    r.CodReserva_750VR
                );
            }

            dataGridView1.DataSource = tabla;
            if (dataGridView1.Columns.Contains("IdReserva"))
                dataGridView1.Columns["IdReserva"].Visible = false;
        }

      

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (_aplicando) return;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                idReservaSeleccionada = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IdReserva"].Value);
                textBox1.Text = idReservaSeleccionada.ToString();
            }
            else
            {
                idReservaSeleccionada = -1;
                textBox1.Clear();
            }

            // cambiar de reserva limpia lista temporal
            _insumosTmp.Clear();
            RefrescarGridInsumosTmp();
            RefrescarEstadoBotones();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idReservaSeleccionada == -1)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeSeleccionarReserva"));
                return;
            }
            if (_insumosTmp.Count == 0)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeDebeRegistrarInsumo"));
                return;
            }

            var bllReserva = new BLLReserva_750VR();
            var estado = bllReserva.ObtenerEstadoReserva(idReservaSeleccionada);
            if (!estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeNoModificable"));
                return;
            }

            try
            {
                _aplicando = true;

                var bllInsumo = new BLLreservaInsumo_750VR();
                foreach (var it in _insumosTmp)
                {
                    if (bllInsumo.InsumoYaAgregado(idReservaSeleccionada, it.IdInsumo))
                        bllInsumo.SumarCantidadInsumo(idReservaSeleccionada, it.IdInsumo, it.Cantidad);
                    else
                        bllInsumo.RegistrarInsumoUsado(idReservaSeleccionada, it.IdInsumo, it.Cantidad);
                }

                bllReserva.ActualizarEstadoReserva(idReservaSeleccionada, "Realizado");

                _insumosTmp.Clear();
                RefrescarGridInsumosTmp();

                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeCorrecto"));

                CargarReservas();
                Disponibilidad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeErrorRegistro") + " " + ex.Message);
            }
            finally
            {
                _aplicando = false;
                RefrescarEstadoBotones();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //if (idReservaSeleccionada == -1)
            //{
            //    MessageBox.Show(
            //        Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeSeleccionaReserva"),
            //        Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Warning
            //    );
            //    return;
            //}

            //BLLReserva_750VR bllReserva = new BLLReserva_750VR();
            //string estadoActual = bllReserva.ObtenerEstadoReserva(idReservaSeleccionada);

            //if (!estadoActual.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            //{
            //    MessageBox.Show(
            //        Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaNoCancelable"),
            //        Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Information
            //    );
            //    return;
            //}

            //// Obtener la reserva completa
            //var reserva = bllReserva.ObtenerReservaPorId(idReservaSeleccionada);

            //if (reserva == null)
            //{
            //    MessageBox.Show("No se encontró la reserva.");
            //    return;
            //}

            //// Cambiar estado de reserva
            //bllReserva.ActualizarEstadoReserva(reserva.CodReserva_750VR, "Cancelado");

            //// Reactivar disponibilidad correspondiente
            //var bllDispo = new BLLdisponibilidad_750VR();

            //var nuevaDispo = new BEdisponibilidad_750VR(
            //    dni: reserva.DNImanic_750VR,
            //    fecha: reserva.Fecha_750VR,
            //    ini: reserva.HoraInicio_750VR,
            //    fin: reserva.HoraFin_750VR,
            //    acr: true,
            //    est: false
            //);

            //bllDispo.CrearDisponibilidad_750VR(nuevaDispo);

            //MessageBox.Show(
            //    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaCancelada"),
            //    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloConfirmacion"),
            //    MessageBoxButtons.OK,
            //    MessageBoxIcon.Information
            //);

            //CargarReservas();
            //Disponibilidad();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (insumosPendientes)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeDebeRegistrarInsumo"));
                return;
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idReservaSeleccionada == -1)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeSeleccionaReserva"),
                                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_insumosTmp.Count > 0)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeNoAusenteConInsumos"));
                return;
            }

            var bll = new BLLReserva_750VR();
            string estado = bll.ObtenerEstadoReserva(idReservaSeleccionada);
            if (!estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaNoModificable"),
                                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bll.ActualizarEstadoReserva(idReservaSeleccionada, "Ausente");

            MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaRealizada"),
                            Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloConfirmacion"),
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            _insumosTmp.Clear();
            RefrescarGridInsumosTmp();
            CargarReservas();
            Disponibilidad();
            RefrescarEstadoBotones();
        }
       

        private void button5_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(textBox1.Text) || comboBox1.SelectedItem == null || string.IsNullOrEmpty(textBox2.Text))
            //{
            //    MessageBox.Show("Completa todos los campos.");
            //    return;
            //}

            //int idReserva = Convert.ToInt32(textBox1.Text);
            //int idInsumo = Convert.ToInt32(comboBox1.SelectedValue);
            //int cantidad;

            //if (!int.TryParse(textBox2.Text, out cantidad) || cantidad <= 0)
            //{
            //    MessageBox.Show("Cantidad inválida.");
            //    return;
            //}

            //// 🚫 Verificamos que la reserva no esté Ausente
            //BLLReserva_750VR bllReserva = new BLLReserva_750VR();
            //string estadoActual = bllReserva.ObtenerEstadoReserva(idReserva);

            //if (estadoActual.Equals("Ausente", StringComparison.OrdinalIgnoreCase))
            //{
            //    MessageBox.Show("No se pueden registrar insumos para una reserva marcada como 'Ausente'.");
            //    return;
            //}

            //if (!estadoActual.Equals("Realizado", StringComparison.OrdinalIgnoreCase))
            //{
            //    MessageBox.Show("Solo puedes registrar insumos para una reserva marcada como 'Realizado'.");
            //    return;
            //}

            //BLLreservaInsumo_750VR bll = new BLLreservaInsumo_750VR();

            //if (bll.InsumoYaAgregado(idReserva, idInsumo))
            //{
            //    MessageBox.Show("Este insumo ya fue cargado para esta reserva.");
            //    return;
            //}

            //try
            //{
            //    bll.RegistrarInsumoUsado(idReserva, idInsumo, cantidad);
            //    MessageBox.Show("Insumo registrado correctamente.");

            //    // ✅ Si era el primero, liberamos botón Salir
            //    insumosPendientes = false;
            //    button1.Enabled = true;

            //    textBox2.Clear();
            //    comboBox1.SelectedIndex = -1;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error al registrar insumo: " + ex.Message);
            //}
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;

            idReservaSeleccionada = -1;
            _insumosTmp.Clear();
            RefrescarGridInsumosTmp();
            RefrescarEstadoBotones();
        }

        private void RefrescarEstadoBotones()
        {
            bool hayReserva = idReservaSeleccionada != -1;

            bool reservaPendiente = false;
            if (hayReserva)
            {
                var bll = new BLLReserva_750VR();
                var estado = bll.ObtenerEstadoReserva(idReservaSeleccionada);
                reservaPendiente = estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase);
            }

            // button5 = Agregar, button3 = Aplicar, button2 = Ausente
            button5.Enabled = hayReserva && reservaPendiente;
            button3.Enabled = hayReserva && reservaPendiente && _insumosTmp.Count > 0;
            button2.Enabled = hayReserva && reservaPendiente && _insumosTmp.Count == 0;
        }

        private void RefrescarGridInsumosTmp()
        {
            var grid = this.Controls.Find("dgvInsumos", true).FirstOrDefault() as DataGridView;
            if (grid == null) return;

            var dt = new DataTable();
            dt.Columns.Add("IdInsumo", typeof(int));
            dt.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.ColInsumo") ?? "Insumo", typeof(string));
            dt.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.ColCantidad") ?? "Cantidad", typeof(int));

            foreach (var it in _insumosTmp)
                dt.Rows.Add(it.IdInsumo, it.Nombre, it.Cantidad);

            grid.DataSource = dt;
            if (grid.Columns.Contains("IdInsumo")) grid.Columns["IdInsumo"].Visible = false;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (idReservaSeleccionada == -1)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeSeleccionarReserva"));
                return;
            }

            var bllReserva = new BLLReserva_750VR();
            var estado = bllReserva.ObtenerEstadoReserva(idReservaSeleccionada);
            if (!estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeNoModificable"));
                return;
            }

            if (comboBox1.SelectedItem == null || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeCompletaCampos"));
                return;
            }

            if (!int.TryParse(textBox2.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormActualizarAgenda_750VR.MensajeCantidadInvalida"));
                return;
            }

            int idInsumo = Convert.ToInt32(comboBox1.SelectedValue);
            string nombre = ((dynamic)comboBox1.SelectedItem).nombre_750VR;

            var existente = _insumosTmp.FirstOrDefault(x => x.IdInsumo == idInsumo);
            if (existente != null) existente.Cantidad += cantidad;
            else _insumosTmp.Add(new InsumoTmp { IdInsumo = idInsumo, Nombre = nombre, Cantidad = cantidad });

            textBox2.Clear();
            comboBox1.SelectedIndex = -1;

            RefrescarGridInsumosTmp();
            RefrescarEstadoBotones();
        }
    }
    
}
