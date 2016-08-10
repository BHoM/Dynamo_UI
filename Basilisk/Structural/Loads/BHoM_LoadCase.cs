using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHS = BHoM.Structural;

namespace Structural.Loads
{
    public static class BHLoadCase
    {
        public static BHS.Loads.Loadcase CreateLoadCase(string name)
        {
            BHoM.Structural.Loads.Loadcase loadcase = new BHoM.Structural.Loads.Loadcase();
            loadcase.Name = name;
            return loadcase;
        }
    }
}
