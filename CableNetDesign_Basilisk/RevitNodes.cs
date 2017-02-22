////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;

////namespace CableNetDesign_Basilisk
////{
////    class RevitNodes
////    {
////    }
////}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autodesk;
//using Revit.Elements;
//using RevitServices.Transactions;

//using Autodesk.Revit.DB.Architecture;
//using Autodesk.Revit.UI;
////using Autodesk.Revit.UI.Selection;
//using Autodesk.Revit.ApplicationServices;
//using Autodesk.Revit.Attributes;

////using ExcelUtilities;
////using BHExcelFormat;
////using BHExcelFormat.Tabs.Geometry;
////using BHExcelFormat.Tabs.Loading;
////using BHExcelFormat.Tabs.Results;
////using IO;
////using StructuralComponents;
////using StructuralComponents.Results;
////using StructuralComponents.SectionProperties;

//using RevitServices.Persistence;
//using Revit.GeometryConversion;

//using Autodesk.DesignScript.Geometry;
//using Autodesk.DesignScript.Interfaces;
//using Autodesk.DesignScript.Runtime;

//using Autodesk.Revit.DB;


//namespace Basilisk.Nodes
//{

//    public class RevitNodes
//    {
//        public static double toMills = 1000;
//        public static double toFeet = 3.2808399;



//        public static void Duplicate(List<BHoM.Structural.Properties.SteelSection> secProp)
//        {
//            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
//            string symName = null;
//            string newSymName = null;
//            List<string> symNames = new List<string>();
//            FilteredElementCollector aFilteredElementCollector = new FilteredElementCollector(aDocument).OfClass(typeof(Autodesk.Revit.DB.FamilySymbol));
//            List<Autodesk.Revit.DB.FamilySymbol> aFamilySymbolList = aFilteredElementCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);
//            foreach (BHoM.Structural.Properties.SteelSection sp in secProp)
//            {

//                if (sp.Type == "UC" || sp.Type == "UB")
//                {
//                    symName = "UBfamily";

//                    if (sp.Type == "UB")
//                    {
//                        symName = "UBfamily";
//                    }

//                    newSymName = sp.Type + " " + (sp.D * 1000).ToString() + "x" + (sp.B * 1000).ToString() + "x" + (sp.tw * 1000).ToString();
//                    Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
//                    if (aFamilySymbol != null && !symNames.Contains(newSymName))
//                    {
//                        TransactionManager.Instance.EnsureInTransaction(aDocument);
//                        ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
//                        Autodesk.Revit.DB.Parameter parBf = aElementType.get_Parameter("B");
//                        parBf.Set(sp.B * 1000 * 0.0032808399);
//                        Autodesk.Revit.DB.Parameter parD = aElementType.get_Parameter("D");
//                        parD.Set(sp.D * 1000 * 0.0032808399);
//                        Autodesk.Revit.DB.Parameter partw1 = aElementType.get_Parameter("tw");
//                        partw1.Set(sp.tw * 1000 * 0.0032808399);
//                        Autodesk.Revit.DB.Parameter partf = aFamilySymbol.get_Parameter("tf");
//                        partf.Set(sp.tf * 1000 * 0.0032808399);
//                        TransactionManager.Instance.TransactionTaskDone();
//                        //if (aElementType != null)
//                        // return aElementType.ToDSType(true) as Revit.Elements.FamilySymbol;

//                    }
//                    symNames.Add(newSymName);
//                }

//                if (sp.Type == "CHS" || sp.Type == "EXP")
//                {
//                    symName = "CHSfamily";
//                    double t = sp.tw * 1000;
//                    double D = sp.D * 1000;
//                    if (sp.Type == "EXP")
//                    {
//                        t = 5;
//                    }
//                    if (D == 0)
//                    {
//                        D = 20;
//                    }
//                    newSymName = sp.Type + " " + D.ToString() + "x" + t.ToString();
//                    Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
//                    if (aFamilySymbol != null && !symNames.Contains(newSymName))
//                    {
//                        TransactionManager.Instance.EnsureInTransaction(aDocument);
//                        ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
//                        Autodesk.Revit.DB.Parameter parOD = aElementType.get_Parameter("d");
//                        parOD.Set(D * 0.0032808399);
//                        Autodesk.Revit.DB.Parameter part = aElementType.get_Parameter("t");
//                        part.Set(t * 0.0032808399);
//                        TransactionManager.Instance.TransactionTaskDone();
//                        //if (aElementType != null)
//                        // return aElementType.ToDSType(true) as Revit.Elements.FamilySymbol;

