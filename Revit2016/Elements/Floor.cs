using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Graph.Nodes;

namespace Revit2016.Elements
{
    /// <summary>
    /// </summary>
    public static class Floor
    {
        /// <summary>
        /// </summary>
        [NodeCategory("Basilisk.Revit2016")]
        public static object ToBHomPanel(List<Revit.Elements.Floor> floors)
        {
            List<Autodesk.Revit.DB.Floor> converted = new List<Autodesk.Revit.DB.Floor>();
            foreach (Revit.Elements.Floor floor in floors)
                converted.Add(floor.InternalElement as Autodesk.Revit.DB.Floor);
            return Revit2016IO.PanelIO.RevitSlabsToBHoMPanels(converted);
        }
    }
}
