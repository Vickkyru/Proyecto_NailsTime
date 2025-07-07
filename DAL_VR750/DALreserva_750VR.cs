using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_VR750;

namespace DAL_VR750
{
    public class DALreserva_750VR
    {
        public List<BEReserva_750VR> ObtenerReservasPorManicurista(int dniManicurista)
        {
            List<BEReserva_750VR> lista = new List<BEReserva_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
        SELECT r.*, 
               c.Nombre_VR750 AS NombreCliente, 
               c.Apellido_VR750 AS ApellidoCliente,
               c.Email_VR750 AS EmailCliente,
               c.Direccion_VR750 AS DireccionCliente,
               c.Celular_VR750 AS CelularCliente,
               c.Activo_VR750 AS ClienteActivo,

               u.Nombre_VR750 AS NombreManic, 
               u.Apellido_VR750 AS ApellidoManic,
               u.Email_VR750 AS EmailManic,
               u.Usuario_VR750 AS LoginManic,
               u.Contra_VR750 AS ContraManic,
               u.Salt_VR750 AS SaltManic,
               u.Rol_VR750 AS RolManic,
               u.Activo_VR750 AS ActivoManic,
               u.Bloqueado_VR750 AS BloqueadoManic,
               u.Idioma_VR750 AS IdiomaManic,

               s.Nombre_VR750 AS NombreServicio, 
               s.Tecnica_VR750 AS TecnicaServicio, 
               s.DuracionMinutos_VR750 AS DuracionServicio,
               s.Precio_VR750 AS PrecioServicio,
               s.Activo_VR750 AS ServicioActivo
        FROM Reserva_VR750 r
        LEFT JOIN Cliente_VR750 c ON r.DNIcli_VR750 = c.DNI_VR750
        LEFT JOIN Usuario_VR750 u ON r.DNImanic_VR750 = u.DNI_VR750
        LEFT JOIN Servicio_VR750 s ON r.IdServicio_VR750 = s.IdServicio_VR750
        WHERE r.DNImanic_VR750 = @DNI";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", dniManicurista);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var cliente = new BECliente_750VR(
                            dni: Convert.ToInt32(reader["DNIcli_VR750"]),
                            nom: reader["NombreCliente"].ToString(),
                            ape: reader["ApellidoCliente"].ToString(),
                            gmail: reader["EmailCliente"].ToString(),
                            dire: reader["DireccionCliente"].ToString(),
                            celu: reader["CelularCliente"].ToString(),
                            act: Convert.ToBoolean(reader["ClienteActivo"])
                        );

                        var manic = new BEusuario_750VR(
                            dni: Convert.ToInt32(reader["DNImanic_VR750"]),
                            nombre: reader["NombreManic"].ToString(),
                            ape: reader["ApellidoManic"].ToString(),
                            mail: reader["EmailManic"].ToString(),
                            user: reader["LoginManic"].ToString(),
                            contra: reader["ContraManic"].ToString(),
                            salt: reader["SaltManic"].ToString(),
                            rol: reader["RolManic"].ToString(),
                            activo: Convert.ToBoolean(reader["ActivoManic"]),
                            bloqueado: Convert.ToBoolean(reader["BloqueadoManic"]),
                            idiom: reader["IdiomaManic"].ToString()
                            //cod: Convert.ToInt32(reader["CodPerfil_VR750"])
                        );

                        var serv = new BEServicio_750VR(
                            id: Convert.ToInt32(reader["IdServicio_VR750"]),
                            nom: reader["NombreServicio"].ToString(),
                            tec: reader["TecnicaServicio"].ToString(),
                            dur: Convert.ToInt32(reader["DuracionServicio"]),
                            pre: Convert.ToDecimal(reader["PrecioServicio"]),
                            act: Convert.ToBoolean(reader["ServicioActivo"])
                        );

