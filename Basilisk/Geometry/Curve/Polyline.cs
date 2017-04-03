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
    public static class Polyline
    {
        /// <summary>Converts DynamoNurbsCurve to BHoMPolyline.</summary>
        public static BHG.Polyline ReadDSPolycurve(DSG.NurbsCurve polyline)
        {
            return Engine.Convert.DSGeometry.Read(polyline);
        }

        /// <summary>Converts BHoMPolyline to DynamoPolyCurve.</summary>
        public static DSG.PolyCurve WriteDSPolycurve(BHG.Polyline polyline)
        {
            return Engine.Convert.DSGeometry.Write(polyline);
        }
    }


}