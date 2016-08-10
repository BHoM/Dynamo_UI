using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.DesignScript.Geometry;

using RevitServices.Transactions;
using Revit.Elements;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Revit
{
    /// <summary>
    /// A Family Instance
    /// </summary>
    public static class FamilyInstance
    {
        /// <summary>
        /// Returns all electrical systems family instance is connected to.
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="ElectricalSystems">Electrical Systems represented as elements</returns>
        /// <search>
        /// Element electrical systems, element electrical systems,  family instance
        /// </search>
        public static List<Elements.Element> ElectricalSystems(Elements.FamilyInstance FamilyInstance)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                if (aFamilyInstance != null && aFamilyInstance.MEPModel != null && aFamilyInstance.MEPModel.ElectricalSystems != null)
                {
                    List<Autodesk.Revit.DB.Electrical.ElectricalSystem> aElectricalSystemList = aFamilyInstance.MEPModel.ElectricalSystems.Cast<Autodesk.Revit.DB.Electrical.ElectricalSystem>().ToList();
                    return aElectricalSystemList.ConvertAll(x => ElementWrapper.ToDSType(x as Autodesk.Revit.DB.Element, true));
                }
            }
            return null;
        }

        /// <summary>
        /// Returns Space of Family Instance.
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Space">Space</returns>
        /// <search>
        /// family instance space, family instance space, family instance
        /// </search>
        public static Elements.Element Space(Elements.FamilyInstance FamilyInstance)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                if (aFamilyInstance != null)
                {
                    Autodesk.Revit.DB.Mechanical.Space aSpace = aFamilyInstance.Space;
                    if (aSpace != null)
                        return ElementWrapper.ToDSType(aSpace as Autodesk.Revit.DB.Element, true);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns Room of family instance.
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Room">Room</returns>
        /// <search>
        /// family instance room, family instance room, family instance
        /// </search>
        public static Elements.Element Room(Elements.FamilyInstance FamilyInstance)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                if (aFamilyInstance != null)
                {
                    Autodesk.Revit.DB.Architecture.Room aRoom = aFamilyInstance.Room;
                    if (aRoom != null)
                        return ElementWrapper.ToDSType(aRoom as Autodesk.Revit.DB.Architecture.Room, true);
                }
            }
            return null;
        }

        /// <summary>
        /// Returns Room for a Widnow or Door Family Instance.
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Room">Room</returns>
        /// <search>
        /// Family Instance Room, family instance room, family instance, toroom, ToRoom, Door, Window
        /// </search>
        public static Elements.Element ToRoom(Elements.FamilyInstance FamilyInstance)
        {
            Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
            if (aFamilyInstance.ToRoom == null)
                return null;
            return ElementWrapper.ToDSType(aFamilyInstance.ToRoom as Autodesk.Revit.DB.Architecture.Room, true);
        }

        /// <summary>
        /// Returns Room for a Widnow or Door Family Instance.
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Room">Room</returns>
        /// <search>
        /// Family Instance Room, family instance room, family instance, fromroom, FromRoom, Door, Window
        /// </search>
        public static Elements.Element FromRoom(Elements.FamilyInstance FamilyInstance)
        {
            Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
            if (aFamilyInstance.FromRoom == null)
                return null;
            return ElementWrapper.ToDSType(aFamilyInstance.FromRoom as Autodesk.Revit.DB.Architecture.Room, true);
        }

        /// <summary>
        /// Returns host of family instance.
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Host">Host element</returns>
        /// <search>
        /// family instance host, family instance host
        /// </search>
        public static Elements.Element Host(Elements.FamilyInstance FamilyInstance)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null)
            {
                if (FamilyInstance.InternalElement is Autodesk.Revit.DB.FamilyInstance)
                {
                    Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                    if (aFamilyInstance.Host != null)
                        return ElementWrapper.ToDSType(aFamilyInstance.Host, true);
                }
            }
            return null;
        }

        /// <summary>
        /// Creates new family instance by point.
        /// </summary>
        /// <param name="FamilyType">Family Type</param>
        /// <param name="Location">Location Point</param>
        /// <returns name="FamilyInstance">Family Instance</returns>
        /// <search>
        /// Create Family Instance by Point, create family instance by point
        /// </search>
        public static Elements.FamilyInstance ByPoint(Elements.FamilyType FamilyType, Autodesk.DesignScript.Geometry.Point Location)
        {
            if (FamilyType != null && FamilyType.InternalElement != null)
            {
                Autodesk.Revit.DB.Document aDocument = FamilyType.InternalElement.Document;
                if (aDocument != null)
                {
                    TransactionManager.Instance.EnsureInTransaction(aDocument);
                    Autodesk.Revit.DB.FamilyInstance aFamilyInstance = aDocument.Create.NewFamilyInstance(new XYZ(Location.X, Location.Y, Location.Z), FamilyType.InternalElement as Autodesk.Revit.DB.FamilySymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    TransactionManager.Instance.TransactionTaskDone();
                    if (aFamilyInstance != null)
                    {
                        Elements.Element aElement = ElementWrapper.ToDSType(aFamilyInstance, true);
                        return aElement as Elements.FamilyInstance;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Creates new family instance on specified face. Only Face hosted families will be created.
        /// </summary>
        /// <param name="Face">Face</param>
        /// <param name="FamilyType">Family Type</param>
        /// <param name="Location">Location Point</param>
        /// <param name="ReferenceDirection">Reference direction</param>
        /// <returns name="FamilyInstance">Family Instance</returns>
        /// <search>
        /// Create Family Instance on face, create family instance on face
        /// </search>
        public static Elements.FamilyInstance ByFace(Autodesk.Revit.DB.Face Face, Elements.FamilyType FamilyType, Autodesk.DesignScript.Geometry.Point Location, Autodesk.DesignScript.Geometry.Vector ReferenceDirection)
        {
            if (FamilyType != null && FamilyType.InternalElement != null && Face != null)
            {
                Autodesk.Revit.DB.Document aDocument = FamilyType.InternalElement.Document;
                if (aDocument != null)
                {
                    XYZ aLocation = new XYZ(Location.X, Location.Y, Location.Z);
                    XYZ aReferenceDirection = new XYZ(ReferenceDirection.X, ReferenceDirection.Y, ReferenceDirection.Z);
                    TransactionManager.Instance.EnsureInTransaction(aDocument);
                    Autodesk.Revit.DB.FamilyInstance aFamilyInstance = aDocument.Create.NewFamilyInstance(Face, aLocation, aReferenceDirection, FamilyType.InternalElement as Autodesk.Revit.DB.FamilySymbol);
                    TransactionManager.Instance.TransactionTaskDone();
                    if (aFamilyInstance != null)
                    {
                        Elements.Element aElement = ElementWrapper.ToDSType(aFamilyInstance, true);
                        return aElement as Elements.FamilyInstance;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns location of Family Instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Point">Location Point</returns>
        /// <search>
        /// Family instance location, family instance location 
        /// </search>
        public static Autodesk.DesignScript.Geometry.Point Location(Elements.FamilyInstance FamilyInstance)
        {
            return Element.Location(FamilyInstance as Elements.Element);
        }

        /// <summary>
        /// Returns location points of Family Instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Points">Location Points</returns>
        /// <search>
        /// Family instance location points, family instance location points
        /// </search>
        public static Autodesk.DesignScript.Geometry.Point[] LocationPoints(Elements.FamilyInstance FamilyInstance)
        {
            return Element.LocationPoints(FamilyInstance as Elements.Element);
        }

        /// <summary>
        /// Returns facing orientation of Family Instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Vector">Facing orienation vactor</returns>
        /// <search>
        /// Family Instance facing orientation, facing orienation, family instance
        /// </search>
        public static Vector FacingOrientation(Elements.FamilyInstance FamilyInstance)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null)
            {
                if (FamilyInstance.InternalElement is Autodesk.Revit.DB.FamilyInstance)
                {
                    Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                    return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(aFamilyInstance.FacingOrientation.X, aFamilyInstance.FacingOrientation.Y, aFamilyInstance.FacingOrientation.Z);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns hand orientation of Family Instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Vector">Hand orienation vactor</returns>
        /// <search>
        /// Family Instance hand orientation, hand orienation, family instance
        /// </search>
        public static Vector HandOrientation(Elements.FamilyInstance FamilyInstance)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null)
            {
                if (FamilyInstance.InternalElement is Autodesk.Revit.DB.FamilyInstance)
                {
                    Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                    return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(aFamilyInstance.HandOrientation.X, aFamilyInstance.HandOrientation.Y, aFamilyInstance.HandOrientation.Z);
                }
            }

            return null;
        }

        /// <summary>
        /// Sets family Sytmbol of family instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <param name="FamilyType">Family Type</param>
        /// <returns name="FamilyInstance">Family Instance</returns>
        /// <search>
        /// Set family instance symbol, set family instance symbol, 
        /// </search>
        public static Elements.FamilyInstance SetFamilySymbol(Elements.FamilyInstance FamilyInstance, Elements.FamilyType FamilyType)
        {
            if(FamilyInstance != null && FamilyType != null)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                if (aFamilyInstance != null)
                {
                    Autodesk.Revit.DB.Document aDocument = aFamilyInstance.Document;
                    Autodesk.Revit.DB.FamilySymbol aFamilySymbol = FamilyType.InternalElement as Autodesk.Revit.DB.FamilySymbol;
                    TransactionManager.Instance.EnsureInTransaction(aDocument);
                    aFamilyInstance.Symbol = aFamilySymbol;
                    TransactionManager.Instance.TransactionTaskDone();
                    if (aFamilyInstance.Symbol.Id == aFamilySymbol.Id)
                        return ElementWrapper.ToDSType(aFamilyInstance, true) as Elements.FamilyInstance;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all Mechanical Elements connected to the family instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Elements">Connected mechanical elements</returns>
        /// <search>
        /// Family Instance, Connected Mechanical Elements
        /// </search>
        public static List<Elements.Element> ConnectedMechanicalElements(Elements.FamilyInstance FamilyInstance)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                if (aFamilyInstance != null)
                {
                    MechanicalElement aMEPSystemElement = new MechanicalElement(aFamilyInstance);
                    foreach(Connector aConnector  in aMEPSystemElement.MEPSystems.First().ConnectorManager.Connectors)
                    {

                    }
                    return aMEPSystemElement.ConnectedElements.ToList().ConvertAll(x => x.ToDSType(true));
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all Mechanical Elements connected to particular system and the family instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <param name="MEPSystem">MEP System</param>
        /// <returns name="Elements">Connected mechanical elements</returns>
        /// <search>
        /// Family Instance, Connected Mechanical Elements
        /// </search>
        public static List<Elements.Element> ConnectedMechanicalElements(Elements.FamilyInstance FamilyInstance, Elements.Element MEPSystem)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null && MEPSystem != null && MEPSystem.InternalElement != null)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                if (aFamilyInstance != null)
                {
                    Autodesk.Revit.DB.MEPSystem aMEPSystem = MEPSystem.InternalElement as Autodesk.Revit.DB.MEPSystem;
                    if (aMEPSystem != null)
                    {
                        MechanicalElement aMEPSystemElement = new MechanicalElement(aFamilyInstance, aMEPSystem);
                        return aMEPSystemElement.ConnectedElements.ToList().ConvertAll(x => x.ToDSType(true));
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all MEP systems the family instance is connected to 
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="MEPSystems">MEP Systems</returns>
        /// <search>
        /// Family Instance, Connected MEP Systems, family instance, connected mep systems
        /// </search>
        public static List<Elements.Element> MEPSystems(Elements.FamilyInstance FamilyInstance)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                if (aFamilyInstance != null)
                {
                    MechanicalElement aMEPSystemElement = new MechanicalElement(aFamilyInstance);
                    List<Autodesk.Revit.DB.MEPSystem> aMEPSystems = aMEPSystemElement.MEPSystems;
                    if (aMEPSystems != null)
                        return aMEPSystems.ConvertAll(x => x.ToDSType(true));
                }
            }
            return null;
        }

        /// <summary>
        /// Gets loss method of family instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="LossMethod">Loss Method</returns>
        /// <search>
        /// Loss method for Family Instance, family instance, loss method
        /// </search>
        public static LossMethod? GetLossMethod(Elements.FamilyInstance FamilyInstance)
        {
            Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
            Autodesk.Revit.DB.Parameter aParameter = aFamilyInstance.get_Parameter(BuiltInParameter.RBS_DUCT_FITTING_LOSS_METHOD_SERVER_PARAM);

            if (aParameter == null || aParameter.StorageType != StorageType.String)
                return null;

            return Functions.GetLossMethod(aParameter.AsString());
        }

        /// <summary>
        /// Sets loss method of family instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <param name="LossMethod">Loss Method</param>
        /// <returns name="FamilyInstance">Family Instance</returns>
        /// <search>
        /// Loss method of Family Instance, family instance, loss method
        /// </search>
        public static Elements.FamilyInstance SetLossMethod(Elements.FamilyInstance FamilyInstance, LossMethod LossMethod)
        {
            Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
            Autodesk.Revit.DB.Parameter aParameter = aFamilyInstance.get_Parameter(BuiltInParameter.RBS_DUCT_FITTING_LOSS_METHOD_SERVER_PARAM);

            if (aParameter != null && aParameter.StorageType == StorageType.String && !aParameter.IsReadOnly)
            {
                TransactionManager.Instance.EnsureInTransaction(aFamilyInstance.Document);
                aParameter.Set(Functions.GetLossMethod(LossMethod));
                TransactionManager.Instance.TransactionTaskDone();
            }

            return FamilyInstance;
        }

        /// <summary>
        /// Gets loss value of family instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="Value">Loss Value</returns>
        /// <search>
        /// Gets loss value of family instance, family instance, get loss value of family instance
        /// </search>
        public static double? GetLossValue(Elements.FamilyInstance FamilyInstance)
        {
            Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
            LossMethod? aLossMethod = GetLossMethod(FamilyInstance);
            if (aLossMethod != null)
            {
                switch (aLossMethod.Value)
                {
                    case LossMethod.SpecificCoefficient:
                        return GetSpecificCoefficient(aFamilyInstance);
                    case LossMethod.SpecificLoss:
                        return GetSpecificLoss(aFamilyInstance);
                }
            }
            return null;
        }

        /// <summary>
        /// Sets loss value of family instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <param name="Value">Loss Value</param>
        /// <returns name="FamilyInstance">Family Instance</returns>
        /// <search>
        /// Sets loss value of Family Instance, family instance, loss value
        /// </search>
        public static Elements.FamilyInstance SetLossValue(Elements.FamilyInstance FamilyInstance, double Value)
        {
            Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
            LossMethod? aLossMethod = GetLossMethod(FamilyInstance);
            if (aLossMethod != null)
            {
                Autodesk.Revit.DB.FamilyInstance aResult = null;
                switch (aLossMethod.Value)
                {
                    case LossMethod.SpecificCoefficient:
                        aResult = SetSpecificCoefficient(aFamilyInstance, Value);
                        break;
                    case LossMethod.SpecificLoss:
                        aResult = SetSpecificLoss(aFamilyInstance, Value);
                        break;
                }
                if (aResult != null)
                    return aResult.ToDSType(true) as Elements.FamilyInstance;
            }
            return null;
        }

        /// <summary>
        /// Gets sub components of Family Instance (nested family instances)
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="SubComponents">Elements</returns>
        /// <search>
        /// Family Instance, Get sub components, Get subcomponents, family instance, get subcomponents, get sub components,
        /// </search>
        public static List<Elements.Element> SubComponents(Elements.Element FamilyInstance)
        {
            Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
            List<ElementId> aElementIdList = aFamilyInstance.GetSubComponentIds().ToList();
            List<Elements.Element> aResult = new List<Elements.Element>();
            foreach (ElementId aElementId in aElementIdList)
            {
                Autodesk.Revit.DB.Element aElement = aFamilyInstance.Document.GetElement(aElementId);
                if (aElement != null)
                    aResult.Add(aElement.ToDSType(true));
            }
            return aResult;
        }

        /// <summary>
        /// Gets super component of Family Instance
        /// </summary>
        /// <param name="FamilyInstance">Family Instance</param>
        /// <returns name="SuperComponent">Elements</returns>
        /// <search>
        /// Family Instance, Get super components, Get supercomponents, family instance, get supercomponents, get super components,
        /// </search>
        public static Elements.Element SuperComponent(Elements.Element FamilyInstance)
        {
            if (FamilyInstance != null && FamilyInstance.InternalElement != null)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = FamilyInstance.InternalElement as Autodesk.Revit.DB.FamilyInstance;
                return aFamilyInstance.SuperComponent.ToDSType(true);
            }
            return null;
        }

        private static Autodesk.Revit.DB.FamilyInstance SetSpecificCoefficient(Autodesk.Revit.DB.FamilyInstance FamilyInstance, double Value)
        {
            Field aField = null;
            Entity aEntity = null;

            GetField(FamilyInstance, "SpecificCoefficient", "Coefficient", out aField, out aEntity);
            if (aField != null && aEntity != null)
            {
                TransactionManager.Instance.EnsureInTransaction(FamilyInstance.Document);
                aEntity.Set<string>(aField, Value.ToString());
                FamilyInstance.SetEntity(aEntity);
                TransactionManager.Instance.TransactionTaskDone();
                return FamilyInstance;
            }


            return null;
        }

        private static Autodesk.Revit.DB.FamilyInstance SetSpecificLoss(Autodesk.Revit.DB.FamilyInstance FamilyInstance, double Value)
        {
            Field aField = null;
            Entity aEntity = null;

            GetField(FamilyInstance, "SpecificLoss", "PressureLoss", out aField, out aEntity);
            if(aField != null && aEntity != null)
            {
                TransactionManager.Instance.EnsureInTransaction(FamilyInstance.Document);
                aEntity.Set<string>(aField, Value.ToString());
                FamilyInstance.SetEntity(aEntity);
                TransactionManager.Instance.TransactionTaskDone();
                return FamilyInstance;
            }

            return null;
        }

        private static double? GetSpecificCoefficient(Autodesk.Revit.DB.FamilyInstance FamilyInstance)
        {
            Field aField = null;
            Entity aEntity = null;

            GetField(FamilyInstance, "SpecificCoefficient", "Coefficient", out aField, out aEntity);

            if (aField != null && aEntity != null)
            {
                double aValue;
                if (double.TryParse(aEntity.Get<string>(aField), out aValue))
                    return aValue;
            }

            return null;
        }

        private static double? GetSpecificLoss(Autodesk.Revit.DB.FamilyInstance FamilyInstance)
        {
            Field aField = null;
            Entity aEntity = null;
            
            GetField(FamilyInstance, "SpecificLoss", "PressureLoss", out aField, out aEntity);

            if(aField != null && aEntity != null)
            {
                double aValue;
                if (double.TryParse(aEntity.Get<string>(aField), out aValue))
                    return aValue;
            }

            return null;
        }

        private static void GetField(Autodesk.Revit.DB.FamilyInstance FamilyInstance, string SchemaName, string FieldName, out Field Field, out Entity Entity)
        {
            Entity = null;
            Field = null;
            
            List<Schema> aSchemaList = Schema.ListSchemas().ToList();
            foreach (Schema aSchema in aSchemaList)
                if (aSchema.SchemaName == SchemaName)
                {
                    Entity = FamilyInstance.GetEntity(aSchema);
                    Field = aSchema.GetField(FieldName);
                    break;
                }
        }
    }
}