//                    }
//                    symNames.Add(newSymName);
//                }


//                if (sp.Type == "RHS")
//                {
//                    if (!sp.IsTapered)
//                    {
//                        symName = "RHS";
//                        newSymName = sp.Type + " " + (sp.B * 1000).ToString() + "x" + (sp.D * 1000).ToString() + "x" + (sp.tw * 1000).ToString();
//                        Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
//                        if (aFamilySymbol != null && !symNames.Contains(newSymName))
//                        {

//                            TransactionManager.Instance.EnsureInTransaction(aDocument);
//                            ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
//                            Autodesk.Revit.DB.Parameter parHt = aElementType.get_Parameter("D");
//                            parHt.Set(sp.D * 1000 * 0.0032808399);
//                            Autodesk.Revit.DB.Parameter parB = aElementType.get_Parameter("B");
//                            parB.Set(sp.D * 1000 * 0.0032808399);
//                            Autodesk.Revit.DB.Parameter partw2 = aElementType.get_Parameter("t");
//                            partw2.Set(sp.tw * 1000 * 0.0032808399);
//                            TransactionManager.Instance.TransactionTaskDone();
//                            // (aElementType != null)
//                            // return aElementType.ToDSType(true) as Revit.Elements.FamilySymbol;

//                        }
//                    }


//                    if (sp.IsTapered)
//                    {
//                        if (!sp.TaperIntermediatePos[1].IsValidNumber() || sp.TaperIntermediatePos[1] == 0)
//                        {
//                            symName = "RHS Tapered 1";
//                            newSymName = "Tapered1 " + sp.Type + " " + (sp.TaperDepth[0] * 1000).ToString() + "x" + (sp.TaperDepth[1] * 1000).ToString() + "x" + (sp.TaperDepth[3] * 1000).ToString() + (sp.B * 1000).ToString() + "x" + (sp.TaperIntermediatePos[0]).ToString() + "x" + (sp.CutbackS * 1000).ToString() + "x" + (sp.CutbackE * 1000).ToString();
//                            Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
//                            if (aFamilySymbol != null && !symNames.Contains(newSymName))
//                            {

//                                TransactionManager.Instance.EnsureInTransaction(aDocument);
//                                ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
//                                Autodesk.Revit.DB.Parameter parSd = aElementType.get_Parameter("Start depth");
//                                parSd.Set(sp.TaperDepth[0] * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parTd = aElementType.get_Parameter("TIM1 depth");
//                                parTd.Set(sp.TaperDepth[1] * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parEd = aElementType.get_Parameter("End depth");
//                                parEd.Set(sp.TaperDepth[3] * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parTl = aElementType.get_Parameter("TIM1 pos");
//                                parTl.Set(sp.TaperIntermediatePos[0]);//sp.TaperIntermediatePos[0]
//                                Autodesk.Revit.DB.Parameter parTb = aElementType.get_Parameter("Breadth");
//                                parTb.Set(sp.B * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parThick = aElementType.get_Parameter("Thickness");
//                                parThick.Set(10 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parSc = aElementType.get_Parameter("Start cutback");
//                                parSc.Set(sp.CutbackS * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parEc = aElementType.get_Parameter("End cutback");
//                                parEc.Set(sp.CutbackE * 1000 * 0.0032808399);
//                                TransactionManager.Instance.TransactionTaskDone();
//                            }
//                        }

//                        else
//                        {
//                            symName = "RHS Tapered 2";
//                            newSymName = "Tapered2 " + sp.Type + " " + (sp.TaperDepth[0] * 1000).ToString() + "x" + (sp.TaperDepth[1] * 1000).ToString() + "x" + (sp.TaperDepth[3] * 1000).ToString() + (sp.B * 1000).ToString() + "x" + (sp.TaperIntermediatePos[0]).ToString() + "x" + (sp.TaperIntermediatePos[1]).ToString() + "x" + (sp.CutbackS * 1000).ToString() + "x" + (sp.CutbackE * 1000).ToString();
//                            Autodesk.Revit.DB.FamilySymbol aFamilySymbol = aFamilySymbolList.Find(x => x.Name == symName);
//                            if (aFamilySymbol != null && !symNames.Contains(newSymName))
//                            {

