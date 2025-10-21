using DAL_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_VR750
{
    public class BLLreservaInsumo_750VR
    {
        private readonly DALReservaInsumo_750VR dal = new DALReservaInsumo_750VR();
        private readonly BLLbitacora_750VR log = new BLLbitacora_750VR();

        /// <summary>
        /// Inserta SÓLO si no existe; si existe, lanza excepción.
        /// Útil si querés evitar duplicados sin sumar.
        /// </summary>
        public void RegistrarInsumoUsado(int idReserva, int idInsumo, int cantidad)
        {
            ValidarParametros(idReserva, idInsumo, cantidad);

            if (dal.YaExisteInsumoParaReserva(idReserva, idInsumo))
                throw new InvalidOperationException("El insumo ya está registrado para esta reserva.");

            dal.InsertarInsumoReserva(idReserva, idInsumo, cantidad);
            log.InsumosUtilizados(idReserva);
        }

        /// <summary>
        /// Inserta o suma cantidad si ya existía (recomendado para 'Aplicar').
        /// </summary>
        public void UpsertInsumo(int idReserva, int idInsumo, int cantidad)
        {
            ValidarParametros(idReserva, idInsumo, cantidad);

            dal.UpsertInsumoReserva(idReserva, idInsumo, cantidad);
            log.InsumosUtilizados(idReserva);
        }

        /// <summary>
        /// Suma cantidad a un registro existente (si no existe, lanza excepción).
        /// </summary>
        public void SumarCantidadInsumo(int idReserva, int idInsumo, int delta)
        {
            if (delta <= 0) throw new ArgumentOutOfRangeException(nameof(delta), "La cantidad debe ser mayor a cero.");
            ValidarIds(idReserva, idInsumo);

            bool ok = dal.SumarCantidadInsumo(idReserva, idInsumo, delta);
            if (!ok) throw new InvalidOperationException("No existe el insumo para esta reserva.");
            log.InsumosUtilizados(idReserva);
        }

        public bool InsumoYaAgregado(int idReserva, int idInsumo)
        {
            ValidarIds(idReserva, idInsumo);
            return dal.YaExisteInsumoParaReserva(idReserva, idInsumo);
        }

        // ===== Helpers =====
        private static void ValidarParametros(int idReserva, int idInsumo, int cantidad)
        {
            ValidarIds(idReserva, idInsumo);
            if (cantidad <= 0) throw new ArgumentOutOfRangeException(nameof(cantidad), "La cantidad debe ser mayor a cero.");
        }

        private static void ValidarIds(int idReserva, int idInsumo)
        {
            if (idReserva <= 0) throw new ArgumentOutOfRangeException(nameof(idReserva));
            if (idInsumo <= 0) throw new ArgumentOutOfRangeException(nameof(idInsumo));
        }

    }
}
