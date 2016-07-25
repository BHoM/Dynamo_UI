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
        public static List<string> SetNodes(object app, List<BH.Node> nodes, bool active = true)
        {
            List<string> ids = new List<string>();
            if (active)
                ((BH.IStructuralAdapter)app).SetNodes(nodes, out ids);
            return ids;
        }
    }
}
