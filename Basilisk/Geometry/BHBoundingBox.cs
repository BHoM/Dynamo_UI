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
    public static class BHBoundingBox
    {
        /// <summary></summary>
        public static BHG.BoundingBox FromDSBoundingBox(DSG.BoundingBox DSBox)
        {
            return FromDSPoints(DSBox.MinPoint, DSBox.MaxPoint);
        }

        /// <summary></summary>
        public static BHG.BoundingBox FromDSPoints(DSG.Point startPoint, DSG.Point endPoint)
        {
            return new BHG.BoundingBox(new BHG.Point(startPoint.X, startPoint.Y, startPoint.Z), new BHG.Point(endPoint.X, endPoint.Y, endPoint.Z));
        }

        /// <summary></summary>
        public static DSG.BoundingBox ToDSBoundingBox(BHG.BoundingBox BHBox)
        {
            return DSG.BoundingBox.ByCorners(BHPoint.ToDSPoint(BHBox.Min), BHPoint.ToDSPoint(BHBox.Max));
        }
    }


}