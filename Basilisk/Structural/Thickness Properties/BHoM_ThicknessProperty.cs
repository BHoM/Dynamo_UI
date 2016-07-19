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
    public static class BHThicknessProperty
    {
        public static BHS.ConstantThickness CreateConstantThickness(string name = "")
        {
            return new BHS.ConstantThickness(name);
        }
    }  
}
