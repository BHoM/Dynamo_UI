using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotToolkit;
using BH = BHoM.Structural;

namespace External
{
    public static class ExtNode
    {
        public static List<string> SetNodes(BH.IStructuralAdapter app, List<BH.Node> nodes)
        {
            List<string> ids = null;
            app.SetNodes(nodes, out ids);
            return ids;
        }
    }
}
