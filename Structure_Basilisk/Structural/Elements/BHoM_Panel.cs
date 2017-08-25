using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Structural.Elements;

namespace Structural.Elements
{
    public static class BHPanel
    {
        /// <summary></summary>
        public static BHE.Panel FromDSCurve(DSG.Curve curve)
        {
            BHG.Group<BH.oM.Geometry.Curve> group = new BHG.Group<BH.oM.Geometry.Curve>();
            foreach (DSG.Geometry geometry in curve.Explode())
            {
                DSG.Curve c = geometry as DSG.Curve;
                DSG.Point start = c.StartPoint;
                DSG.Point end = c.EndPoint;
                group.Add(new BHG.Line(new BH.oM.Geometry.Point(start.X, start.Y, start.Z), new BHG.Point(end.X, end.Y, end.Z)));

            }
            return new BHE.Panel(group);
        }

        /// <summary></summary>
        public static BHE.Panel FromDSSurface(DSG.Surface surface)
        {
            BH.oM.Geometry.Group<BH.oM.Geometry.Curve> group = new BH.oM.Geometry.Group<BH.oM.Geometry.Curve>();
            foreach (DSG.Edge edge in surface.Edges)
            {
                DSG.Point start = edge.StartVertex.PointGeometry;
                DSG.Point end = edge.EndVertex.PointGeometry;
                group.Add(new BH.oM.Geometry.Line(new BH.oM.Geometry.Point(start.X, start.Y, start.Z), new BH.oM.Geometry.Point(end.X, end.Y, end.Z)));
            }
            return new BHE.Panel(group);
        }

        /// <summary></summary>
        public static List<DSG.Polygon> ToDSPolygon(BHE.Panel panel)
        {
            List<DSG.Polygon> contours = new List<DSG.Polygon>();
            foreach (BHG.Curve curve in panel.External_Contours)
                contours.Add(DSG.Polygon.ByPoints(Geometry.BHCurve.ToDSPoints(curve)));
            foreach (BHG.Curve curve in panel.Internal_Contours)
                contours.Add(DSG.Polygon.ByPoints(Geometry.BHCurve.ToDSPoints(curve)));
            return contours;
        }
    }
}
