using BE_VR750;
using DAL_VR750;
using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL_VR750.BLLbitacora_750VR;

namespace BLL_VR750
{
    public enum Criticidad_750VR : byte { C1 = 1, C2, C3, C4, C5 }

    public class BLLbitacora_750VR
    {
        private readonly DALbitacora_750VR dal = new DALbitacora_750VR();

        private void RegistrarDesdeSesion(string evento, string modulo, Criticidad_750VR crit)
        {
            var sesion = SessionManager_750VR.ObtenerInstancia;
            string login = sesion.user?.user_750VR;
            if (string.IsNullOrWhiteSpace(login))
                throw new System.Exception("No hay usuario logueado para registrar en bitácora.");
            dal.RegistrarEvento_750VR(login, evento, modulo, (byte)crit);
        }

        public List<BEbitacora_750VR> FiltrarEventos(
            int? dniUsuario, int? criticidad, string evento, string modulo,
            DateTime? fechaInicio, DateTime? fechaFin)
            => dal.FiltrarEventos(dniUsuario, criticidad, evento, modulo, fechaInicio, fechaFin);

        // =========================
        // USUARIO / SEGURIDAD
        // =========================
        public void LoginOK() => RegistrarDesdeSesion("Iniciar sesión", "Usuario", Criticidad_750VR.C1);
        public void Logout() => RegistrarDesdeSesion("Cerrar sesión", "Usuario", Criticidad_750VR.C1);   // C1 (ajustado)
        public void CambioClave() => RegistrarDesdeSesion("Cambio de clave", "Usuario", Criticidad_750VR.C1); // C1 (ajustado)

        // =========================
        // USUARIOS (ADMIN)
        // =========================
        public void CrearUsuario(int dniNuevo) => RegistrarDesdeSesion($"Crear usuario ", "Administrador", Criticidad_750VR.C1);
        public void ActivarUsuario(int dni) => RegistrarDesdeSesion($"Activar usuario ", "Administrador", Criticidad_750VR.C1);
        public void DesactivarUsuario(int dni) => RegistrarDesdeSesion($"Desactivar usuario ", "Administrador", Criticidad_750VR.C1);
        public void ModificarUsuario(int dni) => RegistrarDesdeSesion($"Modificar usuario ", "Administrador", Criticidad_750VR.C1);
        public void DesbloquearUsuario(int dni) => RegistrarDesdeSesion($"Desbloquear usuario ", "Administrador", Criticidad_750VR.C1);
        public void BloquearUsuario(string loginBloqueado) => RegistrarDesdeSesion($"Bloquear usuario", "Administrador", Criticidad_750VR.C1);

        // =========================
        // CLIENTES (ADMIN)
        // =========================
        public void CrearCliente(int dni) => RegistrarDesdeSesion($"Crear cliente", "Administrador", Criticidad_750VR.C2);
        public void ModificarCliente(int dni) => RegistrarDesdeSesion($"Modificar cliente", "Administrador", Criticidad_750VR.C2);
       

        // =========================
        // RESERVAS
        // =========================
        public void CrearReserva(int idReserva) => RegistrarDesdeSesion($"Crear reserva", "Reserva", Criticidad_750VR.C2);
        public void ModificarReserva(int idReserva) => RegistrarDesdeSesion($"Modificar reserva", "Reserva", Criticidad_750VR.C2);

        // =========================
        // TURNOS
        // =========================
        public void TurnoRealizado(int idReserva) => RegistrarDesdeSesion($"Turno realizado", "Turno", Criticidad_750VR.C3);
        public void TurnoCancelado(int idReserva) => RegistrarDesdeSesion($"Turno cancelado", "Turno", Criticidad_750VR.C3);
        public void TurnoAusentado(int idReserva) => RegistrarDesdeSesion($"Turno ausentado", "Turno", Criticidad_750VR.C3);
        public void InsumosUtilizados(int idReserva) => RegistrarDesdeSesion($"Insumos utilizados", "Turno", Criticidad_750VR.C3);

        // =========================
        // COBROS
        // =========================

