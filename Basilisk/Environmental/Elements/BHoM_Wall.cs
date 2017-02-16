using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHE = BHoM.Environmental.Elements;
using DC = Dynamo.Core;

namespace Environmental.Elements
{

    /// <summary>
    /// Environmental tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public static class BHWall
    {
        /// <summary></summary>
        public static BHE.Wall FromPolyline(BHG.Line Line, List<BHE.Panel> Panels = null, Dictionary<string, object> CustomData = null, string Name = null)
        {
            BHE.Wall wall = new BHE.Wall();
            wall.Line = Line;
            wall.CustomData = CustomData;
            wall.Name = Name;
            return wall;
        }
    }
}