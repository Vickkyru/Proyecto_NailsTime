using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BE_VR750
{
    public class BEfactura_750VR
    {
  
        public int CodFactura_750VR { get; set; }
        public int CodReserva_750VR { get; set; }
        public DateTime fecha_750VR { get; set; }
        public TimeSpan horaEmision_750VR { get; set; }
        public decimal total_750VR { get; set; }
        public string metodoPago_750VR { get; set; }

        public string titular_750VR { get; set; }

        public BEfactura_750VR(int codReserva, DateTime fecha, TimeSpan hora, decimal total, string metodoPago, string titular)
        {
            CodReserva_750VR = codReserva;
            fecha_750VR = fecha;
            horaEmision_750VR = hora;
            total_750VR = total;
            metodoPago_750VR = metodoPago;
            titular_750VR = titular;
        }
        public BEfactura_750VR(int codFactura, int codReserva, DateTime fecha, TimeSpan hora, decimal total, string metodoPago, string titular)
        {
           CodFactura_750VR = codFactura;
            CodReserva_750VR = codReserva;
            fecha_750VR = fecha;
            horaEmision_750VR = hora;
            total_750VR = total;
            metodoPago_750VR = metodoPago;
            titular_750VR = titular;
        }
    }
}
