using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;

namespace Geometry
{

    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public static class Point
    {
        /// <summary></summary>
        public static BHG.Point FromXYZ(double x = 0, double y = 0, double z = 0)
        {
            return new BHG.Point(x, y, z);
        }

        /// <summary></summary>
        [MultiReturn(new[] {"x", "y", "z"})]
        public static Dictionary<string, double> ToXYZ(BHG.Point point)
        {
            Dictionary<string, double> xyz = new Dictionary<string, double>();
            xyz["x"] = point.X;
            xyz["y"] = point.Y;
            xyz["z"] = point.Z;
            return xyz;
        }

        /// <summary></summary>
        public static BHG.Point FromDSPoint(DSG.Point point)
        {
            return new BHG.Point(point.X, point.Y, point.Z);
        }

        /// <summary></summary>
        public static DSG.Point ToDSPoint(BHG.Point point)
        {
            return DSG.Point.ByCoordinates(point.X, point.Y, point.Z);
        }

    }


}