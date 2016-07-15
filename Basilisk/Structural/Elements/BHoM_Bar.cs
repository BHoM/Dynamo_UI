using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;

namespace Structural
{

    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public static class Bar
    {
        /// <summary></summary>
        public static BHoM.Structural.Bar FromNodes(BHoM.Structural.Node startNode, BHoM.Structural.Node endNode)
        {
            return new BHoM.Structural.Bar(startNode, endNode);
        }

        /// <summary></summary>
        public static BHoM.Structural.Bar FromDSPoints(Point startPoint, Point endPoint)
        {
            return new BHoM.Structural.Bar(new BHoM.Structural.Node(startPoint.X, startPoint.Y, startPoint.Z)
                                            , new BHoM.Structural.Node(endPoint.X, endPoint.Y, endPoint.Z));
        }
    }

    
}