using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BHoM.Structural.Elements;
using BHL = BHoM.Structural.Loads;
using BHI = BHoM.Structural.Interface;
using BHoM.Base.Results;
using BHoM.Structural.Results;
using Autodesk.DesignScript.Runtime;

namespace Structural.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public static class ResultAdapter
    {
        /// <summary>
        /// Import Node Reactions from the selected program
        /// </summary>
        /// <param name="ResultServer"></param>
        /// <param name="Nodes"></param>
        /// <param name="Loadcases"></param>
        /// <param name="OrderBy">Name = 0, Loadcase = 1, TimeStep = 2, None = 3</param>
        /// <param name="active"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Id", "Name", "Loadcase", "TimeStep", "FX", "FY", "FZ", "MX", "MY", "MZ" })]
        public static Dictionary<string, List<List<object>>> GetNodeReactions(object ResultServer, List<string> Nodes = null, List<string> Loadcases = null, int OrderBy = 0, bool active = true)
        {
            NodeReaction nodeReaction = new NodeReaction();          

            if (active)
            {
                BHI.IResultAdapter adapter = ResultServer as BHI.IResultAdapter;
                Dictionary<string, IResultSet> results = null;
                if (adapter.GetNodeReactions(Nodes, Loadcases, (ResultOrder)OrderBy, out results))
                {
                    return GetResults(results, nodeReaction.ColumnHeaders);
                    //foreach (IResultSet set in results.Values)
                    //{
                    //    List<object[]> data = set.ToListData();
                    //    for (int i = 0; i < data.Count; i++)
                    //    {
                    //        for (int j = 0; j < data[i].Length; j++)
                    //        {
                    //            List<List<object>> o1 = output[nodeReaction.ColumnHeaders[j]];
                    //            if (i == 0)
                    //            {
                    //                o1.Add(new List<object>());
                    //            }
                    //            currentList = o1.Last();
                    //            currentList.Add(data[i][j]);
                    //        }
                    //    }

                    //}
                }
            }
            return null;
        }

        private static Dictionary<string, List<List<object>>> GetResults(Dictionary<string, IResultSet> results, string[] columnHeaders)
        {
            Dictionary<string, List<List<object>>> output = new Dictionary<string, List<List<object>>>();
            List<object> currentList = null;
            foreach (IResultSet set in results.Values)
            {
                List<object[]> data = set.ToListData();
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < data[i].Length; j++)
                    {
                        List<List<object>> o1 = output[columnHeaders[j]];
                        if (i == 0)
                        {
                            o1.Add(new List<object>());
                        }
                        currentList = o1.Last();
                        currentList.Add(data[i][j]);
                    }
                }
            }
            return output;
        }
    }
}