//                                TransactionManager.Instance.EnsureInTransaction(aDocument);
//                                ElementType aElementType = aFamilySymbol.Duplicate(newSymName);
//                                Autodesk.Revit.DB.Parameter parSd = aElementType.get_Parameter("Start depth");
//                                parSd.Set(sp.TaperDepth[0] * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parTd = aElementType.get_Parameter("TIM1 depth");
//                                parTd.Set(sp.TaperDepth[1] * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parEd = aElementType.get_Parameter("End depth");
//                                parEd.Set(sp.TaperDepth[3] * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parTl1 = aElementType.get_Parameter("TIM1 pos");
//                                parTl1.Set(sp.TaperIntermediatePos[0]);
//                                Autodesk.Revit.DB.Parameter parTl2 = aElementType.get_Parameter("TIM2 pos");
//                                parTl2.Set(sp.TaperIntermediatePos[1]);
//                                Autodesk.Revit.DB.Parameter parTb = aElementType.get_Parameter("Breadth");
//                                parTb.Set(sp.B * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parThick = aElementType.get_Parameter("Thickness");
//                                parThick.Set(10 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parSc = aElementType.get_Parameter("Start cutback");
//                                parSc.Set(sp.CutbackS * 1000 * 0.0032808399);
//                                Autodesk.Revit.DB.Parameter parEc = aElementType.get_Parameter("End cutback");
//                                parEc.Set(sp.CutbackE * 1000 * 0.0032808399);
//                                TransactionManager.Instance.TransactionTaskDone();
//                            }

//                        }

//                    }
//                    symNames.Add(newSymName);
//                }
//            }
//            //return null;
//        }

//        public static List<string> CreateSectionTypes(List<SectionProperty> sectionProperties)
//        {
//            Document activeDoc = DocumentManager.Instance.CurrentDBDocument;
//            List<string> typeNames = GetActiveDocFamilyTypeNames();

//            foreach (SectionProperty sectionProperty in sectionProperties)
//            {
//                switch (sectionProperty.Type)
//                {
//                    case ("UC"):
//                        CheckAndCreateUBFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
//                        break;

//                    case ("UB"):
//                        CheckAndCreateUBFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
//                        break;

//                    case ("CHS"):
//                        CheckAndCreateCHSFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
//                        break;

//                    case ("EXP"):
//                        CheckAndCreateCHSFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
//                        break;

//                    case ("RHS"):
//                        if (!sectionProperty.IsTapered)
//                            CheckAndCreateRHSFamilyType(sectionProperty, activeDoc, typeNames, out typeNames);

//                        else if (sectionProperty.TaperIntermediatePos[1] > 0)
//                            CheckAndCreateRHSTapered2FamilyType(sectionProperty, activeDoc, typeNames, out typeNames);

//                        else
//                            CheckAndCreateRHSTapered1FamilyType(sectionProperty, activeDoc, typeNames, out typeNames);
//                        break;
//                }
//            }

//            return typeNames;
//        }



//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------


//        //private static void CheckAndCreateUBFamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
//        private static void CheckAndCreateUBFamilyType(BHoM.Structural.Properties.SteelSection sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
//        {
//            string typeParentName = "UBfamily";
//            //string newTypeName = sectionProperty.Type + " " + (sectionProperty.D * toMills).ToString() + "x" + (sectionProperty.B * toMills).ToString() + "x" + (sectionProperty.tw * toMills).ToString();
//            string newTypeName = sectionProperty.Name;
//            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
//            typeNamesNew = typeNames;

//            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
//                return;

//            else
//            {
//                CreateUBFamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
//                typeNamesNew.Add(newTypeName);
//            }
//        }

//        private static void CreateUBFamilyType(BHoM.Structural.Properties.SteelSection sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
//        {
//            TransactionManager.Instance.EnsureInTransaction(activeDoc);



//            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);


//            List<Autodesk.Revit.DB.Parameter> paramlist = (List<Autodesk.Revit.DB.Parameter>)newFamilyType.GetParameters("B");

//            //newFamilyType.get_Parameter("B").Set(sectionProperty.TotalWidth * toFeet);
//            //newFamilyType.get_Parameter("D").Set(sectionProperty.TotalDepth * toFeet);
//            //newFamilyType.get_Parameter("tw").Set(sectionProperty.Tw * toFeet);
//            //newFamilyType.get_Parameter("tf").Set(sectionProperty.Tf1 * toFeet);



//            TransactionManager.Instance.TransactionTaskDone();
//        }


