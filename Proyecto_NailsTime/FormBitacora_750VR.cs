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
    public partial class FormBitacora_750VR : Form, Iobserver_750VR
    {
        public FormBitacora_750VR()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
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
            TraducirEncabezadosBitacora();
        }

        // Mapea columnas -> etiquetas traducidas
        private void TraducirEncabezadosBitacora()
        {
            if (dataGridView1.Columns.Count == 0) return;

            var map = new Dictionary<string, string>
    {
        // keys = NOMBRES DE PROPIEDAD en BEbitacora_750VR
        // ajustá si tus propiedades se llaman distinto
        { "Login",       Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Grid.Login") },
        { "Fecha",       Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Grid.Fecha") },
        { "Hora",        Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Grid.Hora") },
        { "Modulo",      Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Grid.Modulo") },
        { "Evento",      Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Grid.Evento") },
        { "Criticidad",  Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Grid.Criticidad") }
    };

            foreach (DataGridViewColumn c in dataGridView1.Columns)
                if (map.TryGetValue(c.DataPropertyName, out var header))
                    c.HeaderText = header;
        }



        private class Opcion { public string Code { get; set; } public string Label { get; set; } }

        private void CargarCombos()
        {
            // Usuarios (igual que antes)
            var bllUser = new BLLusuario_750VR();
            cmblog.DataSource = bllUser.leerEntidades_750VR();
            cmblog.DisplayMember = "user_750VR";
            cmblog.ValueMember = "dni_750VR";
            cmblog.SelectedIndex = -1;

            // Eventos (Code = valor que entiende tu BLL/BD, Label = traducido)
            var eventos = new[]
            {
        new Opcion{ Code="Login",              Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.Login") },
        new Opcion{ Code="Logout",             Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.Logout") },
        new Opcion{ Code="Crear Usuario",      Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearUsuario") },
        new Opcion{ Code="Modificar Usuario",  Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ModificarUsuario") },
        new Opcion{ Code="Crear Cliente",      Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearCliente") },
        new Opcion{ Code="Cancelar Reserva",   Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CancelarReserva") },
        new Opcion{ Code="Imprimir Factura",   Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ImprimirFactura") },
    }.ToList();
            cmbeve.DataSource = eventos;
            cmbeve.DisplayMember = "Label";
            cmbeve.ValueMember = "Code";
            cmbeve.SelectedIndex = -1;

            // Módulos
            var modulos = new[]
            {
        new Opcion{ Code="Usuario",        Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Usuario") },
        new Opcion{ Code="Administrador",  Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Administrador") },
        new Opcion{ Code="Reserva",        Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Reserva") },
        new Opcion{ Code="Cobro",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Cobro") },
        new Opcion{ Code="Turno",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Turno") },
    }.ToList();
            cmbmodu.DataSource = modulos;
            cmbmodu.DisplayMember = "Label";
            cmbmodu.ValueMember = "Code";
            cmbmodu.SelectedIndex = -1;

            // Criticidad (número se entiende en cualquier idioma)
            cmbcrit.DataSource = new List<int> { 1, 2, 3, 4, 5 };
            cmbcrit.SelectedIndex = -1;
        }
       

        private void btnapli_Click(object sender, EventArgs e)
        {
            int? dni = cmblog.SelectedValue as int?;
            int? criticidad = cmbcrit.SelectedItem as int?;
            string evento = cmbeve.SelectedValue as string;   // <--- Code
            string modulo = cmbmodu.SelectedValue as string;  // <--- Code

            DateTime fechaInicio = dateTimePicker1.Value.Date;
            DateTime fechaFin = dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1);

            var bll = new BLLbitacora_750VR();
            var lista = bll.FiltrarEventos(dni, criticidad, evento, modulo, fechaInicio, fechaFin);
            dataGridView1.DataSource = lista;

            TraducirEncabezadosBitacora();
           
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
            TraducirEncabezadosBitacora();
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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }
    }
}
