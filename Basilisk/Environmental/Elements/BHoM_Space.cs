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
        /// <summary>Environmental.Elements.BHSpace.FromLines
        /// Creates a BHoM space from boundary lines.
        /// Optional to input list of Walls and Name</summary>
        public static BHE.Space FromBoundaryLines(List<BHG.Line> Lines, List<BHE.Wall> Walls = null, string Name = null)
        {
            BHE.Space space = new BHE.Space();
            space.Lines = Lines;
            if (Name !=null)
            {
                space.Name = Name;
            }
            if (Walls != null)
            {
                space.Walls = Walls;
            }
            else
            {
                space.Walls = new List<BHE.Wall>();
            }

            return space;
        }
    }
}
