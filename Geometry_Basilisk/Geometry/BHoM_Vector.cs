using System;
using System.Collections.Generic;
using System.Linq;
using BHoM.Geometry;

namespace Geometry
{
    /// <summary>
    /// BHoM Vector object
    /// </summary>
    public class Vector 
    {
        /// <summary></summary>
        public static BHoM.Geometry.Vector FromDSVector(Autodesk.DesignScript.Geometry.Vector vector)
        {
            return new BHoM.Geometry.Vector(vector.X, vector.Y, vector.Z);
        }

        /// <summary></summary>
        public static Autodesk.DesignScript.Geometry.Vector ToDSVector(BHoM.Geometry.Vector vector)
        {
            return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(vector.X, vector.Y, vector.Z);
        }

        /// <summary></summary>
        public static string ToJSON(BHoM.Geometry.Vector vector)
        {
            return vector.ToJSON();
        }

        /// <summary></summary>
        public static BHoM.Geometry.Vector FromJSON(string json)
        {
            return BHoM.Geometry.Vector.FromJSON(json) as BHoM.Geometry.Vector;
        }

    }
}