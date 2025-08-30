using SERVICIOS_VR750;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL_VR750.BLLbitacora_750VR;
using DAL_VR750;

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

            // Atajos claros (podés agregar más)
            public void LoginOK() => RegistrarDesdeSesion("Iniciar sesión", "Usuario", Criticidad_750VR.C1);
            public void Logout() => RegistrarDesdeSesion("Cerrar sesión", "Usuario", Criticidad_750VR.C2);
            public void CrearUsuario(int dniNuevo) => RegistrarDesdeSesion($"Crear usuario DNI={dniNuevo}", "Administrador", Criticidad_750VR.C1);
            public void ModificarUsuario(int dni) => RegistrarDesdeSesion($"Modificar usuario DNI={dni}", "Administrador", Criticidad_750VR.C1);
            public void ActivarUsuario(int dni) => RegistrarDesdeSesion($"Activar usuario DNI={dni}", "Administrador", Criticidad_750VR.C1);
            public void DesactivarUsuario(int dni) => RegistrarDesdeSesion($"Desactivar usuario DNI={dni}", "Administrador", Criticidad_750VR.C1);
            public void DesbloquearUsuario(int dni) => RegistrarDesdeSesion($"Desbloquear usuario DNI={dni}", "Administrador", Criticidad_750VR.C1);
        //public void BloquearUsuario(int dni) => RegistrarDesdeSesion($"Bloquear usuario DNI={dni}", "Administrador", Criticidad_750VR.C1);
        public void CambioClave() => RegistrarDesdeSesion("Cambio de clave", "Usuario", Criticidad_750VR.C2);

            // Ejemplos extra según tu tabla
            public void ImprimirFactura() => RegistrarDesdeSesion("Imprimir factura", "Cobro", Criticidad_750VR.C4);
            public void CrearReserva() => RegistrarDesdeSesion("Crear reserva", "Reserva", Criticidad_750VR.C3);
            public void CancelarReserva() => RegistrarDesdeSesion("Cancelar reserva", "Reserva", Criticidad_750VR.C3);
        }

    }

