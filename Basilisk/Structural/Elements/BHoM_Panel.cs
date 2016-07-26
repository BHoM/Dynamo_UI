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
        public static DSG.PolyCurve ToDSPolyCurve(BHoM.Structural.Panel panel)
        {
            List<DSG.NurbsCurve> contours = new List<DSG.NurbsCurve>();
            foreach (BHG.Curve curve in panel.External_Contours)
            {
                List<DSG.Point> points = Geometry.BHCurve.ToDSPoints(curve);
                contours.Add(DSG.NurbsCurve.ByControlPoints(points, 1));
            }
            return DSG.PolyCurve.ByJoinedCurves(contours);
        }
    }
}
