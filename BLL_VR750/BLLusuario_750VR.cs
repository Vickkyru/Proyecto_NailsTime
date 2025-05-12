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


        //public void CrearUsuario(Usuario_750VR usuario)
        //{

        //    dal.CrearUsuario(usuario);
        //}
        public void CrearUsuario(string dniStr, string nombre, string apellido, string mail, string rol)
        {
            if (!int.TryParse(dniStr, out int dni))
                throw new Exception("El DNI ingresado no es válido.");

            Usuario_750VR nuevoUsuario = new Usuario_750VR();
            nuevoUsuario.dni = dni;
            nuevoUsuario.nombre = nombre;
            nuevoUsuario.apellido = apellido;
            nuevoUsuario.mail = mail;

            // Login y contraseña predeterminada
            nuevoUsuario.user = $"{mail}";
            string contraseña = $"{dni}{nombre}";

            // Encriptar
            nuevoUsuario.salt = SERVICIOS_VR750.Encriptador_VR750.GenerarSalt();
            nuevoUsuario.contraseña = SERVICIOS_VR750.Encriptador_VR750.HashearConSalt(contraseña, nuevoUsuario.salt);

            // Otros datos
            nuevoUsuario.rol = rol;
            nuevoUsuario.activo = true;
            nuevoUsuario.bloqueado = false;

            // Llamar a DAL
          
            dal.CrearUsuario(nuevoUsuario);
        }

        public bool ModificarUsuario(int dni, string nombre, string apellido, string mail, string rol, bool activo)
        {

            return dal.ModificarUsuario(dni, nombre, apellido, mail, rol, activo);
        }

        public bool CambiarEstadoUsuario(int dni, bool nuevoEstado)
        {
            return dal.CambiarEstadoUsuario(dni, nuevoEstado);
        }

        public bool DesbloquearUsuario(int dni)
        {
            
            return dal.DesbloquearUsuario(dni); // le pasa el dni
        }

        public List<Usuario_750VR> ObtenerUsuarios(bool soloActivos) 
        {
            return dal.ObtenerUsuarios(soloActivos);
        }

        public List<Usuario_750VR> BuscarUsuarios(string dni, string nombre, string apellido, string email)
        {
            return dal.BuscarUsuarios(dni, nombre, apellido, email);
        }

        public List<Usuario_750VR> leerEntidades()
        {
            return dal.leerEntidades();
        }

        private static int intentosFallidos = 0;

        public string Login(string usuarioLogin, string contraseñaIngresada)
        {
            Usuario_750VR user = dal.ObtenerUsuarioPorLogin(usuarioLogin);

            if (user == null)
                return "Usuario no encontrado.";

            if (!user.activo)
                return "Cuenta inactiva. Contacte al administrador.";

            if (user.bloqueado)
                return "Cuenta bloqueada. Contacte al administrador.";

            string hashIngresado = Encriptador_VR750.HashearConSalt(contraseñaIngresada, user.salt);

            if (hashIngresado == user.contraseña)
            {
                intentosFallidos = 0;

                bool sesionIniciada = SessionManager_VR750.ObtenerInstancia.IniciarSesion(user);
                if (!sesionIniciada)
                    return "Ya hay una sesión activa.";

                return "Login exitoso.";
            }
            else
            {
                intentosFallidos++;
                if (intentosFallidos >= 3)
                {
                    dal.BloquearUsuario(usuarioLogin);
                    return "Cuenta bloqueada tras 3 intentos fallidos.";
                }

                return $"Contraseña incorrecta. Intentos fallidos: {intentosFallidos}";
            }

        }

        //public void Logout()
        //{
        //    if (SessionManager_VR750.Instancia.EstaLogueado())
        //        SessionManager_VR750.Instancia.CerrarSesion();
        //}
        private void BloquearUsuario(string usuarioLogin)
        {
            dal.BloquearUsuario(usuarioLogin);
        }

       

        public Usuario_750VR ObtenerUsuarioPorDNI(int dni)
        {
            return dal.ObtenerUsuarioPorDNI(dni);
        }

       
        public Resultado_VR750<Usuario_750VR> recuperarUsuario(string user, string contraseña)
        {
            return dal.recuperarUsuario(user, contraseña);
        }
    }
}
