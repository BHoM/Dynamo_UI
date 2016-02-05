using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using RobotToolkit;
using BHoM.Structural;

namespace Robot
{
    /// <summary>
    /// Robot bar tools
    /// BuroHappold
    /// <class name="Bar">Robot bar tools</class>
    /// </summary>
    public class RobotBar
    {
        internal RobotBar() {}
        /// <summary>
        /// Create Robot bars using the cache method
        /// BuroHappold
        /// </summary>
        /// <param name="bars">BHoM bars must be constructed first</param>
        /// <param name="deleteBars">Set true to delete all bars in model first</param>
        /// <param name="activate">Set true to activate</param>
        /// <returns></returns>
        /// <search>BH, robot, bars</search>
        [MultiReturn(new[] { "Centrelines", "Numbers", "Bars" })]
        public static Dictionary<string, object> CreateByBar(IEnumerable<BHoM.Structural.Bar> bars, bool deleteBars = false, bool activate = false)
        {
            //Output dictionary definition
            Dictionary<string, object> bars_out = new Dictionary<string, object>();
            if (deleteBars) RobotToolkit.Bar.DeleteBars("all");
            if (deleteBars) RobotToolkit.Node.DeleteNodes("all");

            List<BHoM.Structural.Bar> str_bars = new List<BHoM.Structural.Bar>();
            if (activate == true)
            {
                   foreach (BHoM.Structural.Bar bar in bars)
                    {
                        str_bars.Add(bar);
                    }
                RobotToolkit.Bar.CreateBarsByCache(str_bars.ToArray());
                RobotToolkit.General.RefreshView();
            } 
                return bars_out;
            }       

        /// <summary>
        /// Get bar elements from Robot
        /// BuroHappold
        /// </summary>
        /// <param name="activate">Set true to activate</param>
        /// <param name="allBarData">If true gets all data for each node - slower method</param>
        /// <param name="filepath">If path string is added then Robot file will be opened in a new instance</param>
        /// <returns></returns>
        /// <search>BH, robot, bars</search>
        [MultiReturn(new[] { "Lines", "Numbers", "Bars" })]
        public static Dictionary<string, object> Import(bool activate = false, bool allBarData = false, string filepath = "")
        {
            //Output dictionary definition
            Dictionary<string, object> getbars_out = new Dictionary<string, object>();

           
            //Bar parameters
            Dictionary<string, object> lines = new Dictionary<string, Object>();
            Dictionary<string, BHoM.Structural.Bar> bars = new Dictionary<String, BHoM.Structural.Bar>();
            Dictionary<int, BHoM.Structural.Bar> str_bars = new Dictionary<int, BHoM.Structural.Bar>();
            List<int> bar_numbers = new List<int>();
                 
            if (activate == true)
            {
                                
                if (allBarData != true)
                {
                    try { RobotToolkit.Bar.GetBarsQuery(out str_bars, filepath); } catch { RobotToolkit.Bar.GetBars(out str_bars, filepath); }
                    RobotToolkit.Bar.GetBarsQuery(out str_bars, filepath);
                    foreach (BHoM.Structural.Bar str_bar in str_bars.Values)
                    {
                        Line ln = Line.ByStartPointEndPoint(Point.ByCoordinates(str_bar.StartNode.X, str_bar.StartNode.Y,str_bar.StartNode.Z),
                                                            Point.ByCoordinates(str_bar.EndNode.X, str_bar.EndNode.Y, str_bar.EndNode.Z));
                        lines.Add(str_bar.Number.ToString(), ln);
                        bar_numbers.Add(str_bar.Number);
                    }
                }
                else
                {
                    RobotToolkit.Bar.GetBars(out str_bars, filepath);
                    foreach (BHoM.Structural.Bar str_bar in str_bars.Values)
                    {
                        Line ln = Line.ByStartPointEndPoint(Point.ByCoordinates(str_bar.StartNode.X, str_bar.StartNode.Y, str_bar.StartNode.Z),
                                                           Point.ByCoordinates(str_bar.EndNode.X, str_bar.EndNode.Y, str_bar.EndNode.Z));
                        lines.Add(str_bar.Number.ToString(), ln);
                        bar_numbers.Add(str_bar.Number);
                    }
                }
                getbars_out.Add("Lines", lines.Values);
                getbars_out.Add("Numbers", bar_numbers);
                getbars_out.Add("Bars", str_bars.Values);
                }
            return getbars_out;
        }
 }
}

