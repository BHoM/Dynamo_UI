using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;
using BHP = BH.oM.Structural.Properties;

namespace Structural.Properties
{
    public static class BHPanelProperty
    {
        public static BHP.PanelProperty CreateConstantThickness(string name = "")
        {
            return new BHP.ConstantThickness(name);
        }
    }  
}
