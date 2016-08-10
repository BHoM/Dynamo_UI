using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Revit.Elements;

using Autodesk.Revit.DB.Electrical;

namespace Revit
{
    /// <summary>
    /// A Electrical System
    /// </summary>
    public static class ElectricalSystem
    {
        /// <summary>
        /// Returns all elements connected to the electrical system.
        /// </summary>
        /// <param name="ElectricalSystem">Electrical System</param>
        /// <returns name="Elements">Elements connected to electrical system</returns>
        /// <search>
        /// Electrical system elements, electrical system elements, Electrical System, electrical system
        /// </search>
        public static List<Revit.Elements.Element> Elements(Revit.Elements.Element ElectricalSystem)
        {
            Autodesk.Revit.DB.Electrical.ElectricalSystem aElectricalSystem = ElectricalSystem.InternalElement as Autodesk.Revit.DB.Electrical.ElectricalSystem;
            return aElectricalSystem.Elements.Cast<Autodesk.Revit.DB.Element>().ToList().ConvertAll(x => x.ToDSType(true));
        }
    }
}
