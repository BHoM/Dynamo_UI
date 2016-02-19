using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace Structural
{
    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class Bar
    {        
        internal Bar(){}

        /// <summary>
        /// Creates BHoM structural bar elements by inputing curves. When curves are not lines, they are split into lines
        /// based on the input facet length.
        /// BuroHappold
        /// </summary>
        /// <param name="curves"></param>
        /// <param name="facetLength"></param>
        /// <returns></returns>
        /// <search>BH, structure, bar, barbycurves</search>
        
        [RegisterForTrace]
        [MultiReturn(new[] { "Bars", "Nodes"})]
        public static Dictionary<string, object> CreateByCurves(IEnumerable<Curve> curves,
        double facetLength = 0)     
        {

            Dictionary<string, object> bars_out = new Dictionary<string, object>();
            List<BHoM.Structural.Bar> str_bars = new List<BHoM.Structural.Bar>();
            Dictionary<string, BHoM.Structural.Node> str_nodes = new Dictionary<string, BHoM.Structural.Node>();


            int tol = 12;
            List<Line> lines = new List<Line>();
            int nod_kounta = 1;
            int bar_kounta = 1;


            foreach (Curve crv in curves)
            {
                List<Curve> split_crvs = new List<Curve>();
                if (facetLength > 0)
                {
                    List<double> facet_params = new List<double>();
                    for (int j = 0; j < Math.Round(crv.Length / facetLength, 0); j++)
                    {
                        facet_params.Add((1 / Math.Round(crv.Length / facetLength, 0)) * j);
                    }
                    split_crvs = crv.ParameterSplit(facet_params.ToArray()).ToList();
                }
                else
                {
                    split_crvs.Add(crv);
                };

                foreach (Curve split_crv in split_crvs)
                {
                    Point start_pnt = split_crv.StartPoint;
                    string start_node_key = Convert.ToString(Math.Round(start_pnt.X, tol)) + ","
                    + Convert.ToString(Math.Round(start_pnt.Y, tol)) + "," + Convert.ToString(Math.Round(start_pnt.Z, tol));
                    if (!str_nodes.ContainsKey(start_node_key))
                    {
                        int start_nod_num = Structural.NodeManager.GetNextUnusedID();
                        str_nodes.Add(start_node_key, new BHoM.Structural.Node(start_pnt.X, start_pnt.Y, start_pnt.Z, start_nod_num));
                        nod_kounta++;
                    }

                    Point end_pnt = split_crv.EndPoint;
                    string end_node_key = Convert.ToString(Math.Round(end_pnt.X, tol)) + ","
                    + Convert.ToString(Math.Round(end_pnt.Y, tol)) + "," + Convert.ToString(Math.Round(end_pnt.Z, tol));
                    if (!str_nodes.ContainsKey(end_node_key))
                    {
                        int end_nod_num = Structural.NodeManager.GetNextUnusedID();
                        str_nodes.Add(end_node_key, new BHoM.Structural.Node(end_pnt.X, end_pnt.Y, end_pnt.Z, end_nod_num));
                        nod_kounta++;
                    }
                    int bar_num = Basilisk.Structural.BarManager.GetNextUnusedID();
                    BHoM.Structural.Bar bar = new BHoM.Structural.Bar(str_nodes[start_node_key], str_nodes[end_node_key], bar_num);
                    str_bars.Add(bar);
                    bar_kounta++;
                }
            }

            bars_out.Add("Bars", str_bars.ToArray());
            bars_out.Add("Nodes", str_nodes.Values.ToArray());
            
            
            return bars_out;
        }

        /// <summary>
        /// Calculate gamma angles for rotation of beam/column/bar objects
        /// BuroHappold
        /// </summary>
        /// <param name="barCentrelines"></param>
        /// <returns></returns>
        /// <search>BH, gamma, orientation, angle</search>
        [MultiReturn(new[] { "Orientation Angle", "Local Axes" })]
        public static Dictionary<string, object> CalculateOrientation(Curve[] barCentrelines)
        {
            //Output dictionary definition
            Dictionary<string, object> gammaAngles_out = new Dictionary<string, object>();
            double[] gamma_angles = null;
            List<Plane> plns = new List<Plane>();
        
            for (int i = 0; i < barCentrelines.Length; i++)
            {
            Plane pln = barCentrelines[i].PlaneAtParameter(0.5);
            plns.Add(pln);
            }

            if (barCentrelines != null) gammaAngles_out.Add("Orientation Angle", gamma_angles);
            if (barCentrelines != null) gammaAngles_out.Add("Local Axes", plns);
            return gammaAngles_out;
        }


        /// <summary>
        /// Sets the properties to to BHoM bars
        /// BuroHappold
        /// </summary>
        /// <param name="bars"></param>
        /// <param name="sectionName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        /// <search>BH, bar, properties</search>
        [MultiReturn(new[] { "Bars" })]
        public static Dictionary<string, object> SetProperties(IEnumerable<BHoM.Structural.Bar> bars,
            [DefaultArgument("\"\"")] string sectionName,
            [DefaultArgument("Unassigned")] string typeName)
        {
            Dictionary<string, object> bars_out = new Dictionary<string, object>();
            foreach (BHoM.Structural.Bar bar in bars)
            {
                bar.SectionProperty.Name = sectionName;
                bar.SetDesignGroupName(typeName);
            }
  
            bars_out.Add("Nodes", bars);
            return bars_out;
        }
    }
}