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
    /// 
    /// </summary>
    public static class Foundation
    {
        /// <summary>
        /// </summary>
        public static object ToBHomPanel(List<Revit.Elements.FamilyInstance> Foundations)
        {
            List<Autodesk.Revit.DB.FamilyInstance> converted = new List<Autodesk.Revit.DB.FamilyInstance>();
            foreach (Revit.Elements.FamilyInstance floor in Foundations)
                converted.Add(floor.InternalElement as Autodesk.Revit.DB.FamilyInstance);
            return RSE.PanelIO.RevitFoundationsToBHoMPanels(converted);
        }
    }
}
