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

        Encriptador_VR750 encriptador = new Encriptador_VR750();

     
        public void CrearUsuario(Usuario_750VR usuario)
        {
     
          
           dal.CrearUsuario(usuario);
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

        public List<Usuario_750VR> BuscarUsuarios(string dni, string nombre, string apellido, string email, string user, string rol)
        {
            return dal.BuscarUsuarios(dni, nombre, apellido, email,user,rol);
        }

        public List<Usuario_750VR> leerEntidades()
        {
            return dal.leerEntidades();
        }

        public Usuario_750VR ObtenerUsuarioPorLogin(string login)
        {
            return dal.ObtenerUsuarioPorLogin(login);
        }

    
        public void CambiarContraseña(Usuario_750VR usuario, string NuevaContraseña)
        {
            dal.CambiarContraseña(usuario,NuevaContraseña);
        }

        public Usuario_750VR ObtenerUsuarioPorDNI(int dni)
        {
            return dal.ObtenerUsuarioPorDNI(dni);
        }

       
        public Resultado_VR750<Usuario_750VR> Login(string user, string contraseña)
        {
            return dal.recuperarUsuario(user, contraseña);
        }

        public void BloquearUsuario(string login)
        {
            dal.BloquearUsuario(login);
        }
    }
}
