using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revit.GeometryConversion;

namespace Revit
{
    /// <summary>
    /// A Revit Transform
    /// </summary>
    public static class Transform
    {
        /// <summary>
        /// Gets transformation of point
        /// </summary>
        /// <param name="Transform">Revit Transform</param>
        /// <param name="Point">Point</param>
        /// <returns name="Point">Point</returns>
        /// <search>
        /// Revit, Transform, OfPoint
        /// </search>
        public static Autodesk.DesignScript.Geometry.Point OfPoint(Autodesk.Revit.DB.Transform Transform, Autodesk.DesignScript.Geometry.Point Point)
        {
            return Transform.OfPoint(Point.ToXyz(false)).ToPoint(false);
        }
    }
}
