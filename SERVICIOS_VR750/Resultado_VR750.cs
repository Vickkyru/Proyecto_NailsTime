using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS_VR750
{
    public class Resultado_VR750<T>
    {
        public bool resultado { get; set; }
        public T entidad { get; set; }
        public string mensaje { get; set; }

    }
}
