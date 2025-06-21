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
    public partial class FormActualizarAgenda_750VR : Form, Iobserver_750VR
    {
        public FormActualizarAgenda_750VR()
        {
            InitializeComponent();
            //Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            //ActualizarIdioma();
        }

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
            //Valida();
            CargarReservas();
            Disponibilidad();
            CargarInsumos();
        }
        private void CargarInsumos()
        {
            BLLinsumos_750VR bllInsumo = new BLLinsumos_750VR();
            var listaInsumos = bllInsumo.ObtenerInsumosActivos();

            comboBox1.DataSource = listaInsumos;
            comboBox1.DisplayMember = "nombre_750VR";
            comboBox1.ValueMember = "codinsumo_750VR";
        }

        private void Disponibilidad()
        {

            BLLdisponibilidad_750VR bllDispo = new BLLdisponibilidad_750VR();
            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();
            var sesion = SessionManager_750VR.ObtenerInstancia;
            int dniManicurista = sesion.user.dni_750VR;

            var listaDispo = bllDispo.ObtenerDisponibilidadesPorManicurista(dniManicurista);
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
                    dispo.CodDisponibilidad_750VR,
                    nombreCompleto,
                    dispo.DNImanic_750VR,
                    dispo.Fecha_750VR.Date,
                    dispo.HoraInicio_750VR.ToString(@"hh\:mm"),
                    dispo.HoraFin_750VR.ToString(@"hh\:mm"),
                     //"Disponible" 
                     Lenguaje_750VR.ObtenerEtiqueta("FormRegistrarReserva_750VR.Grid1_Disponible")
                );
            }

            dataGridView2.DataSource = tabla;

            if (dataGridView2.Columns.Contains("IdDisponibilidad"))
                dataGridView2.Columns["IdDisponibilidad"].Visible = false;
            if (dataGridView2.Columns.Contains("DNImanicurista"))
                dataGridView2.Columns["DNImanicurista"].Visible = false;
            // 🔤 Traducción de encabezados
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
            var reservas = bll.ObtenerReservasPorManicurista(dniManicurista);

            DataTable tabla = new DataTable();
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.DNICliente"), typeof(int));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.NombreCliente"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.NombreManic"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.Servicio"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.Tecnica"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.FechaReserva"), typeof(DateTime));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.HoraInicio"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.HoraFin"), typeof(string));
            tabla.Columns.Add(Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.Estado"), typeof(string));
            tabla.Columns.Add("IdReserva", typeof(int)); // oculta

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

        private int idReservaSeleccionada = -1;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                idReservaSeleccionada = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IdReserva"].Value);
                textBox1.Text = idReservaSeleccionada.ToString(); 
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idReservaSeleccionada == -1)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeSeleccionaReserva"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            BLLReserva_750VR bll = new BLLReserva_750VR();
            string estadoActual = bll.ObtenerEstadoReserva(idReservaSeleccionada);

            if (!estadoActual.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaNoModificable"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            // ✅ Validar que se hayan cargado insumos
            BLLreservaInsumo_750VR bllInsumo = new BLLreservaInsumo_750VR();
            if (!bllInsumo.TieneInsumosRegistrados(idReservaSeleccionada))
            {
                MessageBox.Show("Debes registrar al menos un insumo utilizado antes de marcar como 'Realizado'.");
                return;
            }

            // ✅ Actualizar estado si pasó validación
            bll.ActualizarEstadoReserva(idReservaSeleccionada, "Realizado");

            MessageBox.Show(
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaRealizada"),
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloConfirmacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            CargarReservas();
            Disponibilidad();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (idReservaSeleccionada == -1)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeSeleccionaReserva"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
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
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaNoCancelable"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            // Obtener la reserva completa
            var reserva = bllReserva.ObtenerReservaPorId(idReservaSeleccionada);

            if (reserva == null)
            {
                MessageBox.Show("No se encontró la reserva.");
                return;
            }

            // Cambiar estado de reserva
            bllReserva.ActualizarEstadoReserva(reserva.CodReserva_750VR, "Cancelado");

            // Reactivar disponibilidad correspondiente
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
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaCancelada"),
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloConfirmacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            CargarReservas();
            Disponibilidad();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idReservaSeleccionada == -1)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeSeleccionaReserva"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            BLLReserva_750VR bll = new BLLReserva_750VR();
            string estadoActual = bll.ObtenerEstadoReserva(idReservaSeleccionada);

            if (!estadoActual.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaNoModificable"),
                    Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                return;
            }

            bll.ActualizarEstadoReserva(idReservaSeleccionada, "Ausente");

            MessageBox.Show(
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaRealizada"),
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloConfirmacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            CargarReservas();
            Disponibilidad();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || comboBox1.SelectedItem == null || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Completa todos los campos");
                return;
            }

            int idReserva = Convert.ToInt32(textBox1.Text);
            int idInsumo = Convert.ToInt32(comboBox1.SelectedValue);
            int cantidad;

            if (!int.TryParse(textBox2.Text, out cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Cantidad inválida");
                return;
            }

            BLLreservaInsumo_750VR bll = new BLLreservaInsumo_750VR();

            if (bll.InsumoYaAgregado(idReserva, idInsumo))
            {
                MessageBox.Show("Este insumo ya fue cargado para esta reserva.");
                return;
            }

            try
            {
                bll.RegistrarInsumoUsado(idReserva, idInsumo, cantidad);
                MessageBox.Show("Insumo registrado correctamente");

                textBox2.Clear();
                comboBox1.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar insumo: " + ex.Message);
            }
        }
    }
    
}
