using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class BECliente_750VR
    {

        public int dni_750VR {  get; set; }
        public string nombre_750VR { get; set; }
        public string apellido_750VR { get; set; }
        public string gmail_750VR { get; set; }
        public string direccion_750VR { get; set; }
        public string celular_750VR { get; set; }
        public bool activo_750VR { get; set; }

        public BECliente_750VR(int dni, string nom, string ape, string gmail, string dire, string celu, bool act)
        {
                this.dni_750VR = dni;
            this.nombre_750VR = nom;
            this.apellido_750VR = ape;
            this.gmail_750VR = gmail;
            this.direccion_750VR = dire;
            this.celular_750VR = celu;
            this.activo_750VR = act;

        }
   

    }
}
