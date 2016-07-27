using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotToolkit;
using BH = BHoM.Structural;

namespace External
{
    public static class ExtLoad
    {
        public static void SetLoads(object app, List<object> loads, bool active = true)
        {
            List<BH.Loads.ILoad> converted = new List<BH.Loads.ILoad>();
            foreach(object load in loads)
            {
                if (load is BH.Loads.ILoad)
                    converted.Add(load as BH.Loads.ILoad);
            }

            if (active)
            {
                BH.IStructuralAdapter adapter = app as BH.IStructuralAdapter;
                adapter.SetLoads(converted);
            }
        }
    }
}
