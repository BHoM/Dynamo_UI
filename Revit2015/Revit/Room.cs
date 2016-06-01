using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using RevitServices.Persistence;
using RevitServices.Transactions;
using Revit.Elements;
using Autodesk.DesignScript.Runtime;


namespace Revit
{
    /// <summary>
    /// A room
    /// </summary>
    public static class Room
    {
        /// <summary>
        /// get room at specified point.
        /// </summary>
        /// <param name="Point">Point</param>
        /// <returns name="Element">Room</returns>
        /// <search>
        /// Room, Get At Point, room, get at point
        /// </search>
        public static Elements.Element Get(Autodesk.DesignScript.Geometry.Point Point)
        {
            if (Point != null)
            {
                Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
                if (aDocument != null)
                {
                    Autodesk.Revit.DB.Architecture.Room aResult = aDocument.GetRoomAtPoint(new XYZ(Point.X, Point.Y, Point.Z));
                    if (aResult != null)
                        return aResult.ToDSType(true);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets room boundary elements and curves.
        /// </summary>
        /// <param name="Element">Room</param>
        /// <param name="BoundaryLocation">Boundary Location</param>
        /// <returns name="Elements">Lists of boundary elements</returns>
        /// <returns name="Curves">Lists of boundary curves</returns>
        /// <search>
        /// Room, Get room boundary elements and curves, room, get room boundary elements and curves
        /// </search>
        [MultiReturn(new[] { "Elements", "Curves" })]
        public static Dictionary<string, object> Boundaries(Revit.Elements.Element Element, BoundaryLocation BoundaryLocation)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Architecture.Room)
            {
                List<Revit.Elements.Element> aElementList = null;
                List<Autodesk.DesignScript.Geometry.Curve> aCurveList = null;

                Functions.GetBoundaries(Element.InternalElement as SpatialElement, BoundaryLocation, out aCurveList, out aElementList);
                
                return new Dictionary<string, object>
                {
                    { "Elements", aElementList},
                    { "Curves", aCurveList}
                };
            }
            return null;
        }
    }
}
