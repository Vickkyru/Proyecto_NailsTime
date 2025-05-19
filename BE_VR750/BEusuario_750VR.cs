using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class BEusuario_750VR
    {
        public int dni_750VR { get; set; }
        public string nombre_750VR { get; set; }
        public string apellido_750VR { get; set; }
        public string mail_750VR { get; set; }
        public string user_750VR { get; set; }
        public string contraseña_750VR { get; set; }
        public string salt_750VR { get; set; }
        public string rol_750VR { get; set; }
        public bool activo_750VR { get; set; }
        public bool bloqueado_750VR { get; set; }


        public BEusuario_750VR(int dni, string nombre, string ape, string mail,string user,string contra, string salt, string rol, bool activo, bool bloqueado)
        {
            this.dni_750VR = dni;
            this.nombre_750VR = nombre;
            this.apellido_750VR = ape;
            this.mail_750VR = mail;
            this.user_750VR = user;
            this.contraseña_750VR = contra;
            this.salt_750VR = salt;
            this.rol_750VR = rol;
            this.activo_750VR = activo;
            this.bloqueado_750VR = bloqueado;

        }

        public BEusuario_750VR()
        {
               
            
        }

        public BEusuario_750VR(int dni, string nombre, string ape, string mail,string salt, string rol, string user, bool activo, bool bloqueado)
        {
            this.dni_750VR = dni;
            this.nombre_750VR = nombre;
            this.apellido_750VR = ape;
            this.mail_750VR = mail;
            this.user_750VR = user;
            this.rol_750VR = rol;
            this.activo_750VR = activo;
            this.bloqueado_750VR = bloqueado;
        }


    }
}
