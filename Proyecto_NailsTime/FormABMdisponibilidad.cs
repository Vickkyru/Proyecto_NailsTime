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

        private void CargarDisponibilidad()
        {
            BLLdisponibilidad_750VR bll = new BLLdisponibilidad_750VR();
            var lista = bll.LeerDisponibilidades();

            dataGridView1.DataSource = lista;
            PintarFilasInactivas();
        }

        private void PintarFilasInactivas()
        {
            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.DataBoundItem is BEdisponibilidad_750VR dispo && !dispo.Activo)
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

        private void AplicarAlta()
        {
            var disponibilidad = new BEdisponibilidad_750VR
            {
                DNIempleado = Convert.ToInt32(cmbmanic.SelectedValue),
                DiaSemana = cmbdia.SelectedIndex + 1,
                HoraInicio = TimeSpan.Parse(txtinicio.Text),
                HoraFin = TimeSpan.Parse(txtfin.Text),
                Activo = true,
                estado = false
            };
            bll.Crear(disponibilidad);
            MessageBox.Show("Disponibilidad creada exitosamente.");

            try
            {
                // Validamos primero
                if (!ValidarCampos())
                    return;
                int dni = Convert.ToInt32(cmbmanic.SelectedValue);
                int dia = cmbdia.SelectedIndex + 1;
                TimeSpan inicio = TimeSpan.Parse(txtinicio.Text);
                TimeSpan fin = TimeSpan.Parse(txtfin.Text);
            

                BLLdisponibilidad_750VR bll = new BLLdisponibilidad_750VR();

                // me falta verificar existencia

                BEdisponibilidad_750VR nuevo = new BEdisponibilidad_750VR
                {
                    dni_750VR = dni,
                    nombre_750VR = nombre,
                    apellido_750VR = apellido,
                    mail_750VR = mail,
                    user_750VR = user,
                    salt_750VR = salt,
                    contraseña_750VR = encriptador.HashearConSalt_750VR(contraseña, salt),
                    rol_750VR = rol,
                    activo_750VR = true,
                    bloqueado_750VR = false
                };

                bll.CrearUsuario_750VR(nuevo);
                MessageBox.Show("Usuario creado correctamente.");
                MessageBox.Show("Recuerde que usuario=nombre+apellido, contraseña=dni+nombre");


                LimpiarCampos();

                CargarUsuarios(true);
            }
private bool ValidarCampos()
        {
            if (cmbmanicurista.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un manicurista.");
                return false;
            }

            if (cmbdia.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un día de la semana.");
                return false;
            }

            if (!TimeSpan.TryParse(txthorainicio.Text.Trim(), out TimeSpan horaInicio))
            {
                MessageBox.Show("La hora de inicio no es válida. Formato esperado: HH:mm");
                return false;
            }

            if (!TimeSpan.TryParse(txthorafin.Text.Trim(), out TimeSpan horaFin))
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
                d.DiaSemana = cmbdia.SelectedIndex + 1;
                d.HoraInicio = TimeSpan.Parse(txthoraInicio.Text);
                d.HoraFin = TimeSpan.Parse(txthoraFin.Text);
                bll.Modificar(d);
                MessageBox.Show("Disponibilidad modificada correctamente.");
            }
        }

        private void AplicarCambioEstado()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEdisponibilidad_750VR d)
            {
                bll.CambiarEstado(d.IdDisponibilidad, !d.Activo);
                string msg = d.Activo ? "Disponibilidad desactivada." : "Disponibilidad activada.";
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
            cmbdia.Enabled = txthoraInicio.Enabled = txthoraFin.Enabled = cmbmanicurista.Enabled = true;
            btnaplicar.Enabled = true;
            btncancelar.Enabled = true;
            btncrear.Enabled = btnmodificar.Enabled = btnestado.Enabled = false;
            dataGridView1.Enabled = true;
        }

        private void ResetearInterfaz()
        {
            btnaplicar.Enabled = false;
            btncancelar.Enabled = false;
            btncrear.Enabled = btnmodificar.Enabled = btnestado.Enabled = true;
            cmbdia.Enabled = txthoraInicio.Enabled = txthoraFin.Enabled = cmbmanicurista.Enabled = false;
            dataGridView1.Enabled = true;
        }

        private void LimpiarCampos()
        {
            cmbdia.SelectedIndex = -1;
            cmbmanicurista.SelectedIndex = -1;
            txthoraInicio.Clear();
            txthoraFin.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEdisponibilidad_750VR d)
            {
                cmbmanicurista.SelectedValue = d.DNIempleado;
                cmbdia.SelectedIndex = d.DiaSemana - 1;
                txthoraInicio.Text = d.HoraInicio.ToString();
                txthoraFin.Text = d.HoraFin.ToString();
            }
        }
        private void CargarManicuristas()
        {
            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();
            var manicuristas = bllUsuario.ObtenerManicuristasActivos_750VR();

            cmbmanicurista.DataSource = manicuristas;
            cmbmanicurista.DisplayMember = "nombre_750VR";   // o $"{nombre} {apellido}" si querés mostrar ambos
            cmbmanicurista.ValueMember = "dni_750VR";
        }
    }
}
