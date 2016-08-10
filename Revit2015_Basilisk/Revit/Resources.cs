using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using RevitServices.Persistence;
using Revit.Elements;
using Autodesk.DesignScript.Runtime;
using Revit.GeometryConversion;

namespace Revit
{

    /// <summary>
    /// Tag Mode
    /// </summary>
    public enum TagMode
    {
        /// <summary>By Category</summary>
        ByCategory,
        /// <summary>By Material</summary>
        ByMaterial,
        /// <summary>By Multi-Category</summary>
        ByMultiCategory
    }

    /// <summary>
    /// Tag Orientation
    /// </summary>
    public enum TagOrientation
    {
        /// <summary>Vertical</summary>
        Vertical,
        /// <summary>Horizontal</summary>
        Horizontal
    }

    /// <summary>
    /// Print Range
    /// </summary>
    public enum PrintRange
    {
        /// <summary>Current</summary>
        Current,
        /// <summary>Select</summary>
        Select,
        /// <summary>Visible</summary>
        Visible
    }

    /// <summary>
    /// Boundary Location
    /// </summary>
    public enum BoundaryLocation
    {
        /// <summary>Center</summary>
        Center,
        /// <summary>Core Boundary</summary>
        CoreBoundary,
        /// <summary>Core Center</summary>
        CoreCenter,
        /// <summary>Finish</summary>
        Finish
    }

    /// <summary>
    /// Space Occupancy Unit
    /// </summary>
    public enum OccupancyUnit
    {
        /// <summary>Number Of People</summary>
        NumberOfPeople,
        /// <summary>Area Per Person</summary>
        AreaPerPerson,
        /// <summary>Default</summary>
        Default
    }

    /// <summary>
    /// Loss method
    /// </summary>
    public enum LossMethod
    {
        /// <summary>Coefficient from ASHRAE Table</summary>
        ASHRAETable,
        /// <summary>Not Defined</summary>
        NotDefined,
        /// <summary>Specific Coefficient</summary>
        SpecificCoefficient,
        /// <summary>Specific Loss</summary>
        SpecificLoss
    }

    /// <summary>
    /// Workset Kind
    /// </summary>
    public enum WorksetKind
    {
        /// <summary>User Workset</summary>
        UserWorkset,
        /// <summary>Family Workset</summary>
        FamilyWorkset,
        /// <summary>Other Workset</summary>
        OtherWorkset,
        /// <summary>Standard Workset</summary>
        StandardWorkset,
        /// <summary>View Workset</summary>
        ViewWorkset
    }

    internal static class Functions
    {
        internal static Autodesk.Revit.DB.TagMode GetTagMode(TagMode TagMode)
        {
            switch (TagMode)
            {
                case TagMode.ByCategory:
                    return Autodesk.Revit.DB.TagMode.TM_ADDBY_CATEGORY;
                case Revit.TagMode.ByMaterial:
                    return Autodesk.Revit.DB.TagMode.TM_ADDBY_MATERIAL;
                case Revit.TagMode.ByMultiCategory:
                    return Autodesk.Revit.DB.TagMode.TM_ADDBY_MULTICATEGORY;
            }
            return Autodesk.Revit.DB.TagMode.TM_ADDBY_MULTICATEGORY;
        }

        internal static Autodesk.Revit.DB.TagOrientation GetTagOrientation(TagOrientation TagOrienation)
        {
            switch (TagOrienation)
            {
                case Revit.TagOrientation.Vertical:
                    return Autodesk.Revit.DB.TagOrientation.Vertical;
                case Revit.TagOrientation.Horizontal:
                    return Autodesk.Revit.DB.TagOrientation.Horizontal;
            }
            return Autodesk.Revit.DB.TagOrientation.Horizontal;
        }

        internal static LossMethod? GetLossMethod(string Value)
        {
            foreach(LossMethod aLossMethod in Enum.GetValues(typeof(LossMethod)))
            {
                string aValue = GetLossMethod(aLossMethod);
                if (aValue == Value)
                    return aLossMethod;
            }
            return null;
        }

        internal static string GetLossMethod(LossMethod LossMethod)
        {
            switch(LossMethod)
            {
                case Revit.LossMethod.ASHRAETable:
                    return "8baf7d75-8b9b-46d0-b8ce-3ad1c19e6b19";
                case Revit.LossMethod.NotDefined:
                    return "76eff5da-2e71-45f7-b940-cc5716328ba0";
                case Revit.LossMethod.SpecificCoefficient:
                    return "5a598293-1504-46cc-a9c0-de55c82848b9";
                case Revit.LossMethod.SpecificLoss:
                    return "46245996-eebb-4536-ac17-9c1cd917d8cf";
            }
            return null;
        }

        internal static SpatialElementBoundaryLocation GetSpatialElementBoundaryLocation(BoundaryLocation BoundaryLocation)
        {
            switch(BoundaryLocation)
            {
                case Revit.BoundaryLocation.Center:
                    return SpatialElementBoundaryLocation.Center;
                case Revit.BoundaryLocation.CoreBoundary:
                    return SpatialElementBoundaryLocation.CoreBoundary;
                case Revit.BoundaryLocation.CoreCenter:
                    return SpatialElementBoundaryLocation.CoreCenter;
                case Revit.BoundaryLocation.Finish:
                    return SpatialElementBoundaryLocation.Finish;
                default:
                    return SpatialElementBoundaryLocation.Center;
            }
        }

