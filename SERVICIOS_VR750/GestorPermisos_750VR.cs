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
        public static void AplicarPermisosAlMenu(ToolStripMenuItem menuItem, List<string> permisos)
        {
            foreach (ToolStripMenuItem item in menuItem.DropDownItems)
            {
                item.Enabled = permisos.Contains(item.Name);
                if (item.HasDropDownItems)
                    AplicarPermisosAlMenu(item, permisos);
            }
        }
    }
}
