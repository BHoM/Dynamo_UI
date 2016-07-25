using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Geometry;

namespace ModelLaundry
{
    public static class Diagnostic
    {
        public static List<Point> CheckSnappedPoints(List<object> bhElements, double tolerance)
        {
            return ModelLaundry_Engine.Diagnostic.CheckSnappedPoints(bhElements, tolerance);
        }
    }
}
