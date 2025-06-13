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

    
    public partial class FormABMdisponibilidad : Form, Iobserver_750VR
    {
        public FormABMdisponibilidad()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }
        private string modoActual = "consulta";
        BLLdisponibilidad_750VR bll = new BLLdisponibilidad_750VR();

        private void CargarDisponibilidad()
        {
            
            var lista = bll.LeerDisponibilidades_750VR();
            dataGridView1.DataSource = lista;
            PintarFilasInactivas();
        }

        private void PintarFilasInactivas()
        {
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.DataBoundItem is BEdisponibilidad_750VR dispo && !dispo.activo_750VR)
                {
                    fila.DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void btnañadir_Click(object sender, EventArgs e)
        {
            modoActual = "añadir";
            ActivarModoEdicion();
            lblmensaje.Text = "Modo Añadir";
        }

        private void FormABMdisponibilidad_Load(object sender, EventArgs e)
        {
            modoActual = "Consulta";
            lblmensaje.Text = modoActual;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            cmbmanic.SelectedIndexChanged += cmbmanic_SelectedIndexChanged;
            CargarManicuristas();
            CargarDisponibilidad();
            PintarFilasInactivas();
        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Modificar";
            modoActual = "modificar";
            ActivarModoEdicion();
        }

        private void btnelim_Click(object sender, EventArgs e)
        {
            modoActual = "cambiarEstado";
            lblmensaje.Text = "Modo Activar/Desactivar";
           
            ActivarModoEdicion();
        }

        private void btnapli_Click(object sender, EventArgs e)
        {
            switch (modoActual)
            {
                case "añadir": AplicarAlta(); break;
                case "modificar": AplicarModificacion(); break;
                case "cambiarEstado": AplicarCambioEstado(); break;
            }
            modoActual = "consulta";
            lblmensaje.Text = "Modo Consulta";
            ResetearInterfaz();
            CargarDisponibilidad();
            LimpiarCampos();
        }

        private void CargarManicuristas()
        {
            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();
            var manicuristas = bllUsuario.ObtenerManicuristasActivos_750VR();

            // Insertamos un "item vacío" al principio usando el constructor completo
            var vacio = new BEusuario_750VR(
                dni: 0,
                nombre: "-- Seleccione --",
                ape: "",
                mail: "",
                user: "",
                contra: "",
                salt: "",
                rol: "manicurista",
                activo: true,
                bloqueado: false
            );

            manicuristas.Insert(0, vacio);

            cmbmanic.DataSource = manicuristas;
            cmbmanic.DisplayMember = "nombre_750VR";
            cmbmanic.ValueMember = "dni_750VR";
            cmbmanic.SelectedIndex = 0;
        }

        private void AplicarAlta()
        {
            try
            {
                if (!ValidarCampos()) return;

                int dni = Convert.ToInt32(txtdnimanic.Text);
                DateTime dia = dateTimePicker1.Value.Date;
                TimeSpan inicio = TimeSpan.Parse(txtinicio.Text);
                TimeSpan fin = TimeSpan.Parse(txtfin.Text);

                // Buscar si ya existe una disponibilidad con ese mismo manicurista, fecha y hora
                var existentes = bll.LeerDisponibilidades_750VR();
                bool yaExiste = existentes.Any(d =>
                    d.DNImanic_750VR == dni &&
                    d.Fecha_750VR.Date == dia.Date &&
                    d.HoraInicio_750VR == inicio &&
                    d.HoraFin_750VR == fin &&
                    d.activo_750VR // solo verificamos entre las activas
                );

                if (yaExiste)
                {
                    MessageBox.Show("Ya existe una disponibilidad para ese manicurista, fecha y horario.");
                    return;
                }

                BEdisponibilidad_750VR nuevo = new BEdisponibilidad_750VR(dni, dia, inicio, fin, true, false);

                bll.CrearDisponibilidad_750VR(nuevo);
                MessageBox.Show("Disponibilidad creada correctamente.");
                LimpiarCampos();
                ResetearInterfaz();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear disponibilidad: " + ex.Message);
            }
        }
            private bool ValidarCampos()
        {
            if (cmbmanic.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un manicurista.");
                return false;
            }

            if (dateTimePicker1.Text == null)
            {
                MessageBox.Show("Debe seleccionar un día de la semana.");
                return false;
            }

            if (!TimeSpan.TryParse(txtinicio.Text.Trim(), out TimeSpan horaInicio))
            {
                MessageBox.Show("La hora de inicio no es válida. Formato esperado: HH:mm");
                return false;
            }

            if (!TimeSpan.TryParse(txtfin.Text.Trim(), out TimeSpan horaFin))
            {
                MessageBox.Show("La hora de fin no es válida. Formato esperado: HH:mm");
                return false;
            }

            if (horaInicio >= horaFin)
            {
                MessageBox.Show("La hora de inicio debe ser anterior a la hora de fin.");
                return false;
            }

            return true;
        }

        private void AplicarModificacion()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEdisponibilidad_750VR d)
            {
                // Validación de selección de manicurista
                if (cmbmanic.SelectedItem is BEusuario_750VR manicuristaSeleccionado)
                {
                    // Validar horas
                    if (!TimeSpan.TryParse(txtinicio.Text, out TimeSpan nuevaHoraInicio))
                    {
                        MessageBox.Show("Hora de inicio inválida.");
                        return;
                    }

                    if (!TimeSpan.TryParse(txtfin.Text, out TimeSpan nuevaHoraFin))
                    {
                        MessageBox.Show("Hora de fin inválida.");
                        return;
                    }

                    if (nuevaHoraInicio >= nuevaHoraFin)
                    {
                        MessageBox.Show("La hora de inicio debe ser anterior a la hora de fin.");
                        return;
                    }

                    // Obtener nuevos valores desde los controles
                    d.DNImanic_750VR = manicuristaSeleccionado.dni_750VR; // ← esto asegura que cambia el DNI
                    d.Fecha_750VR = dateTimePicker1.Value.Date;
                    d.HoraInicio_750VR = nuevaHoraInicio;
                    d.HoraFin_750VR = nuevaHoraFin;

                    // Ejecutar modificación
                    bool modificado = bll.ModificarDisponibilidad_750VR(d);

                    if (modificado)
                    {
                        MessageBox.Show("Disponibilidad modificada correctamente.");
                        CargarDisponibilidad();
                        ResetearInterfaz();
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo modificar la disponibilidad.");
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione un manicurista válido.");
                }
            }
            else
            {
                MessageBox.Show("Seleccione una disponibilidad de la grilla.");
            }
        }

        private void AplicarCambioEstado()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEdisponibilidad_750VR d)
            {
                bll.CambiarEstado_750VR(d.IdDisponibilidad_750VR, !d.activo_750VR);
                string msg = d.activo_750VR ? "Disponibilidad desactivada." : "Disponibilidad activada.";
                MessageBox.Show(msg);
                ResetearInterfaz();
                LimpiarCampos();
            }
        }

        private void btncance_Click(object sender, EventArgs e)
        {
            modoActual = "consulta";
            lblmensaje.Text = "Modo Consulta";
            ResetearInterfaz();
            LimpiarCampos();
        }
        private void ActivarModoEdicion()
        {
            dateTimePicker1.Enabled = txtinicio.Enabled = txtfin.Enabled = cmbmanic.Enabled = true;
            btnapli.Enabled = true;
            btncance.Enabled = true;
            btnañadir.Enabled = btnmod.Enabled = btnelim.Enabled = false;
            dataGridView1.Enabled = true;
        }

        private void ResetearInterfaz()
        {
            btnapli.Enabled = false;
            btncance.Enabled = false;
            btnañadir.Enabled = btnmod.Enabled = btnelim.Enabled = true;
            txtinicio.Enabled = txtfin.Enabled = cmbmanic.Enabled = false;
            dataGridView1.Enabled = true;
        }

        private void LimpiarCampos()
        {
            txtdnimanic.Clear();
            cmbmanic.SelectedIndex = -1;
            txtinicio.Clear();
            txtfin.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEdisponibilidad_750VR d)
            {
                cmbmanic.SelectedValue = d.DNImanic_750VR;                 // ✅ Selecciona al manicurista por DNI
                dateTimePicker1.Value = d.Fecha_750VR;                     // ✅ Asigna la fecha completa
                txtinicio.Text = d.HoraInicio_750VR.ToString(@"hh\:mm");  // ✅ Formato claro
                txtfin.Text = d.HoraFin_750VR.ToString(@"hh\:mm");
            }
        }
   

        private void lblmensaje_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var d = dataGridView1.SelectedRows[0].DataBoundItem as BEdisponibilidad_750VR;
                if (d != null)
                {
                    cmbmanic.SelectedValue = d.DNImanic_750VR;
                    txtdnimanic.Text = d.DNImanic_750VR.ToString();
                    dateTimePicker1.Value = d.Fecha_750VR;
                    txtinicio.Text = d.HoraInicio_750VR.ToString();
                    txtfin.Text = d.HoraFin_750VR.ToString();
                }
            }
        }

        private void cmbmanic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbmanic.SelectedIndex > 0 && cmbmanic.SelectedItem is BEusuario_750VR manicurista)
            {
                txtdnimanic.Text = manicurista.dni_750VR.ToString();
            }
            else
            {
                txtdnimanic.Text = ""; // o podrías dejarlo en blanco
            }
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
