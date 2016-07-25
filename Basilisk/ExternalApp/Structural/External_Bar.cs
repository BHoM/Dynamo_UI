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
        public static List<string> SetBars(object app, List<BH.Bar> bars, bool active = true)
        {
            List<string> ids = new List<string>();
            if (active)
            {
                BH.IStructuralAdapter adapter = app as BH.IStructuralAdapter;
                adapter.SetBars(bars, out ids);
            }
            return ids;
        }
    }
}
