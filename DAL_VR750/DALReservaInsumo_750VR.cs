using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALReservaInsumo_750VR
    {
        public void InsertarInsumoReserva(int idReserva, int idInsumo, int cantidad)
        {
            const string sql = @"
                INSERT INTO ReservaInsumo_VR750 (IdReserva_VR750, CodInsumo_VR750, CantidadUsada_VR750)
                VALUES (@reserva, @insumo, @cantidad);";

            using (var conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@reserva", SqlDbType.Int).Value = idReserva;
                cmd.Parameters.Add("@insumo", SqlDbType.Int).Value = idInsumo;
                cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = cantidad;

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool YaExisteInsumoParaReserva(int idReserva, int idInsumo)
        {
            const string sql = @"
                SELECT 1
                FROM ReservaInsumo_VR750
                WHERE IdReserva_VR750 = @reserva AND CodInsumo_VR750 = @insumo;";

            using (var conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@reserva", SqlDbType.Int).Value = idReserva;
                cmd.Parameters.Add("@insumo", SqlDbType.Int).Value = idInsumo;

                conn.Open();
                var o = cmd.ExecuteScalar();
                return o != null;
            }
        }

        /// <summary>
        /// Suma cantidad a una fila existente. Devuelve true si actualizó.
        /// </summary>
        public bool SumarCantidadInsumo(int idReserva, int idInsumo, int delta)
        {
            const string sql = @"
                UPDATE ReservaInsumo_VR750
                SET CantidadUsada_VR750 = CantidadUsada_VR750 + @delta
                WHERE IdReserva_VR750 = @reserva AND CodInsumo_VR750 = @insumo;";

            using (var conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@delta", SqlDbType.Int).Value = delta;
                cmd.Parameters.Add("@reserva", SqlDbType.Int).Value = idReserva;
                cmd.Parameters.Add("@insumo", SqlDbType.Int).Value = idInsumo;

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

        /// <summary>
        /// Si existe: suma cantidad. Si no existe: inserta.
        /// </summary>
        public void UpsertInsumoReserva(int idReserva, int idInsumo, int cantidad)
        {
            using (var conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            using (var cmdCheck = new SqlCommand(@"
                    SELECT 1 FROM ReservaInsumo_VR750
                    WHERE IdReserva_VR750=@r AND CodInsumo_VR750=@i;", conn))
            {
                cmdCheck.Parameters.Add("@r", SqlDbType.Int).Value = idReserva;
                cmdCheck.Parameters.Add("@i", SqlDbType.Int).Value = idInsumo;

                conn.Open();
                bool existe = cmdCheck.ExecuteScalar() != null;

                if (existe)
                {
                    using (var cmdUpd = new SqlCommand(@"
                        UPDATE ReservaInsumo_VR750
                        SET CantidadUsada_VR750 = CantidadUsada_VR750 + @c
                        WHERE IdReserva_VR750=@r AND CodInsumo_VR750=@i;", conn))
                    {
                        cmdUpd.Parameters.Add("@c", SqlDbType.Int).Value = cantidad;
                        cmdUpd.Parameters.Add("@r", SqlDbType.Int).Value = idReserva;
                        cmdUpd.Parameters.Add("@i", SqlDbType.Int).Value = idInsumo;
                        cmdUpd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var cmdIns = new SqlCommand(@"
                        INSERT INTO ReservaInsumo_VR750 (IdReserva_VR750, CodInsumo_VR750, CantidadUsada_VR750)
                        VALUES (@r, @i, @c);", conn))
                    {
                        cmdIns.Parameters.Add("@r", SqlDbType.Int).Value = idReserva;
                        cmdIns.Parameters.Add("@i", SqlDbType.Int).Value = idInsumo;
                        cmdIns.Parameters.Add("@c", SqlDbType.Int).Value = cantidad;
                        cmdIns.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
