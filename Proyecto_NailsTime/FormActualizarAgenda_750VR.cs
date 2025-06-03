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
    public partial class FormActualizarAgenda_750VR : Form
    {
        public FormActualizarAgenda_750VR()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormActualizarAgenda_750VR_Load(object sender, EventArgs e)
        {
            Valida();
            CargarReservas();
        }
        private void CargarReservas()
        {
            BLLReserva_750VR bll = new BLLReserva_750VR();
            var lista = bll.leerEntidades();
        }

        public void Valida()
        {
            var sesion = SessionManager_750VR.ObtenerInstancia;
            int dniManicurista = sesion.user.dni_750VR;

            var bll = new BLLReserva_750VR();
            var reservas = bll.ObtenerReservasPorManicurista(dniManicurista);

            DataTable tabla = new DataTable();
            tabla.Columns.Add("DNI Cliente", typeof(int));
            tabla.Columns.Add("Nombre Cliente", typeof(string));
            tabla.Columns.Add("Nombre Manicurista", typeof(string));
            tabla.Columns.Add("Nombre Servicio", typeof(string));
            tabla.Columns.Add("Técnica", typeof(string));
            tabla.Columns.Add("Fecha Reserva", typeof(DateTime));
            tabla.Columns.Add("Hora Inicio", typeof(string));
            tabla.Columns.Add("Hora Fin", typeof(string));
            tabla.Columns.Add("Estado", typeof(string));
            tabla.Columns.Add("IdReserva", typeof(int)); // ocultable, para selección

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
                MessageBox.Show("Seleccioná una reserva.");
                return;
            }

            BLLReserva_750VR bll = new BLLReserva_750VR();
            bll.ActualizarEstadoReserva(idReservaSeleccionada, "Realizado");
            MessageBox.Show("Reserva marcada como realizada.");
            Valida(); // vuelve a refrescar el DGV
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (idReservaSeleccionada == -1)
            {
                MessageBox.Show("Seleccioná una reserva.");
                return;
            }

            BLLReserva_750VR bll = new BLLReserva_750VR();
            bll.ActualizarEstadoReserva(idReservaSeleccionada, "Cancelado");
            MessageBox.Show("Reserva cancelada.");
            CargarReservas();
        }
    }
    
}