//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------

//        private static void CheckAndCreateCHSFamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
//        {
//            string typeParentName = "CHSfamily";
//            //string newTypeName = sectionProperty.Type + " " + (sectionProperty.D * toMills).ToString() + "x" + (sectionProperty.tw * toMills).ToString();
//            string newTypeName = sectionProperty.Name;
//            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
//            typeNamesNew = typeNames;

//            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
//                return;

//            else
//            {
//                CreateCHSFamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
//                typeNamesNew.Add(newTypeName);
//            }
//        }


//        private static void CreateCHSFamilyType(SectionProperty sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
//        {
//            TransactionManager.Instance.EnsureInTransaction(activeDoc);

//            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
//            newFamilyType.get_Parameter("d").Set(sectionProperty.D * toFeet);
//            newFamilyType.get_Parameter("t").Set(sectionProperty.tw * toFeet);

//            if (sectionProperty.Type == "EXP")
//                newFamilyType.get_Parameter("t").Set(0.005 * toFeet);

//            if (sectionProperty.D == 0)
//                newFamilyType.get_Parameter("d").Set(0.02 * toFeet);

//            TransactionManager.Instance.TransactionTaskDone();
//        }

//        private static void CheckAndCreateRHSFamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
//        {
//            string typeParentName = "RHS";
//            //string newTypeName = sectionProperty.Type + " " + (sectionProperty.D * toMills).ToString() + "x" + (sectionProperty.tw * toMills).ToString();
//            string newTypeName = sectionProperty.Name;
//            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
//            typeNamesNew = typeNames;

//            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
//                return;

//            else
//            {
//                CreateRHSFamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
//                typeNamesNew.Add(newTypeName);
//            }
//        }


//        private static void CreateRHSFamilyType(SectionProperty sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
//        {
//            TransactionManager.Instance.EnsureInTransaction(activeDoc);

//            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
//            newFamilyType.get_Parameter("D").Set(sectionProperty.D * toFeet);
//            newFamilyType.get_Parameter("B").Set(sectionProperty.B * toFeet);
//            newFamilyType.get_Parameter("t").Set(sectionProperty.tw * toFeet);

//            TransactionManager.Instance.TransactionTaskDone();
//        }

//        private static void CheckAndCreateRHSTapered2FamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
//        {
//            string typeParentName = "RHS Tapered 2";
//            //string newTypeName = "Tapered2 " + sectionProperty.Type + " " + (sectionProperty.TaperDepth[0] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[1] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[2] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[3] * toMills).ToString() + (sectionProperty.B * toMills).ToString() + "x" + (sectionProperty.TaperIntermediatePos[0]).ToString() + "x" + (sectionProperty.TaperIntermediatePos[1]).ToString() + "x" + (sectionProperty.CutbackS * toMills).ToString() + "x" + (sectionProperty.CutbackE * toMills).ToString();
//            string newTypeName = sectionProperty.Name;
//            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
//            typeNamesNew = typeNames;

//            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
//                return;

//            else
//            {
//                CreateRHSTapered2FamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
//                typeNamesNew.Add(newTypeName);
//            }
//        }

//        private static void CreateRHSTapered2FamilyType(SectionProperty sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
//        {
//            TransactionManager.Instance.EnsureInTransaction(activeDoc);

//            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
//            newFamilyType.get_Parameter("Start depth").Set(sectionProperty.TaperDepth[0] * toFeet);
//            newFamilyType.get_Parameter("TIM1 depth").Set(sectionProperty.TaperDepth[1] * toFeet);
//            newFamilyType.get_Parameter("TIM2 depth").Set(sectionProperty.TaperDepth[2] * toFeet);
//            newFamilyType.get_Parameter("End depth").Set(sectionProperty.TaperDepth[3] * toFeet);
//            newFamilyType.get_Parameter("TIM1 pos").Set(sectionProperty.TaperIntermediatePos[0] * toFeet);
//            newFamilyType.get_Parameter("TIM2 pos").Set(sectionProperty.TaperIntermediatePos[1] * toFeet);
//            newFamilyType.get_Parameter("Breadth").Set(sectionProperty.B * toFeet);
//            newFamilyType.get_Parameter("Thickness").Set(10 * toMills * toFeet);
//            newFamilyType.get_Parameter("Start cutback").Set(sectionProperty.CutbackS * toFeet);
//            newFamilyType.get_Parameter("End cutback").Set(sectionProperty.CutbackE * toFeet);

