using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class Usuario_750VR
    {
        public int dni { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        //sacar telefono de la bd
        public string mail { get; set; }
        public string user { get; set; }
        public string contraseña { get; set; }
        public string rol { get; set; }
        public string estado { get; set; }


        public Usuario_750VR(int dni, string nombre, string ape, string mail,string rol, string estado)
        {
            this.dni = dni;
            this.nombre = nombre;
            this.apellido = ape;
            this.mail = mail;
            this.user = dni + ape;
            this.contraseña = dni + nombre;
            this.rol = rol;
            this.estado = estado;


        }

        public Usuario_750VR()
        {
                
        }


    }
}
