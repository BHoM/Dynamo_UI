using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.DesignScript.Geometry;

using RevitServices.Persistence;
using RevitServices.Transactions;
using Revit.Elements;
using Revit.GeometryConversion;
using Autodesk.DesignScript.Runtime;

namespace Revit
{
    /// <summary>
    /// A Element
    /// </summary>
    public static class Element
    {
        /// <summary>
        /// Category of element.
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="Category">Category</returns>
        /// <search>
        /// Element category, element category
        /// </search>
        public static Elements.Category Category(Elements.Element Element)
        {
            if (Element.InternalElement.Category != null)
            {
                Autodesk.Revit.DB.Category aCategory = Element.InternalElement.Category;
                BuiltInCategory aBuiltInCategory = (BuiltInCategory)aCategory.Id.IntegerValue;
                return Elements.Category.ByName(aBuiltInCategory.ToString());
            }
            else if (Element.InternalElement is Autodesk.Revit.DB.Family)
            {
                Autodesk.Revit.DB.Family aFamily = Element.InternalElement as Autodesk.Revit.DB.Family;
                Autodesk.Revit.DB.Category aCategory = aFamily.FamilyCategory;
                BuiltInCategory aBuiltInCategory = (BuiltInCategory)aCategory.Id.IntegerValue;
                return Elements.Category.ByName(aBuiltInCategory.ToString());
            }

            return null;
        }

        /// <summary>
        /// GUID of element.
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="GUID">GUID</returns>
        /// <search>
        /// Element GUID, element guid
        /// </search>
        public static string GUID(Elements.Element Element)
        {
            return Element.InternalElement.UniqueId;
        }

        /// <summary>
        /// Faces of element.
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="ComputeReferences">Compute References</param>
        /// <returns name="Faces">Faces</returns>
        /// <search>
        /// Element faces, element faces
        /// </search>
        public static List<Autodesk.Revit.DB.Face> Faces(Elements.Element Element, bool ComputeReferences = false)
        {
            Autodesk.Revit.DB.Element aElement = Element.InternalElement;
            Options aOptions = new Options();
            aOptions.ComputeReferences = ComputeReferences;
            GeometryElement aGeometryElement = aElement.get_Geometry(aOptions);
            if (aGeometryElement != null)
                return Faces(aGeometryElement);
            return null;
        }

        /// <summary>
        /// Gets element from the model with specified Id value.
        /// </summary>
        /// <param name="Id">Element Id</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Get element, get element
        /// </search>
        public static Elements.Element Get(int Id)
        {
            Autodesk.Revit.DB.Element aElement = DocumentManager.Instance.CurrentDBDocument.GetElement(new ElementId(Id));
            if (aElement != null)
                return ElementWrapper.ToDSType(aElement, true);
            return null;
        }

        /// <summary>
        /// Moves element by specified vector
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="Vector">Vector</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Move element, move element
        /// </search>
        public static Elements.Element Move(Elements.Element Element, Vector Vector)
        {
            TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
            ElementTransformUtils.MoveElement(Element.InternalElement.Document, Element.InternalElement.Id, new XYZ(Vector.X, Vector.Y, Vector.Z));
            TransactionManager.Instance.TransactionTaskDone();
            return Element;
        }

        /// <summary>
        /// gets Element Type of Element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="ElementType">Element Type</returns>
        /// <search>
        /// Get Element Type, get element type
        /// </search>
        public static Elements.Element ElementType(Elements.Element Element)
        {
            return Element.InternalElement.Document.GetElement(Element.InternalElement.GetTypeId()).ToDSType(true);
        }

        /// <summary>
        /// Moves element by specified vector
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="Curve">Curve</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Move element, move element
        /// </search>
        public static Elements.Element Move(Elements.Element Element, Autodesk.DesignScript.Geometry.Curve Curve)
        {
            Autodesk.Revit.DB.Element aElement = Element.InternalElement;
            Location aLocation = aElement.Location;
            if (aLocation != null && aLocation is LocationCurve)
            {
                TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
                LocationCurve aLocationCurve = aLocation as LocationCurve;
                Autodesk.Revit.DB.Curve aNewCurve = Curve.ToRevitType(false);
                aLocationCurve.Curve = aNewCurve;
                TransactionManager.Instance.TransactionTaskDone();
                return Element;
            }
            return null;
        }