//            TransactionManager.Instance.TransactionTaskDone();
//        }

//        private static void CheckAndCreateRHSTapered1FamilyType(SectionProperty sectionProperty, Document activeDoc, List<string> typeNames, out List<string> typeNamesNew)
//        {
//            string typeParentName = "RHS Tapered 1";
//            //string newTypeName = "Tapered1 " + sectionProperty.Type + " " + (sectionProperty.TaperDepth[0] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[1] * toMills).ToString() + "x" + (sectionProperty.TaperDepth[3] * toMills).ToString() + (sectionProperty.B * toMills).ToString() + "x" + (sectionProperty.TaperIntermediatePos[0]).ToString() + "x" + (sectionProperty.TaperIntermediatePos[1]).ToString() + "x" + (sectionProperty.CutbackS * toMills).ToString() + "x" + (sectionProperty.CutbackE * toMills).ToString();
//            string newTypeName = sectionProperty.Name;
//            Autodesk.Revit.DB.FamilySymbol typeParentFamilySymbol = GetTypeParentFamilySymbol(typeParentName);
//            typeNamesNew = typeNames;

//            if (typeParentFamilySymbol == null || typeNames.Contains(newTypeName))
//                return;

//            else
//            {
//                CreateRHSTapered1FamilyType(sectionProperty, activeDoc, typeParentFamilySymbol, newTypeName);
//                typeNamesNew.Add(newTypeName);
//            }
//        }


//        private static void CreateRHSTapered1FamilyType(SectionProperty sectionProperty, Document activeDoc, Autodesk.Revit.DB.FamilySymbol familyParentType, string newTypeName)
//        {
//            TransactionManager.Instance.EnsureInTransaction(activeDoc);

//            ElementType newFamilyType = familyParentType.Duplicate(newTypeName);
//            newFamilyType.get_Parameter("Start depth").Set(sectionProperty.TaperDepth[0] * toFeet);
//            newFamilyType.get_Parameter("TIM1 depth").Set(sectionProperty.TaperDepth[1] * toFeet);
//            newFamilyType.get_Parameter("End depth").Set(sectionProperty.TaperDepth[3] * toFeet);
//            newFamilyType.get_Parameter("TIM1 pos").Set(sectionProperty.TaperIntermediatePos[0] * toFeet);
//            newFamilyType.get_Parameter("Breadth").Set(sectionProperty.B * toFeet);
//            newFamilyType.get_Parameter("Thickness").Set(10 * toMills * toFeet);
//            newFamilyType.get_Parameter("Start cutback").Set(sectionProperty.CutbackS * toFeet);
//            newFamilyType.get_Parameter("End cutback").Set(sectionProperty.CutbackE * toFeet);

//            TransactionManager.Instance.TransactionTaskDone();
//        }

//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------


//        private static Autodesk.Revit.DB.FamilySymbol GetTypeParentFamilySymbol(string typeParentName)
//        {
//            Document activeDoc = DocumentManager.Instance.CurrentDBDocument;
//            FilteredElementCollector familySymbolCollector = new FilteredElementCollector(activeDoc).OfClass(typeof(Autodesk.Revit.DB.FamilySymbol));
//            List<Autodesk.Revit.DB.FamilySymbol> documentFamilyTypes = familySymbolCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);
//            Autodesk.Revit.DB.FamilySymbol familyParentType = documentFamilyTypes.Find(x => x.Name == typeParentName);

//            return familyParentType;
//        }

//        private static List<string> GetActiveDocFamilyTypeNames()
//        {
//            Document activeDoc = DocumentManager.Instance.CurrentDBDocument;
//            FilteredElementCollector familySymbolCollector = new FilteredElementCollector(activeDoc).OfClass(typeof(Autodesk.Revit.DB.FamilySymbol));
//            List<Autodesk.Revit.DB.FamilySymbol> documentFamilyTypes = familySymbolCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);

//            List<string> ActiveDocFamilyTypeNames = new List<string>();

//            foreach (Autodesk.Revit.DB.FamilySymbol type in documentFamilyTypes)
//            {
//                ActiveDocFamilyTypeNames.Add(type.Name);
//            }
//            return ActiveDocFamilyTypeNames;
//        }

//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------
//        //---------------------------------------------------------------------------------------------------------------------

