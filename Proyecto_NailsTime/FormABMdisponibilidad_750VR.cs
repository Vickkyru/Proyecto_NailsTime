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
        private string modoActual = "consulta";
        BLLdisponibilidad_750VR bll = new BLLdisponibilidad_750VR();
        public FormABMdisponibilidad()
        {
            InitializeComponent();
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
            ActualizarMensajeModo();
        }
        private void ActualizarMensajeModo()
        {
            string clave = "FormABMdisponibilidad.Mensaje." + modoActual;
            lblmensaje.Text = Lenguaje_750VR.ObtenerEtiqueta(clave);
        }


        private void CargarDisponibilidad(bool soloActivos)
       
        {
            var lista = bll.LeerDisponibilidades_750VR();

            // Filtrar por manicuristas activos
            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();
            var manicuristasActivos = bllUsuario.ObtenerManicuristasActivos_750VR();
            var dniActivos = manicuristasActivos.Select(m => m.dni_750VR).ToList();

            // Filtrar disponibilidades de manicuristas activos
            var filtradas = lista.Where(d => dniActivos.Contains(d.DNImanic_750VR)).ToList();

            // Aplicar filtro si se quieren solo activos
            if (soloActivos)
            {
                filtradas = filtradas.Where(d => d.activo_750VR).ToList();
            }

            // Configurar DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdDisponibilidad_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Disponibilidad.Id"),
                ReadOnly = true,
                Visible = false
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DNImanic_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Disponibilidad.DNI"),
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Fecha_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Disponibilidad.Fecha"),
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HoraInicio_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Disponibilidad.HoraInicio"),
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HoraFin_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Disponibilidad.HoraFin"),
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "activo_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Disponibilidad.Activo"),
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "estado_750VR",
                HeaderText = Lenguaje_750VR.ObtenerEtiqueta("Grid.Disponibilidad.Estado"),
                ReadOnly = true
            });

            dataGridView1.DataSource = filtradas;
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
            //lblmensaje.Text = "Modo Añadir";
            ActualizarMensajeModo();
        }

        private void FormABMdisponibilidad_Load(object sender, EventArgs e)
        {
            
            modoActual = "consulta";
            //lblmensaje.Text = modoActual;
            //ActualizarMensajeModo();
            btnapli.Enabled = false;
            btncance.Enabled = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            cmbmanic.SelectedIndexChanged += cmbmanic_SelectedIndexChanged;
            CargarManicuristas();
            rbnActivos.Checked = true;
            CargarDisponibilidad(true);
            
        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            //lblmensaje.Text = "Modo Modificar";
            modoActual = "modificar";
            ActualizarMensajeModo();
            ActivarModoEdicion();
        }

        private void btnelim_Click(object sender, EventArgs e)
        {
            modoActual = "cambiarEstado";
            //lblmensaje.Text = "Modo Activar/Desactivar";
            ActualizarMensajeModo();
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
            //lblmensaje.Text = "Modo Consulta";
            ActualizarMensajeModo();
            ResetearInterfaz();
            CargarDisponibilidad(true);
            LimpiarCampos();
        }

        private void CargarManicuristas()
        {
            BLLusuario_750VR bllUsuario = new BLLusuario_750VR();
            var manicuristas = bllUsuario.ObtenerManicuristasActivos_750VR();

            // Traduce el texto "-- Seleccione --" con el sistema de idiomas
            string textoSeleccion = Lenguaje_750VR.ObtenerEtiqueta("ComboBox.Seleccione");
            int cod = 3;

            // Insertamos un "item vacío" al principio usando el idioma actual
            var vacio = new BEusuario_750VR(
                dni: 0,
                nombre: textoSeleccion,
                ape: "",
                mail: "",
                user: "",
                contra: "",
                salt: "",
                rol: "manicurista",
                activo: true,
                bloqueado: false,
                idiom: Lenguaje_750VR.ObtenerInstancia().IdiomaActual,
                cod
            );

            manicuristas.Insert(0, vacio);

            cmbmanic.DataSource = manicuristas;
            cmbmanic.DisplayMember = "nombre_750VR";
            cmbmanic.ValueMember = "dni_750VR";
            cmbmanic.SelectedIndex = 0;
        }
        private void CargarManicuristaEnCombo(int dni)
        {
            var lista = (List<BEusuario_750VR>)cmbmanic.DataSource;
            var existe = lista.Any(m => m.dni_750VR == dni);

            if (existe)
            {
                cmbmanic.SelectedValue = dni;
            }
            else
            {
                // Mostrar solo el DNI en el textbox
                txtdnimanic.Text = dni.ToString();
                cmbmanic.SelectedIndex = 0; // "-- Seleccione --"
            }
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

                var existentes = bll.LeerDisponibilidades_750VR();
                bool yaExiste = existentes.Any(d =>
                    d.DNImanic_750VR == dni &&
                    d.Fecha_750VR.Date == dia.Date &&
                    d.HoraInicio_750VR == inicio &&
                    d.HoraFin_750VR == fin &&
                    d.activo_750VR
                );

                if (yaExiste)
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.DisponibilidadYaExiste"));
                    return;
                }

                BEdisponibilidad_750VR nuevo = new BEdisponibilidad_750VR(dni, dia, inicio, fin, true, false);

                bll.CrearDisponibilidad_750VR(nuevo);
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.DisponibilidadCreada"));
                CargarDisponibilidad(true);
                LimpiarCampos();
                ResetearInterfaz();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.ErrorCrearDisponibilidad") + ": " + ex.Message);
            }
        }
        private bool ValidarCampos()
        {
            if (cmbmanic.SelectedItem == null)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.ManicuristaNoSeleccionado"));
                return false;
            }

            if (string.IsNullOrWhiteSpace(dateTimePicker1.Text))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.FechaNoSeleccionada"));
                return false;
            }

            if (!TimeSpan.TryParse(txtinicio.Text.Trim(), out TimeSpan horaInicio))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.HoraInicioInvalida"));
                return false;
            }

            if (!TimeSpan.TryParse(txtfin.Text.Trim(), out TimeSpan horaFin))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.HoraFinInvalida"));
                return false;
            }

            if (horaInicio >= horaFin)
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.HoraInicioMayor"));
                return false;
            }

            return true;
        }

        private void AplicarModificacion()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEdisponibilidad_750VR d)
            {
                if (cmbmanic.SelectedItem is BEusuario_750VR manicuristaSeleccionado)
                {
                    if (!TimeSpan.TryParse(txtinicio.Text, out TimeSpan nuevaHoraInicio))
                    {
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.HoraInicioInvalida"));
                        return;
                    }

                    if (!TimeSpan.TryParse(txtfin.Text, out TimeSpan nuevaHoraFin))
                    {
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.HoraFinInvalida"));
                        return;
                    }

                    if (nuevaHoraInicio >= nuevaHoraFin)
                    {
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.HoraInicioMayor"));
                        return;
                    }

                    d.DNImanic_750VR = manicuristaSeleccionado.dni_750VR;
                    d.Fecha_750VR = dateTimePicker1.Value.Date;
                    d.HoraInicio_750VR = nuevaHoraInicio;
                    d.HoraFin_750VR = nuevaHoraFin;

                    bool modificado = bll.ModificarDisponibilidad_750VR(d);

                    if (modificado)
                    {
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.DisponibilidadModificada"));
                        CargarDisponibilidad(true);
                        ResetearInterfaz();
                        LimpiarCampos();
                    }
                    else
                    {
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.ModificacionFallida"));
                    }
                }
                else
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.ManicuristaNoValido"));
                }
            }
            else
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("FormABMdisponibilidad_750VR.DisponibilidadNoSeleccionada"));
            }
        }


        private void AplicarCambioEstado()
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is BEdisponibilidad_750VR d)
            {
                bll.CambiarEstado_750VR(d.CodDisponibilidad_750VR, !d.activo_750VR);

                string clave = d.activo_750VR
                    ? "FormABMdisponibilidad_750VR.MensajeDesactivada"
                    : "FormABMdisponibilidad_750VR.MensajeActivada";

                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta(clave));

                ResetearInterfaz();
                LimpiarCampos();
            }
        }


        private void btncance_Click(object sender, EventArgs e)
        {
            modoActual = "consulta";
            //lblmensaje.Text = "Modo Consulta";
            ActualizarMensajeModo();
            ResetearInterfaz();
            LimpiarCampos();
        }
        private void ActivarModoEdicion()
        {
            if (modoActual == "añadir")
            {
                dataGridView1.Enabled = false;
                btnapli.Enabled = true;
                btncance.Enabled = true;
                btnelim.Enabled = false;
                btnmod.Enabled = false;

                txtdnimanic.Enabled = false;
                txtfin.Enabled = true;
                txtinicio.Enabled = true;
                dateTimePicker1.Enabled = true;
                cmbmanic.Enabled = true;
                //txtdire.Enabled = true;
            }

            if (modoActual == "modificar")
            {
                dataGridView1.Enabled = false;
                btnapli.Enabled = true;
                btncance.Enabled = true;
                btnelim.Enabled = false;
                btnañadir.Enabled = false;

                txtdnimanic.Enabled = false;
                txtfin.Enabled = true;
                txtinicio.Enabled = true;
                dateTimePicker1.Enabled = true;
                cmbmanic.Enabled = true;
                //txtdire.Enabled = true;

            }

            else if (modoActual == "cambiarEstado")
            {
                dataGridView1.Enabled = true;

                dateTimePicker1.Enabled = txtinicio.Enabled = txtfin.Enabled = cmbmanic.Enabled = false;
                txtdnimanic.Enabled = false;

                btnapli.Enabled = true;
                btncance.Enabled = true;
                btnañadir.Enabled = false;
                btnmod.Enabled = false;
            }


       


        }

        private void ResetearInterfaz()
        {
            btnapli.Enabled = false;
            btncance.Enabled = false;
            btnañadir.Enabled = btnmod.Enabled = btnelim.Enabled = true;
            //txtinicio.Enabled = txtfin.Enabled = cmbmanic.Enabled = false;
            //dateTimePicker1.Enabled= false;
            //txtdnimanic.Enabled= false;
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


            if (dataGridView1.SelectedRows.Count > 0)
            {
                var d = dataGridView1.SelectedRows[0].DataBoundItem as BEdisponibilidad_750VR;
                if (d != null)
                {
                    CargarManicuristaEnCombo(d.DNImanic_750VR);
                    dateTimePicker1.Value = d.Fecha_750VR;
                    txtinicio.Text = d.HoraInicio_750VR.ToString(@"hh\:mm");
                    txtfin.Text = d.HoraFin_750VR.ToString(@"hh\:mm");
                }
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

        private void rbnActivos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnActivos.Checked) CargarDisponibilidad(true);
        }

        private void rbnTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbnTodos.Checked) CargarDisponibilidad(false);
        }
    }
}
