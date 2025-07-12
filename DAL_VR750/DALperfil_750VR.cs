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
        public List<BEperfil_750VR> ObtenerPerfiles()
        {
            var lista = new List<BEperfil_750VR>();
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT CodPerfil_VR750, NombrePerfil_VR750 FROM Perfil_VR750", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new BEperfil_750VR
                        {
                            CodPerfil_750VR = reader.GetInt32(0),
                            NombrePerfil_750VR = reader.GetString(1)
                        });
                    }
                }
            }
            return lista;
        }

        public int InsertarPerfilYDevolverID(BEperfil_750VR perfil)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Perfil_VR750 (NombrePerfil_VR750) OUTPUT INSERTED.CodPerfil_VR750 VALUES (@Nombre)", conn);
                cmd.Parameters.AddWithValue("@Nombre", perfil.NombrePerfil_750VR);
                return (int)cmd.ExecuteScalar();
            }
        }

        public List<BEperfil_750VR> ObtenerTodosLosPerfiles() => ObtenerPerfiles();

        public void EliminarPerfil(int id)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Perfil_VR750 WHERE CodPerfil_VR750 = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public bool AsignarPermiso(int idPerfil, int idPermiso)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    IF NOT EXISTS (
                        SELECT 1 FROM PerfilXPermiso_VR750
                        WHERE CodPerfil_VR750 = @idPerfil AND CodPermiso_VR750 = @idPermiso
                    )
                    BEGIN
                        INSERT INTO PerfilXPermiso_VR750 (CodPerfil_VR750, CodPermiso_VR750)
                        VALUES (@idPerfil, @idPermiso)
                    END", conn);
                cmd.Parameters.AddWithValue("@idPerfil", idPerfil);
                cmd.Parameters.AddWithValue("@idPermiso", idPermiso);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public void AsignarFamilia(int idPerfil, int idFamilia)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    IF NOT EXISTS (
                        SELECT 1 FROM PerfilXFamilia_VR750
                        WHERE CodPerfil_VR750 = @idPerfil AND CodFamilia_VR750 = @idFamilia
                    )
                    BEGIN
                        INSERT INTO PerfilXFamilia_VR750 (CodPerfil_VR750, CodFamilia_VR750)
                        VALUES (@idPerfil, @idFamilia)
                    END", conn);
                cmd.Parameters.AddWithValue("@idPerfil", idPerfil);
                cmd.Parameters.AddWithValue("@idFamilia", idFamilia);
                cmd.ExecuteNonQuery();
            }
        }

        public List<IComponentePermiso_750VR> ObtenerPermisosDePerfil(int idPerfil)
        {
            var lista = new List<IComponentePermiso_750VR>();
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                SqlCommand cmd1 = new SqlCommand(@"
                    SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750
                    FROM PerfilXPermiso_VR750 pxp
                    JOIN Permiso_VR750 p ON pxp.CodPermiso_VR750 = p.CodPermiso_VR750
                    WHERE pxp.CodPerfil_VR750 = @id", conn);
                cmd1.Parameters.AddWithValue("@id", idPerfil);
                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new PermisoSimple_750VR(reader.GetInt32(0), reader.GetString(1)));
                    }
                }

                SqlCommand cmd2 = new SqlCommand(@"
                    SELECT f.CodFamilia_VR750, f.NombreFamilia_VR750
                    FROM PerfilXFamilia_VR750 pf
                    JOIN Familia_VR750 f ON pf.CodFamilia_VR750 = f.CodFamilia_VR750
                    WHERE pf.CodPerfil_VR750 = @id", conn);
                cmd2.Parameters.AddWithValue("@id", idPerfil);
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new GrupoPermiso_750VR(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            return lista;
        }

        public List<PermisoSimple_750VR> ObtenerPermisosSimples()
        {
            var lista = new List<PermisoSimple_750VR>();
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT CodPermiso_VR750, NombrePermiso_VR750 FROM Permiso_VR750", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new PermisoSimple_750VR(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            return lista;
        }

        public List<GrupoPermiso_750VR> ObtenerFamilias()
        {
            List<GrupoPermiso_750VR> familias = new List<GrupoPermiso_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = "SELECT CodFamilia_VR750, NombreFamilia_VR750 FROM Familia_VR750";
                SqlCommand cmd = new SqlCommand(query, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var familia = new GrupoPermiso_750VR(
                            reader.GetInt32(0),
                            reader.GetString(1)
                        );
                        familias.Add(familia);
                    }
                }
            }

            return familias;
        }


        public List<IComponentePermiso_750VR> ObtenerHijosDeFamilia(int idFamilia)
        {
            var lista = new List<IComponentePermiso_750VR>();
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                SqlCommand cmd1 = new SqlCommand(@"
                    SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750
                    FROM PermisoXFamilia_VR750 pf
                    JOIN Permiso_VR750 p ON pf.CodPermiso_VR750 = p.CodPermiso_VR750
                    WHERE pf.CodFamilia_VR750 = @id", conn);
                cmd1.Parameters.AddWithValue("@id", idFamilia);
                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new PermisoSimple_750VR(reader.GetInt32(0), reader.GetString(1)));
                    }
                }

                SqlCommand cmd2 = new SqlCommand(@"
                    SELECT f.CodFamilia_VR750, f.NombreFamilia_VR750
                    FROM FamiliaXFamilia_VR750 ff
                    JOIN Familia_VR750 f ON ff.CodFamiliaHija_VR750 = f.CodFamilia_VR750
                    WHERE ff.CodFamiliaPadre_VR750 = @id", conn);
                cmd2.Parameters.AddWithValue("@id", idFamilia);
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new GrupoPermiso_750VR(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            return lista;
        }

        public void QuitarPermiso(int idPerfil, int idPermiso)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM PerfilXPermiso_VR750 WHERE CodPerfil_VR750 = @p AND CodPermiso_VR750 = @q", conn);
                cmd.Parameters.AddWithValue("@p", idPerfil);
                cmd.Parameters.AddWithValue("@q", idPermiso);
                cmd.ExecuteNonQuery();
            }
        }

        public void QuitarFamilia(int idPerfil, int idFamilia)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM PerfilXFamilia_VR750 WHERE CodPerfil_VR750 = @p AND CodFamilia_VR750 = @q", conn);
                cmd.Parameters.AddWithValue("@p", idPerfil);
                cmd.Parameters.AddWithValue("@q", idFamilia);
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertarFamilia(string nombre)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Familia_VR750 (NombreFamilia_VR750) VALUES (@n)", conn);
                cmd.Parameters.AddWithValue("@n", nombre);
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarFamilia(int codFamilia)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Familia_VR750 WHERE CodFamilia_VR750 = @id", conn);
                cmd.Parameters.AddWithValue("@id", codFamilia);
                cmd.ExecuteNonQuery();
            }
        }

        public bool AgregarPermisoAFamilia(int idFamilia, int idPermiso)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    IF NOT EXISTS (
                        SELECT 1 FROM PermisoXFamilia_VR750
                        WHERE CodFamilia_VR750 = @f AND CodPermiso_VR750 = @p
                    )
                    BEGIN
                        INSERT INTO PermisoXFamilia_VR750 (CodFamilia_VR750, CodPermiso_VR750)
                        VALUES (@f, @p)
                    END", conn);
                cmd.Parameters.AddWithValue("@f", idFamilia);
                cmd.Parameters.AddWithValue("@p", idPermiso);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public void QuitarPermisoDeFamilia(int idFamilia, int idPermiso)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM PermisoXFamilia_VR750 WHERE CodFamilia_VR750 = @f AND CodPermiso_VR750 = @p", conn);
                cmd.Parameters.AddWithValue("@f", idFamilia);
                cmd.Parameters.AddWithValue("@p", idPermiso);
                cmd.ExecuteNonQuery();
            }
        }

        public void AsignarFamiliaAFamilia(int idPadre, int idHija)
        {
            if (idPadre == idHija)
                throw new InvalidOperationException("Una familia no puede ser hija de sí misma.");

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    IF NOT EXISTS (
                        SELECT 1 FROM FamiliaXFamilia_VR750
                        WHERE CodFamiliaPadre_VR750 = @padre AND CodFamiliaHija_VR750 = @hija
                    )
                    BEGIN
                        INSERT INTO FamiliaXFamilia_VR750 (CodFamiliaPadre_VR750, CodFamiliaHija_VR750)
                        VALUES (@padre, @hija)
                    END", conn);
                cmd.Parameters.AddWithValue("@padre", idPadre);
                cmd.Parameters.AddWithValue("@hija", idHija);
                cmd.ExecuteNonQuery();
            }
        }

        public void QuitarFamiliaDeFamilia(int idPadre, int idHija)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM FamiliaXFamilia_VR750 WHERE CodFamiliaPadre_VR750 = @padre AND CodFamiliaHija_VR750 = @hija", conn);
                cmd.Parameters.AddWithValue("@padre", idPadre);
                cmd.Parameters.AddWithValue("@hija", idHija);
                cmd.ExecuteNonQuery();
            }
        }

        public GrupoPermiso_750VR ObtenerFamiliaPorId(int idFamilia)
        {
            GrupoPermiso_750VR familia = null;

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();

                // 1. Obtener la familia principal
                SqlCommand cmdFamilia = new SqlCommand(
                    "SELECT CodFamilia_VR750, NombreFamilia_VR750 FROM Familia_VR750 WHERE CodFamilia_VR750 = @id", conn);
                cmdFamilia.Parameters.AddWithValue("@id", idFamilia);

                using (SqlDataReader reader = cmdFamilia.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        familia = new GrupoPermiso_750VR(reader.GetInt32(0), reader.GetString(1));
                    }
                }

                if (familia == null)
                    return null;

                // 2. Cargar permisos simples asignados a la familia
                SqlCommand cmdPermisos = new SqlCommand(@"
            SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750
            FROM Permiso_VR750 p
            JOIN PermisoXFamilia_VR750 pf ON pf.CodPermiso_VR750 = p.CodPermiso_VR750
            WHERE pf.CodFamilia_VR750 = @id", conn);
                cmdPermisos.Parameters.AddWithValue("@id", idFamilia);

                using (SqlDataReader readerPerm = cmdPermisos.ExecuteReader())
                {
                    while (readerPerm.Read())
                    {
                        var permiso = new PermisoSimple_750VR(readerPerm.GetInt32(0), readerPerm.GetString(1));
                        familia.Agregar(permiso);
                    }
                }

                // 3. (Opcional) Cargar familias hijas si estás usando FamiliaXFamilia_VR750
                SqlCommand cmdFamiliasHijas = new SqlCommand(@"
            SELECT f.CodFamilia_VR750, f.NombreFamilia_VR750
            FROM Familia_VR750 f
            JOIN FamiliaXFamilia_VR750 ff ON ff.CodFamiliaHija_VR750 = f.CodFamilia_VR750
            WHERE ff.CodFamiliaPadre_VR750 = @id", conn);
                cmdFamiliasHijas.Parameters.AddWithValue("@id", idFamilia);

                using (SqlDataReader readerHijas = cmdFamiliasHijas.ExecuteReader())
                {
                    while (readerHijas.Read())
                    {
                        // Llamada recursiva para armar también las hijas con sus hijos
                        var familiaHija = ObtenerFamiliaPorId(readerHijas.GetInt32(0));
                        if (familiaHija != null)
                            familia.Agregar(familiaHija);
                    }
                }
            }

            return familia;
        }
        public List<PermisoSimple_750VR> ObtenerPermisosSimplesPorFamilia(int codFamilia)
        {
            List<PermisoSimple_750VR> lista = new List<PermisoSimple_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                string query = @"
            SELECT p.CodPermiso_VR750, p.NombrePermiso_VR750
            FROM PermisoXFamilia_VR750 pf
            JOIN Permiso_VR750 p ON pf.CodPermiso_VR750 = p.CodPermiso_VR750
            WHERE pf.CodFamilia_VR750 = @cod";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cod", codFamilia);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new PermisoSimple_750VR(
                        reader.GetInt32(0),         // Código permiso
                        reader.GetString(1)         // Nombre permiso
                    ));
                }
            }

            return lista;
        }

        public List<GrupoPermiso_750VR> ObtenerFamiliasHijas(int codFamilia)
        {
            var lista = new List<GrupoPermiso_750VR>();

            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            SELECT f.CodFamilia_VR750, f.NombreFamilia_VR750
            FROM Familia_VR750 f
            INNER JOIN FamiliaXFamilia_VR750 ff ON f.CodFamilia_VR750 = ff.CodFamiliaHija_VR750
            WHERE ff.CodFamiliaPadre_VR750 = @codFamilia", conn);

                cmd.Parameters.AddWithValue("@codFamilia", codFamilia);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new GrupoPermiso_750VR(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }

            return lista;
        }


        public List<IComponentePermiso_750VR> ObtenerPermisosDePerfilPorNombre(string nombrePerfil)
        {
            using (SqlConnection conn = new SqlConnection(BaseDeDatos_750VR.cadena))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT CodPerfil_VR750 FROM Perfil_VR750 WHERE NombrePerfil_VR750 = @n", conn);
                cmd.Parameters.AddWithValue("@n", nombrePerfil);
                object result = cmd.ExecuteScalar();
                if (result != null)
                    return ObtenerPermisosDePerfil((int)result);
            }
            return new List<IComponentePermiso_750VR>();
        }

    }
}
