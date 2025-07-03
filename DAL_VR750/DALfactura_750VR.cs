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
        public void InsertarFactura(BEfactura_750VR factura)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Factura_VR750 (CodReserva_VR750, fecha_VR750, horaEmision_VR750, total_VR750, metodoPago_VR750, titular_VR750) " +
                    "VALUES (@reserva, @fecha, @hora, @total, @metodo, @titular)", conn);

                cmd.Parameters.AddWithValue("@reserva", factura.CodReserva_750VR);
                cmd.Parameters.AddWithValue("@fecha", factura.fecha_750VR);
                cmd.Parameters.AddWithValue("@hora", factura.horaEmision_750VR);
                cmd.Parameters.AddWithValue("@total", factura.total_750VR);
                cmd.Parameters.AddWithValue("@metodo", factura.metodoPago_750VR);
                cmd.Parameters.AddWithValue("@titular", factura.titular_750VR);

                conn.Open();
                cmd.ExecuteNonQuery();
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
                    BEfactura_750VR f = new BEfactura_750VR
                    {
                        CodFactura_750VR = Convert.ToInt32(reader["CodFactura_VR750"]),
                        CodReserva_750VR = Convert.ToInt32(reader["CodReserva_VR750"]),
                        fecha_750VR = Convert.ToDateTime(reader["fecha_VR750"]),
                        horaEmision_750VR = (TimeSpan)reader["horaEmision_VR750"],
                        metodoPago_750VR = reader["total_VR750"].ToString(),
                        total_750VR = Convert.ToDecimal(reader["metodoPago_VR750"]),
                        titular_750VR = reader["titular_VR750"].ToString()
                    };
                    lista.Add(f);
                }
            }

            return lista;
        }
    }
}
