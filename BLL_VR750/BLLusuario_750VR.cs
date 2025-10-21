using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_VR750;
using BE_VR750;
using SERVICIOS_VR750;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;


//hacer mejor con la interfaz
namespace BLL_VR750
{
    public class BLLusuario_750VR
    {
        DALusuario_750VR dal;

        public BLLusuario_750VR()
        {
          dal = new DALusuario_750VR();
        }
        private readonly BLLbitacora_750VR _log = new BLLbitacora_750VR();
        Encriptador_750VR encriptador = new Encriptador_750VR();

        public List<BEusuario_750VR> ObtenerManicuristasActivos_750VR()
        {
            var lista = leerEntidades_750VR();
            return lista.Where(u => u.rol_750VR.ToLower() == "manicurista" && u.activo_750VR).ToList();
        }

        public BE_VR750.BEusuario_750VR AutenticarEIniciarSesion_750VR(
      string login, string password,
      out bool esAdmin, out bool requiereReparacion)
        {
            requiereReparacion = false;
            esAdmin = false;

            // 1) Credenciales
            var usuario = dal.recuperarUsuario_750VR(login, password);
            if (usuario == null)
                throw new Exception(Lenguaje_750VR.ObtenerEtiqueta("Login.Mensaje.CredencialesInvalidas"));

            // 2) ¿Es admin? -> según rol_750VR
            esAdmin = (!string.IsNullOrWhiteSpace(usuario.rol_750VR) &&
                       usuario.rol_750VR.Equals("Administrador", StringComparison.OrdinalIgnoreCase));

            // 3) Detección DV (sin persistir)
            var gen = DVService_750VR.GenerarDV_BD_SinPersistir();
            var db = DVService_750VR.LeerDV_Persistido();
            requiereReparacion = (gen.DVH_DB != db.DVH_DB) || (gen.DVV_DB != db.DVV_DB);

            if (requiereReparacion && !esAdmin)
                throw new Exception(Lenguaje_750VR.ObtenerEtiqueta("Login.Mensaje.ProblemaSistemaContacteAdmin"));

            // 4) Idioma de sesión
            SessionManager_750VR.IdiomaActual =
                string.IsNullOrWhiteSpace(usuario.idioma_750VR) ? "Español" : usuario.idioma_750VR;

            // 5) Iniciar sesión
            var sesionOK = SessionManager_750VR.ObtenerInstancia.IniciarSesion_750VR(usuario);
            if (!sesionOK)
                throw new Exception(Lenguaje_750VR.ObtenerEtiqueta("Login.Mensaje.SesionActiva"));

            // 6) Bitácora
            _log.LoginOK();

            return usuario;
        }

        public void Logout_750VR()
        {
            // 1) loguear en bitácora antes de cerrar
            _log.Logout();

            // 2) cerrar sesión en SessionManager
            SessionManager_750VR.ObtenerInstancia.CerrarSesion_750VR();

            // 3) resetear idioma por defecto
            SessionManager_750VR.IdiomaActual = "Español";
        }

        public void CrearUsuario_750VR(BEusuario_750VR usuario)
        {
     
          
           dal.CrearUsuario_750VR(usuario);
            _log.CrearUsuario(usuario.dni_750VR);   // la BLL registra, no el Form
        }

        public bool ModificarUsuario_750VR(int dni, string nombre, string apellido, string mail, string rol, string usuario)
        {
            var ok = dal.ModificarUsuario_750VR(dni, nombre, apellido, mail, rol, usuario);
            if (ok) _log.ModificarUsuario(dni);
            return ok;
        }

        public bool CambiarEstadoUsuario_750VR(int dni, bool nuevoEstado)
        {
            var ok = dal.CambiarEstadoUsuario_750VR(dni, nuevoEstado);
            if (ok)
            {
                if (nuevoEstado)
                    _log.ActivarUsuario(dni);
                else
                    _log.DesactivarUsuario(dni);
            }
            return ok;
        }

        public bool DesbloquearUsuario_750VR(int dni)
        {

            var ok = dal.DesbloquearUsuario_750VR(dni);
            if (ok) _log.DesbloquearUsuario(dni);
            return ok;
        }

 
        public List<BEusuario_750VR> BuscarUsuarios_750VR(string dni, string nombre, string apellido, string email, string user, string rol)
        {
            return dal.BuscarUsuarios_750VR(dni, nombre, apellido, email,user,rol);
        }

        public List<BEusuario_750VR> leerEntidades_750VR()
        {
            return dal.leerEntidades_750VR();
        }

        public bool ExisteUsuarioConLoginODNI(string usuarioLogin, int dni)
        {
            
            return dal.ExisteUsuarioPorLoginYDNI(usuarioLogin, dni);
        }



        public void CambiarContraseña_750VR(BEusuario_750VR usuario, string NuevaContraseña)
        {
            dal.CambiarContraseña_750VR(usuario, NuevaContraseña);
            _log.CambioClave(); // registra bitácora usando el login del user que cambió su clave
        }

        public BEusuario_750VR ObtenerUsuarioPorDNI_750VR(int dni)
        {
            return dal.ObtenerUsuarioPorDNI_750VR(dni);
        }

       
        public BEusuario_750VR recuperarUsuario_750VR(string user, string contraseña)
        {
            return dal.recuperarUsuario_750VR(user, contraseña);
        }

        public void BloquearUsuario_750VR(string login)
        {
            dal.BloquearUsuario_750VR(login);
            
        }

        public void ModificarIdiomaUsuario_750VR(string login, string idioma)
        {
           
            dal.ActualizarIdiomaUsuario_750VR(login, idioma);
        }
    }
}
