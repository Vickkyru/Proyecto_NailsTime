using BE_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public  class DALdisponibilidad_750VR
    {

        BaseDeDatos_750VR db = new BaseDeDatos_750VR();

        public void CrearDisponibilidad_750VR(BEdisponibilidad_750VR disp)
        {
            string query = @"INSERT INTO Disponibilidad_VR750
                            (DNImanic_VR750, NombreManic_VR750 DiaSemana_VR750, HoraInicio_VR750, HoraFin_VR750, Activo_VR750, Estado_VR750)
                            VALUES (@DNI, @Nom, @Dia, @Inicio, @Fin, @Activo, @Estado)";

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", disp.DNImanic_750VR);
                cmd.Parameters.AddWithValue("@Nom", disp.Nombremanic_750VR);
                cmd.Parameters.AddWithValue("@Dia", disp.DiaSemana_750VR);
                cmd.Parameters.AddWithValue("@Inicio", disp.HoraInicio_750VR);
                cmd.Parameters.AddWithValue("@Fin", disp.HoraFin_750VR);
                cmd.Parameters.AddWithValue("@Activo", disp.activo_750VR);
                cmd.Parameters.AddWithValue("@Estado", disp.estado_750VR);
                cmd.ExecuteNonQuery();
            }
        }

        public bool ModificarDisponibilidad_750VR(BEdisponibilidad_750VR dispo)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"UPDATE Disponibilidad_VR750 SET 
                            HoraInicio_VR750 = @Inicio, 
                            HoraFin_VR750 = @Fin ,
                            DiaSemana_VR750 = @Dia
                            WHERE IdDisponibilidad_VR750 = @ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Inicio", dispo.HoraInicio_750VR);
                cmd.Parameters.AddWithValue("@Fin", dispo.HoraFin_750VR);
                cmd.Parameters.AddWithValue("@Dia", dispo.DiaSemana_750VR);
                cmd.Parameters.AddWithValue("@ID", dispo.IdDisponibilidad_750VR);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CambiarEstado_750VR(int id, bool nuevoEstado)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "UPDATE Disponibilidad_VR750 SET Activo_VR750 = @Activo WHERE IdDisponibilidad_VR750 = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Activo", nuevoEstado ? 1 : 0);
                cmd.Parameters.AddWithValue("@ID", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<BEdisponibilidad_750VR> LeerDisponibilidades_750VR()
        {
            List<BEdisponibilidad_750VR> lista = new List<BEdisponibilidad_750VR>();
            string query = "SELECT * FROM Disponibilidad_VR750";

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new BEdisponibilidad_750VR
                    {
                        IdDisponibilidad_750VR = Convert.ToInt32(reader["IdDisponibilidad_VR750"]),
                        DNImanic_750VR = Convert.ToInt32(reader["DNImanic_VR750"]),
                        Nombremanic_750VR = Convert.ToString(reader["NombreManic_VR750"]),
                        DiaSemana_750VR = Convert.ToInt32(reader["DiaSemana_VR750"]),
                        HoraInicio_750VR = (TimeSpan)reader["HoraInicio_VR750"],
                        HoraFin_750VR = (TimeSpan)reader["HoraFin_VR750"],
                        activo_750VR = Convert.ToBoolean(reader["Activo_VR750"]),
                        estado_750VR = Convert.ToBoolean(reader["Estado_VR750"])
                    });
                }
            }
            return lista;
        }

    }
}
