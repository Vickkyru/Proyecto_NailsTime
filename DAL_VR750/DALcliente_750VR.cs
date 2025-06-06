using BE_VR750;
using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALcliente_750VR
    {
        BaseDeDatos_750VR db { get; }
        public DALcliente_750VR()
        {
            db = new BaseDeDatos_750VR();
        }

        //fijarme bien las celdas
        public BECliente_750VR ObtenerClientePorDNI_750VR(int dni)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Cliente_VR750 WHERE DNI_VR750 = @DNI";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", dni);

                //var encriptador = new Encriptador_750VR();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    //string emailLeido = reader["Email_VR750"].ToString();
                    //string emailDescifrado;

                    //try
                    //{
                    //    emailDescifrado = encriptador.DesencriptarAES_750VR(emailLeido);
                    //}
                    //catch
                    //{
                    //    emailDescifrado = "[Email no válido]";
                    //}

                    return new BECliente_750VR(
                        dni: Convert.ToInt32(reader["DNI_VR750"]),
                        nom: reader["Nombre_VR750"].ToString(),
                        ape: reader["Apellido_VR750"].ToString(),
                        gmail: reader["Email_VR750"].ToString(), // SIN desencriptar acá
                        dire: reader["Direccion_VR750"].ToString(),
                        celu: reader["Celular_VR750"].ToString(),
                        act: Convert.ToBoolean(reader["Activo_VR750"])
                    );
                }
            }

            return null;
        }

        public void CrearCliente_750VR(BECliente_750VR usuario) //alta user
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string query = @"INSERT INTO Cliente_VR750 
        (DNI_VR750, Nombre_VR750, Apellido_VR750, Email_VR750, Direccion_VR750, Celular_VR750, Activo_VR750) 
        VALUES (@DNI, @Nombre, @Apellido, @Email, @Direccion, @Celular, @Activo)";

                SqlCommand cmd = new SqlCommand(query, conn);

                var encriptador = new Encriptador_750VR();
                string emailCifrado = encriptador.EncriptarAES_750VR(usuario.gmail_750VR); // 👈 ASEGURAR ESTA LÍNEA

                cmd.Parameters.AddWithValue("@DNI", usuario.dni_750VR);
                cmd.Parameters.AddWithValue("@Nombre", usuario.nombre_750VR);
                cmd.Parameters.AddWithValue("@Apellido", usuario.apellido_750VR);
                cmd.Parameters.AddWithValue("@Email", emailCifrado); // 👈 ACÁ VA EL CIFRADO
                cmd.Parameters.AddWithValue("@Direccion", usuario.direccion_750VR);
                cmd.Parameters.AddWithValue("@Celular", usuario.celular_750VR);
                cmd.Parameters.AddWithValue("@Activo", usuario.activo_750VR);

                cmd.ExecuteNonQuery();
            }
        }


        public bool ModificarCliente_750VR(int dni, string nombre, string apellido, string mail, string dire, int celu) //mod user
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"UPDATE Cliente_VR750 
                         SET Nombre_VR750 = @Nombre, Apellido_VR750 = @Apellido, Email_VR750 = @Mail, Direccion_VR750 = @Direccion, Celular_VR750 = @Celular
                         WHERE DNI_VR750 = @DNI";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                //cmd.Parameters.AddWithValue("@Mail", mail);
                cmd.Parameters.AddWithValue("@Direccion", dire);
                cmd.Parameters.AddWithValue("@Celular", celu);
                cmd.Parameters.AddWithValue("@DNI", dni);
                var encriptador = new Encriptador_750VR();
                cmd.Parameters.AddWithValue("@Mail", encriptador.EncriptarAES_750VR(mail));

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CambiarEstadoCliente_750VR(int dni, bool nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE Cliente_VR750 SET Activo_VR750 = @Activo WHERE DNI_VR750 = @DNI";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Activo", nuevoEstado ? 1 : 0);
                cmd.Parameters.AddWithValue("@DNI", dni);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<BECliente_750VR> BuscarClientes_750VR(string dni, string nombre, string apellido, string email, string dire, string celu)
        {
            List<BECliente_750VR> lista = new List<BECliente_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string query = "SELECT * FROM Cliente_VR750 WHERE 1=1";

                if (!string.IsNullOrEmpty(dni))
                    query += " AND CAST(DNI_VR750 AS VARCHAR) LIKE @DNI";

                if (!string.IsNullOrEmpty(nombre))
                    query += " AND Nombre_VR750 LIKE @Nombre";

                if (!string.IsNullOrEmpty(apellido))
                    query += " AND Apellido_VR750 LIKE @Apellido";

                if (!string.IsNullOrEmpty(email))
                    query += " AND Email_VR750 LIKE @Email";

                if (!string.IsNullOrEmpty(dire))
                    query += " AND Direccion_VR750 LIKE @Direccion";

                if (!string.IsNullOrEmpty(celu))
                    query += " AND Celular_VR750 LIKE @Celular";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(dni))
                    cmd.Parameters.AddWithValue("@DNI", "%" + dni + "%");

                if (!string.IsNullOrEmpty(nombre))
                    cmd.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");

                if (!string.IsNullOrEmpty(apellido))
                    cmd.Parameters.AddWithValue("@Apellido", "%" + apellido + "%");

                if (!string.IsNullOrEmpty(email))
                    cmd.Parameters.AddWithValue("@Email", "%" + email + "%");

                if (!string.IsNullOrEmpty(dire))
                    cmd.Parameters.AddWithValue("@Direccion", "%" + dire + "%");

                if (!string.IsNullOrEmpty(celu))
                    cmd.Parameters.AddWithValue("@Celular", "%" + celu + "%");

                SqlDataReader reader = cmd.ExecuteReader();
                //var encriptador = new Encriptador_750VR();


                while (reader.Read())
                {
                    BECliente_750VR cli = new BECliente_750VR(
     dni: Convert.ToInt32(reader["DNI_VR750"]),
     nom: reader["Nombre_VR750"].ToString(),
     ape: reader["Apellido_VR750"].ToString(),
     gmail: reader["Email_VR750"].ToString(),
     dire: reader["Direccion_VR750"].ToString(),
     celu: reader["Celular_VR750"].ToString(),
     act: Convert.ToBoolean(reader["Activo_VR750"])
     //gmail: encriptador.DesencriptarAES_750VR(reader["Email_VR750"].ToString())

 );
                    lista.Add(cli);
                }
            }

            return lista;
        }

        public List<BECliente_750VR> leerEntidades_750VR()
        {
            List<BECliente_750VR> lista = new List<BECliente_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT * FROM Cliente_VR750";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                //var encriptador = new Encriptador_750VR();

                while (reader.Read())
                {
                    //string emailLeido = reader["Email_VR750"].ToString();
                    //string emailDescifrado;
                    //try
                    //{
                    //    emailDescifrado = encriptador.DesencriptarAES_750VR(emailLeido);
                    //}
                    //catch
                    //{
                    //    emailDescifrado = "[ERROR Base64]";
                    //}

                    BECliente_750VR cli = new BECliente_750VR(
                        dni: Convert.ToInt32(reader["DNI_VR750"]),
                        nom: reader["Nombre_VR750"].ToString(),
                        ape: reader["Apellido_VR750"].ToString(),
                        //gmail: emailDescifrado,
                        gmail: reader["Email_VR750"].ToString(), // ← email encriptado
                        dire: reader["Direccion_VR750"].ToString(),
                        celu: reader["Celular_VR750"].ToString(), // CAMBIO: int ➝ string
                        act: Convert.ToBoolean(reader["Activo_VR750"])
                    );
                    lista.Add(cli);
                }
            }

            return lista;
        }
    }
}
