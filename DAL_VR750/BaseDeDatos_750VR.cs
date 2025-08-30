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
        public static string cadena = $"Data source={dataSource};Initial Catalog={dbName};Integrated Security=True;MultipleActiveResultSets=true";
        public SqlConnection Connection = new SqlConnection(cadena); 
        public SqlCommand Command = new SqlCommand(); 

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

-- Tabla de perfiles (roles definidos por el sistema)
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME = 'Perfil_VR750'
)
BEGIN
    CREATE TABLE Perfil_VR750 (
        CodPerfil_VR750 INT PRIMARY KEY IDENTITY(1,1),
        NombrePerfil_VR750 NVARCHAR(100) NOT NULL
    );
END;
   -- Modificación de la tabla Usuario para agregar referencia al perfil
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
        Bloqueado_VR750 BIT NOT NULL DEFAULT 0,
Idioma_VR750 VARCHAR(50) NOT NULL


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
            Direccion_VR750 NVARCHAR(200) NOT NULL,
            Celular_VR750 NVARCHAR(100) NOT NULL,
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
            Estado_VR750 VARCHAR(50) DEFAULT 'Pendiente',
            Cobrado_VR750 BIT DEFAULT 0
        );
    END;

    IF NOT EXISTS (
        SELECT * FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_NAME = 'Factura_VR750'
    )
    BEGIN
        CREATE TABLE Factura_VR750 (
            CodFactura_VR750 INT PRIMARY KEY IDENTITY(1,1),
            CodReserva_VR750 INT NOT NULL,
            fecha_VR750 DATE NOT NULL,
            horaEmision_VR750 TIME NOT NULL,
            metodopago_VR750 VARCHAR(20) NOT NULL,
            total_VR750 DECIMAL(10, 2) NOT NULL,
            titular_VR750 NVARCHAR(100) NOT NULL,
            FOREIGN KEY (CodReserva_VR750) REFERENCES Reserva_VR750(IdReserva_VR750)
        );
    END;

    IF NOT EXISTS (
        SELECT * FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_NAME = 'Insumo_VR750'
    )
    BEGIN
        CREATE TABLE Insumo_VR750 (
            CodInsumo_VR750 INT PRIMARY KEY IDENTITY(1,1),
            Nombre_VR750 NVARCHAR(100) NOT NULL,
            Descripcion_VR750 NVARCHAR(255),
            CantidadActual_VR750 INT NOT NULL,
            StockMinimo_VR750 INT NOT NULL,
            UnidadMedida_VR750 NVARCHAR(50) NOT NULL,
            Activo_VR750 BIT NOT NULL DEFAULT 1
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
        WHERE TABLE_NAME = 'ReservaInsumo_VR750'
    )
    BEGIN
        CREATE TABLE ReservaInsumo_VR750 (
            IdReserva_VR750 INT NOT NULL FOREIGN KEY REFERENCES Reserva_VR750(IdReserva_VR750),
            CodInsumo_VR750 INT NOT NULL FOREIGN KEY REFERENCES Insumo_VR750(CodInsumo_VR750),
            CantidadUsada_VR750 INT NOT NULL,
            PRIMARY KEY (IdReserva_VR750, CodInsumo_VR750)
        );
    END;



-- Tabla de Permisos
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME = 'Permiso_VR750'
)
BEGIN
    CREATE TABLE Permiso_VR750 (
        CodPermiso_VR750 INT PRIMARY KEY IDENTITY(1,1),
        NombrePermiso_VR750 NVARCHAR(100) NOT NULL
    );
END;

-- Tabla de Familias
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME = 'Familia_VR750'
)
BEGIN
    CREATE TABLE Familia_VR750 (
        CodFamilia_VR750 INT PRIMARY KEY IDENTITY(1,1),
        NombreFamilia_VR750 NVARCHAR(100) NOT NULL
    );
END;

-- Tabla de Perfiles
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME = 'Perfil_VR750'
)
BEGIN
    CREATE TABLE Perfil_VR750 (
        CodPerfil_VR750 INT PRIMARY KEY IDENTITY(1,1),
        NombrePerfil_VR750 NVARCHAR(100) NOT NULL
    );
END;

