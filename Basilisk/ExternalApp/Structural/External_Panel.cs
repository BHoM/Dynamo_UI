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
        public static List<string> SetPanels(object app, List<BH.Panel> panels, bool active = true)
        {
            List<string> ids = new List<string>();
            if (active)
                ((BH.IStructuralAdapter)app).SetPanels(panels, out ids);
            return ids;
        }
    }
}
