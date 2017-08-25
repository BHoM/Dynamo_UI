using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;
using BHS = BH.oM.Structural;
using BHL = BH.oM.Structural.Loads;
using BHI = BH.oM.Structural.Interface;

namespace Structural.Loads
{
    public static class BHLoadCase
    {
        /// <param name="nature">Dead = 0, SuperDead = 1, Live = 2, Wind = 3, Seismic = 4, Temp = 5, Snow = 6, Accidental = 7, Prestress = 9 , Other = 9</param>
        public static BHS.Loads.Loadcase CreateLoadCase(string name, int nature)
        {
            BH.oM.Structural.Loads.Loadcase loadcase = new BH.oM.Structural.Loads.Loadcase();
            loadcase.Name = name;
            loadcase.Nature = (BHS.Loads.LoadNature)nature;
            return loadcase;
        }
        public static void ExportLoadcase(object app, List<object> loadcases, bool active = true)
        {
            List<BHL.ICase> converted = new List<BHL.ICase>();
            foreach (object load in loadcases)
            {
                if (load is BHL.ICase)
                    converted.Add(load as BHL.ICase);
            }

            if (active)
            {
                BHI.IElementAdapter adapter = app as BHI.IElementAdapter;
                adapter.SetLoadcases(converted);
            }
        }
    }
}
