using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using Revit.Elements;
using Revit.GeometryConversion;
using RevitServices.Transactions;

namespace Revit
{
    /// <summary>
    /// A duct
    /// </summary>
    public static class Duct
    {
        /// <summary>
        /// Returns MEPSystem of the duct
        /// </summary>
        /// <param name="Element">Duct</param>
        /// <returns name="Element">MEP System</returns>
        /// <search>
        /// Duct, Duct MEP System, duct, cuct mep system
        /// </search>
        public static Elements.Element MEPSystem(Elements.Element Element)
        {
            if (Element.InternalElement is Autodesk.Revit.DB.Mechanical.Duct)
            {
                MechanicalElement aMechanicalElement = new MechanicalElement(Element.InternalElement);
                List<Autodesk.Revit.DB.MEPSystem> aMEPSystems = aMechanicalElement.MEPSystems;
                if (aMEPSystems != null && aMEPSystems.Count > 0)
                    return aMEPSystems.First().ToDSType(true);
            }
            return null;
        }

        /// <summary>
        /// Creates duct
        /// </summary>
        /// <param name="StartPoint">Start Point</param>
        /// <param name="EndPoint">End Point</param>
        /// <param name="DuctType">Duct Type</param>
        /// <returns name="Duct">Duct</returns>
        /// <search>
        /// Duct, duct, ByTwoPoints, By Two Points
        /// </search>
        public static Elements.Element ByTwoPoints(Autodesk.DesignScript.Geometry.Point StartPoint, Autodesk.DesignScript.Geometry.Point EndPoint, Elements.Element DuctType)
        {
            Autodesk.Revit.DB.Document aDocument = DuctType.InternalElement.Document;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.Mechanical.Duct aDuct = aDocument.Create.NewDuct(StartPoint.ToXyz(false), EndPoint.ToXyz(false), DuctType.InternalElement as Autodesk.Revit.DB.Mechanical.DuctType);
            TransactionManager.Instance.TransactionTaskDone();
            return aDuct.ToDSType(true);
        }

        /// <summary>
        /// Returns mechanical elements connected to the duct
        /// </summary>
        /// <param name="Element">Duct</param>
        /// <returns name="Elements">List of connected elements</returns>
        /// <search>
        /// Duct, Connected Mechanical Elements, duct, connected mechanical elements
        /// </search>
        public static List<Elements.Element> ConnectedMechanicalElements(Elements.Element Element)
        {
            if (Element.InternalElement is Autodesk.Revit.DB.Mechanical.Duct)
            {
                MEPCurve aMEPCurve = Element.InternalElement as MEPCurve;
                MechanicalElement aMEPSystemElement = new MechanicalElement(aMEPCurve);
                return aMEPSystemElement.ConnectedElements.ToList().ConvertAll(x => x.ToDSType(true));
            }
            return null;
        }

        /// <summary>
        /// Returns width of the duct
        /// </summary>
        /// <param name="Element">Duct</param>
        /// <returns name="Width">Width</returns>
        /// <search>
        /// Duct, Duct width, duct, duct width
        /// </search>
        public static double? Width(Elements.Element Element)
        {
            if (Element.InternalElement is Autodesk.Revit.DB.Mechanical.Duct)
            {
                MEPCurve aMEPCurve = Element.InternalElement as MEPCurve;
                return aMEPCurve.Width;
            }
            return null;
        }

        /// <summary>
        /// Returns length of the duct
        /// </summary>
        /// <param name="Element">Duct</param>
        /// <returns name="Length">Length</returns>
        /// <search>
        /// Duct, Duct length, duct, duct length
        /// </search>
        public static double? Length(Elements.Element Element)
        {
            if (Element.InternalElement is Autodesk.Revit.DB.Mechanical.Duct)
            {
                MEPCurve aMEPCurve = Element.InternalElement as MEPCurve;
                Autodesk.Revit.DB.Parameter aParameter = aMEPCurve.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                if (aParameter != null)
                    return aParameter.AsDouble();
            }
            return null;
        }
    }
}
