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
    public class NodalLoad
    {
        internal NodalLoad() { }
        /// <summary>
        /// Create a new nodal load with forces and moments. For all other nodal loads (displacement etc) use the relevant constructor.
        /// BuroHappold
        /// </summary>
        /// <param name="loadcase"></param>
        /// <param name="node"></param>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="fz"></param>
        /// <param name="mx"></param>
        /// <param name="my"></param>
        /// <param name="mz"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "NodalLoad" })]
        public static Dictionary<string, object> Force(BHoM.Structural.Node node, BHoM.Structural.Loads.Loadcase loadcase,
            double fx, double fy, double fz, double mx = 0, double my = 0, double mz = 0)
        {
            Dictionary<string, object> force_out = new Dictionary<string, object>();
            BHoM.Structural.Loads.NodalLoad nodalLoad = new BHoM.Structural.Loads.NodalLoad(loadcase, fx, fy, fz, mx, my, mz);

            force_out.Add("NodalLoad", nodalLoad);
            return force_out;
        }

        /// <summary>
        /// Create a new nodal load with translational (imposed) displacements. 
        /// BuroHappold
        /// </summary>
        /// <param name="node"></param>
        /// <param name="loadcase"></param>
        /// <param name="ux"></param>
        /// <param name="uy"></param>
        /// <param name="uz"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "NodalLoad" })]
        public static Dictionary<string, object> Translation(BHoM.Structural.Node node, BHoM.Structural.Loads.Loadcase loadcase, 
            double ux, double uy, double uz)
        {
            Dictionary<string, object> force_out = new Dictionary<string, object>();
            BHoM.Structural.Loads.NodalLoad nodalLoad = new BHoM.Structural.Loads.NodalLoad();
            nodalLoad.SetTranslation(ux, uy, uz);
            nodalLoad.SetLoadcase(loadcase);
            nodalLoad.AddNodeNumber(node.Number);

            force_out.Add("NodalLoad", nodalLoad);
            return force_out;
        }

    }
}
