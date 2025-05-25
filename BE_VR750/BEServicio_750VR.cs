using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public class BEServicio_750VR
    {
        public int id_750VR { get;set;}
        public string nombre_750VR { get;set;}
        public string tecnica_750VR { get;set;}
        
        public int duracion_750VR { get;set; } // En minutos
        public decimal precio_750VR { get;set;}
        public bool activo_750VR { get; set; }

        // Propiedad útil para mostrar en ComboBox, etc.
        public string DescripcionCompleta_750VR
        {
            get
            {
                return $"{nombre_750VR} - {tecnica_750VR} ({duracion_750VR} min)";
            }
        }

    }
}
