using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class BEdisponibilidad_750VR
    {
        public int IdDisponibilidad { get; set; }
        public int DNIempleado { get; set; }             // FK a Empleado o Usuario
        public int DiaSemana { get; set; }              // 1 = Lunes, ..., 7 = Domingo
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public bool Activo { get; set; }
        public bool estado { get; set; }

        // Opcional: para mostrar el nombre del día en pantalla
        public string NombreDia => ObtenerNombreDia(DiaSemana);

        private string ObtenerNombreDia(int dia)
        {
            switch (dia)
            {
                case 1: return "Lunes";
                case 2: return "Martes";
                case 3: return "Miércoles";
                case 4: return "Jueves";
                case 5: return "Viernes";
                case 6: return "Sábado";
                case 7: return "Domingo";
                default: return "Desconocido";
            }
        }


    }
}
