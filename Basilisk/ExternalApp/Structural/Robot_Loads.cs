using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using RobotToolkit;
using BHoM.Structural;

namespace Robot
{
    /// <summary>
    /// Robot load application tools
    /// BuroHappold
    /// <class name="RobotLoad">Robot bar tools</class>
    /// </summary>
    public class RobotLoad
    {
        internal RobotLoad() { }

        /// <summary>
        /// Create nodal displacement by nodal loads
        /// </summary>
        /// <param name="nodalLoads"></param>
        public static List<BHoM.Structural.Loads.AreaUniformalyDistributedLoad> GetLoads(BHoM.Structural.Loads.Loadcase loadcase)
        {
            List<BHoM.Structural.Loads.AreaUniformalyDistributedLoad> loads = null;
            RobotToolkit.Load.GetLoads(loadcase, out loads);


            return loads;
        }

    }
}

