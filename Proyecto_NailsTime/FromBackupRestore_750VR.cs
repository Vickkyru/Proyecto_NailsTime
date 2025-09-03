using DAL_VR750;
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
    public partial class FromBackupRestore_750VR : Form
    {
        // ⚠️ Cambiá por el nombre real de tu base
        private string nombreBD = "ProyectoNailsTime_VR750";

        // ⚠️ Usa la misma cadena de conexión que tenés en tu DAL
        private string connectionString = BaseDeDatos_750VR.cadena;
        public FromBackupRestore_750VR()
        {
            InitializeComponent();
        }

        private void FromBackupRestore_750VR_Load(object sender, EventArgs e)
        {

        }

        private void btnSeleccionarBackUp_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string nombreArchivo = "BCK_" + DateTime.Now.ToString("yyMMdd_HHmm") + ".bak";
                    textBox1.Text = Path.Combine(fbd.SelectedPath, nombreArchivo);
                }
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de Backup (*.bak)|*.bak";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = ofd.FileName;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Seleccione una carpeta de destino para el backup.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = $@"
BACKUP DATABASE [{nombreBD}]
TO DISK = @ruta
WITH FORMAT, INIT, 
     NAME = 'Backup {nombreBD}', 
     SKIP, NOREWIND, NOUNLOAD, STATS = 10;";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ruta", textBox1.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("✅ Backup generado correctamente en:\n" + textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error al generar backup: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Seleccione un archivo .bak para restaurar.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // ⚠️ Forzar modo SINGLE_USER antes de restaurar
                    string sql = $@"
ALTER DATABASE [{nombreBD}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE [{nombreBD}]
FROM DISK = @ruta
WITH REPLACE;
ALTER DATABASE [{nombreBD}] SET MULTI_USER;";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ruta", textBox2.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("✅ Base de datos restaurada desde:\n" + textBox2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error al restaurar backup: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
