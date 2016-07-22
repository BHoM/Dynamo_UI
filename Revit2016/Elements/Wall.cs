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
        public static object ToBHomPanel(object wall)
        {
            if (wall is Revit.Elements.Wall)
                return RevitToolkit.Elements.Wall.ToBHomPanel((wall as Revit.Elements.Wall).InternalElement as Autodesk.Revit.DB.Wall);
            else
                return null;
        }
    }
}
