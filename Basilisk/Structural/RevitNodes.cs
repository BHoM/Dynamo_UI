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

using ExcelUtilities;
using BHExcelFormat;
using BHExcelFormat.Tabs.Geometry;
using BHExcelFormat.Tabs.Loading;
using BHExcelFormat.Tabs.Results;
using IO;
using StructuralComponents;
using StructuralComponents.Results;
using StructuralComponents.SectionProperties;

using RevitServices.Persistence;
using Revit.GeometryConversion;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

using Autodesk.Revit.DB;


namespace Basilisk.Structural
{

    public static class RevitNodes
    {
        public static double toMills = 1000;
        public static double toFeet = 3.2808399;

        
 
        public static void Duplicate(List<SectionProperty> secProp)
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            string symName = null;
            string newSymName = null;
            List<string> symNames = new List<string>();
            FilteredElementCollector aFilteredElementCollector = new FilteredElementCollector(aDocument).OfClass(typeof(Autodesk.Revit.DB.FamilySymbol));
            List<Autodesk.Revit.DB.FamilySymbol> aFamilySymbolList = aFilteredElementCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);
            foreach (SectionProperty sp in secProp)
            {
                if (sp.Type == "UC" || sp.Type == "UB")
                {
                    symName = "UBfamily";
                    
                    if (sp.Type == "UB")
                    {
                        symName = "UBfamily";
                    }

                    newSymName = sp.Type + " " + (sp.D * 1000).ToString() + "x" + (sp.B * 1000).ToString() + "x" + (sp.tw * 1000).ToString();
                    Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
                    if (aFamilySymbol != null && !symNames.Contains(newSymName))
                    {
                        TransactionManager.Instance.EnsureInTransaction(aDocument);
                        ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
                        Autodesk.Revit.DB.Parameter parBf = aElementType.get_Parameter("B");
                        parBf.Set(sp.B * 1000 * 0.0032808399);
                        Autodesk.Revit.DB.Parameter parD = aElementType.get_Parameter("D");
                        parD.Set(sp.D * 1000 * 0.0032808399);
                        Autodesk.Revit.DB.Parameter partw1 = aElementType.get_Parameter("tw");
                        partw1.Set(sp.tw * 1000 * 0.0032808399);
                        Autodesk.Revit.DB.Parameter partf = aFamilySymbol.get_Parameter("tf");
                        partf.Set(sp.tf * 1000 * 0.0032808399);
                        TransactionManager.Instance.TransactionTaskDone();
                        //if (aElementType != null)
                        // return aElementType.ToDSType(true) as Revit.Elements.FamilySymbol;

                    }
                    symNames.Add(newSymName);
                }

                if (sp.Type == "CHS" || sp.Type == "EXP")
                {
                    symName = "CHSfamily";
                    double t = sp.tw * 1000;
                    double D = sp.D * 1000;
                    if (sp.Type == "EXP")
                    {
                        t = 5;
                    }
                    if (D == 0)
                    {
                        D = 20;
                    }
                    newSymName = sp.Type + " " + D.ToString() + "x" + t.ToString();
                    Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
                    if (aFamilySymbol != null && !symNames.Contains(newSymName))
                    {
                        TransactionManager.Instance.EnsureInTransaction(aDocument);
                        ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
                        Autodesk.Revit.DB.Parameter parOD = aElementType.get_Parameter("d");
                        parOD.Set(D * 0.0032808399);
                        Autodesk.Revit.DB.Parameter part = aElementType.get_Parameter("t");
                        part.Set(t * 0.0032808399);
                        TransactionManager.Instance.TransactionTaskDone();
                        //if (aElementType != null)
                        // return aElementType.ToDSType(true) as Revit.Elements.FamilySymbol;

                    }
                    symNames.Add(newSymName);
                }


                if (sp.Type == "RHS")
                {
                    if (!sp.IsTapered)
                    {
                        symName = "RHS";
                        newSymName = sp.Type + " " + (sp.B * 1000).ToString() + "x" + (sp.D * 1000).ToString() + "x" + (sp.tw * 1000).ToString();
                        Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
                        if (aFamilySymbol != null && !symNames.Contains(newSymName))
                        {

                            TransactionManager.Instance.EnsureInTransaction(aDocument);
                            ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
                            Autodesk.Revit.DB.Parameter parHt = aElementType.get_Parameter("D");
                            parHt.Set(sp.D * 1000 * 0.0032808399);
                            Autodesk.Revit.DB.Parameter parB = aElementType.get_Parameter("B");
                            parB.Set(sp.D * 1000 * 0.0032808399);
                            Autodesk.Revit.DB.Parameter partw2 = aElementType.get_Parameter("t");
                            partw2.Set(sp.tw * 1000 * 0.0032808399);
                            TransactionManager.Instance.TransactionTaskDone();
                            // (aElementType != null)
                            // return aElementType.ToDSType(true) as Revit.Elements.FamilySymbol;

                        }
                    }


                    if (sp.IsTapered)
                    {
                        if (!sp.TaperIntermediatePos[1].IsValidNumber() || sp.TaperIntermediatePos[1] == 0)
                        {
                            symName = "RHS Tapered 1";
                            newSymName = "Tapered1 " + sp.Type + " " + (sp.TaperDepth[0] * 1000).ToString() + "x" + (sp.TaperDepth[1] * 1000).ToString() + "x" + (sp.TaperDepth[3] * 1000).ToString() + (sp.B * 1000).ToString() + "x" + (sp.TaperIntermediatePos[0]).ToString() + "x" + (sp.CutbackS * 1000).ToString() + "x" + (sp.CutbackE * 1000).ToString();
                            Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
                            if (aFamilySymbol != null && !symNames.Contains(newSymName))
                            {

                                TransactionManager.Instance.EnsureInTransaction(aDocument);
                                ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
                                Autodesk.Revit.DB.Parameter parSd = aElementType.get_Parameter("Start depth");
                                parSd.Set(sp.TaperDepth[0] * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parTd = aElementType.get_Parameter("TIM1 depth");
                                parTd.Set(sp.TaperDepth[1] * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parEd = aElementType.get_Parameter("End depth");
                                parEd.Set(sp.TaperDepth[3] * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parTl = aElementType.get_Parameter("TIM1 pos");
                                parTl.Set(sp.TaperIntermediatePos[0]);//sp.TaperIntermediatePos[0]
                                Autodesk.Revit.DB.Parameter parTb = aElementType.get_Parameter("Breadth");
                                parTb.Set(sp.B * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parThick = aElementType.get_Parameter("Thickness");
                                parThick.Set(10 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parSc = aElementType.get_Parameter("Start cutback");
                                parSc.Set(sp.CutbackS * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parEc = aElementType.get_Parameter("End cutback");
                                parEc.Set(sp.CutbackE * 1000 * 0.0032808399);
                                TransactionManager.Instance.TransactionTaskDone();
                            }
                        }

                        else
                        {
                            symName = "RHS Tapered 2";
                            newSymName = "Tapered2 " + sp.Type + " " + (sp.TaperDepth[0] * 1000).ToString() + "x" + (sp.TaperDepth[1] * 1000).ToString() + "x" + (sp.TaperDepth[3] * 1000).ToString() + (sp.B * 1000).ToString() + "x" + (sp.TaperIntermediatePos[0]).ToString() + "x" + (sp.TaperIntermediatePos[1]).ToString() + "x" + (sp.CutbackS * 1000).ToString() + "x" + (sp.CutbackE * 1000).ToString();
                            Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
                            if (aFamilySymbol != null && !symNames.Contains(newSymName))
                            {

                                TransactionManager.Instance.EnsureInTransaction(aDocument);
                                ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
                                Autodesk.Revit.DB.Parameter parSd = aElementType.get_Parameter("Start depth");
                                parSd.Set(sp.TaperDepth[0] * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parTd = aElementType.get_Parameter("TIM1 depth");
                                parTd.Set(sp.TaperDepth[1] * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parEd = aElementType.get_Parameter("End depth");
                                parEd.Set(sp.TaperDepth[3] * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parTl1 = aElementType.get_Parameter("TIM1 pos");
                                parTl1.Set(sp.TaperIntermediatePos[0]);
                                Autodesk.Revit.DB.Parameter parTl2 = aElementType.get_Parameter("TIM2 pos");
                                parTl2.Set(sp.TaperIntermediatePos[1]);
                                Autodesk.Revit.DB.Parameter parTb = aElementType.get_Parameter("Breadth");
                                parTb.Set(sp.B * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parThick = aElementType.get_Parameter("Thickness");
                                parThick.Set(10 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parSc = aElementType.get_Parameter("Start cutback");
                                parSc.Set(sp.CutbackS * 1000 * 0.0032808399);
                                Autodesk.Revit.DB.Parameter parEc = aElementType.get_Parameter("End cutback");
                                parEc.Set(sp.CutbackE * 1000 * 0.0032808399);
                                TransactionManager.Instance.TransactionTaskDone();
                            }

                        }

                    }
                    symNames.Add(newSymName);
                }
            }
           //return null;
        }

        public static List<string> CreateSectionTypes(List<SectionProperty> sectionProperties)
        {
            Document activeDoc = DocumentManager.Instance.CurrentDBDocument;
            List<string> typeNames = GetActiveDocFamilyTypeNames();
            
            foreach (SectionProperty sectionProperty in sectionProperties)
            {
                switch (sectionProperty.Type)
                { 
                    case ("UC"):
                        CheckAndCreateUBFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
                        break;

                    case ("UB"):
                        CheckAndCreateUBFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
                        break;

                    case ("CHS"):
                        CheckAndCreateCHSFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
                        break;

                    case ("EXP"):
                        CheckAndCreateCHSFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
                        break;

                    case ("RHS"):
                        if (!sectionProperty.IsTapered)
                            CheckAndCreateRHSFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);

                        else
                        {
                            if (!sectionProperty.TaperIntermediatePos[1].IsValidNumber() || sectionProperty.TaperIntermediatePos[1] == 0)
                                CheckAndCreateRHSTapered1FamilyType(sectionProperty, activeDoc, typeNames, out typeNames);

                            else
                                CheckAndCreateRHSTapered2FamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
                        }
                        break;
                }
            }

            return typeNames;
        }

        private static void CheckAndCreateUBFamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
        { 
            string typeParentName = "UBfamily";
            //string newTypeName = sectionProperty.Type + " " + (sectionProperty.D * toMills).ToString() + "x" + (sectionProperty.B * toMills).ToString() + "x" + (sectionProperty.tw * toMills).ToString();
            string newTypeName = sectionProperty.Name;
            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
            typeNamesNew = typeNames;

            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
                return;

            else
            {
                CreateUBFamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
                typeNamesNew.Add(newTypeName);
            }
        }

        private static void CreateUBFamilyType(SectionProperty sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.get_Parameter("B").Set(sectionProperty.B * toFeet);
            newFamilyType.get_Parameter("D").Set(sectionProperty.D * toFeet);
            newFamilyType.get_Parameter("tw").Set(sectionProperty.tw * toFeet);
            newFamilyType.get_Parameter("tf").Set(sectionProperty.tf * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static void CheckAndCreateCHSFamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
        {
            string typeParentName = "CHSfamily";
            //string newTypeName = sectionProperty.Type + " " + (sectionProperty.D * toMills).ToString() + "x" + (sectionProperty.tw * toMills).ToString();
            string newTypeName = sectionProperty.Name;
            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
            typeNamesNew = typeNames;

            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
                return;

            else
            {
                CreateCHSFamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
                typeNamesNew.Add(newTypeName);
            }
        }


        private static void CreateCHSFamilyType(SectionProperty sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.get_Parameter("d").Set(sectionProperty.D * toFeet);
            newFamilyType.get_Parameter("t").Set(sectionProperty.tw * toFeet);

            if (sectionProperty.Type == "EXP")
                newFamilyType.get_Parameter("t").Set(0.005 * toFeet);

            if (sectionProperty.D == 0)
                newFamilyType.get_Parameter("d").Set(0.02 * toFeet);

                

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static void CheckAndCreateRHSFamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
        {
            string typeParentName = "RHS";
            //string newTypeName = sectionProperty.Type + " " + (sectionProperty.D * toMills).ToString() + "x" + (sectionProperty.tw * toMills).ToString();
            string newTypeName = sectionProperty.Name;
            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
            typeNamesNew = typeNames;

            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
                return;

            else
            {
                CreateRHSFamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
                typeNamesNew.Add(newTypeName);
            }
        }


        private static void CreateRHSFamilyType(SectionProperty sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.get_Parameter("D").Set(sectionProperty.D * toFeet);
            newFamilyType.get_Parameter("B").Set(sectionProperty.B * toFeet);
            newFamilyType.get_Parameter("t").Set(sectionProperty.tw * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static void CheckAndCreateRHSTapered2FamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
        {
            string typeParentName = "RHS Tapered 2";
            //string newTypeName = "Tapered2 " + sectionProperty.Type + " " + (sectionProperty.TaperDepth[0] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[1] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[2] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[3] * toMills).ToString() + (sectionProperty.B * toMills).ToString() + "x" + (sectionProperty.TaperIntermediatePos[0]).ToString() + "x" + (sectionProperty.TaperIntermediatePos[1]).ToString() + "x" + (sectionProperty.CutbackS * toMills).ToString() + "x" + (sectionProperty.CutbackE * toMills).ToString();
            string newTypeName = sectionProperty.Name;
            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
            typeNamesNew = typeNames;

            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
                return;

            else
            {
                CreateRHSTapered2FamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
                typeNamesNew.Add(newTypeName);
            }
        }

        private static void CreateRHSTapered2FamilyType(SectionProperty sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.get_Parameter("Start depth").Set(sectionProperty.TaperDepth[0] * toFeet);
            newFamilyType.get_Parameter("TIM1 depth").Set(sectionProperty.TaperDepth[1] * toFeet);
            newFamilyType.get_Parameter("TIM2 depth").Set(sectionProperty.TaperDepth[2] * toFeet);
            newFamilyType.get_Parameter("End depth").Set(sectionProperty.TaperDepth[3] * toFeet);
            newFamilyType.get_Parameter("TIM1 pos").Set(sectionProperty.TaperIntermediatePos[0]);
            newFamilyType.get_Parameter("TIM2 pos").Set(sectionProperty.TaperIntermediatePos[1]);
            newFamilyType.get_Parameter("Breadth").Set(sectionProperty.B * toFeet);
            newFamilyType.get_Parameter("Thickness").Set(0.01 * toFeet);
            newFamilyType.get_Parameter("Start cutback").Set(sectionProperty.CutbackS * toFeet);
            newFamilyType.get_Parameter("End cutback").Set(sectionProperty.CutbackE * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static void CheckAndCreateRHSTapered1FamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
        {
            string typeParentName = "RHS Tapered 1";
            //string newTypeName = "Tapered1 " + sectionProperty.Type + " " + (sectionProperty.TaperDepth[0] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[1] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[3] * toMills).ToString() + (sectionProperty.B * toMills).ToString() + "x" + (sectionProperty.TaperIntermediatePos[0]).ToString() + "x" + (sectionProperty.TaperIntermediatePos[1]).ToString() + "x" + (sectionProperty.CutbackS * toMills).ToString() + "x" + (sectionProperty.CutbackE * toMills).ToString();
            string newTypeName = sectionProperty.Name;
            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
            typeNamesNew = typeNames;

            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
                return;

            else
            {
                CreateRHSTapered1FamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
                typeNamesNew.Add(newTypeName);
            }
        }


        private static void CreateRHSTapered1FamilyType(SectionProperty sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
        {
            TransactionManager.Instance.EnsureInTransaction(activeDoc);

            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
            newFamilyType.get_Parameter("Start depth").Set(sectionProperty.TaperDepth[0] * toFeet);
            newFamilyType.get_Parameter("TIM1 depth").Set(sectionProperty.TaperDepth[1] * toFeet);
            newFamilyType.get_Parameter("End depth").Set(sectionProperty.TaperDepth[3] * toFeet);
            newFamilyType.get_Parameter("TIM1 pos").Set(sectionProperty.TaperIntermediatePos[0]);
            newFamilyType.get_Parameter("Breadth").Set(sectionProperty.B * toFeet);
            newFamilyType.get_Parameter("Thickness").Set(0.01 * toFeet);
            newFamilyType.get_Parameter("Start cutback").Set(sectionProperty.CutbackS * toFeet);
            newFamilyType.get_Parameter("End cutback").Set(sectionProperty.CutbackE * toFeet);

            TransactionManager.Instance.TransactionTaskDone();
        }

        private static Autodesk.Revit.DB.FamilySymbol GetTypeParentFamilySymbol(string typeParentName)
        {
            Document activeDoc = DocumentManager.Instance.CurrentDBDocument;
            FilteredElementCollector familySymbolCollector = new FilteredElementCollector(activeDoc).OfClass(typeof(Autodesk.Revit.DB.FamilySymbol));
            List<Autodesk.Revit.DB.FamilySymbol> documentFamilyTypes = familySymbolCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);
            Autodesk.Revit.DB.FamilySymbol familyParentType = documentFamilyTypes.Find(x => x.Name == typeParentName);

            return familyParentType;
        }

        private static List<string> GetActiveDocFamilyTypeNames()
        {
            Document activeDoc = DocumentManager.Instance.CurrentDBDocument;
            FilteredElementCollector familySymbolCollector = new FilteredElementCollector(activeDoc).OfClass(typeof(Autodesk.Revit.DB.FamilySymbol));
            List<Autodesk.Revit.DB.FamilySymbol> documentFamilyTypes = familySymbolCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);

            List<string> ActiveDocFamilyTypeNames = new List<string>();

            foreach (Autodesk.Revit.DB.FamilySymbol type in documentFamilyTypes)
            {
                ActiveDocFamilyTypeNames.Add(type.Name);
            }
            return ActiveDocFamilyTypeNames;
        }

        

        public static void SetElementRhinoGUID(List<Autodesk.Revit.DB.Element> Elm, List<string> guid)
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            for (int i = 0; i < Elm.Count; i++)
            {
                TransactionManager.Instance.EnsureInTransaction(aDocument);
                Autodesk.Revit.DB.Parameter tempPar = Elm[i].get_Parameter("Rhino_GUID");
                tempPar.Set(guid[i]);
                TransactionManager.Instance.TransactionTaskDone();
            }
        }



        public static void ChangeBeamLine(List<Revit.Elements.Element> famInst, List<Autodesk.DesignScript.Geometry.Line> lines)
        {
            List<Autodesk.Revit.DB.Element> aElementList = new List<Autodesk.Revit.DB.Element>();

            for (int i = 0; i < famInst.Count; i++)
            {
                Autodesk.Revit.DB.Element aTempElement = famInst[i].InternalElement;
                aElementList.Add(aTempElement);
            }

            //List<Autodesk.Revit.DB.FamilySymbol> aFamilySymbolList = aElementList.ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            for (int i = 0; i < aElementList.Count; i++)
            {
                if (aElementList[i].Location is Autodesk.Revit.DB.LocationCurve)
                {
                    Autodesk.DesignScript.Geometry.Point sPts = lines[i].StartPoint;
                    Autodesk.DesignScript.Geometry.Point ePts = lines[i].EndPoint;
                    TransactionManager.Instance.EnsureInTransaction(aDocument);
                    Autodesk.Revit.DB.LocationCurve aLocationCurve = aElementList[i].Location as Autodesk.Revit.DB.LocationCurve;
                    Autodesk.Revit.DB.XYZ aXYZ_1 = new Autodesk.Revit.DB.XYZ(sPts.X * 0.0032808399, sPts.Y * 0.0032808399, sPts.Z * 0.0032808399);
                    Autodesk.Revit.DB.XYZ aXYZ_2 = new Autodesk.Revit.DB.XYZ(ePts.X * 0.0032808399, ePts.Y * 0.0032808399, ePts.Z * 0.0032808399);
                    Autodesk.Revit.DB.Line aLine = Autodesk.Revit.DB.Line.CreateBound(aXYZ_1, aXYZ_2);
                    aLocationCurve.Curve = aLine;
                    TransactionManager.Instance.TransactionTaskDone();
                }
            }
        }



        public static Autodesk.Revit.DB.Element MoveElementByVector(Revit.Elements.Element Elm, Vector Vec)
        {
            Autodesk.Revit.DB.Element movedElm;
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;

            Autodesk.Revit.DB.Element newEl = Elm.InternalElement;

            Autodesk.Revit.DB.XYZ newLoc = Vec.ToRevitType(true);

            TransactionManager.Instance.EnsureInTransaction(aDocument);

            ElementTransformUtils.MoveElement(aDocument, newEl.Id, newLoc);

            TransactionManager.Instance.TransactionTaskDone();
            newEl.ToDSType(true);

            movedElm=newEl;
            

            return movedElm;
        }



        public static List<Autodesk.DesignScript.Geometry.Curve> FindLineByGrid(List<Autodesk.DesignScript.Geometry.Curve> Curves, List<Revit.Elements.Grid> Grids)
        {
            List<Autodesk.DesignScript.Geometry.Curve> CloseCurves = new List<Autodesk.DesignScript.Geometry.Curve>();


            foreach (Revit.Elements.Grid g in Grids)
            
            {
                Autodesk.DesignScript.Geometry.Curve CloseCurve = null;

                double distMin = g.Curve.ClosestPointTo(Curves[0].StartPoint).DistanceTo(Curves[0].StartPoint);

                foreach (Autodesk.DesignScript.Geometry.Curve crv in Curves)
                {
                    double dist = g.Curve.ClosestPointTo(crv.StartPoint).DistanceTo(crv.StartPoint);

                    if (dist <= distMin)
                    {
                        CloseCurve = crv;
                        distMin = dist;
                    }
                }

                CloseCurves.Add(CloseCurve);
            }

            return CloseCurves;
        }


        [MultiReturn(new[] { "Worksets", "Names", "IDs" })]
        public static Dictionary<string, object> GetWorksets()
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;

            FilteredWorksetCollector collector = new FilteredWorksetCollector(aDocument);
            FilteredWorksetCollector user_worksets = collector.OfKind(WorksetKind.UserWorkset);

            List<Workset> worksets = collector.ToWorksets().ToList();
            
            List<WorksetId> worksetIds = collector.ToWorksetIds().ToList();

            List<string> worksetNames = new List<string>();

            foreach (Workset workset in worksets)
		         worksetNames.Add(workset.Name.ToString());

            
            return new Dictionary<string, object>
            {
                {"Worksets", worksets},
                {"Names", worksetNames},
                {"IDs", worksetIds},
            };

        }

        public static Autodesk.Revit.DB.Element CreateFloorOpening(Revit.Elements.Floor Floor, List<Autodesk.DesignScript.Geometry.Curve> Curves)
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);

            CurveArray CurveArray = new CurveArray();

            foreach (Autodesk.DesignScript.Geometry.Curve crv in Curves)
                CurveArray.Append(crv.ToRevitType());

            Autodesk.Revit.DB.Element newFloor = Floor.InternalElement;

            Opening opening = aDocument.Create.NewOpening(newFloor, CurveArray, true);

            newFloor.ToDSType(false);

            TransactionManager.Instance.TransactionTaskDone();

            return newFloor;

        }

      public static List<Revit.Elements.Element> GetElementsByID(List<Revit.Elements.Element> Elements, List<string> ElementIDs, List<string> IDs)
      {
        List<Revit.Elements.Element> ElementsByID = new List<Revit.Elements.Element>();

        foreach (string ID in IDs)
	    {
            for (int i = 0; i < ElementIDs.Count; i++)
                if (ElementIDs[i] == ID)
                {
                    ElementsByID.Add(Elements[i]);
                    break;
                }
	    }

        return ElementsByID;
      }

        public static List<Autodesk.Revit.DB.Curve> NurbsToRevitCurves(List<Autodesk.DesignScript.Geometry.NurbsCurve> nurbsCrvs)
        {
            List<Autodesk.Revit.DB.Curve> revitCrvs = new List<Autodesk.Revit.DB.Curve>();
            Autodesk.Revit.DB.Curve revitCrv;
            foreach (NurbsCurve nurb in nurbsCrvs)
            {
                revitCrv = nurb.ToRevitType();
                revitCrvs.Add(revitCrv);
            }

            return revitCrvs;
        }

        public static Dimension Dimension(Revit.Elements.Element Element)
        {
            Autodesk.Revit.DB.FamilyInstance aFamilyInstance = Element.InternalElement as Autodesk.Revit.DB.FamilyInstance;

            LocationCurve aLocationCurve = aFamilyInstance.Location as LocationCurve;
            if (aLocationCurve != null && aLocationCurve.Curve != null)
            {
                Autodesk.Revit.DB.Line aLine = aLocationCurve.Curve as Autodesk.Revit.DB.Line;
                if (aLine != null)
                {
                    Document aDocument = DocumentManager.Instance.CurrentDBDocument;
                    TransactionManager.Instance.EnsureInTransaction(aDocument);
                    ReferenceArray aReferenceArray = new ReferenceArray();
                    aReferenceArray.Append(aLine.GetEndPointReference(0));
                    aReferenceArray.Append(aLine.GetEndPointReference(1));
                    Dimension aDimension = aDocument.Create.NewDimension(aDocument.ActiveView, aLine, aReferenceArray);
                    TransactionManager.Instance.TransactionTaskDone();
                    return aDimension;
                }
            }
            return null;
        }

        public static Autodesk.DesignScript.Geometry.Line ByStartEndPoint(Autodesk.DesignScript.Geometry.Point StartPoint, Autodesk.DesignScript.Geometry.Point EndPoint)
        {
          
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);


            Autodesk.Revit.DB.Line aLine = Autodesk.Revit.DB.Line.CreateBound(new XYZ(36.4094488189023, Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(StartPoint.Y, DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(StartPoint.Z, DisplayUnitType.DUT_MILLIMETERS)), new XYZ(36.4094488189023, Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(EndPoint.Y, DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(EndPoint.Z, DisplayUnitType.DUT_MILLIMETERS)));
            if(aLine != null)
            {
                DetailCurve aDetailCurve = aDocument.Create.NewDetailCurve(aDocument.ActiveView, aLine);
                if(aDetailCurve != null)
                {
                    TransactionManager.Instance.TransactionTaskDone();
                    return aDetailCurve.GeometryCurve.ToProtoType() as Autodesk.DesignScript.Geometry.Line;
                }
            }
            TransactionManager.Instance.TransactionTaskDone();
            return null;
        }

        public static Autodesk.DesignScript.Geometry.Line CreateDimensionBetweenNodes(Autodesk.DesignScript.Geometry.Point StartPoint, Autodesk.DesignScript.Geometry.Point EndPoint)
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);


            Autodesk.Revit.DB.Line aLine = Autodesk.Revit.DB.Line.CreateBound(new XYZ(36.4094488189023, Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(StartPoint.Y, DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(StartPoint.Z, DisplayUnitType.DUT_MILLIMETERS)), new XYZ(36.4094488189023, Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(EndPoint.Y, DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(EndPoint.Z, DisplayUnitType.DUT_MILLIMETERS)));
            if (aLine != null)
            {
                DetailCurve aDetailCurve = aDocument.Create.NewDetailCurve(aDocument.ActiveView, aLine);
                
                if (aDetailCurve != null)
                {
                    ReferenceArray aReferenceArray = new ReferenceArray();
                    aReferenceArray.Append(aDetailCurve.GeometryCurve.GetEndPointReference(0));
                    aReferenceArray.Append(aDetailCurve.GeometryCurve.GetEndPointReference(1));
                    Dimension aDimension = aDocument.Create.NewDimension(aDocument.ActiveView, Autodesk.Revit.DB.Line.CreateBound(aDetailCurve.GeometryCurve.GetEndPoint(0), aDetailCurve.GeometryCurve.GetEndPoint(1)), aReferenceArray);

                    TransactionManager.Instance.TransactionTaskDone();
                    return aDetailCurve.GeometryCurve.ToProtoType() as Autodesk.DesignScript.Geometry.Line;
                }
            }
            TransactionManager.Instance.TransactionTaskDone();
            return null;
        }

        public static Autodesk.DesignScript.Geometry.Line CreateDimensionBetweenNodes2(Autodesk.DesignScript.Geometry.Point StartPoint, Autodesk.DesignScript.Geometry.Point EndPoint, string viewName)
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);

            FilteredElementCollector viewCollector = new FilteredElementCollector(aDocument);
            viewCollector.OfClass(typeof(View));

            View view = null;

            foreach (Autodesk.Revit.DB.Element viewElement in viewCollector)
            {
                View viewTemp = (View)viewElement;
                if (viewTemp.Name == viewName)
                    view = viewTemp;
            }

            if (view != null)
            {
                Autodesk.Revit.DB.Line aLine = Autodesk.Revit.DB.Line.CreateBound(new XYZ(Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(StartPoint.X, DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(StartPoint.Y, DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(StartPoint.Z, DisplayUnitType.DUT_MILLIMETERS)), new XYZ(Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(EndPoint.X, DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(EndPoint.Y, DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(EndPoint.Z, DisplayUnitType.DUT_MILLIMETERS)));
                if (aLine != null)
                {
                    DetailCurve aDetailCurve = aDocument.Create.NewDetailCurve(view, aLine);
                    if (aDetailCurve != null)
                    {
                        ReferenceArray aReferenceArray = new ReferenceArray();
                        aReferenceArray.Append(aDetailCurve.GeometryCurve.GetEndPointReference(0));
                        aReferenceArray.Append(aDetailCurve.GeometryCurve.GetEndPointReference(1));
                        Dimension aDimension = aDocument.Create.NewDimension(view, Autodesk.Revit.DB.Line.CreateBound(aDetailCurve.GeometryCurve.GetEndPoint(0), aDetailCurve.GeometryCurve.GetEndPoint(1)), aReferenceArray);

                        TransactionManager.Instance.TransactionTaskDone();
                        return aDetailCurve.GeometryCurve.ToProtoType() as Autodesk.DesignScript.Geometry.Line;
                    }
                }
            }
            TransactionManager.Instance.TransactionTaskDone();
            return null;
        }

        public static void CreateSpotDimensionOnNode(Autodesk.DesignScript.Geometry.Point Point)
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            XYZ pt = new XYZ(36.4094488189023, Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(Point.Y, DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(Point.Z, DisplayUnitType.DUT_MILLIMETERS));
            DetailCurve aDetailCurve = aDocument.Create.NewDetailCurve(aDocument.ActiveView, Autodesk.Revit.DB.Line.CreateBound(pt, new XYZ(36.4094488189023, Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits((Point.Y+1000), DisplayUnitType.DUT_MILLIMETERS), Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(Point.Z, DisplayUnitType.DUT_MILLIMETERS))));
            aDocument.Create.NewSpotElevation(aDocument.ActiveView, aDetailCurve.GeometryCurve.GetEndPointReference(0), pt, pt, pt, pt, true);
        }

        public static List<Revit.Elements.Element> GetViewsByNames(List<string> Names)
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);

            List<Revit.Elements.Element> viewList = new List<Revit.Elements.Element>();

            foreach (string name in Names)
            {
                FilteredElementCollector viewCollector = new FilteredElementCollector(aDocument);
                viewCollector.OfClass(typeof(View));

                foreach (Autodesk.Revit.DB.Element viewElement in viewCollector)
                {
                    View view = (View)viewElement;
                    if (view.Name == name)
                        viewList.Add(view.ToDSType(true));
                }

            }
            
            return viewList;
        }

        public static List<string> SetZOffset(List<Revit.Elements.Element> Elements, List<SectionProperty> SectionProperties)
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            List<string> Offsets = new List<string>();
            foreach (Revit.Elements.Element Element in Elements)
            {
                Autodesk.Revit.DB.FamilyInstance aFamilyInstance = Element.InternalElement as Autodesk.Revit.DB.FamilyInstance;

                foreach (SectionProperty prop in SectionProperties)
                {
                    string name = prop.Name.ToString();
                    string ElName = Element.Name.ToString();

                    if (prop.Name.ToString() == Element.Name.ToString())
                    {
                        aFamilyInstance.get_Parameter("z Offset Value").Set(prop.ZOffsetS * toFeet);

                        Offsets.Add(prop.ZOffsetS.ToString());
                    }
                }
            }
            TransactionManager.Instance.TransactionTaskDone();
            return Offsets;
        }

        public static void SetAnalyticalLineID(Autodesk.Revit.DB.Element Element)
        {
            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            string ID = Element.get_Parameter("SUniqueID").ToString();
            Element.GetAnalyticalModel().get_Parameter("SUniqueID").Set(ID);
            TransactionManager.Instance.TransactionTaskDone();
        }



    }


}


