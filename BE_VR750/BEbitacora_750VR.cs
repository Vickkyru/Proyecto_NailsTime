using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class BEbitacora_750VR
    {
        public int Id_Evento { get; set; }
        public string Login { get; set; }
        public DateTime Fecha { get; set; }        // solo fecha
        public TimeSpan Hora { get; set; }         // solo hora
        public string Modulo { get; set; }
        public string Evento { get; set; }
        public byte Criticidad { get; set; }       // 1..5

        // Opcionalmente, para la GUI:
     

        public BEbitacora_750VR(
            int idEvento,
            string login,
            DateTime fecha,
            TimeSpan hora,
            string modulo,
            string evento,
            byte criticidad)
        {
            Id_Evento = idEvento;
            Login = login;
            Fecha = fecha;
            Hora = hora;
            Modulo = modulo;
            Evento = evento;
            Criticidad = criticidad;
        
        }

    }
}
