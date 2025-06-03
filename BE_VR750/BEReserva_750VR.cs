using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public  class BEReserva_750VR
    {
  
        public int IdReserva_750VR { get; set; }
        public int DNIcli_750VR { get; set; }

        public BECliente_750VR cliente { get; set; }

        public int DNImanic_750VR { get; set; }
        public BEusuario_750VR manic { get; set; }

        public int IdServicio_750VR { get; set; }
        public BEServicio_750VR serv { get; set; }

        public DateTime Fecha_750VR { get; set; }
        public TimeSpan HoraInicio_750VR { get; set; }
        public TimeSpan HoraFin_750VR { get; set; }
        public decimal Precio_750VR { get; set; }

        public string Estado_750VR { get; set; }  // "Pendiente", "Realizado", "Cancelado"
        public bool Cobrado_750VR { get; set; }

    }
}
