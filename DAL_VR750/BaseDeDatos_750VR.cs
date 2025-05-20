using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAL_VR750
{
    public class BaseDeDatos_750VR
    {
        public static string dataSource = "HP_Victoria\\SQLEXPRESS";
        public static string dbName = "ProyectoNailsTime_VR750";
        public static string cadena = $"Data source={dataSource};Initial Catalog={dbName};Integrated Security=True;";
        public SqlConnection Connection = new SqlConnection(cadena); //conexion a bd
        public SqlCommand Command = new SqlCommand(); //ejecutar consultas SQL

        public BaseDeDatos_750VR()
        {
            
        }
        public bool Conectar_750VR()
        {
            if (Connection.State == ConnectionState.Closed)
            {
               

                try
                {
                    Connection.Open();
                    return true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
                }
            }
            return false;
        }
        public bool Desconectar_750VR()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                return true;
            }
            return false;
        }


        public void VerificarOCrearBaseDeDatos()
        {
            string conexionMaster = $"Data Source={dataSource};Initial Catalog=master;Integrated Security=True";
            string scriptCrearBD = $@"
                IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = '{dbName}')
                BEGIN
                    CREATE DATABASE [{dbName}];
                END
            ";

            using (SqlConnection conn = new SqlConnection(conexionMaster))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(scriptCrearBD, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al crear la base de datos: " + ex.Message);
                }
            }
        }

        public void VerificarYCrearTablaUsuarios_750VR()
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                conn.Open();

                string verificarTabla = @"
                    IF NOT EXISTS (
                        SELECT * FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Usuario_VR750'
                    )
                    BEGIN
                        CREATE TABLE Usuario_VR750 (
                            DNI_VR750 INT PRIMARY KEY,
                            Nombre_VR750 VARCHAR(100) NOT NULL,
                            Apellido_VR750 VARCHAR(100) NOT NULL,
                            Email_VR750 VARCHAR(150) NOT NULL,
                            Usuario_VR750 VARCHAR(150) NOT NULL UNIQUE,
                            Contra_VR750 VARCHAR(256) NOT NULL,
                            Salt_VR750 VARCHAR(50) NOT NULL,
                            Rol_VR750 VARCHAR(50) NOT NULL,
                            Activo_VR750 BIT NOT NULL DEFAULT 1,
                            Bloqueado_VR750 BIT NOT NULL DEFAULT 0
                        )
                    END
                ";

                using (SqlCommand cmd = new SqlCommand(verificarTabla, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }







    }
}
