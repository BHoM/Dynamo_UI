using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHE = BHoM.Structural.Elements;


namespace Structural.Elements
{

    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public static class BHBar
    {
        /// <summary></summary>
        public static BHE.Bar FromBHNodes(BHE.Node startNode, BHE.Node endNode)
        {
            return new BHE.Bar(startNode, endNode);
        }

        /// <summary></summary>
        public static BHE.Bar FromDSPoints(DSG.Point startPoint, DSG.Point endPoint)
        {
            return new BHE.Bar(new BHE.Node(startPoint.X, startPoint.Y, startPoint.Z)
                                            , new BHE.Node(endPoint.X, endPoint.Y, endPoint.Z));
        }

        /// <summary></summary>
        public static DSG.Line ToDSLine(BHE.Bar bar)
        {
            return DSG.Line.ByStartPointEndPoint(Engine.Convert.DSGeometry.Write(bar.StartPoint), Engine.Convert.DSGeometry.Write(bar.EndPoint));
        }
    }

    
}