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

        //public void ScriptDatos()
        //{
        //    EjecutarQuery(
        //        "$USE ProyectoNailsTime_VR750; INSERT INTO Usuario (DNI, Nombre, Apellido, Email, Usuario, Contra, Salt, Rol, Activo, Bloqueado) " + 
        //        "VALUES" +
        //         $"('22333444', 'Victoria', 'Russo', 'vicky.russo@gmail.com', '223334444Russo', '22333444Vicky', 'SALT1234SALT1234SALT12', Cliente, 1, 0)," +  
        //        $"('45.984.456', 'Valentin', 'Giraldes', 'DjZdD/7Aksao6E0lKeym8g==', 'lMPxDzCF7FEwOYwHAShO6Q==', 'valen@gmail.com', '2004-08-26', 2, '39', 'ES')," +  
        //        $"('55.666.777', 'Nicolas', 'Spagnolo', 'Xc+kIThHkb8o0TwkWoE2mw==', 'e+F4/sgxP3/Nm8+Bjgrcdw==', 'nico@gmail.com', '2024-11-29', 3, '12', 'EN');");  

        
            
        //}

    }
}
