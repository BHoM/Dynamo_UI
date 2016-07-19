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
    public static class BHBarLoad
    {
        public static BHS.Loads.BarUniformlyDistributedLoad CreateUniformLoad(BHS.Bar bar, DSG.Vector force)
        {
            BHS.Loads.BarUniformlyDistributedLoad load = new BHS.Loads.BarUniformlyDistributedLoad();
            load.Objects.Add(bar);
            load.ForceVector = new BHG.Vector(force.X, force.Y, force.Z);
            return load;
        }
    }
}
