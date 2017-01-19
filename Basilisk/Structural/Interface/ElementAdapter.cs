using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BHoM.Structural.Elements;
using BHL = BHoM.Structural.Loads;
using BHI = BHoM.Structural.Interface;
using Autodesk.DesignScript.Runtime;
using DSG = Autodesk.DesignScript.Geometry;
using Geometry;

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

        public static List<string> SetNodes(object app, List<BHE.Node> nodes, bool active = true)
        {
            List<string> ids = new List<string>();
            if (active)
                ((BHI.IElementAdapter)app).SetNodes(nodes, out ids);
            return ids;
        }

        public static List<string> SetPanels(object app, List<BHE.Panel> panels, bool active = true)
        {
            List<string> ids = new List<string>();
            if (active)
                ((BHI.IElementAdapter)app).SetPanels(panels, out ids);
            return ids;
        }

        /// <param name="selection">All = 0, Selected = 1, FromInput = 2</param>
        [MultiReturn(new[] { "Panels", "ids" })]
        public static Dictionary<string, List<object>> GetPanels(object app, List<string> objectIds = null, int selection =0, bool activate = true)
        {
            string[] columnheaders = { "Panels", "ids" };
            List<BHE.Panel> panels = null;
            List < DSG.PolyCurve > geometry = null;
            List<string> outIds = new List<string>();

            Dictionary<string,List<object>> output = new Dictionary<string, List<object>>();

            if (activate)
            {
                ((BHI.IElementAdapter)app).Selection = (BHI.ObjectSelection)selection;
                outIds = ((BHI.IElementAdapter)app).GetPanels(out panels, objectIds);
            }

            List<object> o1 = new List<object>();
            List<object> o2 = new List<object>();

            for (int j = 0; j < panels.Count; j++)
            {
                o1.Add(panels[j]);
                o2.Add(outIds[j]);
            }

            output[columnheaders[0]] = o1;
            output[columnheaders[1]] = o2;


            return output;
            //return panels;
        } 
    }
}
