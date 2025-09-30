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

        private const string DB_NAME = "ProyectoNailsTime_VR750";

        private static string GetProjectBakDir()
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ArchivosBak");
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return dir;
        }

        private static string NewBakName() => $"BCK_{DateTime.Now:yyMMdd_HHmm}.bak";

        private static string GetSqlWritableDir()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(BaseDeDatos_750VR.cadena);
            csb.InitialCatalog = "master";

            using (SqlConnection cn = new SqlConnection(csb.ConnectionString))
            {
                cn.Open();

                string sql = @"
DECLARE @bk nvarchar(4000)=NULL, @data nvarchar(4000)=NULL;
BEGIN TRY
  EXEC master.dbo.xp_instance_regread
    N'HKEY_LOCAL_MACHINE',
    N'SOFTWARE\Microsoft\MSSQLServer\MSSQLServer',
    N'BackupDirectory', @bk OUTPUT;
END TRY BEGIN CATCH SET @bk=NULL END CATCH;

SELECT @data = CAST(SERVERPROPERTY('InstanceDefaultDataPath') AS nvarchar(4000));

IF @bk IS NOT NULL SELECT @bk;
ELSE IF @data IS NOT NULL SELECT @data;
ELSE
SELECT SUBSTRING(physical_name,1,LEN(physical_name)-CHARINDEX('\',REVERSE(physical_name))+1)
FROM sys.master_files WHERE database_id=DB_ID('master') AND type=0;";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    object o = cmd.ExecuteScalar();
                    return (o == null || o == DBNull.Value) ? null : o.ToString();
                }
            }
        }

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

        private bool VerificarAccesoDespuesDeRestore()
        {
            try
            {
                using (var cn = new SqlConnection(BaseDeDatos_750VR.cadena))
                {
                    cn.Open();

                    var sql = @"
SELECT CASE WHEN EXISTS (
    SELECT 1
    FROM sys.database_principals
    WHERE name = SUSER_SNAME()
) THEN 1 ELSE 0 END;";
                    using (var cmd = new SqlCommand(sql, cn))
                    {
                        var existe = Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                        if (!existe)
                        {
                            MessageBox.Show(
                                Lenguaje_750VR.ObtenerEtiqueta("Restore.UsuarioNoPertenece"),
                                Lenguaje_750VR.ObtenerEtiqueta("Restore.AccesoRequerido"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning
                            );
                            Application.Restart();
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (SqlException ex) when (ex.Number == 4060 || ex.Number == 18456 || ex.Number == 916)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.UsuarioSinAcceso"),
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.AccesoRequerido"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning
                );
                Application.Restart();
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.ErrorValidar") + ex.Message,
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.Titulo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                return true;
            }
        }

        private void FromBackupRestore_750VR_Load(object sender, EventArgs e)
        {
            textBox1.ReadOnly = textBox2.ReadOnly = true;
            textBox1.Text = Path.Combine(GetProjectBakDir(), NewBakName());
        }

        private void btnSeleccionarBackUp_Click(object sender, EventArgs e)
        {
            string projDir = GetProjectBakDir();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = Lenguaje_750VR.ObtenerEtiqueta("Backup.Dialogo.FiltroBak");
                sfd.InitialDirectory = projDir;
                sfd.FileName = NewBakName();

                if (sfd.ShowDialog() == DialogResult.OK)
                    textBox1.Text = sfd.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show(Lenguaje_750VR.ObtenerEtiqueta("Backup.SeleccioneDestino"));
                    return;
                }

                string projDir = GetProjectBakDir();
                Directory.CreateDirectory(projDir);

                string finalName = Path.GetFileName(textBox1.Text);
                if (string.IsNullOrWhiteSpace(finalName)) finalName = NewBakName();

                string safeDir = GetSqlWritableDir();
                Directory.CreateDirectory(safeDir);
                string safePath = Path.Combine(safeDir, finalName);

                _bllBackup.GenerarBackup(safePath, OnInfoMessage);

                string finalPath = Path.Combine(projDir, finalName);
                File.Copy(safePath, finalPath, true);
                textBox1.Text = finalPath;

                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("Backup.GeneradoOk") + "\n" + finalPath,
                    Lenguaje_750VR.ObtenerEtiqueta("Backup.Titulo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("Backup.ErrorGenerar") + ex.Message,
                    Lenguaje_750VR.ObtenerEtiqueta("Backup.Titulo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            string projDir = GetProjectBakDir();

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = Lenguaje_750VR.ObtenerEtiqueta("Backup.Dialogo.FiltroBak");
                ofd.InitialDirectory = projDir;

                if (ofd.ShowDialog() == DialogResult.OK)
                    textBox2.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text) || !File.Exists(textBox2.Text))
                {
                    MessageBox.Show(
                        Lenguaje_750VR.ObtenerEtiqueta("Restore.SeleccioneBak"),
                        Lenguaje_750VR.ObtenerEtiqueta("Restore.Titulo"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var rpta = MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.Confirmar"),
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.TituloConfirmar"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (rpta != DialogResult.Yes) return;

                string safeDir = GetSqlWritableDir();
                Directory.CreateDirectory(safeDir);
                string safePath = Path.Combine(safeDir, Path.GetFileName(textBox2.Text));
                File.Copy(textBox2.Text, safePath, true);

                _bllRestore.Restaurar(safePath, OnInfoMessage);

                if (!VerificarAccesoDespuesDeRestore()) return;

                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.Ok") + "\n" + textBox2.Text,
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.Titulo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.Error") + ex.Message,
                    Lenguaje_750VR.ObtenerEtiqueta("Restore.Titulo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e) => this.Close();

    

        private void OnInfoMessage(string msg) => AppendLog(msg);

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

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }
    }
}
