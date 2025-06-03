using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class BEServicio_750VR
    {
        public int idServicio_750VR { get;set;}
        public string nombre_750VR { get;set;}
        public string tecnica_750VR { get;set;}
        
        public int duracion_750VR { get;set; } // En minutos
        public decimal precio_750VR { get;set;}
        public bool activo_750VR { get; set; }

        public BEServicio_750VR(int id, string nom, string tec, int dur, decimal pre, bool act)
        {
                this.idServicio_750VR = id;
            this.nombre_750VR = nom;
            this.tecnica_750VR = tec;
            this.duracion_750VR = dur;
            this.precio_750VR = pre;
            this.activo_750VR = act;
        }
        public BEServicio_750VR(string nom, string tec, int dur, decimal pre, bool act)
        {
            this.nombre_750VR = nom;
            this.tecnica_750VR = tec;
            this.duracion_750VR = dur;
            this.precio_750VR = pre;
            this.activo_750VR = act;
        }
    }
}
