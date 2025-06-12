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
        private List<Iobserver_750VR> ListaForms = new List<Iobserver_750VR>();
        private Dictionary<string, string> Diccionario = new Dictionary<string, string>(); // Inicializa con un diccionario vacío
        private string idiomaActual;
        private static Lenguaje_750VR instance;

        // Constructor privado para patrón Singleton
        private Lenguaje_750VR() { }

        // Método para obtener la instancia única de Lenguaje
        public static Lenguaje_750VR ObtenerInstancia()
        {
            if (instance == null)
            {
                instance = new Lenguaje_750VR();
            }
            return instance;
        }

        // Métodos para agregar y quitar observadores
        public void Agregar(Iobserver_750VR observer)
        {
            ListaForms.Add(observer);
        }

        public void Quitar(Iobserver_750VR observer)
        {
            ListaForms.Remove(observer);
        }

        // Notificación a los observadores para actualizar el idioma
        public void Notificar()
        {
            foreach (Iobserver_750VR observer in ListaForms)
            {
                observer.ActualizarIdioma();
            }
        }

        // Propiedad para establecer y obtener el idioma actual
        public string IdiomaActual
        {
            get
            {
                return idiomaActual;
            }
            set
            {
                idiomaActual = value;
                CargarIdioma();
                Notificar();
            }
        }

        // Método para cargar el archivo de idioma
        public void CargarIdioma()
        {
            try
            {
                // Determina el archivo JSON de acuerdo al idioma
                string idioma = idiomaActual == "ES" ? "Español" : "Inglés";
                var NombreArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Idiomas", $"{idioma}.json");


                // Verifica si el archivo existe
                if (!File.Exists(NombreArchivo))
                {
                    Console.WriteLine($"El archivo de idioma '{NombreArchivo}' no existe.");
                    Diccionario = new Dictionary<string, string>(); // Evita errores posteriores
                    return;
                }

                // Lee el contenido del archivo JSON y deserializa
                var jsonString = File.ReadAllText(NombreArchivo);
                Diccionario = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString)
              ?? new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el archivo de idioma: {ex.Message}");
                Diccionario = new Dictionary<string, string>(); // Inicializa un diccionario vacío para evitar errores
            }
        }

        // Método para obtener el texto traducido
        public string ObtenerTexto(string key)
        {
            return Diccionario != null && Diccionario.ContainsKey(key) ? Diccionario[key] : key;
        }

        // Método para cambiar el idioma de todos los controles en un formulario
        public void CambiarIdiomaControles(Control frm)
        {
            try
            {
                frm.Text = ObtenerTexto(frm.Name + ".Text");

                foreach (Control c in frm.Controls)
                {
                    if (c is Button || c is Label)
                    {
                        c.Text = ObtenerTexto(frm.Name + "." + c.Name);
                    }

                    if (c is MenuStrip m)
                    {
                        foreach (ToolStripMenuItem item in m.Items)
                        {
                            item.Text = ObtenerTexto(frm.Name + "." + item.Name);
                            CambiarIdiomaMenuStrip(item.DropDownItems, frm);
                        }
                    }

                    if (c.Controls.Count > 0)
                    {
                        CambiarIdiomaControles(c);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar el idioma de los controles: {ex.Message}");
            }
        }

        // Método recursivo para cambiar el idioma de los elementos de un menú
        private void CambiarIdiomaMenuStrip(ToolStripItemCollection items, Control frm)
        {
            foreach (ToolStripItem item in items)
            {
                if (item is ToolStripMenuItem item1)
                {
                    item.Text = ObtenerTexto(frm.Name + "." + item.Name);
                    CambiarIdiomaMenuStrip(item1.DropDownItems, frm);
                }
            }
        }

        // Método auxiliar para obtener etiquetas de texto desde fuera de la clase
        public static string ObtenerEtiqueta(string NombreControl)
        {
            return ObtenerInstancia().ObtenerTexto(NombreControl);
        }
    }
}

