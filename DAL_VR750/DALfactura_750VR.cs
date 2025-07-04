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
        public bool InsertarFactura(BEfactura_750VR factura)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(BaseDeDatos_750VR.cadena))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Factura_VR750 (CodReserva_VR750, fecha_VR750, horaEmision_VR750, total_VR750, metodoPago_VR750, titular_VR750) " +
                                                    "VALUES (@codReserva, @fecha, @hora, @total, @metodo, @titular)", con);
                    cmd.Parameters.AddWithValue("@codReserva", factura.CodReserva_750VR);
                    cmd.Parameters.AddWithValue("@fecha", factura.fecha_750VR);
                    cmd.Parameters.AddWithValue("@hora", factura.horaEmision_750VR);
                    cmd.Parameters.AddWithValue("@total", factura.total_750VR);
                    cmd.Parameters.AddWithValue("@metodo", factura.metodoPago_750VR);
                    cmd.Parameters.AddWithValue("@titular", factura.titular_750VR);

                    con.Open();
                    int filas = cmd.ExecuteNonQuery();

                    return filas > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<BEfactura_750VR> LeerFacturas()
        {
            List<BEfactura_750VR> lista = new List<BEfactura_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Factura_VR750", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int codFactura = Convert.ToInt32(reader["CodFactura_VR750"]);
                    int codReserva = Convert.ToInt32(reader["CodReserva_VR750"]);
                    DateTime fecha = Convert.ToDateTime(reader["fecha_VR750"]);
                    TimeSpan hora = (TimeSpan)reader["horaEmision_VR750"];
                    decimal total = Convert.ToDecimal(reader["total_VR750"]);
                    string metodoPago = reader["metodoPago_VR750"].ToString();
                    string titular = reader["titular_VR750"].ToString();

                    BEfactura_750VR f = new BEfactura_750VR(codFactura, codReserva, fecha, hora, total, metodoPago, titular);
                    lista.Add(f);
                }
            }

            return lista;
        }
    }
}
