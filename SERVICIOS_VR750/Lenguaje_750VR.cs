using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;


namespace SERVICIOS_VR750
{
    public class Lenguaje_750VR : Isubject_750VR
    {
        private static Lenguaje_750VR instance;
        private List<Iobserver_750VR> ListaForms = new List<Iobserver_750VR>();
        private Dictionary<string, string> Diccionario = new Dictionary<string, string>();
        private string idiomaActual;

        private Lenguaje_750VR() { }

        public static Lenguaje_750VR ObtenerInstancia()
        {
            if (instance == null)
                instance = new Lenguaje_750VR();
            return instance;
        }

        public void Agregar(Iobserver_750VR obs)
        {
            if (!ListaForms.Contains(obs))
                ListaForms.Add(obs);
        }

        public void Quitar(Iobserver_750VR obs)
        {
            if (ListaForms.Contains(obs))
                ListaForms.Remove(obs);
        }

        public void Notificar()
        {
            foreach (Iobserver_750VR obs in ListaForms)
            {
                obs.ActualizarIdioma();
            }
        }

        public string IdiomaActual
        {
            get { return idiomaActual; }
            set
            {
                idiomaActual = value;
                CargarIdioma();
                Notificar();
            }
        }

        public void CargarIdioma()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Idiomas", idiomaActual + ".json");

                if (!File.Exists(path))
                {
                    MessageBox.Show("No se encontró el archivo: " + path, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Diccionario = new Dictionary<string, string>();
                    return;
                }

                string json = File.ReadAllText(path);
                Diccionario = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                if (Diccionario == null)
                    Diccionario = new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar idioma: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Diccionario = new Dictionary<string, string>();
            }
        }

        public string ObtenerTexto(string clave)
        {
            return Diccionario.ContainsKey(clave) ? Diccionario[clave] : clave;
        }

        public static string ObtenerEtiqueta(string clave)
        {
            return ObtenerInstancia().ObtenerTexto(clave);
        }

        public void CambiarIdiomaControles(Control frm)
        {
            try
            {
                frm.Text = ObtenerTexto(frm.Name + ".Text");

                foreach (Control c in frm.Controls)
                {
                    if (c is Label || c is Button || c is CheckBox || c is GroupBox)
                        c.Text = ObtenerTexto(frm.Name + "." + c.Name);

                    if (c is MenuStrip)
                    {
                        MenuStrip m = (MenuStrip)c;
                        foreach (ToolStripMenuItem item in m.Items)
                        {
                            item.Text = ObtenerTexto(frm.Name + "." + item.Name);
                            CambiarIdiomaMenuStrip(item.DropDownItems, frm);
                        }
                    }

                    if (c.HasChildren)
                        CambiarIdiomaControles(c);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al traducir controles: " + ex.Message, "Idioma", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       
        private void CambiarIdiomaMenuStrip(ToolStripItemCollection items, Control frm)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripMenuItem)
                {
                    ToolStripMenuItem subItem = (ToolStripMenuItem)item;
                    subItem.Text = ObtenerTexto(frm.Name + "." + subItem.Name);
                    CambiarIdiomaMenuStrip(subItem.DropDownItems, frm);
                }
            }
        }
    }
}