        /// <summary>
        /// Deletes element from document
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="Id">Id</returns>
        /// <search>
        /// Delete element, delete element
        /// </search>
        public static int? Delete(Elements.Element Element)
        {
            Autodesk.Revit.DB.Document aDocument = Element.InternalElement.Document;
            if (aDocument != null)
            {
                ElementId aElementId = Element.InternalElement.Id;
                TransactionManager.Instance.EnsureInTransaction(aDocument);
                try
                {
                    aDocument.Delete(aElementId);
                }
                catch
                {

                }
                TransactionManager.Instance.TransactionTaskDone();
                return aElementId.IntegerValue;
            }
            return null;
        }

        /// <summary>
        /// Returns location point of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="Point">Location Point</returns>
        /// <search>
        /// Element location, element location
        /// </search>
        public static Autodesk.DesignScript.Geometry.Point Location(Elements.Element Element)
        {
            if (Element.InternalElement is Autodesk.Revit.DB.Element)
            {
                Autodesk.Revit.DB.Element aElement = Element.InternalElement as Autodesk.Revit.DB.Element;
                if (aElement.Location != null)
                {
                    if (aElement.Location is LocationPoint)
                    {
                        LocationPoint aLocationPoint = aElement.Location as LocationPoint;
                        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(aLocationPoint.Point.X, aLocationPoint.Point.Y, aLocationPoint.Point.Z);
                    }
                    else if (aElement.Location is LocationCurve)
                    {
                        LocationCurve aLocationCurve = aElement.Location as LocationCurve;
                        XYZ aXYZ = aLocationCurve.Curve.GetEndPoint(0);
                        return Autodesk.DesignScript.Geometry.Point.ByCoordinates(aXYZ.X, aXYZ.Y, aXYZ.Z);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns location points of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="Points">Location Points</returns>
        /// <search>
        /// Element locations, element locations
        /// </search>
        public static Autodesk.DesignScript.Geometry.Point[] LocationPoints(Elements.Element Element)
        {
            if (Element.InternalElement is Autodesk.Revit.DB.Element)
            {
                Autodesk.Revit.DB.Element aElement = Element.InternalElement as Autodesk.Revit.DB.Element;
                if (aElement.Location != null)
                {
                    Autodesk.DesignScript.Geometry.Point[] aResult = null;
                    if (aElement.Location is LocationPoint)
                    {
                        aResult = new Autodesk.DesignScript.Geometry.Point[1];
                        LocationPoint aLocationPoint = aElement.Location as LocationPoint;
                        aResult[0] = Autodesk.DesignScript.Geometry.Point.ByCoordinates(aLocationPoint.Point.X, aLocationPoint.Point.Y, aLocationPoint.Point.Z);
                    }
                    else if (aElement.Location is LocationCurve)
                    {
                        aResult = new Autodesk.DesignScript.Geometry.Point[2];
                        LocationCurve aLocationCurve = aElement.Location as LocationCurve;
                        XYZ aXYZ = aLocationCurve.Curve.GetEndPoint(0);
                        aResult[0] = Autodesk.DesignScript.Geometry.Point.ByCoordinates(aXYZ.X, aXYZ.Y, aXYZ.Z);
                        aXYZ = aLocationCurve.Curve.GetEndPoint(1);
                        aResult[1] = Autodesk.DesignScript.Geometry.Point.ByCoordinates(aXYZ.X, aXYZ.Y, aXYZ.Z);
                    }
                    return aResult;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns location curve of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="Curve">Location Curve</returns>
        /// <search>
        /// Element location curve, element location curve
        /// </search>
        public static Autodesk.DesignScript.Geometry.Curve LocationCurve(Elements.Element Element)
        {
            if (Element.InternalElement is Autodesk.Revit.DB.Element)
            {
                Autodesk.Revit.DB.Element aElement = Element.InternalElement as Autodesk.Revit.DB.Element;
                if (aElement.Location != null && aElement.Location is LocationCurve)
                {
                    LocationCurve aLocationCurve = aElement.Location as LocationCurve;
                    return aLocationCurve.Curve.ToProtoType(false);
                }

            }
            return null;
        }

        /// <summary>
        /// Returns level of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="Level">Level</returns>
        /// <search>
        /// Element level, element level
        /// </search>
        public static Elements.Level Level(Elements.Element Element)
        {
            Autodesk.Revit.DB.Document aDocument = Element.InternalElement.Document;
            Autodesk.Revit.DB.Element aElement = aDocument.GetElement(Element.InternalElement.LevelId);
            if (aElement != null)
                return ElementWrapper.ToDSType(aElement, true) as Elements.Level;
            return null;
        }

        /// <summary>
        /// Returns type of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="Type">Type</returns>
        /// <search>
        /// Element Type, element type
        /// </search>
        public static Type Type(Elements.Element Element)
        {
            return Element.InternalElement.GetType();
        }

        /// <summary>
        /// Rotates element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="Angle">Angle</param>
        /// <param name="Vector">Axis</param>
        /// <returns name="Element">Rotated Element</returns>
        /// <search>
        /// Rotate element, rotate element
        /// </search>
        public static Elements.Element Rotate(Elements.Element Element, double Angle, Vector Vector)
        {
            LocationPoint aLocationPoint = Element.InternalElement.Location as LocationPoint;
            XYZ aXYZ_1 = aLocationPoint.Point;
            XYZ aXYZ_2 = new XYZ(aXYZ_1.X + Vector.X, aXYZ_1.Y + Vector.Y, aXYZ_1.Z + Vector.Z);
            Autodesk.Revit.DB.Line aLine = Autodesk.Revit.DB.Line.CreateBound(aXYZ_1, aXYZ_2);
            TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
            Elements.Element aElement = null;
            if (aLocationPoint.Rotate(aLine, Angle))
                aElement = Element;
            TransactionManager.Instance.TransactionTaskDone();
            return aElement;
        }

        /// <summary>
        /// Rotation of Element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="Rotation">Rotation</returns>
        /// <search>
        /// Element Rotation, element rotation
        /// </search>
        public static double Rotation(Elements.Element Element)
        {
            return (Element.InternalElement.Location as LocationPoint).Rotation;
        }

        /// <summary>
        /// Creates tag for element.
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="View">View</param>
        /// <param name="Point">Point</param>
        /// <param name="TagMode">Tag Mode</param>
        /// <param name="AddLeader">Add Leader</param>
        /// <param name="TagOrientation">Tag Orientation</param>
        /// <returns name="Element">Independent Tag</returns>
        /// <search>
        /// Element level, element level
        /// </search>
        public static Elements.Element CreateTag(Elements.Element Element, Elements.Element View, Autodesk.DesignScript.Geometry.Point Point, TagMode TagMode, TagOrientation TagOrientation, bool AddLeader = true)
        {
            Autodesk.Revit.DB.Element aElement = Element.InternalElement;
            Autodesk.Revit.DB.View aView = View.InternalElement as Autodesk.Revit.DB.View;
            Autodesk.Revit.DB.Document aDocument = aElement.Document;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.IndependentTag aIndependentTag = aDocument.Create.NewTag(aView, Element.InternalElement, AddLeader, Functions.GetTagMode(TagMode), Functions.GetTagOrientation(TagOrientation), new XYZ(Point.X, Point.Y, Point.Z));
            TransactionManager.Instance.TransactionTaskDone();
            if (aIndependentTag != null)
                return aIndependentTag.ToDSType(true);
            return null;
        }

        /// <summary>
        /// Overrides cut fill pattern of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="FillPattern">Fill Pattern</param>
        /// <param name="Color">Color</param>
        /// <param name="Visible">Visible</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Element, override cut fill pattern, element, override cut fill pattern
        /// </search>
        public static Elements.Element OverrideCutFillPattern(Elements.Element Element, Elements.Element FillPattern, DSCore.Color Color, bool Visible = true)
        {
            Autodesk.Revit.DB.View aView = DocumentManager.Instance.CurrentUIDocument.ActiveView;

            TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
            ElementId aElementId = Element.InternalElement.Id;
            OverrideGraphicSettings aOverrideGraphicSettings = aView.GetElementOverrides(aElementId);
            aOverrideGraphicSettings.SetCutFillColor(new Color(Color.Red, Color.Green, Color.Blue));
            aOverrideGraphicSettings.SetCutFillPatternId(FillPattern.InternalElement.Id);
            aOverrideGraphicSettings.SetCutFillPatternVisible(Visible);
            aView.SetElementOverrides(aElementId, aOverrideGraphicSettings);
            TransactionManager.Instance.TransactionTaskDone();
            return Element;
        }

        /// <summary>
        /// Overrides cut line pattern of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="LinePattern">Line Pattern</param>
        /// <param name="Color">Color</param>
        /// <param name="Weight">Line Weight</param>
        /// <param name="Halftone">Halftone</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Element, override cut line pattern, element, override cut line pattern
        /// </search>
        public static Elements.Element OverrideCutLinePattern(Elements.Element Element, Elements.Element LinePattern, DSCore.Color Color, int Weight, bool Halftone = false)
        {
            Autodesk.Revit.DB.View aView = DocumentManager.Instance.CurrentUIDocument.ActiveView;

            TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
            ElementId aElementId = Element.InternalElement.Id;
            OverrideGraphicSettings aOverrideGraphicSettings = aView.GetElementOverrides(aElementId);
            aOverrideGraphicSettings.SetCutLineColor(new Color(Color.Red, Color.Green, Color.Blue));
            aOverrideGraphicSettings.SetCutLinePatternId(LinePattern.InternalElement.Id);
            aOverrideGraphicSettings.SetCutLineWeight(Weight);
            aOverrideGraphicSettings.SetHalftone(Halftone);
            aView.SetElementOverrides(aElementId, aOverrideGraphicSettings);
            TransactionManager.Instance.TransactionTaskDone();
            return Element;
        }

        /// <summary>
        /// Overrides projection fill pattern of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="FillPattern">Fill Pattern</param>
        /// <param name="Color">Color</param>
        /// <param name="Visible">Visible</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Element, override projection fill pattern, element, override projection fill pattern
        /// </search>
        public static Elements.Element OverrideProjectionFillPattern(Elements.Element Element, Elements.Element FillPattern, DSCore.Color Color, bool Visible = true)
        {
            Autodesk.Revit.DB.View aView = DocumentManager.Instance.CurrentUIDocument.ActiveView;
            TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
            ElementId aElementId = Element.InternalElement.Id;
            OverrideGraphicSettings aOverrideGraphicSettings = aView.GetElementOverrides(aElementId);
            aOverrideGraphicSettings.SetProjectionFillColor(new Color(Color.Red, Color.Green, Color.Blue));
            aOverrideGraphicSettings.SetProjectionFillPatternId(FillPattern.InternalElement.Id);
            aOverrideGraphicSettings.SetProjectionFillPatternVisible(Visible);
            aView.SetElementOverrides(aElementId, aOverrideGraphicSettings);
            TransactionManager.Instance.TransactionTaskDone();
            return Element;
        }

        /// <summary>
        /// Overrides projection line pattern of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="LinePattern">Fill Pattern</param>
        /// <param name="Color">Color</param>
        /// <param name="Weight">Line Weight</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Element, override projection line pattern, element, override projection line pattern
        /// </search>
        public static Elements.Element OverrideProjectionLinePattern(Elements.Element Element, Elements.Element LinePattern, DSCore.Color Color, int Weight)
        {
            Autodesk.Revit.DB.View aView = DocumentManager.Instance.CurrentUIDocument.ActiveView;

            TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
            ElementId aElementId = Element.InternalElement.Id;
            OverrideGraphicSettings aOverrideGraphicSettings = aView.GetElementOverrides(aElementId);
            aOverrideGraphicSettings.SetProjectionLineColor(new Color(Color.Red, Color.Green, Color.Blue));
            aOverrideGraphicSettings.SetProjectionLinePatternId(LinePattern.InternalElement.Id);
            aOverrideGraphicSettings.SetProjectionLineWeight(Weight);
            aView.SetElementOverrides(aElementId, aOverrideGraphicSettings);
            TransactionManager.Instance.TransactionTaskDone();
            return Element;
        }

        /// <summary>
        /// Gets value of instance parameter for element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="Name">Parameter Name</param>
        /// <returns name="Value">Parameter value</returns>
        /// <search>
        /// Element, Get instance parameter value by name, element, get instance parameter value by name
        /// </search>
        public static object GetInstanceParameterValueByName(Elements.Element Element, string Name)
        {
            return GetParameterValueByName(Element.InternalElement, Element.InternalElement.LookupParameter(Name));
        }

        /// <summary>
        /// Copies Element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="Point">Location Point</param>
        /// <returns name="Elements">Element List</returns>
        /// <search>
        /// Revit, Element, Copy, Copy Element, CopyElement
        /// </search>
        public static List<Elements.Element> Copy(Elements.Element Element, Autodesk.DesignScript.Geometry.Point Point)
        {
            Autodesk.Revit.DB.Document aDocument = Element.InternalElement.Document;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            List<Autodesk.Revit.DB.Element> aElementList = ElementTransformUtils.CopyElement(aDocument, Element.InternalElement.Id, Point.ToRevitType(false)).ToList().ConvertAll(x => aDocument.GetElement(x));
            TransactionManager.Instance.TransactionTaskDone();
            return aElementList.ConvertAll(x => x.ToDSType(true));
        }

        /// <summary>
        /// Gets built in parameter of element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="BuiltInName">Built in name</param>
        /// <returns name="Value">Parameter value</returns>
        /// <search>
        /// Element, Get element built in parameter, element, get element built in parameter
        /// </search>
        public static object GetBuiltInParameterValueByName(Elements.Element Element, string BuiltInName)
        {
            Autodesk.Revit.DB.Element aElement = Element.InternalElement;
            BuiltInParameter aBuiltInParameter;
            Enum.TryParse(BuiltInName, out aBuiltInParameter);
            return GetParameterValueByName(aElement, aElement.get_Parameter(aBuiltInParameter));
        }

        /// <summary>
        /// Gets value of type parameter for element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="Name">Parameter Name</param>
        /// <returns name="Value">Parameter value</returns>
        /// <search>
        /// Element, Get type parameter value by name, element, get type parameter value by name
        /// </search>
        public static object GetTypeParameterValueByName(Elements.Element Element, string Name)
        {
            Autodesk.Revit.DB.Element aElement = Element.InternalElement;
            aElement = aElement.Document.GetElement(aElement.GetTypeId());
            return GetParameterValueByName(aElement, aElement.LookupParameter(Name));
        }

        /// <summary>
        /// Gets Element workset
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="Workset">Workset</returns>
        /// <search>
        /// Element Workset, element workset
        /// </search>
        public static Autodesk.Revit.DB.Workset GetWorkset(Elements.Element Element)
        {
            Autodesk.Revit.DB.Element aElement = Element.InternalElement;
            Autodesk.Revit.DB.Parameter aParameter = aElement.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);
            WorksetId aWorksetId = new WorksetId(aParameter.AsInteger());
            WorksetTable aWorksetTable = Element.InternalElement.Document.GetWorksetTable();
            return aWorksetTable.GetWorkset(aWorksetId);
        }

        /// <summary>
        /// Sets Element workset
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="Workset">Workset</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Element Workset, element workset
        /// </search>
        public static Elements.Element SetWorkset(Elements.Element Element, Autodesk.Revit.DB.Workset Workset)
        {
            Autodesk.Revit.DB.Element aElement = Element.InternalElement;
            Autodesk.Revit.DB.Parameter aParameter = aElement.get_Parameter(BuiltInParameter.ELEM_PARTITION_PARAM);
            TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
            aParameter.Set(Workset.Id.IntegerValue);
            TransactionManager.Instance.TransactionTaskDone();
            return aElement.ToDSType(true);
        }

        /// <summary>
        /// Sets Parameter Value By Name
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="Name">Parameter Name</param>
        /// <param name="Value">Parameter Value</param>
        /// <returns name="Element">Element</returns>
        /// <search>
        /// Set Parameter Value By Name, set parameter value by name 
        /// </search>
        public static Elements.Element SetParameterValueByName(Elements.Element Element, string Name, object Value)
        {
            List<Autodesk.Revit.DB.Parameter> aParameterList = Element.InternalElement.GetParameters(Name).ToList();
            Autodesk.Revit.DB.Parameter aParameter = aParameterList.First();
            
            switch (aParameter.StorageType)
            {
                case StorageType.Double:
                    TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
                    aParameter.Set(Convert.ToDouble(Value));
                    TransactionManager.Instance.TransactionTaskDone();
                    return Element;
                case StorageType.ElementId:
                    TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
                    aParameter.Set(new ElementId(Convert.ToInt32(Value)));
                    TransactionManager.Instance.TransactionTaskDone();
                    return Element;
                case StorageType.Integer:
                    TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
                    aParameter.Set(Convert.ToInt32(Value));
                    TransactionManager.Instance.TransactionTaskDone();
                    return Element;
                case StorageType.String:
                    TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
                    aParameter.Set(Convert.ToString(Value));
                    TransactionManager.Instance.TransactionTaskDone();
                    return Element;
            }
            return null;
        }

        /// <summary>
        /// Gets Analytical Model element
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="AnalyticalModel">Analytical Model Element</returns>
        /// <search>
        /// Element, Analytical Model, AnalyticalModel, analyticalmodel, analytical model
        /// </search>
        public static Elements.Element AnalyticalModel(Elements.Element Element)
        {
            return Element.InternalElement.GetAnalyticalModel().ToDSType(true);
        }

        /// <summary>
        /// Gets Analytical Model Id
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="AnalyticalModel">Analytical Model Id</returns>
        /// <search>
        /// Element, Analytical Model Id, AnalyticalModelId, analyticalmodelid, analytical model id
        /// </search>
        public static int AnalyticalModelId(Elements.Element Element)
        {
            return Element.InternalElement.GetAnalyticalModelId().IntegerValue;
        }

        private static List<Autodesk.Revit.DB.Face> Faces(GeometryElement GeometryElement)
        {
            List<Autodesk.Revit.DB.Face> aFaceList = new List<Autodesk.Revit.DB.Face>();
            foreach (GeometryObject aGeometryObject in GeometryElement)
            {
                if (aGeometryObject is GeometryInstance)
                {
                    GeometryInstance aGeometryInstance = aGeometryObject as GeometryInstance;
                    GeometryElement aGeometryElement = aGeometryInstance.GetSymbolGeometry();
                    if (aGeometryElement != null)
                        aFaceList.AddRange(Faces(aGeometryElement));
                }
                else
                {
                    Autodesk.Revit.DB.Solid aSolid = aGeometryObject as Autodesk.Revit.DB.Solid;
                    if (null != aSolid)
                        aFaceList.AddRange(aSolid.Faces.Cast<Autodesk.Revit.DB.Face>());
                }
            }
            return aFaceList;
        }

        private static object GetParameterValueByName(Autodesk.Revit.DB.Element Element, Autodesk.Revit.DB.Parameter Parameter)
        {
            if (Parameter != null)
            {
                switch (Parameter.StorageType)
                {
                    case StorageType.Double:
                        return Parameter.AsDouble();
                    case StorageType.ElementId:
                        return Element.Document.GetElement(Parameter.AsElementId()).ToDSType(true);
                    case StorageType.Integer:
                        return Parameter.AsInteger();
                    case StorageType.String:
                        return Parameter.AsString();
                }
            }
            return null;
        }
    }
}
