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
    public class RobotNodalDisplacement
    {
        internal RobotNodalDisplacement() {}

        public static void ByNodalLoads(IEnumerable<BHoM.Structural.Loads.NodalLoad> nodalLoads)
        {
            RobotToolkit.NodalLoad.CreateNodalLoadDisplacement(nodalLoads.ToArray());
        }

    }
}

