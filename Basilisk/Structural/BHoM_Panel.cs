using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;

namespace Structural
{
    public static class Panel
    {
        /// <summary></summary>
        public static BHoM.Structural.Panel FromDSCurve(Curve curve)
        {
            List<BHoM.Geometry.Point> bhomPoints = new List<BHoM.Geometry.Point>();
            foreach (Point pt in curve.ToNurbsCurve().ControlPoints())
                bhomPoints.Add(new BHoM.Geometry.Point(pt.X, pt.Y, pt.Z));

            BHoM.Geometry.Group<BHoM.Geometry.Curve> group = new BHoM.Geometry.Group<BHoM.Geometry.Curve>();
            group.Add(new BHoM.Geometry.Polyline(bhomPoints));
            return new BHoM.Structural.Panel(group);
        }
    }
}