-- Asociación: Perfil - Permiso
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME = 'PerfilXPermiso_VR750'
)
BEGIN
    CREATE TABLE PerfilXPermiso_VR750 (
        CodPerfil_VR750 INT NOT NULL,
        CodPermiso_VR750 INT NOT NULL,
        PRIMARY KEY (CodPerfil_VR750, CodPermiso_VR750),
        FOREIGN KEY (CodPerfil_VR750) REFERENCES Perfil_VR750(CodPerfil_VR750),
        FOREIGN KEY (CodPermiso_VR750) REFERENCES Permiso_VR750(CodPermiso_VR750)
    );
END;

-- Asociación: Perfil - Familia
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME = 'PerfilXFamilia_VR750'
)
BEGIN
    CREATE TABLE PerfilXFamilia_VR750 (
        CodPerfil_VR750 INT NOT NULL,
        CodFamilia_VR750 INT NOT NULL,
        PRIMARY KEY (CodPerfil_VR750, CodFamilia_VR750),
        FOREIGN KEY (CodPerfil_VR750) REFERENCES Perfil_VR750(CodPerfil_VR750),
        FOREIGN KEY (CodFamilia_VR750) REFERENCES Familia_VR750(CodFamilia_VR750)
    );
END;

-- Asociación: Permiso - Familia
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME = 'PermisoXFamilia_VR750'
)
BEGIN
    CREATE TABLE PermisoXFamilia_VR750 (
        CodFamilia_VR750 INT NOT NULL,
        CodPermiso_VR750 INT NOT NULL,
        PRIMARY KEY (CodFamilia_VR750, CodPermiso_VR750),
        FOREIGN KEY (CodFamilia_VR750) REFERENCES Familia_VR750(CodFamilia_VR750),
        FOREIGN KEY (CodPermiso_VR750) REFERENCES Permiso_VR750(CodPermiso_VR750)
    );
END;

-- Asociación: Familia - Familia (jerarquía recursiva)
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_NAME = 'FamiliaXFamilia_VR750'
)
BEGIN
    CREATE TABLE FamiliaXFamilia_VR750 (
        CodFamiliaPadre_VR750 INT NOT NULL,
        CodFamiliaHija_VR750 INT NOT NULL,
        PRIMARY KEY (CodFamiliaPadre_VR750, CodFamiliaHija_VR750),
        FOREIGN KEY (CodFamiliaPadre_VR750) REFERENCES Familia_VR750(CodFamilia_VR750),
        FOREIGN KEY (CodFamiliaHija_VR750) REFERENCES Familia_VR750(CodFamilia_VR750)
    );
END;

IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'EVENTOS_VR750'
)
BEGIN
    CREATE TABLE EVENTOS_VR750 (
        Id_Evento     INT IDENTITY(1,1) PRIMARY KEY,     -- PK
        Login         VARCHAR(150) NOT NULL,              -- FK -> Usuario_VR750(Usuario_VR750)
        Fecha         DATE        NOT NULL DEFAULT CAST(GETDATE() AS DATE),
        Hora          TIME(0)     NOT NULL DEFAULT CAST(GETDATE() AS TIME(0)),
        Modulo        NVARCHAR(120) NOT NULL,             -- tipificado por combo en la GUI
        Evento        NVARCHAR(120) NOT NULL,             -- tipificado por combo en la GUI
        Criticidad    TINYINT     NOT NULL,               -- 1 (más importante) .. 5 (menos)
        CONSTRAINT FK_EVENTOS_USUARIO_LOGIN
            FOREIGN KEY (Login) REFERENCES Usuario_VR750(Usuario_VR750)
    );

    -- Validación de rango de criticidad
    ALTER TABLE EVENTOS_VR750
      ADD CONSTRAINT CK_EVENTOS_Criticidad_Rango CHECK (Criticidad BETWEEN 1 AND 5);
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
-- 1. Insertar perfiles si no existen
IF NOT EXISTS (SELECT 1 FROM Perfil_VR750 WHERE NombrePerfil_VR750 = 'Administrador')
    INSERT INTO Perfil_VR750 (NombrePerfil_VR750) VALUES ('Administrador');

