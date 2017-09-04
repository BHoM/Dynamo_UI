using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;

namespace Geometry
{

    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public static class BHCurve
    {
        /// <summary></summary>
        public static List<DSG.Point> ToDSPoints(BHG.ICurve Curve)
        {
            List<DSG.Point> points = new List<DSG.Point>();
            foreach (BHG.Point point in Curve.GetControlPoints())
                points.Add(DSG.Point.ByCoordinates(point.X, point.Y, point.Z));

            return points;
        }

    }


}