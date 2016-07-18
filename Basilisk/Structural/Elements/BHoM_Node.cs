using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;

namespace Structural
{
    /// <summary></summary>
    public static class Node
    {
        /// <summary></summary>
        public static BHoM.Structural.Node FromDSPoint(Point point)
        {
            return new BHoM.Structural.Node(point.X, point.Y, point.Z);
        }

        /// <summary></summary>
        public static Point ToDSPoint(BHoM.Structural.Node node)
        {
            BHoM.Geometry.Point nodePoint = node.Point;
            return Point.ByCoordinates(nodePoint.X, nodePoint.Y, nodePoint.Z);
        }
    }
}
