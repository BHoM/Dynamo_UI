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
    public static class BHPolyline
    {
        /// <summary></summary>
        public static BHG.Polyline FromDSPolyline(DSG.NurbsCurve polyline)
        {
            return FromDSPoints(polyline.ControlPoints());
        }

        /// <summary></summary>
        public static BHG.Polyline FromDSPoints(IEnumerable<DSG.Point> DSPoints)
        {
            List<BHG.Point> points = new List<BHG.Point>();
            foreach (DSG.Point point in DSPoints)
                points.Add(new BHG.Point(point.X, point.Y, point.Z));

            return new BHG.Polyline(points);
        }

        /// <summary></summary>
        public static DSG.NurbsCurve ToDSPolyline(BHG.Polyline polyline)
        {
            List<DSG.Point> points = new List<DSG.Point>();
            foreach (BHG.Point point in polyline.ControlPoints)
                points.Add(DSG.Point.ByCoordinates(point.X, point.Y, point.Z));

            return DSG.NurbsCurve.ByControlPoints(points, 1);
        }
    }


}