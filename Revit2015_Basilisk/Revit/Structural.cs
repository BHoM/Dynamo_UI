using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Revit.Elements;
using RevitServices.Transactions;

using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

using BHoM.Structural;

using RevitServices.Persistence;
using Revit.GeometryConversion;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

using Autodesk.Revit.DB;
using Autodesk.Revit.Creation;

namespace Revit
{
    public class Structural
    {
        internal Structural() { }

        public static double toMills = 1000;
        public static double toFeet = 3.2808399;

        /// <summary>
        /// Update Revit node positions
        /// </summary>
        /// <param name="nodeElements"></param>Revit nodes to update
        /// <param name="points"></param>Points for new node positions
        /// <returns></returns>
        public static void ChangeNodePosition(List<Revit.Elements.Element> nodeElements, List<Autodesk.DesignScript.Geometry.Point> points)
        {
            List<Autodesk.Revit.DB.Element> aElementList = new List<Autodesk.Revit.DB.Element>();

            for (int i = 0; i < nodeElements.Count; i++)
            {
                Autodesk.Revit.DB.Element aTempElement = nodeElements[i].InternalElement;
                aElementList.Add(aTempElement);
            }

            for (int i = 0; i < aElementList.Count; i++)
            {
                if (aElementList[i].Location is Autodesk.Revit.DB.LocationPoint)
                {
                    Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
                    TransactionManager.Instance.EnsureInTransaction(aDocument);
                    Autodesk.Revit.DB.LocationPoint aLocationPoint = aElementList[i].Location as Autodesk.Revit.DB.LocationPoint;
                    aLocationPoint.Point = points[i].ToRevitType();
                    TransactionManager.Instance.TransactionTaskDone();
                }
            }
        }

        /// <summary>
        /// Update Revit beam locations
        /// </summary>
        /// <param name="famInst"></param>Revit family instance to update
        /// <param name="curves"></param>New location curves
        /// <returns></returns>
        public static void ChangeBeamCurve(List<Revit.Elements.Element> famInst, List<Autodesk.DesignScript.Geometry.Curve> curves)
        {
            List<Autodesk.Revit.DB.Element> aElementList = new List<Autodesk.Revit.DB.Element>();

            for (int i = 0; i < famInst.Count; i++)
            {
                Autodesk.Revit.DB.Element aTempElement = famInst[i].InternalElement;
                aElementList.Add(aTempElement);
            }

            for (int i = 0; i < aElementList.Count; i++)
            {
                if (aElementList[i].Location is Autodesk.Revit.DB.LocationCurve)
                {
                    Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
                    TransactionManager.Instance.EnsureInTransaction(aDocument);
                    Autodesk.Revit.DB.LocationCurve aLocationCurve = aElementList[i].Location as Autodesk.Revit.DB.LocationCurve;
                    aLocationCurve.Curve = curves[i].ToRevitType() as Autodesk.Revit.DB.Curve;
                    TransactionManager.Instance.TransactionTaskDone();
                }
            }
        }

        /// <summary>
        /// Delete revit elements
        /// </summary>
        /// <param name="element"></param>Element to be deleted
        /// <returns></returns>
        public static void DeleteElement(Revit.Elements.Element element)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);

            // Delete an element via its id
            Autodesk.Revit.DB.Element autodeskElement = element.InternalElement as Autodesk.Revit.DB.Element;
            Autodesk.Revit.DB.ElementId elementId = autodeskElement.Id;
            ICollection<Autodesk.Revit.DB.ElementId> deletedIdSet = aDocument.Delete(elementId);
            TransactionManager.Instance.TransactionTaskDone();

            if (0 == deletedIdSet.Count)
            {
                throw new Exception("Deleting the selected element in Revit failed.");
            }

            String prompt = "The selected element has been removed and ";
            prompt += deletedIdSet.Count - 1;
            prompt += " more dependent elements have also been removed.";

