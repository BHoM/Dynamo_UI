using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;

namespace Geometry
{

    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public static class BoundingBox
    {
        /// <summary>Converts DynamoBoundingBox to BHoMBoundingBox.</summary>
        public static BHG.BoundingBox ReadDSBoundingBox(DSG.BoundingBox Box)
        {
            return Engine.Convert.DSGeometry.Read(Box);
        }

        /// <summary>Converts BHoMBoundingBox to DynamoBoundingBox.</summary>
        public static DSG.BoundingBox WriteDSBoundingBox(BHG.BoundingBox Box)
        {
            return Engine.Convert.DSGeometry.Write(Box);
        }
    }


}