//        public static void SetElementRhinoGUID(List<Autodesk.Revit.DB.Element> Elm, List<string> guid)
//        {
//            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
//            for (int i = 0; i < Elm.Count; i++)
//            {
//                TransactionManager.Instance.EnsureInTransaction(aDocument);
//                Autodesk.Revit.DB.Parameter tempPar = Elm[i].get_Parameter("Rhino_GUID");
//                tempPar.Set(guid[i]);
//                TransactionManager.Instance.TransactionTaskDone();
//            }
//        }



//        public static void ChangeBeamLine(List<Revit.Elements.Element> famInst, List<Autodesk.DesignScript.Geometry.Line> lines)
//        {
//            List<Autodesk.Revit.DB.Element> aElementList = new List<Autodesk.Revit.DB.Element>();

//            for (int i = 0; i < famInst.Count; i++)
//            {
//                Autodesk.Revit.DB.Element aTempElement = famInst[i].InternalElement;
//                aElementList.Add(aTempElement);
//            }

//            //List<Autodesk.Revit.DB.FamilySymbol> aFamilySymbolList = aElementList.ToList().ConvertAll(x => x as Autodesk.Revit.DB.FamilySymbol);
//            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
//            for (int i = 0; i < aElementList.Count; i++)
//            {
//                if (aElementList[i].Location is Autodesk.Revit.DB.LocationCurve)
//                {
//                    Autodesk.DesignScript.Geometry.Point sPts = lines[i].StartPoint;
//                    Autodesk.DesignScript.Geometry.Point ePts = lines[i].EndPoint;
//                    TransactionManager.Instance.EnsureInTransaction(aDocument);
//                    Autodesk.Revit.DB.LocationCurve aLocationCurve = aElementList[i].Location as Autodesk.Revit.DB.LocationCurve;
//                    Autodesk.Revit.DB.XYZ aXYZ_1 = new Autodesk.Revit.DB.XYZ(sPts.X * 0.0032808399, sPts.Y * 0.0032808399, sPts.Z * 0.0032808399);
//                    Autodesk.Revit.DB.XYZ aXYZ_2 = new Autodesk.Revit.DB.XYZ(ePts.X * 0.0032808399, ePts.Y * 0.0032808399, ePts.Z * 0.0032808399);
//                    Autodesk.Revit.DB.Line aLine = Autodesk.Revit.DB.Line.CreateBound(aXYZ_1, aXYZ_2);
//                    aLocationCurve.Curve = aLine;
//                    TransactionManager.Instance.TransactionTaskDone();
//                }
//            }
//        }



//        public static Autodesk.Revit.DB.Element MoveElementByVector(Revit.Elements.Element Elm, Vector Vec)
//        {
//            Autodesk.Revit.DB.Element movedElm;
//            Document aDocument = DocumentManager.Instance.CurrentDBDocument;

//            Autodesk.Revit.DB.Element newEl = Elm.InternalElement;

//            Autodesk.Revit.DB.XYZ newLoc = Vec.ToRevitType(true);

//            TransactionManager.Instance.EnsureInTransaction(aDocument);

//            ElementTransformUtils.MoveElement(aDocument, newEl.Id, newLoc);

//            TransactionManager.Instance.TransactionTaskDone();
//            newEl.ToDSType(true);

//            movedElm = newEl;


//            return movedElm;
//        }



//        public static List<Autodesk.DesignScript.Geometry.Curve> FindLineByGrid(List<Autodesk.DesignScript.Geometry.Curve> Curves, List<Revit.Elements.Grid> Grids)
//        {
//            List<Autodesk.DesignScript.Geometry.Curve> CloseCurves = new List<Autodesk.DesignScript.Geometry.Curve>();


//            foreach (Revit.Elements.Grid g in Grids)

//            {
//                Autodesk.DesignScript.Geometry.Curve CloseCurve = null;

//                double distMin = g.Curve.ClosestPointTo(Curves[0].StartPoint).DistanceTo(Curves[0].StartPoint);

//                foreach (Autodesk.DesignScript.Geometry.Curve crv in Curves)
//                {
//                    double dist = g.Curve.ClosestPointTo(crv.StartPoint).DistanceTo(crv.StartPoint);

//                    if (dist <= distMin)
//                    {
//                        CloseCurve = crv;
//                        distMin = dist;
//                    }
//                }

//                CloseCurves.Add(CloseCurve);
//            }

//            return CloseCurves;
//        }


