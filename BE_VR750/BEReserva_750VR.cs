using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public  class BEReserva_750VR
    {
        //public int idReserva_750VR { get; set; }

        //public int dniCli_750VR { get; set; }
        //public string nombreCli_750VR { get; set; }
        //public int duracionServ_750VR { get; set; }
        //public decimal precioServ_750VR { get; set; }
        //public string nomManic_750VR { get; set; }
        //public string dateManic_750VR { get; set; }

        //public bool activo_750VR { get; set; }
        //public bool estado_750VR { get; set; }




        public int IdReserva { get; set; }
        public int DNIcli { get; set; }

        public BECliente_750VR cliente { get; set; }

        public int DNImanic { get; set; }
        public BEusuario_750VR manic { get; set; }

        public int IdServicio { get; set; }
        public BEServicio_750VR serv { get; set; }

        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public decimal Precio { get; set; }

        public bool Estado { get; set; } /*= "Pendiente";*/ 
        public bool Cobrado { get; set; }

    }
}