        internal static Autodesk.Revit.DB.Mechanical.OccupancyUnit GetOccupancyUnit(OccupancyUnit OccupancyUnit)
        {
            switch (OccupancyUnit)
            {
                case Revit.OccupancyUnit.AreaPerPerson:
                    return Autodesk.Revit.DB.Mechanical.OccupancyUnit.AreaPerPerson;
                case Revit.OccupancyUnit.Default:
                    return Autodesk.Revit.DB.Mechanical.OccupancyUnit.UseDefaultValues;
                case Revit.OccupancyUnit.NumberOfPeople:
                    return Autodesk.Revit.DB.Mechanical.OccupancyUnit.NumberOfPeople;
                default:
                    return Autodesk.Revit.DB.Mechanical.OccupancyUnit.UseDefaultValues;
            }
        }

        internal static Autodesk.Revit.DB.PrintRange GetPrintRange(PrintRange PrintRange)
        {
            switch (PrintRange)
            {
                case PrintRange.Current:
                    return Autodesk.Revit.DB.PrintRange.Current;
                case PrintRange.Visible:
                    return Autodesk.Revit.DB.PrintRange.Visible;
                case PrintRange.Select:
                    return Autodesk.Revit.DB.PrintRange.Select;
                default:
                    return Autodesk.Revit.DB.PrintRange.Visible;
            }
        }

        internal static Autodesk.Revit.DB.WorksetKind GetWorksetKind(WorksetKind WorksetKind)
        {
            switch (WorksetKind)
            {
                case WorksetKind.FamilyWorkset:
                    return Autodesk.Revit.DB.WorksetKind.FamilyWorkset;
                case WorksetKind.OtherWorkset:
                    return Autodesk.Revit.DB.WorksetKind.OtherWorkset;
                case WorksetKind.StandardWorkset:
                    return Autodesk.Revit.DB.WorksetKind.StandardWorkset;
                case WorksetKind.UserWorkset:
                    return Autodesk.Revit.DB.WorksetKind.UserWorkset;
                case WorksetKind.ViewWorkset:
                    return Autodesk.Revit.DB.WorksetKind.ViewWorkset;
                default:
                    return Autodesk.Revit.DB.WorksetKind.UserWorkset;
            }
        }
        
        internal static List<Elements.Element> GetBoundaryElements(Autodesk.Revit.DB.SpatialElement SpatialElement)
        {
            List<Revit.Elements.Element> aResult = new List<Revit.Elements.Element>();
            if (SpatialElement != null)
            {
                IList<IList<Autodesk.Revit.DB.BoundarySegment>> aBoundarySegmentListList = SpatialElement.GetBoundarySegments(new SpatialElementBoundaryOptions());
                if (aBoundarySegmentListList != null)
                    foreach (IList<Autodesk.Revit.DB.BoundarySegment> aBoundarySegmentList in aBoundarySegmentListList)
                        foreach (Autodesk.Revit.DB.BoundarySegment aBoundarySegment in aBoundarySegmentList)
                        {
                            int aId = aBoundarySegment.Element.Id.IntegerValue;
                            Revit.Elements.Element aElement = aResult.Find(x => x.Id == aId);
                            if (aElement == null)
                                aResult.Add(aBoundarySegment.Element.ToDSType(true));
                        }
                return aResult;
            }
            return null;
        }

        internal static List<Autodesk.DesignScript.Geometry.Curve> GetBoundaryCurves(Autodesk.Revit.DB.SpatialElement SpatialElement, BoundaryLocation BoundaryLocation)
        {
            if (SpatialElement != null)
            {
                SpatialElementBoundaryOptions aSpatialElementBoundaryOptions = new SpatialElementBoundaryOptions();
                aSpatialElementBoundaryOptions.SpatialElementBoundaryLocation = GetSpatialElementBoundaryLocation(BoundaryLocation);
                IList<IList<Autodesk.Revit.DB.BoundarySegment>> aBoundarySegmentListList = SpatialElement.GetBoundarySegments(aSpatialElementBoundaryOptions);
                List<Autodesk.DesignScript.Geometry.Curve> aResult = new List<Autodesk.DesignScript.Geometry.Curve>();
                if (aBoundarySegmentListList != null)
                    foreach (IList<Autodesk.Revit.DB.BoundarySegment> aBoundarySegmentList in aBoundarySegmentListList)
                        foreach (Autodesk.Revit.DB.BoundarySegment aBoundarySegment in aBoundarySegmentList)
                            aResult.Add( aBoundarySegment.Curve.ToProtoType(false, null));
                return aResult;

            }
            return null;
        }

        internal static void GetBoundaries(SpatialElement SpatialElement, BoundaryLocation BoundaryLocation, out List<Autodesk.DesignScript.Geometry.Curve> BoundaryCurves, out List<Revit.Elements.Element> BoundaryElements)
        {
            BoundaryCurves = null;
            BoundaryElements = null;
            
            if (SpatialElement != null)
            {
                SpatialElementBoundaryOptions aSpatialElementBoundaryOptions = new SpatialElementBoundaryOptions();
                aSpatialElementBoundaryOptions.SpatialElementBoundaryLocation = GetSpatialElementBoundaryLocation(BoundaryLocation);
                IList<IList<BoundarySegment>> aBoundarySegmentListList = SpatialElement.GetBoundarySegments(aSpatialElementBoundaryOptions);
                BoundaryCurves = new List<Autodesk.DesignScript.Geometry.Curve>();
                BoundaryElements = new List<Revit.Elements.Element>();
                if (aBoundarySegmentListList != null)
                    foreach (IList<BoundarySegment> aBoundarySegmentList in aBoundarySegmentListList)
                        foreach (BoundarySegment aBoundarySegment in aBoundarySegmentList)
                        {
                            BoundaryCurves.Add(aBoundarySegment.Curve.ToProtoType(false, null));
                            BoundaryElements.Add(aBoundarySegment.Element.ToDSType(true));
                        }
                            
            }
        }
    }

