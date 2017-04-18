using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHE = BHoM.Structural.Elements;

namespace Structural.Elements
{
    public static class BHPanel
    {
        /// <summary></summary>
        public static BHE.Panel FromDSCurve(DSG.Curve curve)
        {
            BHG.Group<BHoM.Geometry.Curve> group = new BHG.Group<BHoM.Geometry.Curve>();
            foreach (DSG.Geometry geometry in curve.Explode())
            {
                DSG.Curve c = geometry as DSG.Curve;
                DSG.Point start = c.StartPoint;
                DSG.Point end = c.EndPoint;
                group.Add(new BHG.Line(new BHoM.Geometry.Point(start.X, start.Y, start.Z), new BHG.Point(end.X, end.Y, end.Z)));

            }
            return new BHE.Panel(group);
        }

        /// <summary></summary>
        public static BHE.Panel FromDSSurface(DSG.Surface surface)
        {
            BHoM.Geometry.Group<BHoM.Geometry.Curve> group = new BHoM.Geometry.Group<BHoM.Geometry.Curve>();
            foreach (DSG.Edge edge in surface.Edges)
            {
                DSG.Point start = edge.StartVertex.PointGeometry;
                DSG.Point end = edge.EndVertex.PointGeometry;
                group.Add(new BHoM.Geometry.Line(new BHoM.Geometry.Point(start.X, start.Y, start.Z), new BHoM.Geometry.Point(end.X, end.Y, end.Z)));
            }
            return new BHE.Panel(group);
        }

        /// <summary></summary>
        public static List<DSG.Polygon> ToDSPolygon(BHE.Panel panel)
        {
            List<DSG.Polygon> contours = new List<DSG.Polygon>();
            foreach (BHG.Curve curve in panel.External_Contours)
                contours.Add(DSG.Polygon.ByPoints(Geometry.Curve.ToDSPoints(curve)));
            foreach (BHG.Curve curve in panel.Internal_Contours)
                contours.Add(DSG.Polygon.ByPoints(Geometry.Curve.ToDSPoints(curve)));
            return contours;
        }
    }
}
