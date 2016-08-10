using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB;
using Revit.Elements;

using RevitServices.Transactions;
using RevitServices.Persistence;
using Autodesk.DesignScript.Runtime;

namespace Revit
{
    /// <summary>
    /// A space
    /// </summary>
    public static class Space
    {
        /// <summary>
        /// Returns space volume. Value is represented in Revit internal units.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="Volume">Volume of Space</returns>
        /// <search>
        /// Space, Volume, space, volume
        /// </search>
        public static double? Volume(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.Volume;
            }

            return null;
        }

        /// <summary>
        /// Returns number of people from the space.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="NumberOfPeople">Number of people</returns>
        /// <search>
        /// Space, Number of people, space, number of people
        /// </search>
        public static double? NumberOfPeople(Elements.Element Element)
        {
            Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
            if (aSpace != null)
                return aSpace.NumberofPeople;

            return null;
        }

        /// <summary>
        /// Sets number of people for the space.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <param name="NumberOfPeople">Number Of People</param>
        /// <returns name="NumberOfPeople">Number of people</returns>
        /// <search>
        /// Space, Number of people, space, number of people
        /// </search>
        public static double? NumberOfPeople(Elements.Element Element, double? NumberOfPeople)
        {
            if (Element != null && Element.InternalElement is Autodesk.Revit.DB.Mechanical.Space && NumberOfPeople.HasValue)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                TransactionManager.Instance.EnsureInTransaction(aSpace.Document);
                aSpace.NumberofPeople = NumberOfPeople.Value;
                TransactionManager.Instance.TransactionTaskDone();
                return aSpace.NumberofPeople;
            }
            return null;
        }

        /// <summary>
        /// Sets occupancy unit for the space.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <param name="OccupancyUnit">Occupancy Unit</param>
        /// <returns name="OccupancyUnit">Occupancy Unit</returns>
        /// <search>
        /// Space, Occupancy Unit, space, occupancy unit
        /// </search>
        public static OccupancyUnit? OccupancyUnit(Elements.Element Element, Revit.OccupancyUnit OccupancyUnit)
        {
            if (Element != null && Element.InternalElement is Autodesk.Revit.DB.Mechanical.Space)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                TransactionManager.Instance.EnsureInTransaction(aSpace.Document);
                aSpace.OccupancyUnit = Functions.GetOccupancyUnit(OccupancyUnit);
                TransactionManager.Instance.TransactionTaskDone();
            }

            return null;
        }

        /// <summary>
        /// Returns space actual exhaust airflow.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="ActualExhaustAirflow">Actual exhaust airflow</returns>
        /// <search>
        /// Space, Actual exhaust airflow, space, actual exhaust airflow
        /// </search>
        public static double? ActualExhaustAirflow(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.ActualExhaustAirflow;
            }

            return null;
        }

        /// <summary>
        /// Returns space actual HVAC load. Value is represented in Revit internal units.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="ActualHVACLoad">Actual HVAC load</returns>
        /// <search>
        /// Space, Actual HVAC Load, space, actual hvac load
        /// </search>
        public static double? ActualHVACLoad(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.ActualHVACLoad;
            }

            return null;
        }

        /// <summary>
        /// Returns space Actual Lighting Load parameter value. Value is represented in Revit internal units.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="ActualLightingLoad">Actual HVAC load</returns>
        /// <search>
        /// Space, Actual HVAC Load, space, actual hvac load
        /// </search>
        public static double? ActualLightingLoad(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.ActualLightingLoad;
            }

            return null;
        }

        /// <summary>
        /// Returns space actual other load parameter value. Value is represented in Revit internal units.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="ActualOtherLoad">Actual Other Load</returns>
        /// <search>
        /// Space, Actual Other Load, space, actual other load
        /// </search>
        public static double? ActualOtherLoad(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.ActualOtherLoad;
            }

            return null;
        }

        /// <summary>
        /// Returns space actual power load parameter value. Value is represented in Revit internal units.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="ActualPowerLoad">Actual Other Load</returns>
        /// <search>
        /// Space, Actual Other Load, space, actual other load
        /// </search>
        public static double? ActualPowerLoad(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.ActualPowerLoad;
            }

            return null;
        }

        /// <summary>
        /// Returns space actual return airflow parameter value. Value is represented in Revit internal units.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="ActualReturnAirflow">Actual Other Load</returns>
        /// <search>
        /// Space, Actual return airflow, space, actual return airflow
        /// </search>
        public static double? ActualReturnAirflow(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.ActualReturnAirflow;
            }

            return null;
        }

        /// <summary>
        /// Returns space actual supply airflow parameter value. Value is represented in Revit internal units.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="ActualSupplyAirflow">Actual supply airflow</returns>
        /// <search>
        /// Space, Actual Supply Airflow, space, actual supply airflow
        /// </search>
        public static double? ActualSupplyAirflow(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.ActualSupplyAirflow;
            }

            return null;
        }

        /// <summary>
        /// Returns space area. Value is represented in Revit internal units.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="Area">Area</returns>
        /// <search>
        /// Space, Actual Supply Airflow, space, actual supply airflow
        /// </search>
        public static double? Area(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.Area;
            }

            return null;
        }

        /// <summary>
        /// Returns area per person for space. Value is represented in Revit internal units.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="AreaPerPerson">Area Per Person</returns>
        /// <search>
        /// Space, Area Per Person, space, area per person
        /// </search>
        public static double? AreaPerPerson(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                if (aSpace != null)
                    return aSpace.AreaperPerson;
            }

            return null;
        }

        /// <summary>
        /// Create space on specyfied Level and UV coordinates.
        /// </summary>
        /// <param name="Level">Level</param>
        /// <param name="UV">UV coordinates</param>
        /// <returns name="Space">Created space</returns>
        /// <search>
        /// Space, By Level And UV, space, by level and uv
        /// </search>
        public static Revit.Elements.Element ByLevelAndUV(Revit.Elements.Level Level, Autodesk.DesignScript.Geometry.UV UV)
        {
            if (Level != null && Level.InternalElement != null && UV != null)
            {
                Autodesk.Revit.DB.Level aLevel = Level.InternalElement as Autodesk.Revit.DB.Level;
                if (aLevel != null)
                {
                    Autodesk.Revit.DB.Document aDocument = aLevel.Document;
                    if (aDocument != null)
                    {
                        TransactionManager.Instance.EnsureInTransaction(aDocument);
                        Autodesk.Revit.DB.Mechanical.Space aSpace = aDocument.Create.NewSpace(aLevel, new Autodesk.Revit.DB.UV(UV.U, UV.V));
                        TransactionManager.Instance.TransactionTaskDone();
                        if (aSpace != null)
                            return aSpace.ToDSType(true) as Revit.Elements.Element;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns level where space is placed.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <returns name="Level">Level</returns>
        /// <search>
        /// Space, Level, space, level
        /// </search>
        public static Revit.Elements.Level Level(Revit.Elements.Element Element)
        {
            return Revit.Element.Level(Element);
        }

        /// <summary>
        /// get space at specified point.
        /// </summary>
        /// <param name="Point">Point</param>
        /// <returns name="Element">Space</returns>
        /// <search>
        /// Space, Get At Point, space, get at point
        /// </search>
        public static Revit.Elements.Element Get(Autodesk.DesignScript.Geometry.Point Point)
        {
            if (Point != null)
            {
                Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
                if (aDocument != null)
                {
                    Autodesk.Revit.DB.Mechanical.Space aResult = aDocument.GetSpaceAtPoint(new XYZ(Point.X, Point.Y, Point.Z));
                    if (aResult != null)
                        return aResult.ToDSType(true);
                }
            }
            return null;
        }

        /// <summary>
        /// Creates tag for space.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <param name="View">View</param>
        /// <param name="UV">UV coordinates</param>
        /// <returns name="Element">Tag</returns>
        /// <search>
        /// Space, Create Space Tag, space, create space tag
        /// </search>
        public static Revit.Elements.Element CreateTag(Revit.Elements.Element Element, Revit.Elements.Element View, Autodesk.DesignScript.Geometry.UV UV)
        {
            if (Element != null && Element.InternalElement != null && View != null && View.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Mechanical.Space)
            {
                Autodesk.Revit.DB.Mechanical.Space aSpace = Element.InternalElement as Autodesk.Revit.DB.Mechanical.Space;
                Autodesk.Revit.DB.View aView = View.InternalElement as Autodesk.Revit.DB.View;
                if (aView != null)
                {
                    Autodesk.Revit.DB.Document aDocument = aSpace.Document;
                    if (aDocument != null)
                    {
                        TransactionManager.Instance.EnsureInTransaction(aDocument);
                        Autodesk.Revit.DB.Mechanical.SpaceTag aSpaceTag = null;
                        try
                        {
                            aSpaceTag = aDocument.Create.NewSpaceTag(aSpace, new UV(UV.U, UV.V), aView);
                        }
                        catch (InvalidOperationException e)
                        {

                        }
                        TransactionManager.Instance.EnsureInTransaction(aDocument);
                        if (aSpaceTag != null)
                            return aSpaceTag.ToDSType(true);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets space boundary elements and curves.
        /// </summary>
        /// <param name="Element">Space</param>
        /// <param name="BoundaryLocation">Boundary Location</param>
        /// <returns name="Elements">Lists of boundary elements</returns>
        /// <returns name="Curves">Lists of boundary curves</returns>
        /// <search>
        /// Space, Get space boundary elements and curves, space, get space boundary elements and curves
        /// </search>
        [MultiReturn(new[] { "Elements", "Curves" })]
        public static Dictionary<string, object> Boundaries(Revit.Elements.Element Element, BoundaryLocation BoundaryLocation)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Mechanical.Space)
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

        /// <summary>
        /// Returns all family Instances in space
        /// </summary>
        /// <param name="Element">Space</param>
        /// <param name="Category">Category of family Instances</param>
        /// <returns name="FamilyInstances">Family Instances</returns>
        /// <search>
        /// Space, All family instances, space, all family instances
        /// </search>
        public static List<Revit.Elements.FamilyInstance> FamilyInstances(Revit.Elements.Element Element, Revit.Elements.Category Category)
        {
            Autodesk.Revit.DB.Document aDocument = Element.InternalElement.Document;
            if(aDocument != null)
            {
                List<Autodesk.Revit.DB.FamilyInstance> aFamilyInstanceList = new FilteredElementCollector(aDocument).OfClass(typeof(Autodesk.Revit.DB.FamilyInstance)).OfCategoryId(new ElementId(Category.Id)).Cast<Autodesk.Revit.DB.FamilyInstance>().ToList();
                aFamilyInstanceList.RemoveAll(x => x.Space != null && x.Space.Id != Element.InternalElement.Id);
                return aFamilyInstanceList.ConvertAll(x => x.ToDSType(true) as Revit.Elements.FamilyInstance);
            }
            return null;
        }
    }
}
