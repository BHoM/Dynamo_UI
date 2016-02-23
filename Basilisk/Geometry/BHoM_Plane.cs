using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using BHoM.Geometry;

namespace Geometry
{
    /// <summary>
    /// BHoM Plane object
    /// </summary>
    public class Plane : IPlane
    {
        /// <summary>X coordinate</summary>
        [IsVisibleInDynamoLibrary(false)]
        public BHoM.Geometry.Point Origin { get; set; }

        /// <summary>X coordinate</summary>
        [IsVisibleInDynamoLibrary(false)]
        public BHoM.Geometry.Vector X { get; set; }

        /// <summary>Y coordinate</summary>
        [IsVisibleInDynamoLibrary(false)]
        public BHoM.Geometry.Vector Y { get; set; }

        /// <summary>Z coordinate</summary>
        [IsVisibleInDynamoLibrary(false)]
        public BHoM.Geometry.Vector Z { get; set; }

       
        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public static Autodesk.DesignScript.Geometry.Plane ToDSPlane(BHoM.Geometry.Plane plane)
        {
            Autodesk.DesignScript.Geometry.Plane DSplane =
            Autodesk.DesignScript.Geometry.Plane.ByOriginXAxisYAxis(
                Autodesk.DesignScript.Geometry.Point.ByCoordinates(plane.Origin.X, plane.Origin.Y, plane.Origin.Z),
                Autodesk.DesignScript.Geometry.Vector.ByCoordinates(plane.X.X, plane.X.Y, plane.X.Z),
                Autodesk.DesignScript.Geometry.Vector.ByCoordinates(plane.Y.X, plane.Y.Y, plane.Y.Z));

            if (plane.Z.X != DSplane.Normal.X && plane.Z.Y != DSplane.Normal.Y && plane.Z.Z != DSplane.Normal.Z) DSplane.Normal.Reverse();

            return DSplane;
        }
    }
}