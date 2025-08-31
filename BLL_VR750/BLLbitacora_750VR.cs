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

        public enum Criticidad_750VR : byte { C1 = 1, C2 = 2, C3 = 3, C4 = 4, C5 = 5 }

        public class BLLbitacora_750VR
        {
            private readonly DALbitacora_750VR dal = new DALbitacora_750VR();

            // Base: toma login actual desde SessionManager y registra
            private void RegistrarDesdeSesion(string evento, string modulo, Criticidad_750VR crit)
            {
                var sesion = SessionManager_750VR.ObtenerInstancia;
                string login = sesion.user?.user_750VR;

                if (string.IsNullOrWhiteSpace(login))
                    throw new System.Exception("No hay usuario logueado para registrar en bitácora.");

                dal.RegistrarEvento_750VR(login, evento, modulo, (byte)crit);
            }

        public List<BEbitacora_750VR> FiltrarEventos(
          int? dniUsuario,               // DNI del usuario (se resuelve por JOIN con Usuario_VR750)
          int? criticidad,               // 1..5
          string evento,                 // texto exacto (ej: "Iniciar sesión")
          string modulo,                 // texto exacto (ej: "Usuario")
          DateTime? fechaInicio,         // inclusive (fecha)
          DateTime? fechaFin)            // inclusive (fecha)
        {
            return dal.FiltrarEventos(dniUsuario, criticidad, evento, modulo, fechaInicio, fechaFin);
        }

        // =========================
        // USUARIO / SEGURIDAD
        // =========================
        public void LoginOK() => RegistrarDesdeSesion("Iniciar sesión", "Usuario", Criticidad_750VR.C1);
        public void Logout() => RegistrarDesdeSesion("Cerrar sesión", "Usuario", Criticidad_750VR.C2);
        public void CambioClave() => RegistrarDesdeSesion("Cambio de clave", "Usuario", Criticidad_750VR.C2);

        public void CrearUsuario(int dniNuevo) => RegistrarDesdeSesion($"Crear usuario DNI={dniNuevo}", "Administrador", Criticidad_750VR.C1);
        public void ModificarUsuario(int dni) => RegistrarDesdeSesion($"Modificar usuario DNI={dni}", "Administrador", Criticidad_750VR.C1);
        public void ActivarUsuario(int dni) => RegistrarDesdeSesion($"Activar usuario DNI={dni}", "Administrador", Criticidad_750VR.C1);
        public void DesactivarUsuario(int dni) => RegistrarDesdeSesion($"Desactivar usuario DNI={dni}", "Administrador", Criticidad_750VR.C1);
        public void DesbloquearUsuario(int dni) => RegistrarDesdeSesion($"Desbloquear usuario DNI={dni}", "Administrador", Criticidad_750VR.C1);
        public void BloquearUsuario(string loginBloqueado) => RegistrarDesdeSesion($"Bloquear usuario Login={loginBloqueado}", "Administrador", Criticidad_750VR.C1);
       

        // =========================
        // CLIENTES
        // =========================
        public void CrearCliente(int dni) => RegistrarDesdeSesion($"Crear cliente DNI={dni}", "Administrador", Criticidad_750VR.C2);
        public void ModificarCliente(int dni) => RegistrarDesdeSesion($"Modificar cliente DNI={dni}", "Administrador", Criticidad_750VR.C2);
        public void EliminarCliente(int dni) => RegistrarDesdeSesion($"Eliminar cliente DNI={dni}", "Administrador", Criticidad_750VR.C2);

        // =========================
        // RESERVAS / TURNOS
        // =========================
        public void CrearReserva(int idReserva) => RegistrarDesdeSesion($"Crear reserva Id={idReserva}", "Reserva", Criticidad_750VR.C3);
        public void CancelarReserva(int idReserva) => RegistrarDesdeSesion($"Cancelar reserva Id={idReserva}", "Reserva", Criticidad_750VR.C3);
        public void ModificarReserva(int idReserva) => RegistrarDesdeSesion($"Modificar reserva Id={idReserva}", "Reserva", Criticidad_750VR.C3);

        public void TurnoRealizado(int idReserva) => RegistrarDesdeSesion($"Turno realizado Id={idReserva}", "Turno", Criticidad_750VR.C3);
        public void TurnoAusentado(int idReserva) => RegistrarDesdeSesion($"Turno ausentado Id={idReserva}", "Turno", Criticidad_750VR.C3);
        public void TurnoCancelado(int idReserva) => RegistrarDesdeSesion($"Turno cancelado Id={idReserva}", "Turno", Criticidad_750VR.C3);

        // =========================
        // COBROS / FACTURAS
        // =========================
        public void CobroRealizado(int codFactura, int idReserva) => RegistrarDesdeSesion($"Cobro realizado Factura={codFactura}, Reserva={idReserva}", "Cobro", Criticidad_750VR.C3);
        public void CobroCancelado(int codFactura, int idReserva) => RegistrarDesdeSesion($"Cobro cancelado Factura={codFactura}, Reserva={idReserva}", "Cobro", Criticidad_750VR.C3);
        public void ImprimirFactura(int codFactura) => RegistrarDesdeSesion($"Imprimir factura Cod={codFactura}", "Cobro", Criticidad_750VR.C4);

        // =========================
        // DISPONIBILIDAD (manicuristas)
        // =========================
        public void CrearDisponibilidad(int idDisp, int dniManic) => RegistrarDesdeSesion($"Crear disponibilidad Id={idDisp} (DNI manicurista={dniManic})", "Administrador", Criticidad_750VR.C2);
        public void ModificarDisponibilidad(int idDisp) => RegistrarDesdeSesion($"Modificar disponibilidad Id={idDisp}", "Administrador", Criticidad_750VR.C2);
        public void EliminarDisponibilidad(int idDisp) => RegistrarDesdeSesion($"Eliminar disponibilidad Id={idDisp}", "Administrador", Criticidad_750VR.C2);

        // =========================
        // INSUMOS
        // =========================
        public void CrearInsumo(int codInsumo) => RegistrarDesdeSesion($"Crear insumo Cod={codInsumo}", "Administrador", Criticidad_750VR.C2);
        public void ModificarInsumo(int codInsumo) => RegistrarDesdeSesion($"Modificar insumo Cod={codInsumo}", "Administrador", Criticidad_750VR.C2);
        public void EliminarInsumo(int codInsumo) => RegistrarDesdeSesion($"Eliminar insumo Cod={codInsumo}", "Administrador", Criticidad_750VR.C2);

        // =========================
        // SERVICIOS
        // =========================
        public void CrearServicio(int idServicio) => RegistrarDesdeSesion($"Crear servicio Id={idServicio}", "Administrador", Criticidad_750VR.C2);
        public void ModificarServicio(int idServicio) => RegistrarDesdeSesion($"Modificar servicio Id={idServicio}", "Administrador", Criticidad_750VR.C2);
        public void EliminarServicio(int idServicio) => RegistrarDesdeSesion($"Eliminar servicio Id={idServicio}", "Administrador", Criticidad_750VR.C2);

        // =========================
        // PERFILES / FAMILIAS / PERMISOS
        // =========================
        public void CrearPerfil(string nombre) => RegistrarDesdeSesion($"Crear perfil '{nombre}'", "Administrador", Criticidad_750VR.C1);
        public void ModificarPerfil(string nombre) => RegistrarDesdeSesion($"Modificar perfil '{nombre}'", "Administrador", Criticidad_750VR.C1);
        public void EliminarPerfil(string nombre) => RegistrarDesdeSesion($"Eliminar perfil '{nombre}'", "Administrador", Criticidad_750VR.C1);

        public void CrearFamilia(string nombre) => RegistrarDesdeSesion($"Crear familia '{nombre}'", "Administrador", Criticidad_750VR.C1);
        public void ModificarFamilia(string nombre) => RegistrarDesdeSesion($"Modificar familia '{nombre}'", "Administrador", Criticidad_750VR.C1);
        public void EliminarFamilia(string nombre) => RegistrarDesdeSesion($"Eliminar familia '{nombre}'", "Administrador", Criticidad_750VR.C1);

        // =========================
        // GENÉRICO (por si te falta algo puntual)
        // =========================
        public void RegistrarLibre(string evento, string modulo, Criticidad_750VR crit)
            => RegistrarDesdeSesion(evento, modulo, crit);
    }

    }

