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

        // --- Dentro de FormBitacora_750VR ---
        private bool _ajustandoFechas = false;

        private void InicializarRangoFechas()
        {
            _ajustandoFechas = true;

            var hoy = DateTime.Today;
            var inicio = hoy.AddDays(-3); // últimos 3 días (inclusive)
            var fin = hoy;

            // Opcional: formatos “bonitos”
            dateTimePicker1.Format = DateTimePickerFormat.Long; // Inicio
            dateTimePicker2.Format = DateTimePickerFormat.Long; // Fin

            // Límites razonables
            dateTimePicker1.MinDate = new DateTime(2000, 1, 1);
            dateTimePicker2.MinDate = new DateTime(2000, 1, 1);
            dateTimePicker1.MaxDate = hoy;
            dateTimePicker2.MaxDate = hoy;

            // Valores por defecto
            dateTimePicker1.Value = inicio;
            dateTimePicker2.Value = fin;

            // Asegurar que FIN no pueda ir por debajo de INICIO
            dateTimePicker2.MinDate = dateTimePicker1.Value.Date;

            _ajustandoFechas = false;
        }

        private void FormBitacora_750VR_Load(object sender, EventArgs e)
        {

            // Rango por defecto y validación automática
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged; // Inicio
            dateTimePicker2.ValueChanged += dateTimePicker2_ValueChanged; // Fin
            InicializarRangoFechas();

            // Cargar datos últimos 3 días (usa los pickers ya inicializados)
            var bll = new BLLbitacora_750VR();
            var lista = bll.FiltrarEventos(
                dniUsuario: null,
                criticidad: null,
                evento: null,
                modulo: null,
                fechaInicio: dateTimePicker1.Value.Date,
                fechaFin: dateTimePicker2.Value.Date.AddDays(1).AddSeconds(-1) // fin inclusivo
            );
            dataGridView1.DataSource = lista;

            // Ocultar Id luego de bindear
            if (dataGridView1.Columns.Contains("Id_Evento"))
                dataGridView1.Columns["Id_Evento"].Visible = false;

            // Combos y headers
            CargarCombos();
            TraducirEncabezadosBitacora();

            // Reaccionar al cambio de usuario
            cmblog.SelectedIndexChanged += cmblog_SelectedIndexChanged;

            // Si no hay filas, que igual muestre Nombre/Apellido del usuario elegido
            if (dataGridView1.Rows.Count == 0)
                cmblog_SelectedIndexChanged(cmblog, EventArgs.Empty);


        }

       
        private void TraducirEncabezadosBitacora()
        {
            if (dataGridView1.Columns.Count == 0) return;

            var map = new Dictionary<string, string>
            {
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
            // =========================
            // Usuarios (con item vacío)
            // =========================
            var bllUser = new BLLusuario_750VR();
            var usuarios = bllUser.leerEntidades_750VR() ?? new List<BEusuario_750VR>();

            // Placeholder traducible si querés: "ComboBox.Seleccione"
            var textoSeleccione = Lenguaje_750VR.ObtenerEtiqueta("ComboBox.Seleccione");
            usuarios.Insert(0, new BEusuario_750VR(
                dni: 0, nombre: textoSeleccione, ape: "", mail: "", user: "",
                contra: "", salt: "", rol: "", activo: true, bloqueado: false, idiom: "Español"
            ));

            cmblog.DropDownStyle = ComboBoxStyle.DropDownList;
            cmblog.DataSource = usuarios;
            cmblog.DisplayMember = "user_750VR";
            cmblog.ValueMember = "dni_750VR";
            cmblog.SelectedIndex = 0; // arranca en "-- Seleccione --"

            // =========================
            // Eventos (Code = texto EXACTO que guarda la BLL/BD)
            // =========================
            var eventos = new List<Opcion>
    {
        // Usuario / Seguridad
        new Opcion { Code="Iniciar sesión",           Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.Login") },
        new Opcion { Code="Cerrar sesión",            Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.Logout") },
        new Opcion { Code="Cambio de clave",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CambioClave") },

        // Usuarios (Admin)
        new Opcion { Code="Crear usuario",            Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearUsuario") },
        new Opcion { Code="Activar usuario",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ActivarUsuario") },
        new Opcion { Code="Desactivar usuario",       Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.DesactivarUsuario") },
        new Opcion { Code="Modificar usuario",        Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ModificarUsuario") },
        new Opcion { Code="Desbloquear usuario",      Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.DesbloquearUsuario") },
        new Opcion { Code="Bloquear usuario",         Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.BloquearUsuario") },

        // Clientes
        new Opcion { Code="Crear cliente",            Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearCliente") },
        new Opcion { Code="Modificar cliente",        Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ModificarCliente") },

        // Reservas
        new Opcion { Code="Crear reserva",            Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearReserva") },
        new Opcion { Code="Modificar reserva",        Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ModificarReserva") },

        // Turnos
        new Opcion { Code="Turno realizado",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.TurnoRealizado") },
        new Opcion { Code="Turno cancelado",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.TurnoCancelado") },
        new Opcion { Code="Turno ausentado",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.TurnoAusentado") },
        new Opcion { Code="Insumos utilizados",       Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.InsumosUtilizados") },

        // Cobros / Reportes
        new Opcion { Code="Cobro realizado",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CobroRealizado") },
        new Opcion { Code="Cobro cancelado",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CobroCancelado") },
        new Opcion { Code="Imprimir factura",         Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ImprimirFactura") },

        // Disponibilidad
        new Opcion { Code="Crear disponibilidad",     Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearDisponibilidad") },
        new Opcion { Code="Modificar disponibilidad", Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ModificarDisponibilidad") },

        // Insumos
        new Opcion { Code="Crear insumos",            Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearInsumos") },
        new Opcion { Code="Modificar insumos",        Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ModificarInsumos") },

        // Servicios
        new Opcion { Code="Crear servicios",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearServicios") },
        new Opcion { Code="Modificar servicios",      Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ModificarServicios") },

        // Perfiles / Familias
        new Opcion { Code="Crear perfil",             Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearPerfil") },
        new Opcion { Code="Modificar perfil",         Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ModificarPerfil") },
        new Opcion { Code="Eliminar perfil",          Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.EliminarPerfil") },
        new Opcion { Code="Crear familia",            Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.CrearFamilia") },
        new Opcion { Code="Modificar familia",        Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.ModificarFamilia") },
        new Opcion { Code="Eliminar familia",         Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.EliminarFamilia") },

        // Compras / Recepción
        new Opcion { Code="Solicita cotización",      Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.SolicitaCotizacion") },
        new Opcion { Code="Pre-Registra proveedor",   Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.PreRegistraProveedor") },
        new Opcion { Code="Genera Orden de compra",   Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.GeneraOC") },
        new Opcion { Code="Registra proveedor",       Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.RegistraProveedor") },
        new Opcion { Code="Registra recepción",       Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.RegistraRecepcion") },

        // Admin – Backup/Restore/Serialización
        new Opcion { Code="Back up",                  Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.Backup") },
        new Opcion { Code="Restore",                  Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.Restore") },
        new Opcion { Code="Serializar",               Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.Serializar") },
        new Opcion { Code="DesSerializar",            Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Event.DesSerializar") },
    };

            cmbeve.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbeve.DataSource = eventos;
            cmbeve.DisplayMember = "Label";
            cmbeve.ValueMember = "Code";
            cmbeve.SelectedIndex = -1;

            // =========================
            // Módulos
            // =========================
            var modulos = new List<Opcion>
    {
        new Opcion { Code="Usuario",       Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Usuario") },
        new Opcion { Code="Administrador", Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Administrador") },
        new Opcion { Code="Reserva",       Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Reserva") },
        new Opcion { Code="Turno",         Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Turno") },
        new Opcion { Code="Cobro",         Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Cobro") },
        new Opcion { Code="Reportes",      Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Reportes") },
        new Opcion { Code="Compra",        Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Compra") },
        new Opcion { Code="Recepción",     Label=Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Module.Recepcion") }
    };

            cmbmodu.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbmodu.DataSource = modulos;
            cmbmodu.DisplayMember = "Label";
            cmbmodu.ValueMember = "Code";
            cmbmodu.SelectedIndex = -1;

            // =========================
            // Criticidad
            // =========================
            cmbcrit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbcrit.DataSource = new List<int> { 1, 2, 3, 4, 5 };
            cmbcrit.SelectedIndex = -1;

            // =========================
            // Rango por defecto: últimos 3 días
            // =========================
            dateTimePicker2.Value = DateTime.Today.AddDays(-3); // inicio
            dateTimePicker1.Value = DateTime.Today;             // fin
        }

        private void btnapli_Click(object sender, EventArgs e)
        {
            // --- Usuario (DNI) opcional ---
            int? dni = null;
            if (cmblog.SelectedIndex > 0 && cmblog.SelectedValue is int v && v != 0)
                dni = v;

            // --- Criticidad opcional ---
            int? criticidad = null;
            if (cmbcrit.SelectedItem != null)
                criticidad = Convert.ToInt32(cmbcrit.SelectedItem);

            // --- Evento / Módulo opcionales (valores "Code") ---
            string evento = (cmbeve.SelectedIndex >= 0) ? cmbeve.SelectedValue as string : null;
            string modulo = (cmbmodu.SelectedIndex >= 0) ? cmbmodu.SelectedValue as string : null;

            // --- Fechas (INICIO = dateTimePicker1, FIN = dateTimePicker2) ---
            // usar solo la parte de fecha; hacer el fin inclusivo (23:59:59.9999999)
            DateTime fechaInicio = dateTimePicker1.Value.Date;
            DateTime fechaFin = dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1);

            // si el usuario puso al revés, normalizamos
            if (fechaInicio > fechaFin)
            {
                var tmp = fechaInicio;
                fechaInicio = fechaFin.Date;               // por si venía con ticks
                fechaFin = tmp.Date.AddDays(1).AddTicks(-1);
            }

            // --- Buscar ---
            var bll = new BLLbitacora_750VR();
            var lista = bll.FiltrarEventos(dni, criticidad, evento, modulo, fechaInicio, fechaFin);

            // --- Bind + headers traducidos ---
            dataGridView1.DataSource = lista;
            TraducirEncabezadosBitacora();

            // --- Ocultar Id si existe ---
            if (dataGridView1.Columns.Contains("Id_Evento"))
                dataGridView1.Columns["Id_Evento"].Visible = false;

            // --- Mostrar nombre/apellido del usuario elegido (aunque no tenga movimientos) ---
            if (cmblog.SelectedIndex > 0 && cmblog.SelectedItem is BEusuario_750VR u)
            {
                txtnom.Text = u.nombre_750VR ?? "";
                txtape.Text = u.apellido_750VR ?? "";
            }
            else
            {
                txtnom.Clear();
                txtape.Clear();
            }
        }

        private void btnlimp_Click(object sender, EventArgs e)
        {
            // limpiar selects y textos
            try { cmblog.SelectedIndex = 0; } catch { cmblog.SelectedIndex = -1; }
            cmbcrit.SelectedIndex = -1;
            cmbeve.SelectedIndex = -1;
            cmbmodu.SelectedIndex = -1;
            txtnom.Clear();
            txtape.Clear();

            // Rango por defecto: últimos 3 días (inicio = hoy-3, fin = hoy)
            var hoy = DateTime.Today;
            var inicio = hoy.AddDays(-3);

            // 1) resetear límites para no chocar con Min/Max antiguos
            dateTimePicker1.MinDate = DateTimePicker.MinimumDateTime;
            dateTimePicker1.MaxDate = DateTimePicker.MaximumDateTime;
            dateTimePicker2.MinDate = DateTimePicker.MinimumDateTime;
            dateTimePicker2.MaxDate = DateTimePicker.MaximumDateTime;

            // 2) asegurar orden válido (por las dudas)
            if (inicio > hoy) (inicio, hoy) = (hoy, inicio);

            // 3) asignar valores (inicio = P1, fin = P2)
            dateTimePicker1.Value = inicio;   // Fecha inicio
            dateTimePicker2.Value = hoy;      // Fecha fin

            // 4) recargar últimos 3 días en la grilla
            var bll = new BLLbitacora_750VR();
            var lista = bll.FiltrarEventos(
                dniUsuario: null,
                criticidad: null,
                evento: null,
                modulo: null,
                fechaInicio: dateTimePicker1.Value.Date,
                fechaFin: dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1)
            );
            dataGridView1.DataSource = lista;

            TraducirEncabezadosBitacora();
            if (dataGridView1.Columns.Contains("Id_Evento"))
                dataGridView1.Columns["Id_Evento"].Visible = false;
        }

        private void btnimp_Click(object sender, EventArgs e)
        {
            var lista = dataGridView1.DataSource as List<BEbitacora_750VR>;
            if (lista == null || lista.Count == 0)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("Bitacora.NoDatos"),
                    Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Titulo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Archivo_750VR.GenerarBitacoraPDF(lista);
            MessageBox.Show(
                Lenguaje_750VR.ObtenerEtiqueta("Bitacora.ExportadaOk"),
                Lenguaje_750VR.ObtenerEtiqueta("Bitacora.Titulo"),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEbitacora_750VR ev)
            {
                var dal = new DAL_VR750.DALbitacora_750VR();
                var na = dal.ObtenerNombreApellidoPorLogin(ev.Login);
                txtnom.Text = na?.Nombre ?? "";
                txtape.Text = na?.Apellido ?? "";
            }
            else
            {
                // 👉 NUEVO: sin selección en grilla, usar el usuario del combo
                cmblog_SelectedIndexChanged(cmblog, EventArgs.Empty);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmblog_SelectedIndexChanged(object sender, EventArgs e)
        {
            // El DataSource del combo es List<BEusuario_750VR>
            if (cmblog.SelectedItem is BEusuario_750VR u)
            {
                txtnom.Text = u.nombre_750VR ?? "";
                txtape.Text = u.apellido_750VR ?? "";
            }
            else
            {
                txtnom.Clear();
                txtape.Clear();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (_ajustandoFechas) return;
            _ajustandoFechas = true;

            // Si INICIO sube, FIN no puede quedar por debajo
            var ini = dateTimePicker1.Value.Date;
            if (dateTimePicker2.Value.Date < ini)
                dateTimePicker2.Value = ini;

            dateTimePicker2.MinDate = ini;

            _ajustandoFechas = false;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (_ajustandoFechas) return;
            _ajustandoFechas = true;

            // Si FIN baja, INICIO no puede quedar por arriba
            var fin = dateTimePicker2.Value.Date;
            if (fin < dateTimePicker1.Value.Date)
                dateTimePicker1.Value = fin;

            _ajustandoFechas = false;
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}