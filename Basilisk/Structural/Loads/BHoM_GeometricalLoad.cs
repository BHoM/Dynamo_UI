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
    public static class GeometricalLoad
    {
        public static BHS.Loads.GeometricalLoad CreateLoad(DSG.NurbsCurve contour, DSG.Vector force)
        {
            BHG.Polyline polyline = Geometry.Polyline.FromDSPolyline(contour);
            return new BHS.Loads.GeometricalLoad(polyline, new BHG.Vector(force.X, force.Y, force.Z));
        }
    }
}