    internal class MechanicalElement
    {
        private Autodesk.Revit.DB.Element pElement;
        private List<Autodesk.Revit.DB.Element> pConnectedElements;

        internal MechanicalElement(Autodesk.Revit.DB.Element Element, Autodesk.Revit.DB.MEPSystem MEPSystem)
        {
            pElement = Element;
            if (pElement != null)
                GetConnectedElements(MEPSystem);
        }

        internal MechanicalElement(Autodesk.Revit.DB.Element Element)
        {
            pElement = Element;
            if (pElement != null)
                GetConnectedElements(null);
        }

        private List<Autodesk.Revit.DB.MEPSystem> GetMEPSystems()
        {
            if (pElement is Autodesk.Revit.DB.FamilyInstance)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = pElement as Autodesk.Revit.DB.FamilyInstance;
                if (aFamilyInstance != null && aFamilyInstance.MEPModel != null)
                    return GetMEPSystems(aFamilyInstance.MEPModel.ConnectorManager);
            }
            else if (pElement is Autodesk.Revit.DB.MEPCurve)
            {
                Autodesk.Revit.DB.MEPCurve aMEPCurve = pElement as Autodesk.Revit.DB.MEPCurve;
                if (aMEPCurve != null && aMEPCurve.MEPSystem != null)
                    return GetMEPSystems(aMEPCurve.MEPSystem.ConnectorManager);
            }
            return null;
        }

        private List<Autodesk.Revit.DB.MEPSystem> GetMEPSystems(ConnectorManager ConnectorManager)
        {
            List<ElementId> aElementIdList = new List<ElementId>();
            List<Autodesk.Revit.DB.MEPSystem> aResult = new List<Autodesk.Revit.DB.MEPSystem>();

            foreach (Connector aConnector in ConnectorManager.Connectors)
                if (aConnector.Owner != null && aConnector.Owner is Autodesk.Revit.DB.MEPSystem)
                {
                    if (!aElementIdList.Contains(aConnector.Owner.Id))
                    {
                        aResult.Add(aConnector.Owner as Autodesk.Revit.DB.MEPSystem);
                        aElementIdList.Add(aConnector.Owner.Id);
                    }

                }
                else if (aConnector.MEPSystem != null)
                {
                    if (!aElementIdList.Contains(aConnector.MEPSystem.Id))
                    {
                        aResult.Add(aConnector.MEPSystem as Autodesk.Revit.DB.MEPSystem);
                        aElementIdList.Add(aConnector.MEPSystem.Id);
                    }
                }

            return aResult;
        }

