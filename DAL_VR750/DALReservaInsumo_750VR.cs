using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALReservaInsumo_750VR
    {
        BaseDeDatos_750VR db = new BaseDeDatos_750VR();
        public void InsertarInsumoReserva(int idReserva, int idInsumo, int cantidad)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = @"INSERT INTO ReservaInsumo_VR750 
                                (IdReserva_VR750, CodInsumo_VR750, CantidadUsada_VR750) 
                                 VALUES (@reserva, @insumo, @cantidad)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@reserva", idReserva);
                cmd.Parameters.AddWithValue("@insumo", idInsumo);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al registrar insumo usado: " + ex.Message);
                }
            }
        }
        //public bool TieneInsumosCargados(int idReserva)
        //{
        //    using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
        //    {
        //        string query = @"SELECT COUNT(*) FROM ReservaInsumo_VR750 WHERE IdReserva_VR750 = @reserva";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        cmd.Parameters.AddWithValue("@reserva", idReserva);

        //        try
        //        {
        //            conn.Open();
        //            int cantidad = (int)cmd.ExecuteScalar();
        //            return cantidad > 0;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception("Error al verificar insumos cargados: " + ex.Message);
        //        }
        //    }
        //}

        public bool YaExisteInsumoParaReserva(int idReserva, int idInsumo)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                string query = @"SELECT COUNT(*) FROM ReservaInsumo_VR750 
                         WHERE IdReserva_VR750 = @reserva AND CodInsumo_VR750 = @insumo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@reserva", idReserva);
                cmd.Parameters.AddWithValue("@insumo", idInsumo);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al validar existencia de insumo en reserva: " + ex.Message);
                }
            }
        }
    }
}
