using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_VR750
{
    public  class BEReserva_750VR
    {
  
        public int CodReserva_750VR { get; set; }
        public int DNIcli_750VR { get; set; }

        public BECliente_750VR cliente { get; set; }

        public int DNImanic_750VR { get; set; }
        public BEusuario_750VR manic { get; set; }

        public int CodServicio_750VR { get; set; }
        public BEServicio_750VR serv { get; set; }

        public DateTime Fecha_750VR { get; set; }
        public TimeSpan HoraInicio_750VR { get; set; }
        public TimeSpan HoraFin_750VR { get; set; }
        public decimal Precio_750VR { get; set; }

        public string Estado_750VR { get; set; }  // "Pendiente", "Realizado", "Cancelado"
        public bool Cobrado_750VR { get; set; }


        public BEReserva_750VR(int dnicli, BECliente_750VR cli, int dnimanic,BEusuario_750VR manic, int idserv, BEServicio_750VR serv, DateTime fecha, TimeSpan ini, TimeSpan fin, decimal pre, string estado, bool cobrado)
        {
            this.DNIcli_750VR = dnicli;
            this.cliente = cli;
            this.DNImanic_750VR = dnimanic;
            this.manic = manic; 
            this.CodServicio_750VR = idserv;
            this.serv = serv;
            this.Fecha_750VR = fecha;
            this.HoraInicio_750VR = ini;
            this.HoraFin_750VR = fin;
            this.Precio_750VR= pre;
            this.Estado_750VR = estado;
            this.Cobrado_750VR=cobrado;
        }

        public BEReserva_750VR(int cod, int dnicli, BECliente_750VR cli, int dnimanic, BEusuario_750VR manic, int idserv, BEServicio_750VR serv, DateTime fecha, TimeSpan ini, TimeSpan fin, decimal pre, string estado, bool cobrado)
        {
            this.CodReserva_750VR = cod;
            this.DNIcli_750VR = dnicli;
            this.cliente = cli;
            this.DNImanic_750VR = dnimanic;
            this.manic = manic;
            this.CodServicio_750VR = idserv;
            this.serv = serv;
            this.Fecha_750VR = fecha;
            this.HoraInicio_750VR = ini;
            this.HoraFin_750VR = fin;
            this.Precio_750VR = pre;
            this.Estado_750VR = estado;
            this.Cobrado_750VR = cobrado;
        }

    }
}
