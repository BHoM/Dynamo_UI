using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BHoM.Structural.Elements;
using BHL = BHoM.Structural.Loads;
using BHI = BHoM.Structural.Interface;

namespace Structural.Interface
{
    public static class ElementAdapter
    {
        public static List<string> SetBars(object app, List<BHE.Bar> bars, bool active = true)
        {
            List<string> ids = new List<string>();
            if (active)
            {
                BHI.IElementAdapter adapter = app as BHI.IElementAdapter;
                adapter.SetBars(bars, out ids);
            }
            return ids;
        }

        public static void SetLoads(object app, List<object> loads, bool active = true)
        {
            List<BHL.ILoad> converted = new List<BHL.ILoad>();
            foreach (object load in loads)
            {
                if (load is BHL.ILoad)
                    converted.Add(load as BHL.ILoad);
            }

            if (active)
            {
                BHI.IElementAdapter adapter = app as BHI.IElementAdapter;
                adapter.SetLoads(converted);
            }
        }

        public static class ExtNode
        {
            public static List<string> SetNodes(object app, List<BHE.Node> nodes, bool active = true)
            {
                List<string> ids = new List<string>();
                if (active)
                    ((BHI.IElementAdapter)app).SetNodes(nodes, out ids);
                return ids;
            }
        }

        public static List<string> SetPanels(object app, List<BHE.Panel> panels, bool active = true)
        {
            List<string> ids = new List<string>();
            if (active)
                ((BHI.IElementAdapter)app).SetPanels(panels, out ids);
            return ids;
        }
    }
}
