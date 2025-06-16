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

namespace Proyecto_NailsTime
{
    public partial class FormActualizarAgenda_750VR : Form, Iobserver_750VR
    {
        public FormActualizarAgenda_750VR()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
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
                    r.IdReserva_750VR
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

            bll.ActualizarEstadoReserva(idReservaSeleccionada, "Realizado");

            MessageBox.Show(
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaRealizada"),
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloConfirmacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            CargarReservas();
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

            BLLReserva_750VR bll = new BLLReserva_750VR();
            string estadoActual = bll.ObtenerEstadoReserva(idReservaSeleccionada);

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

            bll.ActualizarEstadoReserva(idReservaSeleccionada, "Cancelado");

            MessageBox.Show(
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.MensajeReservaCancelada"),
                Lenguaje_750VR.ObtenerEtiqueta("FormAgenda_750VR.TituloConfirmacion"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
            CargarReservas();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
