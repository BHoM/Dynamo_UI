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
        /// <summary>Environmental.Elements.BHPanel.FromLines
        /// Creates a BHoM Panel from panel boundary lines.
        /// Optional to input list of Name.</summary>
        public static BHE.Panel FromPolyline(List<BHG.Line> Lines, string Name = null)
        {
            BHE.Panel panel= new BHE.Panel();
            panel.Lines = Lines;
            if (Name != null)
            {
                panel.Name = Name;
            }
            return panel;
        }
    }
}