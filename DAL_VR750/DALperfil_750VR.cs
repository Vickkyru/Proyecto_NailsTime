using BE_VR750;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_VR750
{
    public class DALperfil_750VR
    {
        public void AgregarPerfil(BEperfil_750VR perfil)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "INSERT INTO Perfil_750VR (Nombre_VR750) VALUES (@Nombre)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", perfil.NombrePerfil_750VR);
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarPerfil(int codPerfil)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "DELETE FROM Perfil_750VR WHERE CodPerfil_VR750 = @CodPerfil";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                cmd.ExecuteNonQuery();
            }
        }

        public List<BEperfil_750VR> ObtenerPerfiles()
        {
            List<BEperfil_750VR> perfiles = new List<BEperfil_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT CodPerfil_VR750, Nombre_VR750 FROM Perfil_750VR";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BEperfil_750VR perfil = new BEperfil_750VR
                    {
                        CodPerfil_750VR = Convert.ToInt32(reader["CodPerfil_VR750"]),
                        NombrePerfil_750VR = reader["Nombre_VR750"].ToString()
                    };
                    perfiles.Add(perfil);
                }
            }

            return perfiles;
        }

        public void AsignarPermiso(int codPerfil, int codPermiso)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"INSERT INTO PerfilPermiso_750VR (CodPerfil_VR750, CodPermiso_VR750) 
                                 VALUES (@CodPerfil, @CodPermiso)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                cmd.Parameters.AddWithValue("@CodPermiso", codPermiso);
                cmd.ExecuteNonQuery();
            }
        }

        public void QuitarPermiso(int codPerfil, int codPermiso)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"DELETE FROM PerfilPermiso_750VR 
                                 WHERE CodPerfil_VR750 = @CodPerfil AND CodPermiso_VR750 = @CodPermiso";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                cmd.Parameters.AddWithValue("@CodPermiso", codPermiso);
                cmd.ExecuteNonQuery();
            }
        }

        public void AsignarFamilia(int codPerfil, int codFamilia)
        {
            using (SqlConnection conn = new SqlConnection(  BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"INSERT INTO PerfilFamilia_750VR (CodPerfil_VR750, CodFamilia_VR750) 
                                 VALUES (@CodPerfil, @CodFamilia)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                cmd.Parameters.AddWithValue("@CodFamilia", codFamilia);
                cmd.ExecuteNonQuery();
            }
        }

        public void QuitarFamilia(int codPerfil, int codFamilia)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"DELETE FROM PerfilFamilia_750VR 
                                 WHERE CodPerfil_VR750 = @CodPerfil AND CodFamilia_VR750 = @CodFamilia";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                cmd.Parameters.AddWithValue("@CodFamilia", codFamilia);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