//        [MultiReturn(new[] { "Worksets", "Names", "IDs" })]
//        public static Dictionary<string, object> GetWorksets()
//        {
//            Document aDocument = DocumentManager.Instance.CurrentDBDocument;

//            FilteredWorksetCollector collector = new FilteredWorksetCollector(aDocument);
//            FilteredWorksetCollector user_worksets = collector.OfKind(WorksetKind.UserWorkset);

//            List<Workset> worksets = collector.ToWorksets().ToList();

//            List<WorksetId> worksetIds = collector.ToWorksetIds().ToList();

//            List<string> worksetNames = new List<string>();

//            foreach (Workset workset in worksets)
//                worksetNames.Add(workset.Name.ToString());


//            return new Dictionary<string, object>
//            {
//                {"Worksets", worksets},
//                {"Names", worksetNames},
//                {"IDs", worksetIds},
//            };

//        }

//        public static Autodesk.Revit.DB.Element CreateFloorOpening(Revit.Elements.Floor Floor, List<Autodesk.DesignScript.Geometry.Curve> Curves)
//        {
//            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
//            TransactionManager.Instance.EnsureInTransaction(aDocument);

//            CurveArray CurveArray = new CurveArray();

//            foreach (Autodesk.DesignScript.Geometry.Curve crv in Curves)
//                CurveArray.Append(crv.ToRevitType());

//            Autodesk.Revit.DB.Element newFloor = Floor.InternalElement;

//            Opening opening = aDocument.Create.NewOpening(newFloor, CurveArray, true);

//            newFloor.ToDSType(false);

//            TransactionManager.Instance.TransactionTaskDone();

//            return newFloor;

//        }

//        public static List<Revit.Elements.Element> GetElementsByID(List<Revit.Elements.Element> Elements, List<string> ElementIDs, List<string> IDs)
//        {
//            List<Revit.Elements.Element> ElementsByID = new List<Revit.Elements.Element>();

//            foreach (string ID in IDs)
//            {
//                for (int i = 0; i < ElementIDs.Count; i++)
//                    if (ElementIDs[i] == ID)
//                    {
//                        ElementsByID.Add(Elements[i]);
//                        break;
//                    }
//            }

//            return ElementsByID;
//        }

//        public static List<Autodesk.Revit.DB.Curve> NurbsToRevitCurves(List<Autodesk.DesignScript.Geometry.NurbsCurve> nurbsCrvs)
//        {
//            List<Autodesk.Revit.DB.Curve> revitCrvs = new List<Autodesk.Revit.DB.Curve>();
//            Autodesk.Revit.DB.Curve revitCrv;
//            foreach (NurbsCurve nurb in nurbsCrvs)
//            {
//                revitCrv = nurb.ToRevitType();
//                revitCrvs.Add(revitCrv);
//            }

//            return revitCrvs;
//        }


//        public static Dimension Dimension(Autodesk.DesignScript.Geometry.Line Line)
//        {
//            //Revit.Elements.Views.SectionView
//            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
//            TransactionManager.Instance.EnsureInTransaction(aDocument);

//            Autodesk.Revit.DB.Line ln = Line.ToRevitType() as Autodesk.Revit.DB.Line;

//            Reference ref1 = ln.GetEndPointReference(0);

//            ReferenceArray refArray = new ReferenceArray();
//            refArray.Append(ln.GetEndPointReference(0));
//            refArray.Append(ln.GetEndPointReference(1));

//            Dimension dimension = aDocument.Create.NewDimension(aDocument.ActiveView, ln, refArray);
//            // aDocument.Create.NewDimension(View.InternalElement as View, ln.ToRevitType() as Autodesk.Revit.DB.Line, refArray);

//            TransactionManager.Instance.TransactionTaskDone();

//            return dimension;
//        }

//        public static List<View> GetViewsByNames(List<string> Names)
//        {
//            Document aDocument = DocumentManager.Instance.CurrentDBDocument;
//            TransactionManager.Instance.EnsureInTransaction(aDocument);

//            List<View> viewList = new List<View>();

//            foreach (string name in Names)
//            {
//                FilteredElementCollector viewCollector = new FilteredElementCollector(aDocument);
//                viewCollector.OfClass(typeof(View));

//                foreach (Autodesk.Revit.DB.Element viewElement in viewCollector)
//                {
//                    View view = (View)viewElement;
//                    if (view.Name == name)
//                        viewList.Add(view);
//                }

//            }
//            return viewList;
//        }


//    }

//}


