using System;
using System.Collections.Generic;
using System.Linq;
using BHoM.Geometry;

namespace Geometry
{
    /// <summary>
    /// BHoM Plane object
    /// </summary>
    public class Plane 
    {
        /// <summary></summary>
        public static BHoM.Geometry.Plane FromDSPlane(Autodesk.DesignScript.Geometry.Plane plane)
        {
            return new BHoM.Geometry.Plane(
                new BHoM.Geometry.Point(plane.Origin.X, plane.Origin.Y, plane.Origin.Z),
                new BHoM.Geometry.Vector(plane.Normal.X, plane.Normal.Y, plane.Normal.Z));
        }


        /// <summary></summary>
        public static Autodesk.DesignScript.Geometry.Plane ToDSPlane(BHoM.Geometry.Plane plane)
        {
            return Autodesk.DesignScript.Geometry.Plane.ByOriginNormal(
                Autodesk.DesignScript.Geometry.Point.ByCoordinates(plane.Origin.X, plane.Origin.Y, plane.Origin.Z),
                Autodesk.DesignScript.Geometry.Vector.ByCoordinates(plane.Normal.X, plane.Normal.Y, plane.Normal.Z));
        }

        /// <summary></summary>
        public static string ToJSON(BHoM.Geometry.Plane plane)
        {
            return plane.ToJSON();
        }

        /// <summary></summary>
        public static BHoM.Geometry.Plane FromJSON(string json)
        {
            return BHoM.Geometry.Plane.FromJSON(json) as BHoM.Geometry.Plane;
        }
    }
}