using BE_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL_VR750;

namespace BLL_VR750
{
    public class BLLReserva_750VR
    {
        private readonly DALreserva_750VR dal;
        private readonly BLLbitacora_750VR log;
        public BLLReserva_750VR()
        {
                dal = new DALreserva_750VR();
            log = new BLLbitacora_750VR();
        }
        public List<BEReserva_750VR> ObtenerReservasPorManicurista(int dniManicurista)
        {
            return dal.ObtenerReservasPorManicurista(dniManicurista);
        }
        public bool ModificarReserva_750VR(BEReserva_750VR reserva)
        {
            var ok = dal.ModificarReserva(reserva);
            if (ok)
            {
                // si tu BE tiene otra propiedad para el ID, ajustá acá
                log.ModificarReserva(reserva.CodReserva_750VR);
            }
            return ok;
        }
        public int CrearReserva_750VR(BEReserva_750VR reserva)
        {
            int id = dal.CrearReserva_750VR(reserva);
            if (id > 0) log.CrearReserva(id);
            return id;
        }
        public string ObtenerEstadoReserva(int idReserva)
        {
            
            return dal.ObtenerEstadoReserva(idReserva);
        }

        public List<BEReserva_750VR> leerEntidades_750VR()
        {
            return dal.leerEntidades_750VR();
        }
        public BEReserva_750VR ObtenerReservaPorId(int id)
        {
            return dal.ObtenerReservaPorId(id);
        }
        public bool MarcarComoCobrado(int id)
        {
            return dal.MarcarComoCobrado(id);
        }

        public void ActualizarEstadoReserva(int idReserva, string nuevoEstado)
        {

            dal.ActualizarEstadoReserva(idReserva, nuevoEstado);

            // Log según tu catálogo (Turno realizado/ausentado/cancelado = Criticidad 3, módulo "Turno")
            var estado = (nuevoEstado ?? "").Trim().ToLowerInvariant();

            if (estado == "realizado" || estado == "completado" || estado == "finalizado")
            {
                log.TurnoRealizado(idReserva);
            }
            else if (estado == "cancelado" || estado == "cancelada")
            {
                log.TurnoCancelado(idReserva);
            }
            else if (estado == "ausente" || estado == "ausentado" || estado == "no asistió" || estado == "no asistio")
            {
                log.TurnoAusentado(idReserva);
            }
            else
            {
                // fallback genérico dentro de Reserva (Criticidad 3)
                log.RegistrarLibre($"Cambiar estado reserva Id={idReserva} a '{nuevoEstado}'", "Reserva", Criticidad_750VR.C3);
            }
        }
    }
}
