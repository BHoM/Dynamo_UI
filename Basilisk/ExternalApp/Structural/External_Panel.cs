using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotToolkit;
using BH = BHoM.Structural;

namespace External
{
    public static class ExtPanel
    {
        public static List<string> SetPanels(BH.IStructuralAdapter app, List<BH.Panel> panels)
        {
            List<string> ids = null;
            app.SetPanels(panels, out ids);
            return ids;
        }
    }
}
