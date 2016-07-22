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
    public static class Column
    {
        /// <summary>
        /// </summary>
        [NodeCategory("Basilisk.Revit2016")]
        public static object ToBHomBar(object column)
        {
            if (column is Revit.Elements.StructuralFraming)
                return RevitToolkit.Elements.Column.ToBHomBar((column as Revit.Elements.StructuralFraming).InternalElement as Autodesk.Revit.DB.FamilyInstance);
            else
                return null;
        }
    }
}
