using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SERVICIOS_VR750
{
    public static class GestorPermisos_750VR
    {
        public static void AplicarPermisosAlMenuCompleto(MenuStrip menuStrip, List<string> permisos)
        {
            foreach (ToolStripMenuItem item in menuStrip.Items)
            {
                bool tieneHabilitado = AplicarPermisosAlMenu(item, permisos);

                // 🔽 Activar solo si su nombre está en la lista o alguno de sus hijos fue habilitado
                item.Enabled = permisos.Contains(item.Name) || tieneHabilitado;
            }
        }

        public static bool AplicarPermisosAlMenu(ToolStripMenuItem menuItem, List<string> permisos)
        {
            bool alMenosUnoHabilitado = false;

            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                if (subItem is ToolStripMenuItem subMenu)
                {
                    bool habilitado = permisos.Contains(subMenu.Name) || AplicarPermisosAlMenu(subMenu, permisos);
                    subMenu.Enabled = habilitado;

                    if (habilitado)
                        alMenosUnoHabilitado = true;
                }
            }

            return alMenosUnoHabilitado;
        }


    }
}
