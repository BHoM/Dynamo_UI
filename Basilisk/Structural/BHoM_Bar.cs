using System;
using System.Collections.Generic;
using System.Linq;

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
    }

    
}