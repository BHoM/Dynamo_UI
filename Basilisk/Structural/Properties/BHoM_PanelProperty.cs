using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHP = BHoM.Structural.Properties;

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
