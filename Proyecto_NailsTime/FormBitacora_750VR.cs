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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_NailsTime
{
    public partial class FormBitacora_750VR : Form
    {
        public FormBitacora_750VR()
        {
            InitializeComponent();
        }

        private void FormBitacora_750VR_Load(object sender, EventArgs e)
        {
            BLLbitacora_750VR bll = new BLLbitacora_750VR();
            DateTime fechaInicio = DateTime.Now.AddDays(-3);
            DateTime fechaFin = DateTime.Now;

            var lista = bll.FiltrarEventos(null, null, null, null, fechaInicio, fechaFin);
            dataGridView1.DataSource = lista;

            // Configurar combos desplegables
            CargarCombos();
        }

        private void CargarCombos()
        {
            var bllUser = new BLLusuario_750VR();
            cmblog.DataSource = bllUser.leerEntidades_750VR();
            cmblog.DisplayMember = "user_750VR";
            cmblog.ValueMember = "dni_750VR";
            cmblog.SelectedIndex = -1;

            cmbeve.DataSource = new List<string>
    {
        "Login", "Logout", "Crear Usuario", "Modificar Usuario",
        "Crear Cliente", "Cancelar Reserva", "Imprimir Factura"
    };
            cmbeve.SelectedIndex = -1;

            cmbmodu.DataSource = new List<string>
    {
        "Usuario", "Administrador", "Reserva", "Cobro", "Turno"
    };
            cmbmodu.SelectedIndex = -1;

            cmbcrit.DataSource = new List<int> { 1, 2, 3, 4, 5 };
            cmbcrit.SelectedIndex = -1;
        }

        private void btnapli_Click(object sender, EventArgs e)
        {
            int? dni = cmblog.SelectedValue as int?;
            int? criticidad = cmbcrit.SelectedItem as int?;
            string evento = cmbeve.SelectedItem?.ToString();
            string modulo = cmbmodu.SelectedItem?.ToString();
            DateTime fechaInicio = dateTimePicker1.Value.Date;
            DateTime fechaFin = dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1);

            BLLbitacora_750VR bll = new BLLbitacora_750VR();
            var lista = bll.FiltrarEventos(dni, criticidad, evento, modulo, fechaInicio, fechaFin);
            dataGridView1.DataSource = lista;
        }

        private void btnlimp_Click(object sender, EventArgs e)
        {
            cmblog.SelectedIndex = -1;
            cmbcrit.SelectedIndex = -1;
            cmbeve.SelectedIndex = -1;
            cmbmodu.SelectedIndex = -1;
            txtnom.Clear();
            txtape.Clear();

            dateTimePicker1.Value = DateTime.Now.AddDays(-3);
            dateTimePicker2.Value = DateTime.Now;

            FormBitacora_750VR_Load(sender, e); // recargar últimos 3 días
        }

        private void btnimp_Click(object sender, EventArgs e)
        {
            var lista = dataGridView1.DataSource as List<BEbitacora_750VR>;
            if (lista == null || lista.Count == 0)
            {
                MessageBox.Show("No hay datos para imprimir.");
                return;
            }

            Archivo_750VR.GenerarBitacoraPDF(lista);
            MessageBox.Show("Bitácora exportada correctamente.");
        }
    }
}
