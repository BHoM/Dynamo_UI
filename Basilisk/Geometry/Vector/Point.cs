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
        /// <summary>Creates BHoMPoint from XYZ.</summary>
        public static BHG.Point PointFromXYZ(double x = 0, double y = 0, double z = 0)
        {
            return new BHG.Point(x, y, z);
        }

        /// <summary>Reads XYZ values from BHoMPoint.</summary>
        [MultiReturn(new[] {"x", "y", "z"})]
        public static Dictionary<string, double> PointToXYZ(BHG.Point point)
        {
            Dictionary<string, double> xyz = new Dictionary<string, double>();
            xyz["x"] = point.X;
            xyz["y"] = point.Y;
            xyz["z"] = point.Z;
            return xyz;
        }

        /// <summary>Converts DynamoPoint to BHoMPoint.</summary>
        public static BHG.Point ReadDSPoint(DSG.Point point)
        {
            return Engine.Convert.DSGeometry.Read(point);
        }

        /// <summary>Converts BHoMPoint to DynamoPoint.</summary>
        public static DSG.Point WriteDSPoint(BHG.Point point)
        {
            return Engine.Convert.DSGeometry.Write(point);
        }
    }
}