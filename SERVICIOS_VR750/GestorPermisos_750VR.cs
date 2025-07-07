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
                item.Enabled = permisos.Contains(item.Name);

                if (item.HasDropDownItems)
                    AplicarPermisosAlMenu(item, permisos);
            }
        }

        public static void AplicarPermisosAlMenu(ToolStripMenuItem menuItem, List<string> permisos)
        {
            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                if (subItem is ToolStripMenuItem subMenu)
                {
                    subMenu.Enabled = permisos.Contains(subMenu.Name);

                    if (subMenu.HasDropDownItems)
                        AplicarPermisosAlMenu(subMenu, permisos); // recursión
                }
            }
        }


    }
}
