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
        //public string Nombremanic_750VR { get; set; }
        public DateTime Fecha_750VR { get; set; }         // ✅ Solo fecha
        public TimeSpan HoraInicio_750VR { get; set; }    // ✅ Hora desde
        public TimeSpan HoraFin_750VR { get; set; }       // ✅ Hora hasta
        public bool activo_750VR { get; set; }
        public bool estado_750VR { get; set; }

    }
}
