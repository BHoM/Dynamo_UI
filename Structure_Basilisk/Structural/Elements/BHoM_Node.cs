using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using BHE = BH.oM.Structural.Elements;

namespace Structural.Elements
{
    /// <summary></summary>
    public static class BHNode
    {
        /// <summary></summary>
        public static BHE.Node FromDSPoint(Point point)
        {
            return new BHE.Node(point.X, point.Y, point.Z);
        }

        /// <summary></summary>
        public static Point ToDSPoint(BHE.Node node)
        {
            BH.oM.Geometry.Point nodePoint = node.Point;
            return Point.ByCoordinates(nodePoint.X, nodePoint.Y, nodePoint.Z);
        }
    }
}
