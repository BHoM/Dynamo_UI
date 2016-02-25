using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

using System.Web.Script.Serialization;

namespace Flux.Structural
{
    public class Flux_Node
    {
        internal Flux_Node() { }

        [MultiReturn(new[] { "Json_Nodes" })]
        public static Dictionary<string, object> FromBHoM(IEnumerable<BHoM.Structural.Node> BHNodes)
        {

            Dictionary<string, object> nodes_out = new Dictionary<string, object>();

            List<string> flux_nodes = new List<String>();

            foreach (BHoM.Structural.Node BHNode in BHNodes)
            {
                string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(BHNode);
                flux_nodes.Add(json);
            }

            nodes_out.Add("Json_Nodes", flux_nodes.ToArray());

            return nodes_out;
        }


        [MultiReturn(new[] { "Nodes" })]
        public static Dictionary<string, object> ToBHoM(IEnumerable<String> Jsons)
        {

            Dictionary<string, object> bars_out = new Dictionary<string, object>();

            List<BHoM.Structural.Node> BH_Nodes = new List<BHoM.Structural.Node>();

            foreach (String Json in Jsons)
            {
                BHoM.Structural.Node BH_Node = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<BHoM.Structural.Node>(Json);
                BH_Nodes.Add(BH_Node);
            }

            bars_out.Add("Nodes", BH_Nodes.ToArray());

            return bars_out;
        }
    }
}
