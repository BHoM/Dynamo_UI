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
        /// Create a new load with forces and moments. For all other nodal loads (displacement etc) use the relevant constructor.
        /// BuroHappold
        /// </summary>
        /// <param name="Px"></param>
        /// <param name="Py"></param>
        /// <param name="Pz"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Px", "Py", "Pz" })]
        public static Dictionary<string, object> Pressure(BHoM.Structural.Loads.AreaUniformalyDistributedLoad UDL)
        {
            Dictionary<string, object> Pressure_out = new Dictionary<string, object>();

            Pressure_out.Add("Px", UDL.Pressure.X);
            Pressure_out.Add("Py", UDL.Pressure.Y);
            Pressure_out.Add("Pz", UDL.Pressure.Z);

            return Pressure_out;
        }



    }
}