IF NOT EXISTS (SELECT 1 FROM Perfil_VR750 WHERE NombrePerfil_VR750 = 'Recepcionista')
    INSERT INTO Perfil_VR750 (NombrePerfil_VR750) VALUES ('Recepcionista');

IF NOT EXISTS (SELECT 1 FROM Perfil_VR750 WHERE NombrePerfil_VR750 = 'Manicurista')
    INSERT INTO Perfil_VR750 (NombrePerfil_VR750) VALUES ('Manicurista');

-- 2. Insertar permisos simples si no existen
DECLARE @permisos TABLE (Nombre NVARCHAR(100));
INSERT INTO @permisos (Nombre) VALUES
('pestañaAdmin'), ('pestañaMaestros'), ('pestañaUsuarios'), ('pestañaReserva'), ('pestañaInsumos'),
('pestañaReportes'), ('pestañaAyuda'), ('gestionUsuarios'), ('gestionPerfiles'), ('inicioSesion'),
('cambiarClave'), ('cerrarSesion'), ('cambiarIdioma'), ('registrarReserva'), ('actualizarAgenda'),
('Facturas'), ('ABMclientes'), ('ABMhorarios'), ('ABMservicios'), ('ABMinsumos');

INSERT INTO Permiso_VR750 (NombrePermiso_VR750)
SELECT Nombre FROM @permisos
WHERE NOT EXISTS (
    SELECT 1 FROM Permiso_VR750 WHERE NombrePermiso_VR750 = Nombre
);

-- 3. Insertar familias si no existen
IF NOT EXISTS (SELECT 1 FROM Familia_VR750 WHERE NombreFamilia_VR750 = 'administrador')
    INSERT INTO Familia_VR750 (NombreFamilia_VR750) VALUES ('administrador');

IF NOT EXISTS (SELECT 1 FROM Familia_VR750 WHERE NombreFamilia_VR750 = 'usuario')
    INSERT INTO Familia_VR750 (NombreFamilia_VR750) VALUES ('usuario');

IF NOT EXISTS (SELECT 1 FROM Familia_VR750 WHERE NombreFamilia_VR750 = 'maestros')
    INSERT INTO Familia_VR750 (NombreFamilia_VR750) VALUES ('maestros');

IF NOT EXISTS (SELECT 1 FROM Familia_VR750 WHERE NombreFamilia_VR750 = 'recepcion')
    INSERT INTO Familia_VR750 (NombreFamilia_VR750) VALUES ('recepcion');

-- 4. Asignar permisos a la familia administrador
INSERT INTO PermisoXFamilia_VR750 (CodFamilia_VR750, CodPermiso_VR750)
SELECT f.CodFamilia_VR750, p.CodPermiso_VR750
FROM Familia_VR750 f
JOIN Permiso_VR750 p ON p.NombrePermiso_VR750 IN (
    'pestañaAdmin', 'pestañaMaestros', 'pestañaUsuarios', 'pestañaReserva', 'pestañaInsumos',
    'pestañaReportes', 'pestañaAyuda', 'gestionUsuarios', 'gestionPerfiles', 'inicioSesion',
    'cambiarClave', 'cerrarSesion', 'cambiarIdioma', 'registrarReserva', 'actualizarAgenda',
    'Facturas', 'ABMclientes', 'ABMhorarios', 'ABMservicios', 'ABMinsumos'
)
WHERE f.NombreFamilia_VR750 = 'administrador'
AND NOT EXISTS (
    SELECT 1 FROM PermisoXFamilia_VR750 pf
    WHERE pf.CodFamilia_VR750 = f.CodFamilia_VR750 AND pf.CodPermiso_VR750 = p.CodPermiso_VR750
);

-- 4b. Asignar permisos a la familia maestros
INSERT INTO PermisoXFamilia_VR750 (CodFamilia_VR750, CodPermiso_VR750)
SELECT f.CodFamilia_VR750, p.CodPermiso_VR750
FROM Familia_VR750 f
JOIN Permiso_VR750 p ON p.NombrePermiso_VR750 IN ('ABMclientes', 'ABMhorarios', 'ABMservicios', 'ABMinsumos')
WHERE f.NombreFamilia_VR750 = 'maestros'
AND NOT EXISTS (
    SELECT 1 FROM PermisoXFamilia_VR750 pf
    WHERE pf.CodFamilia_VR750 = f.CodFamilia_VR750 AND pf.CodPermiso_VR750 = p.CodPermiso_VR750
);

