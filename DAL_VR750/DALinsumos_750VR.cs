using BE_VR750;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALinsumos_750VR
    {
        BaseDeDatos_750VR db = new BaseDeDatos_750VR();

        public void CrearInsumo_750VR(BEinsumos_750VR insumo)
        {
            string query = @"INSERT INTO Insumo_VR750
                            (nombre_VR750, descripcion_VR750, cantidadActual_VR750, stockMinimo_VR750, unidadMedida_VR750, activo_VR750)
                            VALUES (@nombre, @descripcion, @cantidad, @stockMin, @unidad, @activo)";

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", insumo.nombre_750VR);
                cmd.Parameters.AddWithValue("@descripcion", insumo.descripcion_750VR);
                cmd.Parameters.AddWithValue("@cantidad", insumo.cantidadActual_750VR);
                cmd.Parameters.AddWithValue("@stockMin", insumo.stockMinimo_750VR);
                cmd.Parameters.AddWithValue("@unidad", insumo.unidadMedida_750VR);
                cmd.Parameters.AddWithValue("@activo", insumo.activo_750VR);
                cmd.ExecuteNonQuery();
            }
        }

        public bool ModificarInsumo_750VR(BEinsumos_750VR insumo)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
                UPDATE Insumo_VR750
                SET nombre_VR750 = @nombre,
                    descripcion_VR750 = @descripcion,
                    cantidadActual_VR750 = @cantidad,
                    stockMinimo_VR750 = @stockMin,
                    unidadMedida_VR750 = @unidad
                WHERE CodInsumo_VR750 = @codigo";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", insumo.nombre_750VR);
                cmd.Parameters.AddWithValue("@descripcion", insumo.descripcion_750VR);
                cmd.Parameters.AddWithValue("@cantidad", insumo.cantidadActual_750VR);
                cmd.Parameters.AddWithValue("@stockMin", insumo.stockMinimo_750VR);
                cmd.Parameters.AddWithValue("@unidad", insumo.unidadMedida_750VR);
                cmd.Parameters.AddWithValue("@codigo", insumo.CodInsumo_750VR);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CambiarEstado_750VR(int id, bool nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE Insumo_VR750 SET activo_VR750 = @activo WHERE CodInsumo_VR750 = @codigo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@activo", nuevoEstado ? 1 : 0);
                cmd.Parameters.AddWithValue("@codigo", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<BEinsumos_750VR> LeerInsumos_750VR()
        {
            List<BEinsumos_750VR> lista = new List<BEinsumos_750VR>();
            string query = "SELECT * FROM Insumo_VR750";

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var insumo = new BEinsumos_750VR
                    {
                        CodInsumo_750VR = Convert.ToInt32(reader["CodInsumo_VR750"]),
                        nombre_750VR = reader["nombre_VR750"].ToString(),
                        descripcion_750VR = reader["descripcion_VR750"].ToString(),
                        cantidadActual_750VR = Convert.ToInt32(reader["cantidadActual_VR750"]),
                        stockMinimo_750VR = Convert.ToInt32(reader["stockMinimo_VR750"]),
                        unidadMedida_750VR = reader["unidadMedida_VR750"].ToString(),
                        activo_750VR = Convert.ToBoolean(reader["activo_VR750"])
                    };

                    lista.Add(insumo);
                }
            }

            return lista;
        }

        public List<BEinsumos_750VR> LeerInsumosActivos_750VR()
        {
            return LeerInsumos_750VR().FindAll(i => i.activo_750VR);
        }

        public List<BEinsumos_750VR> LeerInsumosActivos()
        {
            const string sql = @"SELECT CodInsumo_VR750, nombre_750VR, descripcion_750VR,
                                        cantidadActual_750VR, stockMinimo_750VR, unidadMedida_750VR, activo_750VR
                                 FROM Insumo_VR750
                                 WHERE activo_750VR = 1";

            var lista = new List<BEinsumos_750VR>();
            using (var cn = new SqlConnection(BaseDeDatos_750VR.cadena))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new BEinsumos_750VR
                        {
                            CodInsumo_750VR = dr.GetInt32(0),
                            nombre_750VR = dr.GetString(1),
                            descripcion_750VR = dr.IsDBNull(2) ? "" : dr.GetString(2),
                            cantidadActual_750VR = dr.GetInt32(3),
                            stockMinimo_750VR = dr.GetInt32(4),
                            unidadMedida_750VR = dr.IsDBNull(5) ? "" : dr.GetString(5),
                            activo_750VR = dr.GetBoolean(6)
                        });
                    }
                }
            }
            return lista;
        }

        public void DescontarStock(int codInsumo, int cantidad)
        {
            const string sql = @"UPDATE Insumo_VR750
                                 SET cantidadActual_750VR = cantidadActual_750VR - @c
                                 WHERE CodInsumo_VR750=@i";
            using (var cn = new SqlConnection(BaseDeDatos_750VR.cadena))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.Add("@c", SqlDbType.Int).Value = cantidad;
                cmd.Parameters.Add("@i", SqlDbType.Int).Value = codInsumo;
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
}

