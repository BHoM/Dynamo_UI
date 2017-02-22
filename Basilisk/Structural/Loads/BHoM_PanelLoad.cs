using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHE = BHoM.Structural.Elements;
using BHL = BHoM.Structural.Loads;

namespace Structural.Loads
{
    public static class BHPanelLoad
    {
        public static BHL.AreaUniformalyDistributedLoad CreateUniformLoad(BHE.Panel panel, BHL.Loadcase loadcase, DSG.Vector force)
        {
            return new BHL.AreaUniformalyDistributedLoad(loadcase, force.X, force.Y, force.Z);
        }
    }
}
