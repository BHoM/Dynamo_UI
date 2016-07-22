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
        public static object ToBHomPanel(object floor)
        {
            if (floor is Revit.Elements.Floor)
                return RevitToolkit.Elements.Floor.ToBHomPanel((floor as Revit.Elements.Floor).InternalElement as Autodesk.Revit.DB.Floor);
            else
                return null;
        }
    }
}
