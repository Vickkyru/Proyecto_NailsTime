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
        public void InsertarPerfil(BEperfil_750VR perfil)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "INSERT INTO Perfil_VR750 (NombrePerfil_VR750) VALUES (@Nombre)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", perfil.NombrePerfil_750VR);
                cmd.ExecuteNonQuery();
            }
        }
        public int InsertarPerfilYDevolverID(BEperfil_750VR perfil)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "INSERT INTO Perfil_VR750 (NombrePerfil_VR750) OUTPUT INSERTED.CodPerfil_VR750 VALUES (@Nombre)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", perfil.NombrePerfil_750VR);
                return (int)cmd.ExecuteScalar();
            }
        }

        public static List<string> ObtenerPermisosPorPerfil(int codPerfil)
        {
            List<string> permisos = new List<string>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
        SELECT p.NombrePermiso_VR750
FROM PerfilPermiso_VR750 pp
JOIN Permiso_VR750 p ON pp.CodPermiso_VR750 = p.CodPermiso_VR750
WHERE pp.CodPerfil_VR750 = @codPerfil
        ", conn);

                cmd.Parameters.AddWithValue("@codPerfil", codPerfil);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    permisos.Add(reader.GetString(0));
                }
            }

            return permisos;
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
                string query = "SELECT CodPerfil_VR750, NombrePerfil_VR750 FROM Perfil_VR750";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    BEperfil_750VR perfil = new BEperfil_750VR
                    {
                        CodPerfil_750VR = Convert.ToInt32(reader["CodPerfil_VR750"]),
                        NombrePerfil_750VR = reader["NombrePerfil_VR750"].ToString()
                    };
                    perfiles.Add(perfil);
                }
            }

            return perfiles;
        }
  

      

        public List<GrupoPermiso_750VR> ObtenerFamilias()
        {
            List<GrupoPermiso_750VR> familias = new List<GrupoPermiso_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"SELECT CodPermiso_VR750, NombrePermiso_VR750 
                         FROM Permiso_VR750 
                         WHERE EsFamilia_VR750 = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string nombre = reader.GetString(1);

                            familias.Add(new GrupoPermiso_750VR(id, nombre));
                        }
                    }
                }
            }

            return familias;
        }
        public void QuitarFamilia(int codPerfil, int codFamilia)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"DELETE FROM PerfilPermiso_VR750 
                         WHERE CodPerfil_VR750 = @CodPerfil AND CodPermiso_VR750 = @CodFamilia";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                cmd.Parameters.AddWithValue("@CodFamilia", codFamilia);
                cmd.ExecuteNonQuery();
            }
        }


        public void AsignarFamilia(int codPerfil, int codFamilia)
        {
            using (SqlConnection conn = new SqlConnection(  BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"INSERT INTO PerfilPermiso_VR750 (CodPerfil_VR750, CodPermiso_VR750) 
                 VALUES (@CodPerfil, @CodPermiso)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                cmd.Parameters.AddWithValue("@CodPermiso", codFamilia);
                cmd.ExecuteNonQuery();
            }
        }
        public List<IComponentePermiso_750VR> ObtenerPermisosDePerfil(int idPerfil)
        {
            List<IComponentePermiso_750VR> permisos = new List<IComponentePermiso_750VR>();
            List<(int codPermiso, string nombre, bool esFamilia)> resultados = new List<(int, string, bool)>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
            SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750, p.EsFamilia_VR750
            FROM PerfilPermiso_VR750 pp
            INNER JOIN Permiso_VR750 p ON pp.CodPermiso_VR750 = p.CodPermiso_VR750
            WHERE pp.CodPerfil_VR750 = @idPerfil";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idPerfil", idPerfil);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultados.Add((
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetBoolean(2)
                            ));
                        }
                    }

                    // reader ya está cerrado
                    foreach (var r in resultados)
                    {
                        if (r.esFamilia)
                        {
                            var familia = new GrupoPermiso_750VR(r.codPermiso, r.nombre);
                            CargarHijos(familia, conn);
                            permisos.Add(familia);
                        }
                        else
                        {
                            permisos.Add(new PermisoSimple_750VR(r.codPermiso, r.nombre));
                        }
                    }
                }
            }

            return permisos;
        }
        public void InsertarFamilia(string nombre)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "INSERT INTO Permiso_VR750 (NombrePermiso_VR750, EsFamilia_VR750) VALUES (@Nombre, 1)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.ExecuteNonQuery();
            }
        }
        public List<IComponentePermiso_750VR> ObtenerPermisosDeFamilia(int codFamilia)
        {
            List<IComponentePermiso_750VR> permisos = new List<IComponentePermiso_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
        SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750, p.EsFamilia_VR750
        FROM PermisoComposicion_VR750 pc
        INNER JOIN Permiso_VR750 p ON pc.HijoPermiso_VR750 = p.CodPermiso_VR750
        WHERE pc.PadrePermiso_VR750 = @codFamilia";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@codFamilia", codFamilia);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int codigo = reader.GetInt32(0);
                            string nombre = reader.GetString(1);
                            bool esFamilia = reader.GetBoolean(2);

                            if (esFamilia)
                            {
                                var subFamilia = new GrupoPermiso_750VR(codigo, nombre);
                                CargarHijos(subFamilia, conn); // ← RECURSIVO
                                permisos.Add(subFamilia);
                            }
                            else
                            {
                                permisos.Add(new PermisoSimple_750VR(codigo, nombre));
                            }
                        }
                    }
                }
            }

            return permisos;
        }
        public List<BEperfil_750VR> ObtenerTodosLosPerfiles()
        {
            List<BEperfil_750VR> perfiles = new List<BEperfil_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT CodPerfil_VR750, NombrePerfil_VR750 FROM Perfil_VR750";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        perfiles.Add(new BEperfil_750VR
                        {
                            CodPerfil_750VR = reader.GetInt32(0),
                            NombrePerfil_750VR = reader.GetString(1)
                        });
                    }
                }
            }

            return perfiles;
        }


        public bool AsignarPermiso(int codPerfil, int codPermiso)
        {
            if (codPerfil <= 0 || codPermiso <= 0)
                return false;

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string checkQuery = @"SELECT COUNT(*) 
                              FROM PerfilPermiso_VR750 
                              WHERE CodPerfil_VR750 = @CodPerfil AND CodPermiso_VR750 = @CodPermiso";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                    checkCmd.Parameters.AddWithValue("@CodPermiso", codPermiso);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                        return false; // Ya existe
                }

                string insertQuery = @"INSERT INTO PerfilPermiso_VR750 (CodPerfil_VR750, CodPermiso_VR750) 
                               VALUES (@CodPerfil, @CodPermiso)";
                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                    insertCmd.Parameters.AddWithValue("@CodPermiso", codPermiso);
                    insertCmd.ExecuteNonQuery();
                }

                return true;
            }
        }


        public List<IComponentePermiso_750VR> ObtenerPermisosDePerfilPorNombre(string nombrePerfil)
        {
            List<IComponentePermiso_750VR> permisos = new List<IComponentePermiso_750VR>();
            List<(int codPermiso, string nombre, bool esFamilia)> resultados = new List<(int, string, bool)>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
        SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750, p.EsFamilia_VR750
        FROM PerfilPermiso_VR750 pp
        INNER JOIN Permiso_VR750 p ON pp.CodPermiso_VR750 = p.CodPermiso_VR750
        INNER JOIN Perfil_VR750 perf ON perf.CodPerfil_VR750 = pp.CodPerfil_VR750
        WHERE perf.NombrePerfil_VR750 = @nombrePerfil";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombrePerfil", nombrePerfil);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultados.Add((
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetBoolean(2)
                            ));
                        }
                    }

                    foreach (var r in resultados)
                    {
                        if (r.esFamilia)
                        {
                            var familia = new GrupoPermiso_750VR(r.codPermiso, r.nombre);
                            CargarHijos(familia, conn);
                            permisos.Add(familia);
                        }
                        else
                        {
                            permisos.Add(new PermisoSimple_750VR(r.codPermiso, r.nombre));
                        }
                    }
                }
            }

            return permisos;
        }

        public List<IComponentePermiso_750VR> ObtenerHijosDeFamilia(int codFamilia)
        {
            List<IComponentePermiso_750VR> hijos = new List<IComponentePermiso_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750, p.EsFamilia_VR750
            FROM PermisoComposicion_VR750 pc
            INNER JOIN Permiso_VR750 p ON pc.HijoPermiso_VR750 = p.CodPermiso_VR750
            WHERE pc.PadrePermiso_VR750 = @codFamilia", conn);

                cmd.Parameters.AddWithValue("@codFamilia", codFamilia);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int cod = reader.GetInt32(0);
                        string nombre = reader.GetString(1);
                        bool esFamilia = reader.GetBoolean(2);

                        if (esFamilia)
                        {
                            hijos.Add(new GrupoPermiso_750VR(cod, nombre));
                        }
                        else
                        {
                            hijos.Add(new PermisoSimple_750VR(cod, nombre));
                        }

                    }
                }
            }

            return hijos;
        }
        public void EliminarFamilia(int codFamilia)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                // Primero eliminar asociaciones en PermisoComposicion
                string deleteComposicion = "DELETE FROM PermisoComposicion_VR750 WHERE PadrePermiso_VR750 = @CodFamilia OR HijoPermiso_VR750 = @CodFamilia";
                using (SqlCommand cmd = new SqlCommand(deleteComposicion, conn))
                {
                    cmd.Parameters.AddWithValue("@CodFamilia", codFamilia);
                    cmd.ExecuteNonQuery();
                }

                // Luego eliminar asociaciones con perfiles
                string deletePerfilPermiso = "DELETE FROM PerfilPermiso_VR750 WHERE CodPermiso_VR750 = @CodFamilia";
                using (SqlCommand cmd = new SqlCommand(deletePerfilPermiso, conn))
                {
                    cmd.Parameters.AddWithValue("@CodFamilia", codFamilia);
                    cmd.ExecuteNonQuery();
                }

                // Finalmente eliminar la familia
                string deletePermiso = "DELETE FROM Permiso_VR750 WHERE CodPermiso_VR750 = @CodFamilia";
                using (SqlCommand cmd = new SqlCommand(deletePermiso, conn))
                {
                    cmd.Parameters.AddWithValue("@CodFamilia", codFamilia);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<PermisoSimple_750VR> ObtenerPermisosSimples(IComponentePermiso_750VR componente)
        {
            List<PermisoSimple_750VR> permisos = new List<PermisoSimple_750VR>();

            if (componente is PermisoSimple_750VR simple)
            {
                permisos.Add(simple);
            }
            else if (componente is GrupoPermiso_750VR grupo)
            {
                foreach (var hijo in grupo.ObtenerHijos())
                {
                    permisos.AddRange(ObtenerPermisosSimples(hijo));
                }
            }

            return permisos;
        }

        public GrupoPermiso_750VR ObtenerFamiliaPorId(int id)
        {
            GrupoPermiso_750VR familia = null;

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string query = "SELECT CodPermiso_VR750, NombrePermiso_VR750 FROM Permiso_VR750 WHERE CodPermiso_VR750 = @id AND EsFamilia_VR750 = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            familia = new GrupoPermiso_750VR(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }

                if (familia != null)
                {
                    CargarHijos(familia, conn);
                }
            }

            return familia;
        }


        private List<IComponentePermiso_750VR> ObtenerHijos(int idPadre)
        {
            List<IComponentePermiso_750VR> hijos = new List<IComponentePermiso_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
            SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750, p.EsFamilia_VR750
            FROM PermisoComposicion_VR750 pc
            INNER JOIN Permiso_VR750 p ON pc.Hijo = p.CodPermiso_VR750
            WHERE pc.Padre = @idPadre";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idPadre", idPadre);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int codigo = reader.GetInt32(0);
                            string nombre = reader.GetString(1);
                            bool esFamilia = reader.GetBoolean(2);

                            if (esFamilia)
                            {
                                var familiaHija = new GrupoPermiso_750VR(codigo, nombre)
                                {
                                    Hijos = ObtenerHijos(codigo) // ← Recursivo
                                };
                                hijos.Add(familiaHija);
                            }
                            else
                            {
                                hijos.Add(new PermisoSimple_750VR(codigo, nombre));
                            }
                        }
                    }
                }
            }

            return hijos;
        }

        public bool AgregarPermisoAFamilia(int idFamilia, int idPermiso)
        {
            if (idFamilia == 0 || idPermiso == 0)
                return false;

            if (idFamilia == idPermiso)
                return false; // o lanzar una excepción si preferís

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM PermisoComposicion_VR750 WHERE PadrePermiso_VR750 = @Padre AND HijoPermiso_VR750 = @Hijo";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Padre", idFamilia);
                    checkCmd.Parameters.AddWithValue("@Hijo", idPermiso);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                        return false; // ya existe
                }

                string insertQuery = "INSERT INTO PermisoComposicion_VR750 (PadrePermiso_VR750, HijoPermiso_VR750) VALUES (@Padre, @Hijo)";
                using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@Padre", idFamilia);
                    insertCmd.Parameters.AddWithValue("@Hijo", idPermiso);
                    insertCmd.ExecuteNonQuery();
                }
            }

            return true;
        }




        public void QuitarPermisoDeFamilia(int idFamilia, int idPermiso)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"DELETE FROM PermisoComposicion_VR750
                         WHERE PadrePermiso_VR750 = @Padre AND HijoPermiso_VR750 = @Hijo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Padre", idFamilia);
                cmd.Parameters.AddWithValue("@Hijo", idPermiso);
                cmd.ExecuteNonQuery();
            }
        }
        //private DALperfil_750VR dalPerfil = new DALperfil_750VR();

        public bool AsignarFamiliaAFamilia(int idPadre, int idHija)
        {
            if (idPadre == idHija)
                throw new InvalidOperationException("No se puede agregar una familia a sí misma.");

            var familiaPadre = ObtenerFamiliaPorId(idPadre);
            var familiaHija = ObtenerFamiliaPorId(idHija);

            var permisosPadre = ObtenerPermisosSimples(familiaPadre);
            var permisosHija = ObtenerPermisosSimples(familiaHija);

            // Comparar si alguno de los permisos ya existe en la familia padre
            foreach (var permHijo in permisosHija)
            {
                if (permisosPadre.Any(p => p.Codigo_750VR == permHijo.Codigo_750VR))
                {
                    throw new InvalidOperationException($"El permiso '{permHijo.Nombre_750VR}' ya existe en esta familia.");
                }
            }

            // Si pasa la validación, lo insertás en BD
            return AgregarPermisoAFamilia(idPadre, idHija);
        }


        public void QuitarFamiliaDeFamilia(int idPadre, int idHija)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"DELETE FROM PermisoComposicion_VR750 
                         WHERE PadrePermiso_VR750 = @Padre AND HijoPermiso_VR750 = @Hijo";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Padre", idPadre);
                    cmd.Parameters.AddWithValue("@Hijo", idHija);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public List<PermisoSimple_750VR> ObtenerPermisosSimples()
        {
            List<PermisoSimple_750VR> lista = new List<PermisoSimple_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT CodPermiso_VR750, NombrePermiso_VR750 FROM Permiso_VR750 WHERE EsFamilia_VR750 = 0";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string nombre = reader.GetString(1);
                    lista.Add(new PermisoSimple_750VR(id, nombre));
                }
            }

            return lista;
        }

        public void QuitarPermiso(int codPerfil, int codPermiso)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"DELETE FROM PerfilPermiso_VR750 
                         WHERE CodPerfil_VR750 = @CodPerfil AND CodPermiso_VR750 = @CodPermiso";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CodPerfil", codPerfil);
                cmd.Parameters.AddWithValue("@CodPermiso", codPermiso);
                cmd.ExecuteNonQuery();
            }
        }
        private void CargarHijos(GrupoPermiso_750VR familia, SqlConnection conn)
        {
            string query = @"
    SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750, p.EsFamilia_VR750
    FROM PermisoComposicion_VR750 pc
    INNER JOIN Permiso_VR750 p ON pc.CodHijo_VR750 = p.CodPermiso_VR750
    WHERE pc.CodPadre_VR750 = @idPadre";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idPadre", familia.Codigo_750VR);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int cod = reader.GetInt32(0);
                        string nombre = reader.GetString(1);
                        bool esFamilia = reader.GetBoolean(2);

                        if (esFamilia)
                        {
                            var subFamilia = new GrupoPermiso_750VR(cod, nombre);
                            CargarHijos(subFamilia, conn); // recursivo
                            familia.Agregar(subFamilia);
                        }
                        else
                        {
                            familia.Agregar(new PermisoSimple_750VR(cod, nombre));
                        }
                    }
                }
            }
        }







    }
}
