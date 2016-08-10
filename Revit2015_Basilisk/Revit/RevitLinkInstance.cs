using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revit.GeometryConversion;

using Revit.Elements;
using Autodesk.DesignScript.Runtime;

namespace Revit
{
    /// <summary>
    /// A Revit Link Instance
    /// </summary>
    public static class RevitLinkInstance
    {

        /// <summary>
        /// Returns elements of specified category from Revit Link Instance
        /// </summary>
        /// <param name="Element">Revit Link Instance</param>
        /// <param name="Category">Category</param>
        /// <returns name="Elements">List of elements</returns>
        /// <search>
        /// Revit Link Instance, Get Elements, revit link instance, get elements
        /// </search>
        public static List<Elements.Element> GetElements(Elements.Element Element, Category Category)
        {
            Autodesk.Revit.DB.RevitLinkInstance aRevitLinkInstance = Element.InternalElement as Autodesk.Revit.DB.RevitLinkInstance;
            Autodesk.Revit.DB.Document aLinkDocument = aRevitLinkInstance.GetLinkDocument();
            List<Autodesk.Revit.DB.Element> aElementList = new Autodesk.Revit.DB.FilteredElementCollector(aLinkDocument).OfCategory((Autodesk.Revit.DB.BuiltInCategory)Category.Id).ToList();
            return aElementList.ConvertAll(x => x.ToDSType(true));
        }

        /// <summary>
        /// Returns elements of specified type from Revit Link Instance
        /// </summary>
        /// <param name="Element">Revit Link Instance</param>
        /// <param name="Type">Type</param>
        /// <returns name="Elements">List of elements</returns>
        /// <search>
        /// Revit Link Instance, Get Elements, revit link instance, get elements
        /// </search>
        public static List<Elements.Element> GetElements(Elements.Element Element, Type Type)
        {
            Autodesk.Revit.DB.RevitLinkInstance aRevitLinkInstance = Element.InternalElement as Autodesk.Revit.DB.RevitLinkInstance;
            Autodesk.Revit.DB.Document aLinkDocument = aRevitLinkInstance.GetLinkDocument();
            List<Autodesk.Revit.DB.Element> aElementList = new Autodesk.Revit.DB.FilteredElementCollector(aLinkDocument).OfClass(Type).ToList();
            return aElementList.ConvertAll(x => x.ToDSType(true));
        }

        /// <summary>
        /// Returns elements from Revit Link Instance
        /// </summary>
        /// <param name="Element">Revit Link Instance</param>
        /// <param name="ElementIds">Element Id List</param>
        /// <returns name="Elements">Element List</returns>
        /// <search>
        /// Revit Link Instance, Get Elements, revit link instance, get elements
        /// </search>
        public static List<Elements.Element> GetElements(Elements.Element Element, List<int> ElementIds)
        {
            Autodesk.Revit.DB.RevitLinkInstance aRevitLinkInstance = Element.InternalElement as Autodesk.Revit.DB.RevitLinkInstance;
            Autodesk.Revit.DB.Document aLinkDocument = aRevitLinkInstance.GetLinkDocument();
            return ElementIds.ConvertAll(x => aLinkDocument.GetElement(new Autodesk.Revit.DB.ElementId(x)).ToDSType(true));
        }

        /// <summary>
        /// Returns element from Revit Link Instance
        /// </summary>
        /// <param name="Element">Revit Link Instance</param>
        /// <param name="ElementId">Element Id</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Revit Link Instance, Get Element, revit link instance, get element
        /// </search>
        public static Elements.Element GetElement(Elements.Element Element, int ElementId)
        {
                Autodesk.Revit.DB.RevitLinkInstance aRevitLinkInstance = Element.InternalElement as Autodesk.Revit.DB.RevitLinkInstance;
                Autodesk.Revit.DB.Document aLinkDocument = aRevitLinkInstance.GetLinkDocument();
                return aLinkDocument.GetElement(new Autodesk.Revit.DB.ElementId(ElementId)).ToDSType(true);
         }

