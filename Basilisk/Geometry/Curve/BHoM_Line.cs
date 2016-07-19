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
    public static class BHLine
    {
        /// <summary></summary>
        public static BHoM.Geometry.Line FromDSLine(DSG.Line line)
        {
            return FromDSPoints(line.StartPoint, line.EndPoint);
        }

        /// <summary></summary>
        public static BHG.Line FromDSPoints(DSG.Point startPoint, DSG.Point endPoint)
        {
            return new BHG.Line(new BHG.Point(startPoint.X, startPoint.Y, startPoint.Z), new BHG.Point(endPoint.X, endPoint.Y, endPoint.Z));
        }

        /// <summary></summary>
        public static DSG.Line ToDSLine(BHG.Line line)
        {
            return DSG.Line.ByStartPointEndPoint(BHPoint.ToDSPoint(line.StartPoint), BHPoint.ToDSPoint(line.EndPoint));
        }
    }


}