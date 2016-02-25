using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

using System.Web.Script.Serialization;

namespace Flux.Structural
{
    public class Flux_Bar
    {
        internal Flux_Bar() { }

        [MultiReturn(new[] { "Json_Bars" })]
        public static Dictionary<string, object> FromBHoM(IEnumerable<BHoM.Structural.Bar> BHBars)
        {

            Dictionary<string, object> bars_out = new Dictionary<string, object>();

            List<string> flux_bars = new List<String>();

            foreach (BHoM.Structural.Bar BHBar in BHBars)
            {
                string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(BHBar);
                flux_bars.Add(json);
            }

            bars_out.Add("Json_Bars", flux_bars.ToArray());

            return bars_out;
        }



        [MultiReturn(new[] { "Bars" })]
        public static Dictionary<string, object> ToBHoM(IEnumerable<String> Jsons )
        {

            Dictionary<string, object> bars_out = new Dictionary<string, object>();

            List<BHoM.Structural.Bar> BH_Bars = new List<BHoM.Structural.Bar>();

            foreach (String Json in Jsons)
            {
                BHoM.Structural.Bar BH_Bar = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<BHoM.Structural.Bar>(Json);
                BH_Bars.Add(BH_Bar);
                //bars_out.Add("Bars" + Convert.ToString(barId), flux_bar);
                //++barId;
            }

            bars_out.Add("Bars", BH_Bars.ToArray());

            return bars_out;
        }

    }
}
