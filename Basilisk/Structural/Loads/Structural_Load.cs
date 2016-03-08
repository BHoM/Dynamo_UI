using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

namespace Structural.Loads
{
    /// <summary>
    /// Nodal load class - to create and set BHoM nodal load objects.
    /// BuroHappold
    /// </summary>
    public class UniformLoad
    {
        internal UniformLoad() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UDL"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Px", "Py", "Pz", "ObjectNumbers"})]
        public static Dictionary<string, object> Pressure(BHoM.Structural.Loads.AreaUniformalyDistributedLoad UDL)
        {
            Dictionary<string, object> Pressure_out = new Dictionary<string, object>();

            Pressure_out.Add("Px", UDL.Pressure.X);
            Pressure_out.Add("Py", UDL.Pressure.Y);
            Pressure_out.Add("Pz", UDL.Pressure.Z);
            Pressure_out.Add("ObjectNumbers", UDL.ObjectNumbers);

            return Pressure_out;
        }



    }
}