-- 5. Limpiar asignaciones previas de perfiles (excepto Administrador)
DELETE FROM PerfilXPermiso_VR750
WHERE CodPerfil_VR750 IN (
    SELECT CodPerfil_VR750 FROM Perfil_VR750
    WHERE NombrePerfil_VR750 <> 'Administrador'
);

DELETE FROM PerfilXFamilia_VR750
WHERE CodPerfil_VR750 IN (
    SELECT CodPerfil_VR750 FROM Perfil_VR750
    WHERE NombrePerfil_VR750 <> 'Administrador'
);

-- 6. Asignar solo la familia al perfil Administrador
INSERT INTO PerfilXFamilia_VR750 (CodPerfil_VR750, CodFamilia_VR750)
SELECT p.CodPerfil_VR750, f.CodFamilia_VR750
FROM Perfil_VR750 p, Familia_VR750 f
WHERE p.NombrePerfil_VR750 = 'Administrador'
  AND f.NombreFamilia_VR750 = 'administrador'
  AND NOT EXISTS (
      SELECT 1 FROM PerfilXFamilia_VR750 pf
      WHERE pf.CodPerfil_VR750 = p.CodPerfil_VR750 AND pf.CodFamilia_VR750 = f.CodFamilia_VR750
);

-- 7. Insertar servicios si no existen
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

-- 8. Insertar insumos si no existen
IF NOT EXISTS (SELECT 1 FROM Insumo_VR750)
BEGIN
    INSERT INTO Insumo_VR750 (Nombre_VR750, Descripcion_VR750, CantidadActual_VR750, StockMinimo_VR750, UnidadMedida_VR750, Activo_VR750) VALUES
    ('Esmalte rojo clásico', 'Esmalte rojo clásico', 50, 10, 'unidad', 1),
    ('Quitaesmalte','Removedor universal de esmalte', 100, 20, 'ml', 1),
    ('Algodón', 'Bolsita de algodón', 200, 50, 'unidad', 1),
    ('Lima desechable', 'Lima de uso único', 150, 30, 'unidad', 1),
    ('Base fortalecedora', 'Base antes del esmaltado', 60, 10, 'unidad', 1),
    ('Esmalte semi permanente rosa', 'Para técnica semipermanente', 40, 8, 'unidad', 1),
    ('Gel constructor', 'Gel para uñas esculpidas', 30, 5, 'ml', 1),
    ('Mascarilla facial hidratante', 'Aplicación post limpieza facial', 20, 5, 'unidad', 1),
    ('Crema exfoliante', 'Para limpieza facial profunda', 25, 5, 'ml', 1)
END

-- 9. Insertar usuarios si no existen
IF NOT EXISTS (SELECT 1 FROM Usuario_VR750)
BEGIN
    INSERT INTO Usuario_VR750 (DNI_VR750, Nombre_VR750, Apellido_VR750, Email_VR750, Usuario_VR750, Contra_VR750, Salt_VR750, Rol_VR750, Activo_VR750, Bloqueado_VR750, Idioma_VR750) VALUES
    (10000000, 'Ana', 'López', 'ana@demo.com', 'analopez', 'ana123', '', 'Manicurista', 1, 0, 'Español'),
    (11000000, 'pepita', 'juanes', 'pepi@demo.com', 'pepitajuanes', 'pepi123', '', 'Manicurista', 1, 0,'Español'),
    (11100000, 'joaca', 'perez', 'joa@demo.com', 'joacaperez', 'joa123', '', 'Manicurista', 1, 0,'Español'),
    (10000002, 'Tomás', 'García', 'tomas@demo.com', 'tomasgarcia', 'tomas123', '', 'Recepcionista', 1, 0,'Español'),
    (10000003, 'Carla', 'Gómez', 'carla@demo.com', 'carlagomez', 'carla123', '', 'Administrador', 1, 0,'Español')
END

-- 10. Insertar disponibilidad si no existen
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


";

                    using (SqlCommand cmd = new SqlCommand(script, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar perfiles y permisos: " + ex.Message);
                }
            }
        }


    }
}
