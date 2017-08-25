using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Structural.Elements;
using BHL = BH.oM.Structural.Loads;

namespace Structural.Loads
{
    public static class BHBarLoad
    {
        public static BHL.BarUniformlyDistributedLoad CreateUniformLoad(BHE.Bar bar, DSG.Vector force, DSG.Vector moment, BHL.Loadcase loadcase)
        {
            BHL.BarUniformlyDistributedLoad load = new BHL.BarUniformlyDistributedLoad();
            load.Objects.Data.Add(bar);
            load.Loadcase = loadcase;
            load.ForceVector = new BHG.Vector(force.X, force.Y, force.Z);
            load.MomentVector = new BHG.Vector(moment.X, moment.Y, moment.Z);
            return load;
        }
    }
}
