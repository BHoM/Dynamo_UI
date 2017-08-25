//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DSG = Autodesk.DesignScript.Geometry;
//using BHG = BH.oM.Geometry;
//using BHE = BH.oM.Structural.Elements;
//using BHR = BH.oM.Structural.Results;
//using BHI = BH.oM.Structural.Interface;
//using BH.oM.Base.Results;
//using Autodesk.DesignScript.Runtime;

//namespace Structural.Results
//{
//    public static class BHoM_SlabrReinforcementResults
//    {
//        [MultiReturn(new[] { "Id", "Name", "Loadcase", "TimeStep", "Node" , "AXP", "AXM", "AYP", "AYM" })]
//        public static Dictionary<string, List<List<object>>> SlabReinforcementResults(object app, List<string> id = null, List<string> Loadcases = null, int OrderBy = 0, bool active = true)
//        {
//            BHR.SlabReinforcement SlabReinforcementResult = new BHR.SlabReinforcement();

//            if (active)
//            {
//                Dictionary<string, IResultSet> results = null;
//                BHI.IResultAdapter adapter = app as BHI.IResultAdapter;

//                if (adapter.GetSlabReinforcement(id, Loadcases, (ResultOrder)OrderBy, out results))
//                {
//                    return GetResults(results, SlabReinforcementResult.ColumnHeaders);
//                }

//            }
//            return null;
//        }
//        private static Dictionary<string, List<List<object>>> GetResults(Dictionary<string, IResultSet> results, string[] columnHeaders)
//        {
//            Dictionary<string, List<List<object>>> output = new Dictionary<string, List<List<object>>>();
//            foreach (string header in columnHeaders)
//                output[header] = new List<List<object>>();

//            List<object> currentList = null;
//            foreach (IResultSet set in results.Values)
//            {
//                List<object[]> data = set.ToListData();
//                for (int i = 0; i < data.Count; i++)
//                {
//                    for (int j = 0; j < data[i].Length; j++)
//                    {
//                        List<List<object>> o1 = output[columnHeaders[j]];
//                        if (i == 0)
//                        {
//                            o1.Add(new List<object>());
//                        }
//                        currentList = o1.Last();
//                        currentList.Add(data[i][j]);
//                    }
//                }
//            }
//            return output;
//        }
//    }
//}
