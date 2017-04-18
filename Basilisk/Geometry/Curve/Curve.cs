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
    public static class Curve
    {
        /// <summary>Converts BHoMCurve to DynamoPoints.</summary>
        public static List<DSG.Point> ToDSPoints(BHG.Curve Curve)
        {
            List<DSG.Point> points = new List<DSG.Point>();
            foreach (BHG.Point point in Curve.ControlPoints)
                points.Add(DSG.Point.ByCoordinates(point.X, point.Y, point.Z));

            return points;
        }

    }


}