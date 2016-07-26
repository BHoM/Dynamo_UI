using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHS = BHoM.Structural;

namespace Structural
{
    public static class BHGeometricalLoad
    {
        public static BHS.Loads.GeometricalAreaLoad CreateAreaLoad(DSG.NurbsCurve contour, DSG.Vector force)
        {
            BHG.Polyline polyline = Geometry.BHPolyline.FromDSPolyline(contour);
            return new BHS.Loads.GeometricalAreaLoad(polyline, new BHG.Vector(force.X, force.Y, force.Z));
        }
    }
}
