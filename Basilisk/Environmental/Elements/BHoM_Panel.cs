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
    public static class BHPanel
    {
        /// <summary></summary>
        public static BHE.Panel FromPolyline(List<BHG.Line> Lines, Dictionary<string, object> CustomData = null, string Name = null)
        {
            BHE.Panel panel= new BHE.Panel();
            panel.Lines = Lines;
            panel.CustomData = CustomData;
            panel.Name = Name;
            return panel;
        }
    }
}