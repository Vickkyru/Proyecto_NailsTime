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

        public void CrearDisponibilidad(BEdisponibilidad_750VR disp)
        {
            string query = @"INSERT INTO Disponibilidad_VR750
                            (DNIempleado_VR750, DiaSemana_VR750, HoraInicio_VR750, HoraFin_VR750, Activo_VR750, Estado_VR750)
                            VALUES (@DNI, @Dia, @Inicio, @Fin, @Activo, @Estado)";

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DNI", disp.DNIempleado);
                cmd.Parameters.AddWithValue("@Dia", disp.DiaSemana);
                cmd.Parameters.AddWithValue("@Inicio", disp.HoraInicio);
                cmd.Parameters.AddWithValue("@Fin", disp.HoraFin);
                cmd.Parameters.AddWithValue("@Activo", disp.Activo);
                cmd.Parameters.AddWithValue("@Estado", disp.estado);
                cmd.ExecuteNonQuery();
            }
        }

        public bool ModificarDisponibilidad(BEdisponibilidad_750VR dispo)
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
                cmd.Parameters.AddWithValue("@Inicio", dispo.HoraInicio);
                cmd.Parameters.AddWithValue("@Fin", dispo.HoraFin);
                cmd.Parameters.AddWithValue("@Dia", dispo.DiaSemana);
                cmd.Parameters.AddWithValue("@ID", dispo.IdDisponibilidad);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CambiarEstadoActivo(int id, bool nuevoEstado)
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

        public List<BEdisponibilidad_750VR> LeerDisponibilidades()
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
                        IdDisponibilidad = Convert.ToInt32(reader["IdDisponibilidad_VR750"]),
                        DNIempleado = Convert.ToInt32(reader["DNIempleado_VR750"]),
                        DiaSemana = Convert.ToInt32(reader["DiaSemana_VR750"]),
                        HoraInicio = (TimeSpan)reader["HoraInicio_VR750"],
                        HoraFin = (TimeSpan)reader["HoraFin_VR750"],
                        Activo = Convert.ToBoolean(reader["Activo_VR750"]),
                        estado = Convert.ToBoolean(reader["Estado_VR750"])
                    });
                }
            }
            return lista;
        }

    }
}
