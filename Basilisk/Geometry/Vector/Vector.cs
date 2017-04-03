using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;

namespace Geometry
{

    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public static class Vector
    {
        /// <summary>Converts DynamoVector to BHoMVector.</summary>
        public static BHG.Vector FromDSVector(DSG.Vector Vector)
        {
            return Engine.Convert.DSGeometry.Read(Vector);
        }

        /// <summary>Converts BHoMVector to DynamoVector.</summary>
        public static DSG.Vector ToDSVector(BHG.Vector Vector)
        {
            return Engine.Convert.DSGeometry.Write(Vector);
        }

    }


}