        /// <summary>
        /// Returns total tranform of Revit Link Instance
        /// </summary>
        /// <param name="Element">Revit Link Instance</param>
        /// <returns name="Transform">Total Transform</returns>
        /// <search>
        /// Revit Link Instance, GetTotalTransform, Revit, Get Total Transform
        /// </search>
        public static Autodesk.Revit.DB.Transform GetTotalTransform(Elements.Element Element)
        {
            Autodesk.Revit.DB.RevitLinkInstance aRevitLinkInstance = Element.InternalElement as Autodesk.Revit.DB.RevitLinkInstance;
            return aRevitLinkInstance.GetTotalTransform();
        }

        /// <summary>
        /// Returns path of Revit Link Instance
        /// </summary>
        /// <param name="Element">Revit Link Instance</param>
        /// <returns name="Name">Name of Revit Link Instance</returns>
        /// <search>
        /// Revit Link Instance path, revit link instance path
        /// </search>
        public static string Path(Elements.Element Element)
        {
            Autodesk.Revit.DB.RevitLinkInstance aRevitLinkInstance = Element.InternalElement as Autodesk.Revit.DB.RevitLinkInstance;
            return aRevitLinkInstance.GetLinkDocument().PathName;
        }

        /// <summary>
        /// Returns elements intersected with specified elements from Revit Link Instance Document
        /// </summary>
        /// <param name="RevitLinkInstance">Revit Link Instance</param>
        /// <param name="LinkElementIds">Element from Revit Link Instance Document to be investigated</param>
        /// <param name="Elements">Elements from current document to be investigated</param>
        /// <param name="GetSolids">If true then intersection solids will be calculated</param>
        /// <returns name="LinkElementIds">Elements Id from Revit Link document intersection has been found</returns>
        /// <returns name="Elements">current model elements intersection has been found</returns>
        /// <returns name="Solids">Intersection slids</returns>
        /// <search>
        /// Element, Element intersects, element, element intersects, RevitLinkInstance, Revit Link Instance
        /// </search>
        [MultiReturn(new[] { "LinkElementIds", "Elements", "Solids" })]
        public static Dictionary<string, object> Intersects(Elements.Element RevitLinkInstance, List<int> LinkElementIds, List<Elements.Element> Elements, bool GetSolids = true)
        {
            Autodesk.Revit.DB.RevitLinkInstance aRevitLinkInstance = RevitLinkInstance.InternalElement as Autodesk.Revit.DB.RevitLinkInstance;
            Autodesk.Revit.DB.Document aDocument = aRevitLinkInstance.Document;
            Autodesk.Revit.DB.Document aLinkDocument = aRevitLinkInstance.GetLinkDocument();

            List<Autodesk.Revit.DB.ElementId> aLinkElementIdList = new List<Autodesk.Revit.DB.ElementId>();
            foreach (int aId in LinkElementIds)
            {
                Autodesk.Revit.DB.ElementId aElementId = new Autodesk.Revit.DB.ElementId(aId);
                if (Autodesk.Revit.DB.ElementId.InvalidElementId != aElementId)
                {
                    Autodesk.Revit.DB.Element aElement = aLinkDocument.GetElement(aElementId);
                    if (aElement != null)
                        aLinkElementIdList.Add(aElement.Id);
                }
            }

            Autodesk.Revit.DB.Transform aLinkTransform = aRevitLinkInstance.GetTotalTransform();
            List<Autodesk.Revit.DB.ElementId> aElementIdList = Elements.ConvertAll(x => x.InternalElement.Id);

            List<Intersection> aIntersectionList = new List<Intersection>();

            foreach (Autodesk.Revit.DB.ElementId aElementId in aLinkElementIdList)
            {
                Autodesk.Revit.DB.Element aElement = aLinkDocument.GetElement(aElementId);
                Autodesk.Revit.DB.BoundingBoxXYZ aBoundingBoxXYZ = aElement.get_BoundingBox(null);
                if (aBoundingBoxXYZ != null)
                {
                    Autodesk.Revit.DB.XYZ aTransformedPoint_1 = aBoundingBoxXYZ.Transform.Multiply(aLinkTransform).OfPoint(aBoundingBoxXYZ.Min);
                    Autodesk.Revit.DB.XYZ aTransformedPoint_2 = aBoundingBoxXYZ.Transform.Multiply(aLinkTransform).OfPoint(aBoundingBoxXYZ.Max);
                    Autodesk.Revit.DB.XYZ aMinPoint = new Autodesk.Revit.DB.XYZ(Math.Min(aTransformedPoint_1.X, aTransformedPoint_2.X), Math.Min(aTransformedPoint_1.Y, aTransformedPoint_2.Y), Math.Min(aTransformedPoint_1.Z, aTransformedPoint_2.Z));
                    Autodesk.Revit.DB.XYZ aMaxPoint = new Autodesk.Revit.DB.XYZ(Math.Max(aTransformedPoint_1.X, aTransformedPoint_2.X), Math.Max(aTransformedPoint_1.Y, aTransformedPoint_2.Y), Math.Max(aTransformedPoint_1.Z, aTransformedPoint_2.Z));

                    Autodesk.Revit.DB.Outline aOutline = new Autodesk.Revit.DB.Outline(aMinPoint, aMaxPoint);
                    if (!aOutline.IsEmpty)
                    {
                        Autodesk.Revit.DB.LogicalOrFilter aLogicalOrFilter = new Autodesk.Revit.DB.LogicalOrFilter(new Autodesk.Revit.DB.BoundingBoxIntersectsFilter(aOutline), new Autodesk.Revit.DB.BoundingBoxIsInsideFilter(aOutline));
                        List<Autodesk.Revit.DB.ElementId> aIntersectedElementIdList = new Autodesk.Revit.DB.FilteredElementCollector(aDocument, aElementIdList).WherePasses(aLogicalOrFilter).ToElementIds().ToList();
                        if (aIntersectedElementIdList.Count > 0)
                        {
                            List<Autodesk.Revit.DB.Solid> aSolidList = Intersection.GetSolidList(aElement, aLinkTransform);
                            foreach (Autodesk.Revit.DB.Solid aSolid in aSolidList)
                            {
                                List<Autodesk.Revit.DB.Element> aElementList = new Autodesk.Revit.DB.FilteredElementCollector(aDocument, aIntersectedElementIdList).WherePasses(new Autodesk.Revit.DB.ElementIntersectsSolidFilter(aSolid)).ToList();
                                foreach (Autodesk.Revit.DB.Element aIntersectedElement in aElementList)
                                {
                                    Intersection aIntersection = new Intersection(aIntersectedElement, aElement);
                                    if(GetSolids)
                                        aIntersection.AddIntersections(aSolid);
                                    aIntersectionList.Add(aIntersection);
                                }
                            }
                        }
                    }
                }
            }

            List<int> aResultElementIdList = new List<int>();
            List<Elements.Element> aResultElementList = new List<Elements.Element>();
            List<List<Autodesk.DesignScript.Geometry.Solid>> aResultSolidLists = new List<List<Autodesk.DesignScript.Geometry.Solid>>();
            foreach (Intersection aIntersection in aIntersectionList)
            {
                aResultElementIdList.Add(aIntersection.LinkElement.Id.IntegerValue);
                aResultElementList.Add(aIntersection.Element.ToDSType(true));
                aResultSolidLists.Add(aIntersection.SolidList.ConvertAll(x => x.ToProtoType(false)));
            }

            return new Dictionary<string, object>
                {
                    { "LinkElementIds", aResultElementIdList},
                    { "Elements", aResultElementList},
                    { "Solids", aResultSolidLists}
                };
        }
    }
}
