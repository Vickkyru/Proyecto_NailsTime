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
        public int DNIcli { get; set; }              // FK a Cliente
        public int DNImanic { get; set; }             // FK a Empleado
        public int IdServicio { get; set; }             // FK a Servicio

        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public int DuracionMinutos { get; set; }
        public decimal Precio { get; set; }

        public bool Estado { get; set; } /*= "Pendiente";*/ // Opcional: usar enum
        public bool activo_750VR { get; set; }

        // Propiedades extra para mostrar datos relacionados (no mapeadas directamente)
        public string NombreCliente { get; set; }
        public string NombreManic { get; set; }
        public string NombreServicio { get; set; }

        // Propiedad calculada
        public TimeSpan HoraFin => HoraInicio.Add(TimeSpan.FromMinutes(DuracionMinutos));
    }
}
