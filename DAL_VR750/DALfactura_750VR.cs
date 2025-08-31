using BE_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALfactura_750VR
    {
     
        public int InsertarFactura(BEfactura_750VR factura)
        {
            using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string sql = @"
INSERT INTO Factura_VR750 (CodReserva_VR750, fecha_VR750, horaEmision_VR750, metodopago_VR750, total_VR750, titular_VR750)
OUTPUT INSERTED.CodFactura_VR750
VALUES (@codReserva, @fecha, @hora, @metodo, @total, @titular);";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@codReserva", factura.CodReserva_750VR);
                    cmd.Parameters.AddWithValue("@fecha", factura.fecha_750VR);
                    cmd.Parameters.AddWithValue("@hora", factura.horaEmision_750VR);
                    cmd.Parameters.AddWithValue("@total", factura.total_750VR);
                    cmd.Parameters.AddWithValue("@metodo", factura.metodoPago_750VR);
                    cmd.Parameters.AddWithValue("@titular", factura.titular_750VR);

                    con.Open();
                    object scalar = cmd.ExecuteScalar();
                    if (scalar == null || scalar == DBNull.Value)
                        throw new Exception("No se pudo obtener el código de factura generado.");

                    return Convert.ToInt32(scalar);
                }
            }
        }

        public List<BEfactura_750VR> LeerFacturas()
        {
            var lista = new List<BEfactura_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string sql = "SELECT * FROM Factura_VR750 ORDER BY fecha_VR750 DESC, horaEmision_VR750 DESC;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int codFactura = Convert.ToInt32(reader["CodFactura_VR750"]);
                            int codReserva = Convert.ToInt32(reader["CodReserva_VR750"]);
                            DateTime fecha = Convert.ToDateTime(reader["fecha_VR750"]);
                            TimeSpan hora = (TimeSpan)reader["horaEmision_VR750"];
                            decimal total = Convert.ToDecimal(reader["total_VR750"]);
                            string metodo = reader["metodopago_VR750"].ToString(); // ojo con el nombre de columna
                            string titular = reader["titular_VR750"].ToString();

                            var f = new BEfactura_750VR(codFactura, codReserva, fecha, hora, total, metodo, titular);
                            lista.Add(f);
                        }
                    }
                }
            }
            return lista;
        }

    
        public BEfactura_750VR ObtenerFacturaPorCodigo(int codFactura)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string sql = "SELECT * FROM Factura_VR750 WHERE CodFactura_VR750 = @cod;";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cod", codFactura);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int codReserva = Convert.ToInt32(reader["CodReserva_VR750"]);
                            DateTime fecha = Convert.ToDateTime(reader["fecha_VR750"]);
                            TimeSpan hora = (TimeSpan)reader["horaEmision_VR750"];
                            decimal total = Convert.ToDecimal(reader["total_VR750"]);
                            string metodo = reader["metodopago_VR750"].ToString();
                            string titular = reader["titular_VR750"].ToString();

                            return new BEfactura_750VR(codFactura, codReserva, fecha, hora, total, metodo, titular);
                        }
                    }
                }
            }
            return null;
        }
    }
}
