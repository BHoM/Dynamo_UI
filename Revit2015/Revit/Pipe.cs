using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Revit.Elements;
using RevitServices.Transactions;

namespace Revit
{
    /// <summary>
    /// A pipe
    /// </summary>
    public static class Pipe
    {
        /// <summary>
        /// Returns MEPSystem of the pipe
        /// </summary>
        /// <param name="Element">Pipe</param>
        /// <returns name="Elements">MEP System</returns>
        /// <search>
        /// Pipe, Pipe MEP System, pipe, pipe mep system
        /// </search>
        public static Elements.Element MEPSystem(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Plumbing.Pipe)
            {
                MechanicalElement aMechanicalElement = new MechanicalElement(Element.InternalElement);
                List<Autodesk.Revit.DB.MEPSystem> aMEPSystems = aMechanicalElement.MEPSystems;
                if (aMEPSystems != null && aMEPSystems.Count > 0)
                    return aMEPSystems.First().ToDSType(true);
            }
            return null;
        }

        /// <summary>
        /// Returns mechanical elements connected to the pipe
        /// </summary>
        /// <param name="Element">Pipe</param>
        /// <returns name="Elements[]">List of connected elements</returns>
        /// <search>
        /// Pipe, Connected Mechanical Elements, pipe, connected mechanical elements
        /// </search>
        public static List<Elements.Element> ConnectedMechanicalElements(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Plumbing.Pipe)
            {
                Autodesk.Revit.DB.MEPCurve aMEPCurve = Element.InternalElement as Autodesk.Revit.DB.MEPCurve;
                if (aMEPCurve != null)
                {
                    MechanicalElement aMEPSystemElement = new MechanicalElement(aMEPCurve);
                    return aMEPSystemElement.ConnectedElements.ToList().ConvertAll(x => x.ToDSType(true));
                }
            }
            return null;
        }

        /// <summary>
        /// Returns width of the pipe
        /// </summary>
        /// <param name="Element">Pipe</param>
        /// <returns name="Width">Width</returns>
        /// <search>
        /// Pipe, Pipe width, pipe, pipe width
        /// </search>
        public static double? Width(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Plumbing.Pipe)
            {
                MEPCurve aMEPCurve = Element.InternalElement as MEPCurve;
                if (aMEPCurve != null)
                    return aMEPCurve.Width;
            }
            return null;
        }

        /// <summary>
        /// Returns length of the pipe
        /// </summary>
        /// <param name="Element">Pipe</param>
        /// <returns name="Length">Length</returns>
        /// <search>
        /// Pipe, Pipe length, pipe, pipe length
        /// </search>
        public static double? Length(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Plumbing.Pipe)
            {
                MEPCurve aMEPCurve = Element.InternalElement as MEPCurve;
                if (aMEPCurve != null)
                {
                    Autodesk.Revit.DB.Parameter aParameter = aMEPCurve.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                    if (aParameter != null)
                        return aParameter.AsDouble();
                }
            }
            return null;
        }

        /// <summary>
        /// Creates pipe insulation
        /// </summary>
        /// <param name="Element">Pipe</param>
        /// <param name="PipeInsulationType">Pipe</param>
        /// <param name="Thickness">Insulation Thickness</param>
        /// <returns name="Element">Pipe Insulation</returns>
        /// <search>
        /// Pipe, Pipe length, pipe, pipe insulation, AddInsulation, add insulation
        /// </search>
        public static Elements.Element AddInsulation(Elements.Element Element, Elements.Element PipeInsulationType, double Thickness)
        {
            TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
            Autodesk.Revit.DB.Plumbing.PipeInsulation aPipeInsulation = Autodesk.Revit.DB.Plumbing.PipeInsulation.Create(Element.InternalElement.Document, Element.InternalElement.Id, PipeInsulationType.InternalElement.Id, Thickness);
            TransactionManager.Instance.TransactionTaskDone();
            return aPipeInsulation.ToDSType(true);
        }

        /// <summary>
        /// Gets pipe insulation
        /// </summary>
        /// <param name="Element">Pipe</param>
        /// <returns name="Elements">Pipe Insulation List</returns>
        /// <search>
        /// Pipe, Pipe length, pipe, pipe insulation, Insulation, insulation
        /// </search>
        public static List<Elements.Element> Insulation(Elements.Element Element)
        {
            Autodesk.Revit.DB.Document aDocument = Element.InternalElement.Document;
            List<ElementId> aPipeInsulationIds = InsulationLiningBase.GetInsulationIds(aDocument, Element.InternalElement.Id).ToList();
            return aPipeInsulationIds.ConvertAll(x => aDocument.GetElement(x).ToDSType(true));
        }
    }
}
