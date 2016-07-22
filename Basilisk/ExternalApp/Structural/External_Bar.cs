using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotToolkit;
using BH = BHoM.Structural;

namespace External
{
    public static class ExtBar
    {
        public static List<string> SetBars(object app, List<BH.Bar> bars)
        {
            List<string> ids = null;
            ((BH.IStructuralAdapter)app).SetBars(bars, out ids);
            return ids;
        }
    }
}
