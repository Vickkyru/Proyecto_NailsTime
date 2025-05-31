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

        Encriptador_750VR encriptador = new Encriptador_750VR();

        public List<BEusuario_750VR> ObtenerManicuristasActivos_750VR()
        {
            var lista = leerEntidades_750VR();
            return lista.Where(u => u.rol_750VR.ToLower() == "manicurista" && u.activo_750VR).ToList();
        }

        public void CrearUsuario_750VR(BEusuario_750VR usuario)
        {
     
          
           dal.CrearUsuario_750VR(usuario);
        }

        public bool ModificarUsuario_750VR(int dni, string nombre, string apellido, string mail, string rol, string usuario)
        {

            return dal.ModificarUsuario_750VR(dni, nombre, apellido, mail, rol, usuario);
        }

        public bool CambiarEstadoUsuario_750VR(int dni, bool nuevoEstado)
        {
            return dal.CambiarEstadoUsuario_750VR(dni, nuevoEstado);
        }

        public bool DesbloquearUsuario_750VR(int dni)
        {
            
            return dal.DesbloquearUsuario_750VR(dni); // le pasa el dni
        }

 
        public List<BEusuario_750VR> BuscarUsuarios_750VR(string dni, string nombre, string apellido, string email, string user, string rol)
        {
            return dal.BuscarUsuarios_750VR(dni, nombre, apellido, email,user,rol);
        }

        public List<BEusuario_750VR> leerEntidades_750VR()
        {
            return dal.leerEntidades_750VR();
        }

        public BEusuario_750VR ObtenerUsuarioPorLogin_750VR(string login)
        {
            return dal.ObtenerUsuarioPorLogin_750VR(login);
        }

    
        public void CambiarContraseña_750VR(BEusuario_750VR usuario, string NuevaContraseña)
        {
            dal.CambiarContraseña_750VR(usuario,NuevaContraseña);
        }

        public BEusuario_750VR ObtenerUsuarioPorDNI_750VR(int dni)
        {
            return dal.ObtenerUsuarioPorDNI_750VR(dni);
        }

       
        public BEusuario_750VR Login_750VR(string user, string contraseña)
        {
            return dal.recuperarUsuario_750VR(user, contraseña);
        }

        public void BloquearUsuario_750VR(string login)
        {
            dal.BloquearUsuario_750VR(login);
        }
    }
}
