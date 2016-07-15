using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHS = BHoM.Structural;

namespace Structural
{
    public static class PanelLoad
    {
        public static BHS.Loads.AreaUniformalyDistributedLoad CreateUniformLoad(BHS.Panel panel, DSG.Vector force)
        {
            return new BHS.Loads.AreaUniformalyDistributedLoad(force.X, force.Y, force.Z);
        }
    }
}
