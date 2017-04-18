using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHS = BHoM.Structural;

namespace Structural.Loads
{
    public static class BHGeometricalLoad
    {
        public static BHS.Loads.GeometricalAreaLoad CreateAreaLoad(DSG.NurbsCurve contour, DSG.Vector force)
        {
            BHG.Polyline polyline = Engine.Convert.DSGeometry.Read(contour);
            return new BHS.Loads.GeometricalAreaLoad(polyline, new BHG.Vector(force.X, force.Y, force.Z));
        }

        public static BHS.Loads.GeometricalLineLoad CreateGeometricalLineLoad(DSG.Line line, BHS.Loads.Loadcase loadcase, DSG.Vector force, DSG.Vector moment = null)
        {
            BHG.Line bhLine = Engine.Convert.DSGeometry.Read(line);
            BHS.Loads.GeometricalLineLoad load = new BHS.Loads.GeometricalLineLoad(bhLine, new BHG.Vector(force.X, force.Y, force.Z), new BHG.Vector(moment.X, moment.Y, moment.Z));
            load.Loadcase = loadcase;
            return load;    
        }
    }
}
