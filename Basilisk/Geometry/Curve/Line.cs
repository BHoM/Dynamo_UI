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
    public static class Line
    {
        /// <summary>Converts DynamoLine to BHoMLine.</summary>
        public static BHoM.Geometry.Line ReadDSLine(DSG.Line line)
        {
            return Engine.Convert.DSGeometry.Read(line);
        }

        /// <summary>Converts BHoMLine to DynamoLine.</summary>
        public static DSG.Line WriteDSLine(BHG.Line line)
        {
            return Engine.Convert.DSGeometry.Write(line);
        }
    }


}