using BE_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALinsumos_750VR
    {
        BaseDeDatos_750VR db = new BaseDeDatos_750VR();

        public List<BEinsumos_750VR> LeerInsumos()
        {
            List<BEinsumos_750VR> lista = new List<BEinsumos_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = "SELECT * FROM Insumo_VR750";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        BEinsumos_750VR insumo = new BEinsumos_750VR
                        {
                            CodInsumo_750VR = Convert.ToInt32(reader["CodInsumo_VR750"]),
                            nombre_750VR = reader["Nombre_VR750"].ToString(),
                            descripcion_750VR = reader["Descripcion_VR750"].ToString(),
                            cantidadActual_750VR = Convert.ToInt32(reader["CantidadActual_VR750"]),
                            stockMinimo_750VR = Convert.ToInt32(reader["StockMinimo_VR750"]),
                            unidadMedida_750VR = reader["UnidadMedida_VR750"].ToString(),
                            activo_750VR = Convert.ToBoolean(reader["Activo_VR750"])
                        };

                        lista.Add(insumo);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al leer insumos: " + ex.Message);
                }
            }

            return lista;
        }
    }
}
