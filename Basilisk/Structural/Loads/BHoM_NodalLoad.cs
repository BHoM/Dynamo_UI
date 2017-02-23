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
    public static class BHNodalLoad
    {
        public static BHL.PointForce NodeForceLoad(BHE.Node node, DSG.Vector pointforce, DSG.Vector pointmoment, BHL.Loadcase loadcase)
        //public static BHL.BarUniformlyDistributedLoad CreateUniformLoad(BHE.Bar bar, DSG.Vector force, BHL.Loadcase loadcase)
        {
            BHL.PointForce load = new BHL.PointForce();
            load.Objects.Data.Add(node);
            load.Loadcase = loadcase;
            load.Force = new BHG.Vector(pointforce.X, pointforce.Y, pointforce.Z);
            load.Moment = new BHG.Vector(pointmoment.X, pointmoment.Y, pointforce.Z);
            return load;
        }
    }
}
