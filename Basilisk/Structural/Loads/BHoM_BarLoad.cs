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
    public static class BHBarLoad
    {
        public static BHL.BarUniformlyDistributedLoad CreateUniformLoad(BHE.Bar bar, DSG.Vector force)
        {
            BHL.BarUniformlyDistributedLoad load = new BHL.BarUniformlyDistributedLoad();
            load.Objects.Add(bar);
            load.ForceVector = new BHG.Vector(force.X, force.Y, force.Z);
            return load;
        }
    }
}
