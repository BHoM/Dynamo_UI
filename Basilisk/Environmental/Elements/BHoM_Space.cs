using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHE = BHoM.Environmental.Elements;

namespace Environmental.Elements
{

    /// <summary>
    /// Environmental tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public static class BHSpace
    {
        /// <summary></summary>
        public static BHE.Space FromPolyline(List<BHG.Line> Lines, string Name = null)
        {
            BHE.Space space = new BHE.Space();
            space.Lines = Lines;
            space.Name = Name;
            return space;
        }
    }
}
