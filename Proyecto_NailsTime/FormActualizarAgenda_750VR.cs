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
            // Validar si hay sesión iniciada
            var sesion = SessionManager_750VR.ObtenerInstancia;
            if (sesion == null || sesion.user == null)
            {
                MessageBox.Show("No hay ningún manicurista logueado actualmente.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Hide(); // O podés deshabilitar controles si preferís mantener la pantalla abierta
                return;
            }

            // Obtener DNI del manicurista logueado
            int dniManicurista = sesion.user.dni_750VR;

            // Cargar reservas
            var bll = new BLLReserva_750VR();
            var reservas = bll.ObtenerReservasPorManicurista(dniManicurista);

            // Mostrar en el DataGridView
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = reservas;
        }
    }
    
}
