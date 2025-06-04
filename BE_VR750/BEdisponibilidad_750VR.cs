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
        public DateTime Fecha_750VR { get; set; }         // ✅ Solo fecha
        public TimeSpan HoraInicio_750VR { get; set; }    // ✅ Hora desde
        public TimeSpan HoraFin_750VR { get; set; }       // ✅ Hora hasta
        public bool activo_750VR { get; set; }
        public bool estado_750VR { get; set; } // "Pendiente", "Realizado", "Cancelado"


        public BEdisponibilidad_750VR( int dni, DateTime fecha, TimeSpan ini, TimeSpan fin, bool acr, bool est)
        {
                this.DNImanic_750VR = dni;
            this.Fecha_750VR = fecha;
            this.HoraInicio_750VR = ini;
            this.HoraFin_750VR = fin;
            this.activo_750VR = acr;
            this.estado_750VR = est;
        }


        public BEdisponibilidad_750VR(int id,int dni, DateTime fecha, TimeSpan ini, TimeSpan fin, bool acr, bool est)
        {
            this.IdDisponibilidad_750VR = id;
            this.DNImanic_750VR = dni;
            this.Fecha_750VR = fecha;
            this.HoraInicio_750VR = ini;
            this.HoraFin_750VR = fin;
            this.activo_750VR = acr;
            this.estado_750VR = est;
        }

    }
}
