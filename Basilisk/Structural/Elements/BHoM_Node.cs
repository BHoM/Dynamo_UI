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

    }
}
