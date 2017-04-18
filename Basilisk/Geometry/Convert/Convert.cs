using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;

namespace Geometry
{
    public static class Convert
    {
        /// <summary>Writes DS Geometry from BH Geometry.</summary>
        public static object WriteDSGeometry(object BHgeo)
        {
            return Engine.Convert.DSGeometry.Write(BHgeo);
        }

        /// <summary>Reads DS Geometry to BH Geometry.</summary>
        public static object ReadDSGeometry(object DSgeo)
        {
            return Engine.Convert.DSGeometry.Read(DSgeo);
        }
    }
}