        //falta 
        public void CobroRealizado(int codFactura, int idReserva)
            => RegistrarDesdeSesion($"Cobro realizado", "Cobro", Criticidad_750VR.C3);
        public void CobroCancelado(int codFactura, int idReserva)
            => RegistrarDesdeSesion($"Cobro cancelado", "Cobro", Criticidad_750VR.C3);

       
        public void ImprimirFactura(int codFactura)
            => RegistrarDesdeSesion($"Imprimir factura", "Reportes", Criticidad_750VR.C4);

        // =========================
        // DISPONIBILIDAD (ADMIN)
        // =========================
        public void CrearDisponibilidad(int idDisp, int dniManic)
            => RegistrarDesdeSesion($"Crear disponibilidad", "Administrador", Criticidad_750VR.C2);
        public void ModificarDisponibilidad(int idDisp)
            => RegistrarDesdeSesion($"Modificar disponibilidad", "Administrador", Criticidad_750VR.C2);
    

        // =========================
        // INSUMOS (ADMIN)
        // =========================
        public void CrearInsumo(int codInsumo) => RegistrarDesdeSesion($"Crear insumos", "Administrador", Criticidad_750VR.C2);
        public void ModificarInsumo(int codInsumo) => RegistrarDesdeSesion($"Modificar insumos", "Administrador", Criticidad_750VR.C2);
     

        // =========================
        // SERVICIOS (ADMIN)
        // =========================
        public void CrearServicio(int idServicio) => RegistrarDesdeSesion($"Crear servicios", "Administrador", Criticidad_750VR.C2);
        public void ModificarServicio(int idServicio) => RegistrarDesdeSesion($"Modificar servicios", "Administrador", Criticidad_750VR.C2);
      

        // =========================
        // PERFILES / FAMILIAS (ADMIN)
        // =========================
        public void CrearPerfil(string nombre) => RegistrarDesdeSesion($"Crear perfil ", "Administrador", Criticidad_750VR.C1);
        public void ModificarPerfil(string nombre) => RegistrarDesdeSesion($"Modificar perfil ", "Administrador", Criticidad_750VR.C1);
        public void EliminarPerfil(string nombre) => RegistrarDesdeSesion($"Eliminar perfil ", "Administrador", Criticidad_750VR.C1);

        public void CrearFamilia(string nombre) => RegistrarDesdeSesion($"Crear familia ", "Administrador", Criticidad_750VR.C1);
        public void ModificarFamilia(string nombre) => RegistrarDesdeSesion($"Modificar familia ", "Administrador", Criticidad_750VR.C1);
        public void EliminarFamilia(string nombre) => RegistrarDesdeSesion($"Eliminar familia ", "Administrador", Criticidad_750VR.C1);

        // =========================
        // COMPRAS / RECEPCIÓN
        // =========================
        public void SolicitaCotizacion(string nro) => RegistrarDesdeSesion($"Solicita cotización ", "Compra", Criticidad_750VR.C2);
        public void PreRegistraProveedor(string nombre) => RegistrarDesdeSesion($"Pre-Registra proveedor ", "Compra", Criticidad_750VR.C2);
        public void GeneraOrdenCompra(string nroOC) => RegistrarDesdeSesion($"Genera Orden de compra ", "Compra", Criticidad_750VR.C2);
        public void RegistraProveedor(string nombre) => RegistrarDesdeSesion($"Registra proveedor ", "Compra", Criticidad_750VR.C2);
        public void RegistraRecepcion(string nroRemito) => RegistrarDesdeSesion($"Registra recepción ", "Recepción", Criticidad_750VR.C2);

        // =========================
        // BACKUP / RESTORE / SERIALIZACIÓN (ADMIN)
        // =========================
        public void BackupEjecutado(string destino) => RegistrarDesdeSesion($"Back up ", "Administrador", Criticidad_750VR.C1);
        public void RestoreEjecutado(string origen) => RegistrarDesdeSesion($"Restore ", "Administrador", Criticidad_750VR.C1);
        public void Serializar(string archivo) => RegistrarDesdeSesion($"Serializar archivo ", "Administrador", Criticidad_750VR.C4);
        public void DesSerializar(string archivo) => RegistrarDesdeSesion($"DesSerializar archivo ", "Administrador", Criticidad_750VR.C4);

        // =========================
        // GENÉRICO
        // =========================
        public void RegistrarLibre(string evento, string modulo, Criticidad_750VR crit)
            => RegistrarDesdeSesion(evento, modulo, crit);
    }

}

