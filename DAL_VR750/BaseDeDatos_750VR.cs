using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class BaseDeDatos_750VR
    {
        public static string dataSource = "HP_Victoria\\SQLEXPRESS";
        public static string dbName = "ProyectoNailsTime_VR750";
        public static string cadena = $"Data source={dataSource};Initial Catalog={dbName};Integrated Security=True;";
        public SqlConnection Connection = new SqlConnection(cadena); //conexion a bd
        public SqlCommand Command = new SqlCommand(); //ejecutar consultas SQL
        public bool Conectar_750VR()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
                return true;
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

        public void VerificarYCrearTablaUsuarios_750VR()
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                // Verifica si la tabla ya existe
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
