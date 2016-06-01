using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Revit.Elements;
using Revit.GeometryConversion;

namespace Revit
{
    /// <summary>
    /// A ROpeevit Opening
    /// </summary>
    public static class Opening
    {
        /// <summary>
        /// Gets the host of opening
        /// </summary>
        /// <param name="Element">Opening</param>
        /// <returns name="Host">Host</returns>
        /// <search>
        /// Host, Opening, host, opening
        /// </search>
        public static Revit.Elements.Element Host(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Opening)
            {
                Autodesk.Revit.DB.Opening aOpening = Element.InternalElement as Autodesk.Revit.DB.Opening;
                return aOpening.Host.ToDSType(true);
            }
            return null;
        }

        /// <summary>
        /// Gets boundaries of opening
        /// </summary>
        /// <param name="Element">Opening</param>
        /// <returns name="Boundaries">List of boundary curves</returns>
        /// <search>
        /// Opening, Boundaries, opening, boundaries
        /// </search>
        public static List<Autodesk.DesignScript.Geometry.Curve> Boundaries(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Opening)
            {
                Autodesk.Revit.DB.Opening aOpening = Element.InternalElement as Autodesk.Revit.DB.Opening;
                List<Autodesk.DesignScript.Geometry.Curve> aResult = new List<Autodesk.DesignScript.Geometry.Curve>();
                foreach (Autodesk.Revit.DB.Curve aCurve in aOpening.BoundaryCurves)
                    if (aCurve != null)
                        aResult.Add(aCurve.ToProtoType(false, null));
                return aResult;
            }
            return null;
        }
    }
}
