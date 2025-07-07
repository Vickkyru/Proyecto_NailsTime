using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using System.Windows.Forms;
using BE_VR750;
using BLL_VR750;
using SERVICIOS_VR750;


namespace Proyecto_NailsTime
{
    public partial class FormLogIn : Form, Iobserver_750VR
    {
        public BLLusuario_750VR usuarioBLL = new BLLusuario_750VR();
        BLLusuario_750VR bll = new BLLusuario_750VR();
        private Dictionary<string, int> intentosFallidosPorUsuario = new Dictionary<string, int>();

        private FormPrincipal formPrincipal;
        

        public FormLogIn(FormPrincipal principal)
        {
            InitializeComponent();
            formPrincipal = principal;
            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            //Lenguaje_750VR.ObtenerInstancia().IdiomaActual = "Español";
            ActualizarIdioma();
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string login = txtuser.Text.Trim();
            string password = txtcontra.Text.Trim();

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("Login.Mensaje.CamposVacios"));
                return;
            }

            try
            {
                BEusuario_750VR usuario = bll.recuperarUsuario_750VR(login, password);


                bool sesionOK = SessionManager_750VR.ObtenerInstancia.IniciarSesion_750VR(usuario);
                if (!sesionOK)
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("Login.Mensaje.SesionActiva"));
                    return;
                }

                // 🔽 Cargar permisos si la sesión fue exitosa
                var bllPerfil = new BLLperfil_750VR();
                var permisos = bllPerfil.ObtenerPermisosDePerfilPorNombre(usuario.rol_750VR);


                List<string> nombresPermisos = new List<string>();
                foreach (var permiso in permisos)
                    nombresPermisos.AddRange(ObtenerNombresPermisos(permiso));

                // 🔽 Guardar permisos en la sesión
                SessionManager_750VR.ObtenerInstancia.EstablecerPermisos(nombresPermisos);

                // 🔽 Resto de tu código original
                string idioma = string.IsNullOrEmpty(usuario.idioma_750VR) ? "Español" : usuario.idioma_750VR;
                Lenguaje_750VR.ObtenerInstancia().IdiomaActual = idioma;
                usuario.idioma_750VR = idioma;

                formPrincipal.MostrarDatosUsuarioLogueado();
                //formPrincipal.Actualizar();
                formPrincipal.AplicarPermisos();

                intentosFallidosPorUsuario.Remove(login);

                this.Close();

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;

                if (mensaje.Contains("Contraseña incorrecta"))
                {
                    if (!intentosFallidosPorUsuario.ContainsKey(login))
                        intentosFallidosPorUsuario[login] = 0;

                    intentosFallidosPorUsuario[login]++;

                    if (intentosFallidosPorUsuario[login] >= 3)
                    {
                        bll.BloquearUsuario_750VR(login);
                        MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("Login.Mensaje.Bloqueado"));
                    }
                    else
                    {
                        string texto = string.Format(
                     Lenguaje_750VR.ObtenerEtiqueta("Login.Mensaje.IntentoFallido"),
                     intentosFallidosPorUsuario[login]);

                        MessageBox.Show(texto);
                    }
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }

        }
        private List<string> ObtenerNombresPermisos(IComponentePermiso_750VR permiso)
        {
            List<string> lista = new List<string>();
            lista.Add(permiso.Nombre_750VR);

            foreach (var hijo in permiso.ObtenerHijos())
                lista.AddRange(ObtenerNombresPermisos(hijo));

            return lista;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormLogIn_750VR_Load(object sender, EventArgs e)
        {
            
        }

        private void txtcontra_TextChanged(object sender, EventArgs e)
        {
            txtcontra.UseSystemPasswordChar = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtcontra.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}
