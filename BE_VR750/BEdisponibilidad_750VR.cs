using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class BEdisponibilidad_750VR
    {
        public int IdDisponibilidad_750VR { get; set; }
        public int DNImanic_750VR { get; set; }
        //public BEusuario_750VR manic {  get; set; }
        public string Nombremanic_750VR { get; set; }
        public string DiaSemana_750VR { get; set; }            
        public TimeSpan HoraInicio_750VR { get; set; }
        public TimeSpan HoraFin_750VR { get; set; }
        public bool activo_750VR { get; set; }
        public bool estado_750VR { get; set; }

    }
}
