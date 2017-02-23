using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Graph.Nodes;
using RSE = Revit2017_Adapter.Structural.Elements;

namespace Revit2017.Elements
{
    /// <summary>
    /// 
    /// </summary>
    public static class Piles
    {
        /// <summary>
        /// </summary>
        public static object ToBHomBar(List<Revit.Elements.FamilyInstance> Piles)
        {
            List<Autodesk.Revit.DB.FamilyInstance> converted = new List<Autodesk.Revit.DB.FamilyInstance>();
            foreach (Revit.Elements.FamilyInstance beam in Piles)
                converted.Add(beam.InternalElement as Autodesk.Revit.DB.FamilyInstance);
            return RSE.BarIO.RevitPilesToBHoMBars(converted);
        }
    }
}