            // Give the user some information
            TaskDialog.Show("Revit", prompt);
        }

        /* COMMENTED BY JAKUB ZIOLKOWSKI

        /// <summary>
        /// Duplicate revit family types that matches section property revit name with section property dimensions
        /// </summary>
        /// <param name="sectionProperties"></param>Section property to make family type of
        /// <returns></returns>
        public static List<string> DuplicateSectionTypes(List<SectionProperty> sectionProperties)
        {
            Autodesk.Revit.DB.Document activeDoc = DocumentManager.Instance.CurrentDBDocument;
            List<string> typeNames = GetActiveDocFamilyTypeNames();

            foreach (SectionProperty sectionProperty in sectionProperties)
            {
                if (sectionProperty.RevitFamilyTypeName.ToString() != "")
                    if (CheckFamilyType(sectionProperty, activeDoc, typeNames, sectionProperty.RevitFamilyTypeName))
                        switch (sectionProperty.Type)
                        {
                            case ("UC"):
                                CreateUBUCFamilyType(sectionProperty, activeDoc, GetTypeParentFamilySymbol("_UC-Universal Columns-Column"), sectionProperty.RevitFamilyTypeName);
                                break;

                            case ("UB"):
                                CreateUBUCFamilyType(sectionProperty, activeDoc, GetTypeParentFamilySymbol("Z_UB-Universal Beams"), sectionProperty.RevitFamilyTypeName);
                                break;

                            case ("CHS"):
                                CreateCHSFamilyType(sectionProperty, activeDoc, GetTypeParentFamilySymbol("Z_Circular Hollow Sections"), sectionProperty.RevitFamilyTypeName);
                                break;

                            case ("EXP"):
                                CreateCableFamilyType(sectionProperty, activeDoc, GetTypeParentFamilySymbol("Z_Cable"), sectionProperty.RevitFamilyTypeName);
                                break;

                            case ("RHS"):
                                if (!sectionProperty.IsTapered)
                                    CreateRHSFamilyType(sectionProperty, activeDoc, GetTypeParentFamilySymbol("Z_Rectangular Hollow Sections"), sectionProperty.RevitFamilyTypeName);

                                else
                                {
                                    if (sectionProperty.TaperIntermediatePos[1].Equals(double.NaN) || sectionProperty.TaperIntermediatePos[1] == 0)
                                    {
                                        string typeParentName = "Z_Cantilever Taper Box";
                                        if (sectionProperty.Name.StartsWith("STRUT"))
                                            typeParentName = "Z_Strut Taper Box";

                                        CreateRHSTapered1FamilyType(sectionProperty, activeDoc, GetTypeParentFamilySymbol(typeParentName), sectionProperty.RevitFamilyTypeName);
                                    }
                                    else
                                        CreateRHSTapered2FamilyType(sectionProperty, activeDoc, GetTypeParentFamilySymbol("Z_Rafter Taper Box"), sectionProperty.RevitFamilyTypeName);
                                }
                                break;
                        }
            }

            return typeNames;
        }


        private static bool CheckFamilyType(SectionProperty sectionProperty, Autodesk.Revit.DB.Document activeDoc, List<string> typeNames, string newTypeName)
        {
            if (typeNames.Contains(newTypeName))
                return false;
            else
            {
                typeNames.Add(newTypeName);
                return true;
            }
        }

        private static void CreateUBUCFamilyType(SectionProperty sectionProperty, Autodesk.Revit.DB.Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.GetParameters("bf").First().Set(sectionProperty.B * toFeet);
            newFamilyType.GetParameters("d").First().Set(sectionProperty.D * toFeet);
            newFamilyType.GetParameters("tw").First().Set(sectionProperty.tw * toFeet);
            newFamilyType.GetParameters("tf").First().Set(sectionProperty.tf * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static void CreateCHSFamilyType(SectionProperty sectionProperty, Autodesk.Revit.DB.Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.GetParameters("OD").First().Set(sectionProperty.D * toFeet);
            newFamilyType.GetParameters("t").First().Set(sectionProperty.tw * toFeet);

            if (sectionProperty.D == 0)
                newFamilyType.GetParameters("OD").First().Set(0.02 * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static void CreateCableFamilyType(SectionProperty sectionProperty, Autodesk.Revit.DB.Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.GetParameters("OD").First().Set(sectionProperty.D * toFeet);

            if (sectionProperty.D == 0)
                newFamilyType.GetParameters("OD").First().Set(0.02 * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static void CreateRHSFamilyType(SectionProperty sectionProperty, Autodesk.Revit.DB.Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.GetParameters("b").First().Set(sectionProperty.B * toFeet);
            newFamilyType.GetParameters("t").First().Set(sectionProperty.tw * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static void CreateRHSTapered2FamilyType(SectionProperty sectionProperty, Autodesk.Revit.DB.Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.GetParameters("Start depth").First().Set(sectionProperty.TaperDepth[0] * toFeet);
            newFamilyType.GetParameters("TIM1 depth").First().Set(sectionProperty.TaperDepth[1] * toFeet);
            newFamilyType.GetParameters("TIM2 depth").First().Set(sectionProperty.TaperDepth[2] * toFeet);
            newFamilyType.GetParameters("End depth").First().Set(sectionProperty.TaperDepth[3] * toFeet);
            newFamilyType.GetParameters("TIM1 pos").First().Set(sectionProperty.TaperIntermediatePos[0]);
            newFamilyType.GetParameters("TIM2 pos").First().Set(sectionProperty.TaperIntermediatePos[1]);
            newFamilyType.GetParameters("Breadth").First().Set(sectionProperty.B * toFeet);
            newFamilyType.GetParameters("Thickness").First().Set(0.01 * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static void CreateRHSTapered1FamilyType(SectionProperty sectionProperty, Autodesk.Revit.DB.Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.GetParameters("Start depth").First().Set(sectionProperty.TaperDepth[0] * toFeet);
            newFamilyType.GetParameters("TIM1 depth").First().Set(sectionProperty.TaperDepth[1] * toFeet);
            newFamilyType.GetParameters("End depth").First().Set(sectionProperty.TaperDepth[3] * toFeet);
            newFamilyType.GetParameters("TIM1 pos").First().Set(sectionProperty.TaperIntermediatePos[0]);
            newFamilyType.GetParameters("Breadth").First().Set(sectionProperty.B * toFeet);
            newFamilyType.GetParameters("Thickness").First().Set(0.01 * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }


        COMMENTED BY JAKUB ZIOLKOWSKI */

        private static Autodesk.Revit.DB.FamilySymbol GetTypeParentFamilySymbol(string typeParentName)
        {
            Autodesk.Revit.DB.Document activeDoc = DocumentManager.Instance.CurrentDBDocument;
            FilteredElementCollector familySymbolCollector = new FilteredElementCollector(activeDoc).OfClass(typeof(Autodesk.Revit.DB.FamilySymbol));
            List<Autodesk.Revit.DB.FamilySymbol> documentFamilyTypes = familySymbolCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);
            Autodesk.Revit.DB.FamilySymbol familyParentType = documentFamilyTypes.Find(x => x.Name == typeParentName);

            return familyParentType;
        }

        private static List<string> GetActiveDocFamilyTypeNames()
        {
            Autodesk.Revit.DB.Document activeDoc = DocumentManager.Instance.CurrentDBDocument;
            FilteredElementCollector familySymbolCollector = new FilteredElementCollector(activeDoc).OfClass(typeof(Autodesk.Revit.DB.FamilySymbol));
            List<Autodesk.Revit.DB.FamilySymbol> documentFamilyTypes = familySymbolCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);

            List<string> ActiveDocFamilyTypeNames = new List<string>();

            foreach (Autodesk.Revit.DB.FamilySymbol type in documentFamilyTypes)
            {
                ActiveDocFamilyTypeNames.Add(type.Name);
            }
            return ActiveDocFamilyTypeNames;
        }

       

        /// <summary>
        /// Get revit worksets with names and IDs
        /// </summary>
        /// <returns></returns>
        [MultiReturn(new[] { "Worksets", "Names", "IDs" })]
        public static Dictionary<string, object> GetWorksets()
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;

            FilteredWorksetCollector collector = new FilteredWorksetCollector(aDocument);
            FilteredWorksetCollector user_worksets = collector.OfKind(Autodesk.Revit.DB.WorksetKind.UserWorkset);

            List<Autodesk.Revit.DB.Workset> worksets = collector.ToWorksets().ToList();

            List<WorksetId> worksetIds = collector.ToWorksetIds().ToList();

            List<string> worksetNames = new List<string>();

            foreach (Autodesk.Revit.DB.Workset workset in worksets)
                worksetNames.Add(workset.Name.ToString());


            return new Dictionary<string, object>
            {
                {"Worksets", worksets},
                {"Names", worksetNames},
                {"IDs", worksetIds},
            };

        }

        /// <summary>
        /// Get Revit views based on their names
        /// </summary>
        /// <param name="Names"></param>Names of views
        /// <returns></returns>
        public static List<Revit.Elements.Element> GetViewsByNames(List<string> Names)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);

            List<Revit.Elements.Element> viewList = new List<Revit.Elements.Element>();

            foreach (string name in Names)
            {
                FilteredElementCollector viewCollector = new FilteredElementCollector(aDocument);
                viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));

                foreach (Autodesk.Revit.DB.Element viewElement in viewCollector)
                {
                    Autodesk.Revit.DB.View view = (Autodesk.Revit.DB.View)viewElement;
                    if (view.Name == name)
                        viewList.Add(view.ToDSType(true));
                }

            }

            return viewList;
        }




    }

}
