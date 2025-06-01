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

    //hacerlo mejor, voy a buscar el nombre del manicurista por el combobox y ahi se me va a mostrar su dni y ahi me va a crear la disponibilidad
    public partial class FormABMdisponibilidad : Form
    {
        public FormABMdisponibilidad()
        {
            InitializeComponent();
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
            lblmensaje.Text = modoActual;
        }

        private void FormABMdisponibilidad_Load(object sender, EventArgs e)
        {
            modoActual = "Consulta";
            lblmensaje.Text = modoActual;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            CargarManicuristas();
            CargarDisponibilidad();
        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = "Modo Modificar";
            modoActual = "modificar";
            ActivarModoEdicion();
        }

        private void btnelim_Click(object sender, EventArgs e)
        {
            lblmensaje.Text = modoActual;
            modoActual = "Modo Activar/Desactivar";
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

            cmbmanic.DataSource = manicuristas;
            cmbmanic.DisplayMember = "nombre_750VR";   // o $"{nombre} {apellido}" si querés mostrar ambos
            cmbmanic.ValueMember = "dni_750VR";
        }

        private void AplicarAlta()
        {
            try
            {
                // Validamos primero
                if (!ValidarCampos())
                    return;
                int dni = Convert.ToInt32(txtdnimanic.Text);
                string nom = cmbmanic.Text;
                DateTime dia = dateTimePicker1.MinDate;
                TimeSpan inicio = TimeSpan.Parse(txtinicio.Text);
                TimeSpan fin = TimeSpan.Parse(txtfin.Text);


                // me falta verificar existencia

                BEdisponibilidad_750VR nuevo = new BEdisponibilidad_750VR
                {
                    DNImanic_750VR = dni,
                    HoraInicio_750VR = inicio,
                    Fecha_750VR = dia,
                    HoraFin_750VR = fin,
                    activo_750VR = true,
                    estado_750VR = false
                };

                bll.CrearDisponibilidad_750VR(nuevo);
                MessageBox.Show("Disponibilidad creada correctamente.");

                LimpiarCampos();
                //CargarUsuarios();
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo");

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
                d.Fecha_750VR = dateTimePicker1.MinDate;
                d.HoraInicio_750VR = TimeSpan.Parse(txtinicio.Text);
                d.HoraFin_750VR = TimeSpan.Parse(txtfin.Text);
                bll.ModificarDisponibilidad_750VR(d);
                MessageBox.Show("Disponibilidad modificada correctamente.");
            }
        }

        private void AplicarCambioEstado()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEdisponibilidad_750VR d)
            {
                bll.CambiarEstado_750VR(d.IdDisponibilidad_750VR, !d.activo_750VR);
                string msg = d.activo_750VR ? "Disponibilidad desactivada." : "Disponibilidad activada.";
                MessageBox.Show(msg);
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
            //btnaplicar.Enabled = true;
            //btncancelar.Enabled = true;
            //btncrear.Enabled = btnmodificar.Enabled = btnestado.Enabled = false;
            dataGridView1.Enabled = true;
        }

        private void ResetearInterfaz()
        {
            //btnaplicar.Enabled = false;
            //btncancelar.Enabled = false;
            //btncrear.Enabled = btnmodificar.Enabled = btnestado.Enabled = true;
            //cmbdia.Enabled = txthoraInicio.Enabled = txthoraFin.Enabled = cmbmanicurista.Enabled = false;
            dataGridView1.Enabled = true;
        }

        private void LimpiarCampos()
        {
            
            cmbmanic.SelectedIndex = -1;
            txtinicio.Clear();
            txtfin.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEdisponibilidad_750VR d)
            {
                //cmbmanic.SelectedValue = d.Nombremanic_750VR;
                //cmbdia.SelectedIndex = d.DiaSemana_750VR - 1;
                txtinicio.Text = d.HoraInicio_750VR.ToString();
                txtfin.Text = d.HoraFin_750VR.ToString();
            }
        }
   

        private void lblmensaje_Click(object sender, EventArgs e)
        {

        }
    }
}
