using System;
using System.Collections.Generic;
using System.Linq;
using BHoM.Geometry;

namespace Geometry
{
    /// <summary>
    /// BHoM Point object
    /// </summary>
    public class Point
    {
        /// <summary></summary>
        public static BHoM.Geometry.Point FromDSPoint(Autodesk.DesignScript.Geometry.Point point)
        {
            return new BHoM.Geometry.Point(point.X, point.Y, point.Z);
        }

        /// <summary></summary>
        public static Autodesk.DesignScript.Geometry.Point ToDSPoint(BHoM.Geometry.Point point)
        {
            return Autodesk.DesignScript.Geometry.Point.ByCoordinates(point.X, point.Y, point.Z);
        }

        /// <summary></summary>
        public static string ToJSON(BHoM.Geometry.Point point)
        {
            return point.ToJSON();
        }

        /// <summary></summary>
        public static BHoM.Geometry.Point FromJSON(string json)
        {
            return BHoM.Geometry.Point.FromJSON(json) as BHoM.Geometry.Point;
        }
    }
}