using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Graph.Nodes;
using RSE = Revit2016_Adapter.Structural.Elements;

namespace Revit2016.Elements
{
    /// <summary>
    /// </summary>
    public static class Beam
    {
        /// <summary>
        /// </summary>
        public static object ToBHomBar(List<Revit.Elements.StructuralFraming> beams)
        {
            List<Autodesk.Revit.DB.FamilyInstance> converted = new List<Autodesk.Revit.DB.FamilyInstance>();
            foreach (Revit.Elements.StructuralFraming beam in beams)
                converted.Add(beam.InternalElement as Autodesk.Revit.DB.FamilyInstance);
            return RSE.BarIO.RevitColumnsToBHomBars(converted);
        }
    }
}
