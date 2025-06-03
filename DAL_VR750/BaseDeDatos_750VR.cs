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
                        );
                    END;

                    IF NOT EXISTS (
                        SELECT * FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Cliente_VR750'
                    )
                    BEGIN
                        CREATE TABLE Cliente_VR750 (
                            DNI_VR750 INT PRIMARY KEY,
                            Nombre_VR750 NVARCHAR(50) NOT NULL,
                            Apellido_VR750 NVARCHAR(50) NOT NULL,
                            Email_VR750 NVARCHAR(100) NOT NULL,
                            Direccion_VR750 NVARCHAR(100) NOT NULL,
                            Celular_VR750 INT NOT NULL,
                            Activo_VR750 BIT DEFAULT 1
                        );
                    END;

                    IF NOT EXISTS (
                        SELECT * FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Servicio_VR750'
                    )
                    BEGIN
                        CREATE TABLE Servicio_VR750 (
                            IdServicio_VR750 INT PRIMARY KEY IDENTITY(1,1),
                            Nombre_VR750 NVARCHAR(100) NOT NULL,
                            Tecnica_VR750 NVARCHAR(100) NOT NULL,
                            DuracionMinutos_VR750 INT NOT NULL,
                            Precio_VR750 DECIMAL(10,2) NOT NULL,
                            Activo_VR750 BIT DEFAULT 1
                        );
                    END;

                    IF NOT EXISTS (
                        SELECT * FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Disponibilidad_VR750'
                    )
                    BEGIN
                        CREATE TABLE Disponibilidad_VR750 (
                            IdDisponibilidad_VR750 INT PRIMARY KEY IDENTITY(1,1),
                            DNImanic_VR750 INT NOT NULL FOREIGN KEY REFERENCES Usuario_VR750(DNI_VR750),
                            Fecha_VR750 DATE NOT NULL,
                            HoraInicio_VR750 TIME NOT NULL,
                            HoraFin_VR750 TIME NOT NULL,
                            Activo_VR750 BIT DEFAULT 1,
                            Estado_VR750 BIT DEFAULT 0
                        );
                    END;

                    IF NOT EXISTS (
                        SELECT * FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Reserva_VR750'
                    )
                    BEGIN
                        CREATE TABLE Reserva_VR750 (
                            IdReserva_VR750 INT PRIMARY KEY IDENTITY(1,1),
                            DNIcli_VR750 INT NOT NULL FOREIGN KEY REFERENCES Cliente_VR750(DNI_VR750),
                            DNImanic_VR750 INT NOT NULL FOREIGN KEY REFERENCES Usuario_VR750(DNI_VR750),
                            IdServicio_VR750 INT NOT NULL FOREIGN KEY REFERENCES Servicio_VR750(IdServicio_VR750),
                            Fecha_VR750 DATE NOT NULL,
                            HoraInicio_VR750 TIME NOT NULL,
                            HoraFin_VR750 TIME NOT NULL,
                            Precio_VR750 DECIMAL(10,2) NOT NULL,
                            Estado_VR750 varchar(50) default 'Pendiente',
                            Cobrado_VR750 BIT DEFAULT 0

                        );
                    END;
                ";

                using (SqlCommand cmd = new SqlCommand(verificarTabla, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void InsertarServiciosIniciales()
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                try
                {
                    conn.Open();
                    string script = @"
            IF NOT EXISTS (SELECT 1 FROM Servicio_VR750)
            BEGIN
                INSERT INTO Servicio_VR750 (Nombre_VR750, Tecnica_VR750, DuracionMinutos_VR750, Precio_VR750, Activo_VR750) VALUES
                ('Masajes', 'Relajantes', 60, 8000.00, 1),
                ('Masajes', 'Descontracturantes', 60, 9000.00, 1),
                ('Pedicura', 'Esmaltado tradicional', 20, 4000.00, 1),
                ('Pedicura', 'Esmaltado semi', 40, 5000.00, 1),
                ('Pedicura', 'Esmaltado tradicional + Spa', 60, 6000.00, 1),
                ('Manicura', 'Esmaltado tradicional', 30, 4000.00, 1),
                ('Manicura', 'Semipermanente', 45, 8000.00, 1),
                ('Manicura', 'Kapping con gel', 60, 10000.00, 1),
                ('Manicura', 'Uñas esculpidas acrílicas', 90, 12000.00, 1),
                ('Limpieza facial', 'Profunda', 60, 7000.00, 1),
                ('Limpieza facial', 'Express', 30, 4000.00, 1),
                ('Limpieza facial', 'Punta de diamante', 45, 6500.00, 1),
                ('Limpieza facial', 'Peeling químico', 60, 7500.00, 1)
            END

            IF NOT EXISTS (SELECT 1 FROM Usuario_VR750)
            BEGIN
                INSERT INTO Usuario_VR750 (DNI_VR750, Nombre_VR750, Apellido_VR750, Email_VR750, Usuario_VR750, Contra_VR750, Salt_VR750, Rol_VR750, Activo_VR750, Bloqueado_VR750) VALUES
               (10000000, 'Ana', 'López', 'ana@demo.com', 'analopez', 'ana123', '', 'Manicurista', 1, 0),
(11000000, 'pepita', 'juanes', 'pepi@demo.com', 'pepitajuanes', 'pepi123', '', 'Manicurista', 1, 0),
(11100000, 'joaca', 'perez', 'joa@demo.com', 'joacaperez', 'joa123', '', 'Manicurista', 1, 0),

(10000002, 'Tomás', 'García', 'tomas@demo.com', 'tomasgarcia', 'tomas123', '', 'Recepcionista', 1, 0),
(10000003, 'Carla', 'Gómez', 'carla@demo.com', 'carlagomez', 'carla123', '', 'Administrador', 1, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM Disponibilidad_VR750)
            BEGIN
                INSERT INTO Disponibilidad_VR750 (DNImanic_VR750, Fecha_VR750, HoraInicio_VR750, HoraFin_VR750, Activo_VR750, Estado_VR750) VALUES
                (10000000, '2025-07-14', '09:00', '13:00', 1, 0),
                (10000000, '2025-07-16', '14:00', '18:00', 1, 0),
                (11000000, '2025-07-15', '10:00', '14:00', 1, 0),
                (11000000, '2025-07-17', '15:00', '19:00', 1, 0),
                (11100000, '2025-07-18', '09:30', '12:30', 1, 0),
                (11100000, '2025-07-19', '11:00', '15:00', 1, 0)
            END

            IF NOT EXISTS (SELECT 1 FROM Cliente_VR750 WHERE DNI_VR750 = 33111222)
            BEGIN
                INSERT INTO Cliente_VR750 (DNI_VR750, Nombre_VR750, Apellido_VR750, Email_VR750, Direccion_VR750, Celular_VR750, Activo_VR750)
                VALUES (33111222, 'martina', 'ruiz', 'martina@gmail.com', 'Cordoba1234', 1134567890, 1)
            END
            ";

                    using (SqlCommand cmd = new SqlCommand(script, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar servicios iniciales: " + ex.Message);
                }
            }
        }



    }
}
