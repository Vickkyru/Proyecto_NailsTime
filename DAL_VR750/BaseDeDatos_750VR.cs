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
        public static string dbName = "Proyecto_NailsTime_750VR";
        public static string cadena = $"Data source={dataSource};Initial Catalog=master;Integrated Security=True;";
        public SqlConnection Connection = new SqlConnection(cadena); //conexion a bd
        public SqlCommand Command = new SqlCommand(); //ejecutar consultas SQL
        public bool Conectar()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
                return true;
            }
            return false;
        }
        public bool Desconectar()
        {
            if (Connection.State == ConnectionState.Open)
            {
                Connection.Close();
                return true;
            }
            return false;
        }

    }
}