        private void GetConnectedElements(Autodesk.Revit.DB.MEPSystem MEPSystem)
        {
            if (pElement is Autodesk.Revit.DB.FamilyInstance)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = pElement as Autodesk.Revit.DB.FamilyInstance;
                if (aFamilyInstance != null && aFamilyInstance.MEPModel != null)
                    GetConnetcedElements(aFamilyInstance.MEPModel.ConnectorManager, MEPSystem);
            }
            else if (pElement is Autodesk.Revit.DB.MEPCurve)
            {
                Autodesk.Revit.DB.MEPCurve aMEPCurve = pElement as Autodesk.Revit.DB.MEPCurve;
                if (aMEPCurve != null)
                    GetConnetcedElements(aMEPCurve.ConnectorManager, MEPSystem);
            }
        }

        private void GetConnetcedElements(ConnectorManager ConnectorManager, Autodesk.Revit.DB.MEPSystem MEPSystem)
        {
            if (ConnectorManager != null)
            {
                pConnectedElements = new List<Autodesk.Revit.DB.Element>();
                ElementId aElementId = pElement.Id;

                foreach (Connector aConnector in ConnectorManager.Connectors)
                    if (aConnector.ConnectorType != ConnectorType.Logical)
                        foreach (Connector aRefConnetcor in aConnector.AllRefs)
                            if (aRefConnetcor.ConnectorType != ConnectorType.Logical)
                                if (aRefConnetcor.Owner != null && aRefConnetcor.Owner.Id != aElementId && !(aRefConnetcor.Owner is Autodesk.Revit.DB.MEPSystem))
                                    if (MEPSystem == null)
                                        pConnectedElements.Add(aRefConnetcor.Owner);
                                    else if (aRefConnetcor.Domain != Domain.DomainUndefined && aRefConnetcor.MEPSystem.Id == MEPSystem.Id)
                                        pConnectedElements.Add(aRefConnetcor.Owner);
            }
        }

        public Autodesk.Revit.DB.Element Element
        {
            get
            {
                return pElement;
            }
        }

        public List<Autodesk.Revit.DB.Element> ConnectedElements
        {
            get
            {
                return new List<Autodesk.Revit.DB.Element>(pConnectedElements);
            }
        }

        public List<Autodesk.Revit.DB.MEPSystem> MEPSystems
        {
            get
            {
                return GetMEPSystems();
            }
        }

        public int Count
        {
            get
            {
                return pConnectedElements.Count;
            }
        }

        public Autodesk.Revit.DB.Element GetConnectedElement(int index)
        {
            return pConnectedElements[index];
        }
    }

    internal class MechanicalElementList
    {
        private Autodesk.Revit.DB.MEPSystem pMEPSystem;
        private List<MechanicalElement> pMechanicalElementList;
        private ObjectGraph pObjectGraph;

        internal MechanicalElementList(Autodesk.Revit.DB.MEPSystem MEPSystem)
        {
            pMEPSystem = MEPSystem;
            List<Autodesk.Revit.DB.Element> aElementList = null;
            if (MEPSystem is Autodesk.Revit.DB.Mechanical.MechanicalSystem)
            {
                Autodesk.Revit.DB.Mechanical.MechanicalSystem aMechanicalSystem = MEPSystem as Autodesk.Revit.DB.Mechanical.MechanicalSystem;
                aElementList = aMechanicalSystem.DuctNetwork.Cast<Autodesk.Revit.DB.Element>().ToList();
                aElementList.RemoveAll(x => x is Autodesk.Revit.DB.Mechanical.DuctInsulation || x is Autodesk.Revit.DB.Plumbing.PipeInsulation);
            }
            else
            {
                aElementList = MEPSystem.Elements.Cast<Autodesk.Revit.DB.Element>().ToList();
            }
            pMechanicalElementList = aElementList.ConvertAll(x => new MechanicalElement(x, MEPSystem));
            pMechanicalElementList.RemoveAll(x => x == null);
            if (pMechanicalElementList.Count > 0)
            {
                pObjectGraph = new ObjectGraph(pMechanicalElementList.Count, false);
                for (int aIndex_1 = 0; aIndex_1 < pMechanicalElementList.Count; aIndex_1++)
                {
                    List<Autodesk.Revit.DB.Element> aConnectedElementList = pMechanicalElementList[aIndex_1].ConnectedElements;
                    foreach (Autodesk.Revit.DB.Element aElement in aConnectedElementList)
                    {
                        int aIndex_2 = pMechanicalElementList.FindIndex(x => x.Element.Id == aElement.Id);
                        if (aIndex_2 >= 0)
                            pObjectGraph.Connect(aIndex_1, aIndex_2, true);
                    }
                }
            }

        }

        internal List<List<Autodesk.Revit.DB.Element>> Branches(Autodesk.Revit.DB.Element FirstElement)
        {
            List<List<Autodesk.Revit.DB.Element>> aResult = null;
            if (pObjectGraph != null)
            {
                aResult = new List<List<Autodesk.Revit.DB.Element>>();
                int aFirstIndex = pMechanicalElementList.FindIndex(x => x.Element.Id == FirstElement.Id);
                List<List<int>> aBranches = pObjectGraph.GetBranches(aFirstIndex);
                foreach (List<int> aIndexList in aBranches)
                {
                    List<Autodesk.Revit.DB.Element> aElementList = aIndexList.ConvertAll(x => pMechanicalElementList[x].Element);
                    aResult.Add(aElementList);
                }
            }
            return aResult;
        }

        internal void OrderedElements(Autodesk.Revit.DB.Element FirstElement, bool ByLongest, out List<Autodesk.Revit.DB.Element> Elements, out List<List<Autodesk.Revit.DB.Element>> Branches)
        {
            Elements = null;
            Branches = null;
            if (pObjectGraph != null)
            {
                int aFirstIndex = pMechanicalElementList.FindIndex(x => x.Element.Id == FirstElement.Id);
                if (aFirstIndex >= 0)
                {
                    List<List<int>> aBranches = pObjectGraph.GetBranches(aFirstIndex);
                    List<List<double>> aLengthsBranches = new List<List<double>>();
                    foreach (List<int> aBranch in aBranches)
                    {
                        List<double> aLengths = new List<double>();

                        if (aBranch.Count > 0)
                        {
                            MEPCurve aMEPCurve = pMechanicalElementList[aBranch.First()].Element as MEPCurve;
                            double aLength = 0;
                            if (aMEPCurve != null)
                                aLength = GetLength(aMEPCurve);
                            aLengths.Add(aLength);

                            for (int i = 1; i < aBranch.Count - 1; i++)
                                aLengths.Add(GetLength(pMechanicalElementList[aBranch[i - 1]].Element, pMechanicalElementList[aBranch[i]].Element, pMechanicalElementList[aBranch[i + 1]].Element));

                            if (aBranch.Count > 2)
                            {
                                aMEPCurve = pMechanicalElementList[aBranch.Last()].Element as MEPCurve;
                                aLength = 0;
                                if (aMEPCurve != null)
                                    aLength = GetLength(aMEPCurve);
                                aLengths.Add(aLength);
                            }

                            aLengthsBranches.Add(aLengths);
                        }
                    }

                    List<List<int>> aSortedBranches = Sort(aBranches, aLengthsBranches, ByLongest);


                    Elements = new List<Autodesk.Revit.DB.Element>();
                    Branches = new List<List<Autodesk.Revit.DB.Element>>();
                    foreach (List<int> aIndexList in aSortedBranches)
                    {
                        List<Autodesk.Revit.DB.Element> aElementList = aIndexList.ConvertAll(x => pMechanicalElementList[x].Element);
                        Branches.Add(aElementList);
                        Elements.AddRange(aElementList);
                    }
                }
            }
        }

        internal void ShortestPath(Autodesk.Revit.DB.Element FirstElement, Autodesk.Revit.DB.Element LastElement, out List<Autodesk.Revit.DB.Element> Elements)
        {
            Elements = null;
            if (pObjectGraph != null)
            {
                int aFirstIndex = pMechanicalElementList.FindIndex(x => x.Element.Id == FirstElement.Id);
                int aLastIndex = pMechanicalElementList.FindIndex(x => x.Element.Id == LastElement.Id);
                if (aFirstIndex >= 0 && aLastIndex >= 0)
                {
                    List<List<int>> aBranches = pObjectGraph.GetBranches(aFirstIndex);
                    if (aBranches != null)
                    {
                        aBranches.RemoveAll(x => x == null || x.Count == 0 || !x.Contains(aLastIndex));

                        List<List<double>> aLengthsBranches = new List<List<double>>();

                        for (int i = 0; i < aBranches.Count; i++)
                        {
                            int aIndex = aBranches[i].IndexOf(aLastIndex);
                            List<int> aBranch = aBranches[i].GetRange(0, aIndex + 1);
                            aBranches[i] = aBranch;

                            List <double> aLengths = new List<double>();
                            MEPCurve aMEPCurve = pMechanicalElementList[aBranch.First()].Element as MEPCurve;
                            double aLength = 0;
                            if (aMEPCurve != null)
                                aLength = GetLength(aMEPCurve);
                            aLengths.Add(aLength);

                            for (int j = 1; j < aBranch.Count - 1; j++)
                                aLengths.Add(GetLength(pMechanicalElementList[aBranch[j - 1]].Element, pMechanicalElementList[aBranch[j]].Element, pMechanicalElementList[aBranch[j + 1]].Element));

                            if (aBranch.Count > 2)
                            {
                                aMEPCurve = pMechanicalElementList[aBranch.Last()].Element as MEPCurve;
                                aLength = 0;
                                if (aMEPCurve != null)
                                    aLength = GetLength(aMEPCurve);
                                aLengths.Add(aLength);
                            }

                            aLengthsBranches.Add(aLengths);
                        }

                        if (aLengthsBranches != null)
                        {
                            List<double> aLengthList = aLengthsBranches.ConvertAll(x => x.Sum());
                            int aIndex = aLengthList.IndexOf(aLengthList.Min());
                            List<int> aSelectedBranch = aBranches[aIndex];

                            Elements = new List<Autodesk.Revit.DB.Element>();
                            Elements = aBranches[aIndex].ConvertAll(x => pMechanicalElementList[x].Element);
                        }
                    }
                }


            }
        }

        private double GetLength(MEPCurve MEPCurve)
        {
            Autodesk.Revit.DB.Parameter aParameter = MEPCurve.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
            if (aParameter != null)
                return aParameter.AsDouble();
            return 0;
        }

        private double GetLength(Autodesk.Revit.DB.Element Element_1, Autodesk.Revit.DB.Element Element_2, Autodesk.Revit.DB.Element Element_3)
        {
            if (Element_2 is MEPCurve)
                return GetLength(Element_2 as MEPCurve);

            ConnectorManager aConnectorManager = GetConnectorManager(Element_1);
            if (aConnectorManager != null)
            {
                Connector aConnector_1 = GetConnector(aConnectorManager, Element_2);
                if (aConnector_1 != null)
                {
                    aConnectorManager = GetConnectorManager(Element_3);
                    if (aConnectorManager != null)
                    {
                        Connector aConnector_2 = GetConnector(aConnectorManager, Element_2);
                        if (aConnector_2 != null)
                        {
                            XYZ aPoint_1 = aConnector_1.Origin;
                            XYZ aPoint_2 = aConnector_2.Origin;
                            return aPoint_1.DistanceTo(aPoint_2);
                        }
                    }
                }
            }
            return 0;
        }

        private ConnectorManager GetConnectorManager(Autodesk.Revit.DB.Element Element)
        {
            if (Element is Autodesk.Revit.DB.FamilyInstance)
                return (Element as Autodesk.Revit.DB.FamilyInstance).MEPModel.ConnectorManager;
            else if (Element is MEPCurve)
                return (Element as MEPCurve).ConnectorManager;

            return null;
        }

        private Connector GetConnector(ConnectorManager ConnectorManager, Autodesk.Revit.DB.Element Element)
        {
            if (ConnectorManager != null)
            {
                foreach (Connector aConnector in ConnectorManager.Connectors)
                    if (aConnector.ConnectorType != ConnectorType.Logical && aConnector.AllRefs != null)
                        foreach (Connector aRefConnector in aConnector.AllRefs)
                            if (aRefConnector.ConnectorType != ConnectorType.Logical && aRefConnector.Domain == aConnector.Domain && aRefConnector.MEPSystem != null && pMEPSystem != null && aRefConnector.MEPSystem.Id == pMEPSystem.Id)
                                if (aRefConnector.Owner.Id == Element.Id)
                                    return aConnector;
            }
            return null;
        }

        private static int GetMaxIndex(List<List<double>> aValues)
        {
            double aMax = double.MinValue;
            int aIndex = -1;
            for (int i = 0; i < aValues.Count; i++)
            {
                double aValue = aValues[i].Sum();
                if (aMax < aValue)
                {
                    aIndex = i;
                    aMax = aValue;
                }
            }
            return aIndex;
        }

        private static int GetMinIndex(List<List<double>> aValues)
        {
            double aMax = double.MaxValue;
            int aIndex = -1;
            for (int i = 0; i < aValues.Count; i++)
            {
                double aValue = aValues[i].Sum();
                if (aMax > aValue)
                {
                    aIndex = i;
                    aMax = aValue;
                }
            }
            return aIndex;
        }

        private static List<List<int>> Sort(List<List<int>> Branches, List<List<double>> Values, bool ByLongest)
        {
            List<List<int>> aResult = new List<List<int>>();

            List<List<double>> aValues = new List<List<double>>();
            foreach (List<double> aValue in Values)
                aValues.Add(new List<double>(aValue));

            List<List<int>> aBranches = new List<List<int>>();
            foreach (List<int> aBranch in Branches)
                aBranches.Add(new List<int>(aBranch));

            int aStartIndex = -1;
            if (ByLongest)
                aStartIndex = GetMaxIndex(Values);
            else
                aStartIndex = GetMinIndex(Values);

            if (aStartIndex >= 0)
            {
                List<int> aMainBranch = aBranches[aStartIndex];
                aResult.Add(aMainBranch);
                aBranches.RemoveAt(aStartIndex);
                aValues.RemoveAt(aStartIndex);
                for (int i = aMainBranch.Count - 1; i >= 0; i--)
                {
                    int aIndex = aMainBranch[i];
                    List<int> aIndexList = FindAll(aBranches, aIndex);
                    if (aIndexList.Count > 0)
                    {
                        List<List<int>> aNewBranches = null;
                        List<List<double>> aNewValues = null;

                        Move(aIndexList, aBranches, aValues, out aNewBranches, out aNewValues);
                        List<List<int>> aNewResult = Sort(aNewBranches, aNewValues, ByLongest);
                        foreach (List<int> aBranch in aNewResult)
                        {
                            RemoveRoot(aMainBranch, aBranch);
                            aResult.Add(aBranch);
                        }

                    }
                }

            }

            return aResult;
        }

        private static List<int> FindAll(List<List<int>> Branches, int Index)
        {
            List<int> aResult = new List<int>();
            for (int i = 0; i < Branches.Count; i++)
                if (Branches[i].Contains(Index))
                    aResult.Add(i);
            return aResult;
        }

        private static void Move(List<int> IndexList, List<List<int>> Branches, List<List<double>> Values, out List<List<int>> NewBranches, out List<List<double>> NewValues)
        {
            NewBranches = new List<List<int>>();
            NewValues = new List<List<double>>();

            foreach (int aIndex in IndexList)
            {
                NewBranches.Add(new List<int>(Branches[aIndex]));
                NewValues.Add(new List<double>(Values[aIndex]));
            }

            IndexList.Sort();

            for (int i = IndexList.Count - 1; i >= 0; i--)
            {
                Branches.RemoveAt(IndexList[i]);
                Values.RemoveAt(IndexList[i]);
            }
        }

        private static void RemoveRoot(List<int> MainBranch, List<int> Branch)
        {
            int aCount = 0;
            for (int i = 0; i < MainBranch.Count; i++)
            {
                if (i >= Branch.Count || MainBranch[i] != Branch[i])
                    break;

                aCount++;
            }

            if (aCount > 0)
                Branch.RemoveRange(0, aCount);
        }
    }

    internal class FamilyLoadOptions : IFamilyLoadOptions
    {
        public bool OnFamilyFound(bool FamilyInUse, out bool OverwriteParameterValues)
        {
            OverwriteParameterValues = true;
            return true;
        }

        public bool OnSharedFamilyFound(Autodesk.Revit.DB.Family SharedFamily, bool FamilyInUse, out FamilySource Source, out bool OverwriteParameterValues)
        {
            Source = FamilySource.Family;
            OverwriteParameterValues = true;
            return true;
        }
    }

    internal class Intersection
    {
        private static Options pOptions;
        private Autodesk.Revit.DB.Element pElement;
        private Autodesk.Revit.DB.Element pLinkElement;
        private List<Solid> pSolidList;
        private List<Solid> pIntersectionSolidList;

        public Intersection(Autodesk.Revit.DB.Element Element, Autodesk.Revit.DB.Element LinkElement)
        {
            pElement = Element;
            pLinkElement = LinkElement;
            pSolidList = null;
            pIntersectionSolidList = new List<Solid>();
        }

        public void InitializeSolids()
        {
            pSolidList = GetSolidList(pElement, null);
        }

        public void AddIntersections(Solid Solid)
        {
            foreach (Solid aSolid in SolidList)
            {
                Solid aIntersectionSolid = BooleanOperationsUtils.ExecuteBooleanOperation(aSolid, Solid, BooleanOperationsType.Intersect);
                if (aIntersectionSolid != null && aIntersectionSolid.Edges.Size > 0)
                    pIntersectionSolidList.Add(aIntersectionSolid);
            }
        }

        public static List<Solid> GetSolidList(Autodesk.Revit.DB.Element Element, Autodesk.Revit.DB.Transform Transform)
        {
            GeometryElement aGeometryElement = Element.get_Geometry(Intersection.Options);

            if (Transform != null)
                aGeometryElement = aGeometryElement.GetTransformed(Transform);

            List<Solid> aSolidList = new List<Solid>();
            foreach (GeometryObject aGeometryObject in aGeometryElement)
            {
                if (aGeometryObject is Solid)
                {
                    Solid aSolid = aGeometryObject as Solid;
                    if (aSolid.Faces.Size != 0)
                        aSolidList.Add(aGeometryObject as Solid);
                }
                else
                {
                    GeometryInstance aGeometryInstance = aGeometryObject as GeometryInstance;
                    if (aGeometryInstance != null)
                    {
                        GeometryElement aInstanceGeometryElement = aGeometryInstance.GetSymbolGeometry();
                        if (Transform == null)
                            aInstanceGeometryElement = aInstanceGeometryElement.GetTransformed(aGeometryInstance.Transform);

                        foreach (GeometryObject aInstanceGeometryObject in aInstanceGeometryElement)
                        {
                            Solid aSolid = aInstanceGeometryObject as Solid;
                            if (aSolid != null && aSolid.Faces.Size != 0)
                            {
                                aSolidList.Add(aSolid);
                            }
                        }

                    }
                }

            }
            return aSolidList;
        }

        public List<Solid> SolidList
        {
            get
            {
                if (pSolidList == null)
                    InitializeSolids();
                return pSolidList;
            }
        }

        public Autodesk.Revit.DB.Element Element
        {
            get
            {
                return pElement;
            }
        }

        public Autodesk.Revit.DB.Element LinkElement
        {
            get
            {
                return pLinkElement;
            }
        }

        private static Options Options
        {
            get
            {
                if (pOptions == null)
                {
                    pOptions = new Options();
                    pOptions.ComputeReferences = false;
                }
                return pOptions;
            }
        }
    }

    internal class ObjectGraph
    {
        private object[][] pGraph;
        private static object pEmptyValue = null;

        private ObjectGraph(object[][] Graph)
        {
            pGraph = new object[Graph.Length][];
            for (int i = 0; i < Graph.Length; i++)
            {
                pGraph[i] = Graph[i].ToList().ToArray();
            }
        }

        public ObjectGraph(int Count)
        {
            pGraph = new object[Count][];
            for (int i = 0; i < Count; i++)
                pGraph[i] = Enumerable.Repeat(pEmptyValue, Count).ToArray();
        }

        public ObjectGraph(int Count, object EmptyValue)
        {
            pEmptyValue = EmptyValue;

            pGraph = new object[Count][];
            for (int i = 0; i < Count; i++)
                pGraph[i] = Enumerable.Repeat(pEmptyValue, Count).ToArray();
        }

        public object this[int i, int j]
        {
            get
            {
                return pGraph[i][j];
            }
            set
            {
                pGraph[i][j] = value;
            }
        }

        private void SetAll(object Value)
        {
            for (int i = 0; i < pGraph.Length; i++)
                for (int j = 0; j < pGraph[i].Length; j++)
                    pGraph[i][j] = Value;
        }

        public void ConnectAll(object Value)
        {
            SetAll(Value);
        }

        public void DisconnectAll()
        {
            SetAll(pEmptyValue);
        }

        public void DisconnectAll(List<int> Indexes)
        {
            foreach (int aIndex in Indexes)
                DisconnectAll(aIndex);
        }

        public void DisconnectAll(int Index)
        {
            for (int i = 0; i < pGraph.Length; i++)
                Disconnect(Index, i);
        }

        public void Disconnect(int Index_1, int Index_2)
        {
            pGraph[Index_1][Index_2] = pGraph[Index_2][Index_1] = pEmptyValue;
        }

        public void Disconnect(int ConnectionsCount)
        {
            Disconnect(ConnectionsCount, this);
        }

        public int CountConnections(int Index)
        {
            return pGraph[Index].Count(x => !x.Equals(pEmptyValue));
        }

        public List<int> Connections(int Index)
        {
            return Connections(Index, this);
        }

        public static List<int> Connections(int Index, ObjectGraph ObjectGraph)
        {
            List<int> aResult = new List<int>();
            for (int i = 0; i < ObjectGraph.pGraph[Index].Length; i++)
                if (!ObjectGraph.pGraph[Index][i].Equals(pEmptyValue))
                    aResult.Add(i);
            return aResult;
        }

        public virtual void Connect(int Index_1, int Index_2, object Value)
        {
            pGraph[Index_1][Index_2] = pGraph[Index_2][Index_1] = Value;
        }

        public int Count
        {
            get
            {
                return pGraph.Length;
            }
        }

        private bool GetHasLoops()
        {
            ObjectGraph aObjectGraph = this.Copy();
            Disconnect(1, aObjectGraph);

            return aObjectGraph.GetHasConnections();
        }

        private bool GetHasConnections()
        {
            for (int i = 0; i < pGraph.Length; i++)
                if (CountConnections(i) > 0)
                    return true;
            return false;
        }

        public bool HasLoops
        {
            get
            {
                return GetHasLoops();
            }
        }

        public bool HasConnections
        {
            get
            {
                return GetHasConnections();
            }
        }

        public int GetFirstConnected()
        {
            for (int i = 0; i < pGraph.Length; i++)
                if (CountConnections(i) > 0)
                    return i;
            return -1;
        }

        public int GetFirstConnected(int ConnectionsCount)
        {
            for (int i = 0; i < pGraph.Length; i++)
                if (CountConnections(i) == ConnectionsCount)
                    return i;
            return -1;
        }

        public ObjectGraph Copy()
        {
            return new ObjectGraph(pGraph);
        }

        public static void Disconnect(int ConnectionsCount, ObjectGraph ObjectGraph)
        {
            for (int aIndex_1 = 0; aIndex_1 < ObjectGraph.pGraph.Length; aIndex_1++)
            {
                List<int> aIndexes = Connections(aIndex_1, ObjectGraph);
                if (aIndexes.Count == ConnectionsCount)
                {
                    foreach (int aIndex_2 in aIndexes)
                        ObjectGraph.Disconnect(aIndex_1, aIndex_2);

                    Disconnect(ConnectionsCount, ObjectGraph);
                }

            }
        }

        private void CopyConnections(int Index, ObjectGraph ObjectGraph)
        {
            for (int i = 0; i < pGraph.Length; i++)
                ObjectGraph.pGraph[Index][i] = ObjectGraph.pGraph[i][Index] = pGraph[Index][i];
        }

        private static bool AddFirstConnection(List<int> Indexes, ObjectGraph ObjectGraph)
        {
            List<int> aIndexes = ObjectGraph.Connections(Indexes.Last(), ObjectGraph);

            aIndexes.Remove(Indexes.Last());
            if (Indexes.Count > 1)
                aIndexes.Remove(Indexes[Indexes.Count - 2]);

            if (aIndexes.Count > 0)
            {
                foreach (int aIndex in aIndexes)
                    if (Indexes.Contains(aIndex))
                    {
                        Indexes.Add(aIndex);
                        return true;
                    }

                Indexes.Add(aIndexes.First());
                return true;
            }
            return false;
        }

        public static List<int> GetBranch(int Index, ObjectGraph ObjectGraph)
        {
            List<int> aResult = new List<int>();
            if (ObjectGraph.CountConnections(Index) > 0)
            {
                aResult.Add(Index);
                while (AddFirstConnection(aResult, ObjectGraph))
                {
                    if (aResult.FindAll(x => x == aResult.Last()).Count > 1)
                        break;
                }
            }
            return aResult;
        }

        public List<List<int>> GetBranches(int Index)
        {
            List<List<int>> aResult = new List<List<int>>();
            List<ObjectGraph> aObjectGraphList = Split();
            foreach (ObjectGraph aObjectGraph in aObjectGraphList)
                if (aObjectGraph.CountConnections(Index) > 0)
                {
                    int aCount = int.MaxValue;
                    while (aCount > 1)
                    {
                        List<int> aIndexes = GetBranch(Index, aObjectGraph);
                        aCount = aIndexes.Count;
                        if (aCount > 1)
                        {
                            aResult.Add(aIndexes);
                            aObjectGraph.Disconnect(aIndexes[aCount - 1], aIndexes[aCount - 2]);
                            DisconnectEnd(aIndexes, aObjectGraph);
                        }
                    }
                }
            return aResult;
        }

        public List<ObjectGraph> Split()
        {
            List<ObjectGraph> aResult = new List<ObjectGraph>();
            ObjectGraph aObjectGraph = Copy();
            int aIndex = aObjectGraph.GetFirstConnected();
            while (aIndex >= 0)
            {
                List<int> aIndexes = new List<int>();
                AddAllConnections(aIndex, aIndexes, aObjectGraph);
                aResult.Add(aObjectGraph.Copy(aIndexes));
                aObjectGraph.DisconnectAll(aIndexes);
                aIndex = aObjectGraph.GetFirstConnected();
            }
            return aResult;

        }

        public ObjectGraph Copy(List<int> IndexList)
        {
            ObjectGraph aResult = new ObjectGraph(pGraph.Length);
            foreach (int aIndex in IndexList)
                CopyConnections(aIndex, aResult);

            return aResult;
        }

        private static void AddAllConnections(int Index, List<int> Indexes, ObjectGraph ObjectGraph)
        {
            List<int> aIndexes = Connections(Index, ObjectGraph);
            foreach (int aIndex in aIndexes)
            {
                if (!Indexes.Contains(aIndex))
                {
                    Indexes.Add(aIndex);
                    AddAllConnections(aIndex, Indexes, ObjectGraph);
                }
            }
        }

        public static void DisconnectEnd(List<int> Indexes, ObjectGraph ObjectGraph)
        {
            if (Indexes.Count > 1)
                for (int i = Indexes.Count - 1; i > 0; i--)
                    if (ObjectGraph.CountConnections(Indexes[i]) < 2)
                        ObjectGraph.Disconnect(Indexes[i], Indexes[i - 1]);
                    else
                        break;
        }
    }

    internal class DoubleGraph
    {
        private ObjectGraph pObjectGraph;

        public DoubleGraph(int Count)
        {
            pObjectGraph = new ObjectGraph(Count, double.NaN);
        }

        public void Connect(int Index_1, int Index_2, double Value)
        {
            pObjectGraph.Connect(Index_1, Index_2, Value);
        }

        public List<List<int>> GetLoops()
        {
            List<List<int>> aResult = new List<List<int>>();

            ObjectGraph aCopiedObjectGraph = pObjectGraph.Copy();
            aCopiedObjectGraph.Disconnect(1);

            List<ObjectGraph> aObjectGraphList = aCopiedObjectGraph.Split();
            foreach (ObjectGraph aObjectGraph in aObjectGraphList)
            {
                int aIndex = aObjectGraph.GetFirstConnected(2);
                while (aIndex >= 0)
                {
                    List<int> aIndexes = GetLoop(aIndex, aObjectGraph);
                    int aCount = aIndexes.Count;
                    if (aCount > 1)
                    {
                        aResult.Add(aIndexes);
                        throw new NotImplementedException();
                        //aObjectGraph.Disconnect(aIndexes[aCount - 1], aIndexes[aCount - 2]);
                        //aObjectGraph.Disconnect(1);
                        aIndex = aObjectGraph.GetFirstConnected(2);
                    }
                }
            }
            return aResult;
        }

        private static List<int> GetLoop(int Index, ObjectGraph ObjectGraph)
        {
            List<int> aResult = new List<int>();
            if (ObjectGraph.CountConnections(Index) > 0)
            {
                List<List<int>> aBranchList = new List<List<int>>();
                List<double> aValueList = new List<double>();
                int aNextIndex = ObjectGraph.Connections(Index).First();
                List<int> aIndexes = new List<int>();
                aIndexes.Add(Index);
                aIndexes.Add(aNextIndex);
                aBranchList.Add(aIndexes);
                aValueList.Add((double)ObjectGraph[Index, aNextIndex]);

                while (Next(aBranchList, aValueList, ObjectGraph))
                {

                }

                if(aBranchList.Count > 0)
                {
                    int aIndex = aValueList.IndexOf(aValueList.Min());
                    aResult = aBranchList[aIndex];
                }
            }
            return aResult;
        }

        private static bool Next(List<List<int>> BranchList, List<double> ValueList, ObjectGraph ObjectGraph)
        {
            bool aResult = false;
            double aMin = ValueList.Min();
            for (int i=0; i < BranchList.Count; i++)
            {
                List<int> aBranch = BranchList[i];
                if (aBranch.First() != aBranch.Last() && ValueList[i] <= aMin)
                {
                    int aIndex = aBranch.Last();
                    List<int> aIndexList = ObjectGraph.Connections(aIndex);
                    aIndexList.RemoveAll(x => x == aIndex);
                    if (aIndexList.Count > 0)
                    {
                        List<double> aValueList = aIndexList.ConvertAll(x => (double)ObjectGraph[i, x]);
                        if (aValueList.Count > 0)
                        {
                            for (int j = 1; j < aValueList.Count; j++)
                            {
                                List<int> aNewBranch = aBranch.ToList();
                                aNewBranch.Add(aIndexList[j]);
                                BranchList.Add(aNewBranch);
                                ValueList.Add(ValueList[i] + aValueList[j]);
                            }
                            aBranch.Add(aIndexList[0]);
                            aValueList[i] += aValueList[0];
                        }
                        aResult = true;
                    }
                }
            }
            return aResult;
        }
    }
}
