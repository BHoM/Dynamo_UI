using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;

namespace Geometry
{

    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public static class BHVector
    {
        /// <summary></summary>
        public static BHG.Vector FromDSVector(DSG.Vector Vector)
        {
            return new BHG.Vector(Vector.X, Vector.Y, Vector.Z);
        }

        /// <summary></summary>
        public static DSG.Vector ToDSVector(BHG.Vector Vector)
        {
            return DSG.Vector.ByCoordinates(Vector.X, Vector.Y, Vector.Z);
        }

    }


}