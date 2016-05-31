using System;
using System.Collections.Generic;
using System.Linq;
using BHoM.Geometry;

namespace Geometry
{
    /// <summary>
    /// BHoM Line object
    /// </summary>
    public static class Line 
    {
        /// <summary></summary>
        public static BHoM.Geometry.Line FromDSLine(Autodesk.DesignScript.Geometry.Line line)
        {
            return new BHoM.Geometry.Line(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z, line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z);
        }

        /// <summary></summary>
        public static BHoM.Geometry.Line FromDSPoints(Autodesk.DesignScript.Geometry.Point start, Autodesk.DesignScript.Geometry.Point end)
        {
            return new BHoM.Geometry.Line(start.X, start.Y, start.Z, end.X, end.Y, end.Z);
        }

        /// <summary></summary>
        public static Autodesk.DesignScript.Geometry.Line ToDSLine(BHoM.Geometry.Line line)
        {
            return Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(
                Autodesk.DesignScript.Geometry.Point.ByCoordinates(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z),
                Autodesk.DesignScript.Geometry.Point.ByCoordinates(line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z));
        }

        /// <summary></summary>
        public static string ToJSON(BHoM.Geometry.Line line)
        {
            return line.ToJSON();
        }

        /// <summary></summary>
        public static BHoM.Geometry.Line FromJSON(string json)
        {
            return BHoM.Geometry.Line.FromJSON(json) as BHoM.Geometry.Line;
        }
    }
}