                        var reserva = new BEReserva_750VR(
                            cod: Convert.ToInt32(reader["IdReserva_VR750"]),
                    dnicli: Convert.ToInt32(reader["DNIcli_VR750"]),
                            cli: cliente,
                            dnimanic: Convert.ToInt32(reader["DNImanic_VR750"]),
                            manic: manic,
                            idserv: Convert.ToInt32(reader["IdServicio_VR750"]),
                            serv: serv,
                            fecha: Convert.ToDateTime(reader["Fecha_VR750"]),
                            ini: TimeSpan.Parse(reader["HoraInicio_VR750"].ToString()),
                            fin: TimeSpan.Parse(reader["HoraFin_VR750"].ToString()),
                            pre: Convert.ToDecimal(reader["Precio_VR750"]),
                            estado: reader["Estado_VR750"].ToString(),
                            cobrado: Convert.ToBoolean(reader["Cobrado_VR750"])
                        );

                       
                        lista.Add(reserva);
                    }
                }
            }

            return lista;
        }


        BaseDeDatos_750VR db = new BaseDeDatos_750VR();

        public int CrearReserva_750VR(BEReserva_750VR reserva)
        {
            using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = @"
            INSERT INTO Reserva_VR750 
            (DNIcli_VR750, DNImanic_VR750, IdServicio_VR750, Fecha_VR750, HoraInicio_VR750, HoraFin_VR750, Precio_VR750, Estado_VR750, Cobrado_VR750)
            VALUES 
            (@DNIcli, @DNImanic, @IdServicio, @Fecha, @HoraInicio, @HoraFin, @Precio, @Estado, @Cobrado);
            SELECT SCOPE_IDENTITY();";  // <<< esto devuelve el ID generado

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DNIcli", reserva.DNIcli_750VR);
                cmd.Parameters.AddWithValue("@DNImanic", reserva.DNImanic_750VR);
                cmd.Parameters.AddWithValue("@IdServicio", reserva.CodServicio_750VR);
                cmd.Parameters.AddWithValue("@Fecha", reserva.Fecha_750VR);
                cmd.Parameters.AddWithValue("@HoraInicio", reserva.HoraInicio_750VR);
                cmd.Parameters.AddWithValue("@HoraFin", reserva.HoraFin_750VR);
                cmd.Parameters.AddWithValue("@Precio", reserva.Precio_750VR);
                cmd.Parameters.AddWithValue("@Estado", reserva.Estado_750VR);
                cmd.Parameters.AddWithValue("@Cobrado", reserva.Cobrado_750VR);

                con.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result); // <<< retorna el ID a tu objeto
            }
        }
        public bool ModificarReserva(BEReserva_750VR reservaModificada)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = @"
            UPDATE Reserva_VR750
            SET DNIcli_VR750 = @DNIcli,
                DNImanic_VR750 = @DNImanic,
                IdServicio_VR750 = @IdServicio,
                Fecha_VR750 = @Fecha,
                HoraInicio_VR750 = @HoraInicio,
                HoraFin_VR750 = @HoraFin,
                Precio_VR750 = @Precio,
                Estado_VR750 = @Estado,
                Cobrado_VR750 = @Cobrado
            WHERE IdReserva_VR750 = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNIcli", reservaModificada.DNIcli_750VR);
                cmd.Parameters.AddWithValue("@DNImanic", reservaModificada.DNImanic_750VR);
                cmd.Parameters.AddWithValue("@IdServicio", reservaModificada.CodServicio_750VR);
                cmd.Parameters.AddWithValue("@Fecha", reservaModificada.Fecha_750VR);
                cmd.Parameters.AddWithValue("@HoraInicio", reservaModificada.HoraInicio_750VR);
                cmd.Parameters.AddWithValue("@HoraFin", reservaModificada.HoraFin_750VR);
                cmd.Parameters.AddWithValue("@Precio", reservaModificada.Precio_750VR);
                cmd.Parameters.AddWithValue("@Estado", reservaModificada.Estado_750VR);
                cmd.Parameters.AddWithValue("@Cobrado", reservaModificada.Cobrado_750VR);
                cmd.Parameters.AddWithValue("@Id", reservaModificada.CodReserva_750VR);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public string ObtenerEstadoReserva(int idReserva)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = "SELECT Estado_VR750 FROM Reserva_VR750 WHERE IdReserva_VR750 = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idReserva);

                conn.Open();
                object estado = cmd.ExecuteScalar();
                return estado != null ? estado.ToString() : null;
            }
        }
        public List<BEReserva_750VR> leerEntidades_750VR()
        {
            List<BEReserva_750VR> lista = new List<BEReserva_750VR>();

            using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = @"
        SELECT 
            r.*, 
            c.Nombre_VR750 AS NombreCliente, 
            c.Apellido_VR750 AS ApellidoCliente,
            c.Email_VR750 AS EmailCliente,
            c.Direccion_VR750 AS DireccionCliente,
            c.Celular_VR750 AS CelularCliente,
            c.Activo_VR750 AS ClienteActivo,

            u.Nombre_VR750 AS NombreManic, 
            u.Apellido_VR750 AS ApellidoManic,
            u.Email_VR750 AS EmailManic,
            u.Usuario_VR750 AS LoginManic,
            u.Contra_VR750 AS ContraManic,
            u.Salt_VR750 AS SaltManic,
            u.Rol_VR750 AS RolManic,
            u.Activo_VR750 AS ActivoManic,
            u.Bloqueado_VR750 AS BloqueadoManic,
            u.Idioma_VR750 AS IdiomaManic,

            s.Nombre_VR750 AS NombreServicio, 
            s.Tecnica_VR750 AS TecnicaServicio, 
            s.DuracionMinutos_VR750 AS DuracionServicio,
            s.Precio_VR750 AS PrecioServicio,
            s.Activo_VR750 AS ServicioActivo

        FROM Reserva_VR750 r
        LEFT JOIN Cliente_VR750 c ON r.DNIcli_VR750 = c.DNI_VR750
        LEFT JOIN Usuario_VR750 u ON r.DNImanic_VR750 = u.DNI_VR750
        LEFT JOIN Servicio_VR750 s ON r.IdServicio_VR750 = s.IdServicio_VR750";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var cliente = new BECliente_750VR(
                        dni: Convert.ToInt32(reader["DNIcli_VR750"]),
                        nom: reader["NombreCliente"].ToString(),
                        ape: reader["ApellidoCliente"].ToString(),
                        gmail: reader["EmailCliente"].ToString(),
                        dire: reader["DireccionCliente"].ToString(),
                        celu: reader["CelularCliente"].ToString(),
                        act: Convert.ToBoolean(reader["ClienteActivo"])
                    );

                    var manic = new BEusuario_750VR(
                        dni: Convert.ToInt32(reader["DNImanic_VR750"]),
                        nombre: reader["NombreManic"].ToString(),
                        ape: reader["ApellidoManic"].ToString(),
                        mail: reader["EmailManic"].ToString(),
                        user: reader["LoginManic"].ToString(),
                        contra: reader["ContraManic"].ToString(),
                        salt: reader["SaltManic"].ToString(),
                        rol: reader["RolManic"].ToString(),
                        activo: Convert.ToBoolean(reader["ActivoManic"]),
                        bloqueado: Convert.ToBoolean(reader["BloqueadoManic"]),
                        idiom: reader["IdiomaManic"].ToString()
                        //cod: Convert.ToInt32(reader["CodPerfil_VR750"])
                    );

                    var serv = new BEServicio_750VR(
                        id: Convert.ToInt32(reader["IdServicio_VR750"]),
                        nom: reader["NombreServicio"].ToString(),
                        tec: reader["TecnicaServicio"].ToString(),
                        dur: Convert.ToInt32(reader["DuracionServicio"]),
                        pre: Convert.ToDecimal(reader["PrecioServicio"]),
                        act: Convert.ToBoolean(reader["ServicioActivo"])
                    );

                    var reserva = new BEReserva_750VR(
                        cod: Convert.ToInt32(reader["IdReserva_VR750"]),
                        dnicli: Convert.ToInt32(reader["DNIcli_VR750"]),
                        cli: cliente,
                        dnimanic: Convert.ToInt32(reader["DNImanic_VR750"]),
                        manic: manic,
                        idserv: Convert.ToInt32(reader["IdServicio_VR750"]),
                        serv: serv,
                        fecha: Convert.ToDateTime(reader["Fecha_VR750"]),
                        ini: TimeSpan.Parse(reader["HoraInicio_VR750"].ToString()),
                        fin: TimeSpan.Parse(reader["HoraFin_VR750"].ToString()),
                        pre: Convert.ToDecimal(reader["Precio_VR750"]),
                        estado: reader["Estado_VR750"].ToString(),
                        cobrado: Convert.ToBoolean(reader["Cobrado_VR750"])
                    );


                    lista.Add(reserva);
                }

                reader.Close();
            }

            return lista;
        }



        public BEReserva_750VR ObtenerReservaPorId(int id)
        {
            using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = "SELECT * FROM Reserva_VR750 WHERE IdReserva_VR750 = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // Se crea la reserva con campos mínimos requeridos, el resto con valores por defecto
                    var reserva = new BEReserva_750VR(
                        cod: Convert.ToInt32(dr["IdReserva_VR750"]),
                        dnicli: Convert.ToInt32(dr["DNIcli_VR750"]),
                        cli: null,
                        dnimanic: Convert.ToInt32(dr["DNImanic_VR750"]),
                        manic: null,
                        idserv: Convert.ToInt32(dr["IdServicio_VR750"]),
                        serv: null,
                        fecha: Convert.ToDateTime(dr["Fecha_VR750"]),
                        ini: TimeSpan.Parse(dr["HoraInicio_VR750"].ToString()),
                        fin: TimeSpan.Parse(dr["HoraFin_VR750"].ToString()),
                        pre: Convert.ToDecimal(dr["Precio_VR750"]),
                        estado: dr["Estado_VR750"].ToString(),
                        cobrado: Convert.ToBoolean(dr["Cobrado_VR750"])
                    );

                    

                    dr.Close();
                    return reserva;
                }

                return null;
            }
        }



        public bool MarcarComoCobrado(int id)
        {
            using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = "UPDATE Reserva_VR750 SET Cobrado_VR750 = 1 WHERE IdReserva_VR750 = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public void ActualizarEstadoReserva(int idReserva, string nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = "UPDATE Reserva_VR750 SET Estado_VR750 = @estado WHERE IdReserva_VR750 = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@estado", nuevoEstado);
                cmd.Parameters.AddWithValue("@id", idReserva);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

    }
}
