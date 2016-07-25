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
    public static class Wall
    {
        /// <summary>
        /// </summary>
        [NodeCategory("Basilisk.Revit2016")]
        public static object ToBHomPanel(List<Revit.Elements.Wall> walls)
        {
            List<Autodesk.Revit.DB.Wall> converted = new List<Autodesk.Revit.DB.Wall>();
            foreach (Revit.Elements.Wall wall in walls)
                converted.Add(wall.InternalElement as Autodesk.Revit.DB.Wall);
            return Revit2016IO.PanelIO.RevitWallsToBHoMPanels(converted);
        }
    }
}
