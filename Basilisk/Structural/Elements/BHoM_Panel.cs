using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;

namespace Structural
{
    public static class BHPanel
    {
        /// <summary></summary>
        public static BHoM.Structural.Panel FromDSCurve(DSG.Curve curve)
        {
            List<BHoM.Geometry.Point> bhomPoints = new List<BHoM.Geometry.Point>();
            foreach (DSG.Point pt in curve.ToNurbsCurve().ControlPoints())
                bhomPoints.Add(new BHoM.Geometry.Point(pt.X, pt.Y, pt.Z));

            BHoM.Geometry.Group<BHoM.Geometry.Curve> group = new BHoM.Geometry.Group<BHoM.Geometry.Curve>();
            group.Add(new BHoM.Geometry.Polyline(bhomPoints));
            return new BHoM.Structural.Panel(group);
        }

        /// <summary></summary>
        public static BHoM.Structural.Panel FromDSSurface(DSG.Surface surface)
        {
            BHoM.Geometry.Group<BHoM.Geometry.Curve> group = new BHoM.Geometry.Group<BHoM.Geometry.Curve>();
            foreach (DSG.Curve curve in surface.PerimeterCurves())
            {
                List<BHoM.Geometry.Point> bhomPoints = new List<BHoM.Geometry.Point>();
                foreach (DSG.Point pt in curve.ToNurbsCurve().ControlPoints())
                    bhomPoints.Add(new BHoM.Geometry.Point(pt.X, pt.Y, pt.Z));
                group.Add(new BHoM.Geometry.Polyline(bhomPoints));
            }
            return new BHoM.Structural.Panel(group);
        }

        /// <summary></summary>
        public static List<DSG.Polygon> ToDSPolygon(BHoM.Structural.Panel panel)
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
