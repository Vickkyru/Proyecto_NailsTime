using BLL_VR750;
using DAL_VR750;
using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_NailsTime
{
    public partial class FormBackupRestore_750VR : Form, Iobserver_750VR
    {
      
        private readonly string _conn = BaseDeDatos_750VR.cadena;

        private BLLbackUp_750VR _bllBackup;
        private BLLrestore_750VR _bllRestore;

        public FormBackupRestore_750VR()
        {
            InitializeComponent();

            Lenguaje_750VR.ObtenerInstancia().Agregar(this);
            ActualizarIdioma();
            _bllBackup = new BLLbackUp_750VR(_conn);
            _bllRestore = new BLLrestore_750VR(_conn);
        }
        public void ActualizarIdioma()
        {
            Lenguaje_750VR.ObtenerInstancia().CambiarIdiomaControles(this);
        }

        private void FromBackupRestore_750VR_Load(object sender, EventArgs e)
        {
            // opcional: limpiar rutas
            //textBox1.Clear();
            //textBox2.Clear();
            try
            {
               
                string carpetaBackup = @"C:\Users\mavru\OneDrive\Escritorio\hoy\Proyecto_NailsTime\Proyecto_NailsTime\bin\Debug\registrosBackUp";

               
                if (!Directory.Exists(carpetaBackup))
                {
                    Directory.CreateDirectory(carpetaBackup);
                }

                string nombreArchivo = "BCK_" + DateTime.Now.ToString("yyMMdd_HHmm") + ".bak";

           
                textBox1.Text = Path.Combine(carpetaBackup, nombreArchivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error preparando carpeta de backups:\n" + ex.Message,
                    "Respaldo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
        private void btnSeleccionarBackUp_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string nombreArchivo = "BCK_" + DateTime.Now.ToString("yyMMdd_HHmm") + ".bak";
                    textBox1.Text = Path.Combine(fbd.SelectedPath, nombreArchivo);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) 
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Seleccione carpeta de destino para el backup.", "Respaldo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var dir = Path.GetDirectoryName(textBox1.Text);
                if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
                {
                    MessageBox.Show("La carpeta destino no existe.", "Respaldo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ToggleUi(false);
                AppendLog("Iniciando backup...");
                _bllBackup.GenerarBackup(textBox1.Text, OnInfoMessage);
                AppendLog("Backup finalizado.");
                MessageBox.Show("✅ Backup generado correctamente en:\n" + textBox1.Text,
                    "Respaldo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog("ERROR backup: " + ex.Message);
                MessageBox.Show("Error al generar backup:\n" + ex.Message,
                    "Respaldo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleUi(true);
            }
        }

       
        private void iconButton1_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de Backup (*.bak)|*.bak";
                ofd.Title = "Seleccionar archivo .bak";
                if (ofd.ShowDialog() == DialogResult.OK)
                    textBox2.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e) 
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Seleccione un archivo .bak para restaurar.", "Restore",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!File.Exists(textBox2.Text))
                {
                    MessageBox.Show("El archivo .bak no existe.", "Restore",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var rpta = MessageBox.Show(
                    "Se restaurará la base de datos y se reemplazará el estado actual.\n" +
                    "Asegúrese de cerrar otras aplicaciones que usen la BD.\n\n¿Continuar?",
                    "Confirmar Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (rpta != DialogResult.Yes) return;

                ToggleUi(false);
                AppendLog("Iniciando restore...");
               
                _bllRestore.Restaurar(textBox2.Text, OnInfoMessage);
                AppendLog("Restore finalizado.");
                MessageBox.Show("Base de datos restaurada desde:\n" + textBox2.Text,
                    "Restore", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog("ERROR restore: " + ex.Message);
                MessageBox.Show("Error al restaurar:\n" + ex.Message,
                    "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ToggleUi(true);
            }
        }

    
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void ToggleUi(bool enabled)
        {
            try
            {
                btnSeleccionarBackUp.Enabled = enabled;
                button1.Enabled = enabled;        // Realizar backup
                iconButton1.Enabled = enabled;    // buscar .bak
                button2.Enabled = enabled;        // Realizar restore
                button3.Enabled = enabled;        // Volver
            }
            catch { /* por si cambian nombres no romper */ }
        }

        private void OnInfoMessage(string msg)
        {
            
            AppendLog(msg);
        }

        private void AppendLog(string line)
        {
            
            Control[] found = this.Controls.Find("txtLog", true);
            if (found != null && found.Length > 0 && found[0] is TextBox)
            {
                var t = (TextBox)found[0];
                if (!string.IsNullOrEmpty(t.Text)) t.AppendText(Environment.NewLine);
                t.AppendText($"[{DateTime.Now:HH:mm:ss}] {line}");
            }
            else
            {
                this.Text = "Respaldo/Restore - " + line;
            }
        }
    }
}
