using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Interfaces;


namespace Structural
{
    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class Grid
    {
        /// <summary></summary>
        internal Grid() { } 

        /// <summary>
        /// Creates BHoM structural grid elements by inputing curves. 
        /// BuroHappold
        /// </summary>
        /// <param name="curve"></param> 
        /// <param name="name"></param>
        /// <returns></returns>
        /// <search>BH, structure, grid, gridbycurves</search>
        [RegisterForTrace]
        public static BHoM.Structural.Grid CreateByCurves(Line curve,  string name = "")
        {
            BHoM.Global.Project project = new BHoM.Global.Project();
            BHoM.Structural.GridFactory gridFactory = new BHoM.Structural.GridFactory(project);
            return gridFactory.Create(new Geometry.Line(curve), name);
        }

        /// <summary>
        /// Creates BHoM structural grid elements by inputing json. 
        /// BuroHappold
        /// </summary>
        /// <param name="json"></param> 
        /// <returns></returns>
        /// <search>BH, structure, grid, gridbyjson</search>
        [RegisterForTrace]
        public static BHoM.Structural.Grid CreateByJson(string json = "") 
        {
            BHoM.Global.Project project = new BHoM.Global.Project();
            BHoM.Structural.GridFactory gridFactory = new BHoM.Structural.GridFactory(project);
            return gridFactory.Create(json);
        }
    }
}
