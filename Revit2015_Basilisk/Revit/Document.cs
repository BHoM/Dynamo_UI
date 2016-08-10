using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;

using Revit.Elements;
using RevitServices.Persistence;
using RevitServices.Transactions;
using Autodesk.Revit.ApplicationServices;

namespace Revit
{
    /// <summary>
    /// A Revit current document
    /// </summary>
    public static class Document
    {
        /// <summary>
        /// Loads family symbol to the project.
        /// </summary>
        /// <param name="FilePath">File Path</param>
        /// <param name="Name">Family Symbol (Type) name</param>
        /// <returns name="FamilySymbol">Family Symbol</returns>
        /// <search>
        /// Document, Load Family Symbol, document, load family symbol
        /// </search>
        public static Revit.Elements.FamilyType LoadFamilySymbol(string FilePath, string Name)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;

            FamilySymbol aFamilySymbol = null;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Revit.Elements.FamilyType aFamilyType = null;
            if (aDocument.LoadFamilySymbol(FilePath, Name, out aFamilySymbol))
                aFamilyType = aFamilySymbol.ToDSType(true) as Revit.Elements.FamilyType;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            return aFamilyType;
        }

        /// <summary>
        /// Returns path name of current document.
        /// </summary>
        /// <returns name="PathName">Path Name</returns>
        /// <search>
        /// Document, Path Name, document, path name
        /// </search>
        public static string PathName()
        {
            return DocumentManager.Instance.CurrentDBDocument.PathName;
        }

        /// <summary>
        /// Gets project parameter definitions
        /// </summary>
        /// <returns name="Definitions">List of project parameters definitions</returns>
        /// <search>
        /// Document, Get definitions, document, get definitions
        /// </search>      
        public static List<Autodesk.Revit.DB.Definition> ProjectParameterDefinitions()
        {
            List<Autodesk.Revit.DB.Definition> aDefinitionList = new List<Autodesk.Revit.DB.Definition>();
            DefinitionBindingMapIterator aDefinitionBindingMapIterator = DocumentManager.Instance.CurrentDBDocument.ParameterBindings.ForwardIterator();
            while (aDefinitionBindingMapIterator.MoveNext())
                aDefinitionList.Add(aDefinitionBindingMapIterator.Key);
            return aDefinitionList;
        }

        /// <summary>
        /// Sets project parameter
        /// </summary>
        /// <param name="Definition">Definition</param>
        /// <param name="Categories">List of categories for parameter</param>
        /// <param name="GroupName">Group Name</param>
        /// <param name="IsInstance">Is Instance</param>
        /// <returns name="Set">Boolean result</returns>
        /// <search>
        /// Document, Set Project Paramater, document, set project parameter
        /// </search>
        public static bool SetProjectParamater(Autodesk.Revit.DB.Definition Definition, List<Revit.Elements.Category> Categories, string GroupName, bool IsInstance = true)
        {
            bool aResult = false;
            Categories.RemoveAll(x => x == null);
            if (Categories.Count > 0)
            {
                BuiltInParameterGroup aBuiltInParameterGroup = BuiltInParameterGroup.INVALID;
                BuiltInParameterGroup? aTempBuiltInParameterGroup = GetBuiltInParameterGroup(GroupName);
                if (aTempBuiltInParameterGroup != null)
                    aBuiltInParameterGroup = aTempBuiltInParameterGroup.Value;

                if (DocumentManager.Instance.CurrentUIDocument.Application != null && DocumentManager.Instance.CurrentUIDocument.Application.Application != null && DocumentManager.Instance.CurrentDBDocument != null)
                {
                    Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
                    Autodesk.Revit.ApplicationServices.Application aApplication = DocumentManager.Instance.CurrentUIDocument.Application.Application;

                    TransactionManager.Instance.EnsureInTransaction(aDocument);
                    CategorySet aCategories = aApplication.Create.NewCategorySet();
                    foreach (Revit.Elements.Category aCategory in Categories)
                        aCategories.Insert(aDocument.Settings.Categories.get_Item((BuiltInCategory)aCategory.Id));

                    Binding aBinding = null;
                    if (IsInstance)
                        aBinding = aApplication.Create.NewInstanceBinding(aCategories);
                    else
                        aBinding = aApplication.Create.NewTypeBinding(aCategories);

                    BindingMap aBindingMap = aDocument.ParameterBindings;
                    aResult = aBindingMap.Insert(Definition, aBinding, aBuiltInParameterGroup);
                    TransactionManager.Instance.TransactionTaskDone();
                }
            }
            return aResult;
        }

        /// <summary>
        /// Remove project parameter
        /// </summary>
        /// <param name="Definition">Definition</param>
        /// <returns name="Removed">Boolean result</returns>
        /// <search>
        /// Document, Remove Project Paramater, document, remove project parameter
        /// </search>
        public static bool RemoveProjectParameter(Autodesk.Revit.DB.Definition Definition)
        {
            bool aResult = false;
            BindingMap aBindingMap = DocumentManager.Instance.CurrentDBDocument.ParameterBindings;
            if (aBindingMap != null)
            {
                TransactionManager.Instance.EnsureInTransaction(DocumentManager.Instance.CurrentDBDocument);
                aResult = aBindingMap.Remove(Definition);
                TransactionManager.Instance.TransactionTaskDone();
                return aResult;
            }
            return aResult;
        }

        /// <summary>
        /// Remove project parameter
        /// </summary>
        /// <param name="Name">name of the parameter</param>
        /// <returns name="Removed">Boolean result</returns>
        /// <search>
        /// Document, Remove Project Paramater, document, remove project parameter
        /// </search>
        public static bool RemoveProjectParameter(string Name)
        {
            bool aResult = false;
            if (!string.IsNullOrEmpty(Name))
            {
                Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
                BindingMap aBindingMap = aDocument.ParameterBindings;
                if (aBindingMap != null)
                {
                    DefinitionBindingMapIterator aDefinitionBindingMapIterator = aBindingMap.ForwardIterator();
                    while (aDefinitionBindingMapIterator.MoveNext())
                    {
                        Autodesk.Revit.DB.Definition aDefinition = aDefinitionBindingMapIterator.Key;
                        if (aDefinition.Name == Name)
                        {
                            TransactionManager.Instance.EnsureInTransaction(aDocument);
                            aResult = aBindingMap.Remove(aDefinition);
                            TransactionManager.Instance.TransactionTaskDone();
                            return aResult;
                        }
                    }
                }
            }
            return aResult;
        }

        /// <summary>
        /// Gets all view templates
        /// </summary>
        /// <returns name="ViewTemplates">View Templates</returns>
        /// <search>
        /// Get view templates, get view templates
        /// </search>
        public static List<Autodesk.Revit.DB.View> ViewTemplates()
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;

            List<Autodesk.Revit.DB.Element> aElementList = new FilteredElementCollector(aDocument).OfClass(typeof(View)).ToList();
            List<Autodesk.Revit.DB.View> aViewList = aElementList.ConvertAll(x => x as Autodesk.Revit.DB.View);
            aViewList.RemoveAll(x => !x.IsTemplate);
            return aViewList;
        }

        /// <summary>
        /// Sets shared parameter to the family loaded to the project.
        /// </summary>
        /// <param name="Family">Family</param>
        /// <param name="Definition">Definition</param>
        /// <param name="GroupName">Group Name</param>
        /// <param name="IsInstance">Is Instance</param>
        /// <returns name="Set">Boolean result</returns>
        /// <search>
        /// Document, Set Family Paramater, document, set family parameter
        /// </search>
        private static bool SetFamilyParameter(Elements.Family Family, Autodesk.Revit.DB.Definition Definition, string GroupName, bool IsInstance = true)
        {
            bool aResult = false;
            if (Definition is ExternalDefinition)
            {
                BuiltInParameterGroup aBuiltInParameterGroup = BuiltInParameterGroup.INVALID;
                BuiltInParameterGroup? aTempBuiltInParameterGroup = GetBuiltInParameterGroup(GroupName);
                if(aTempBuiltInParameterGroup != null)
                    aBuiltInParameterGroup = aTempBuiltInParameterGroup.Value;
                
                Autodesk.Revit.DB.Document aDocument = Family.InternalElement.Document;
                if (aDocument != null)
                {
                    Autodesk.Revit.DB.Document aFamilyDocument = aDocument.EditFamily(Family.InternalElement as Autodesk.Revit.DB.Family);
                    if (aFamilyDocument != null)
                    {
                        TransactionManager.Instance.EnsureInTransaction(aFamilyDocument);
                        FamilyManager aFamilyManager = aFamilyDocument.FamilyManager;
                        try
                        {
                            aFamilyManager.AddParameter(Definition as ExternalDefinition, aBuiltInParameterGroup, IsInstance);
                        }
                        catch
                        {

                        }
                        TransactionManager.Instance.TransactionTaskDone();

                        TransactionManager.Instance.EnsureInTransaction(aDocument);
                        Autodesk.Revit.DB.Family aFamily = null;
                        try
                        {
                            aFamily = aFamilyDocument.LoadFamily(aDocument, new FamilyLoadOptions());
                            aDocument.Regenerate();
                        }
                        catch (Exception e)
                        {

                        }
                        if (aFamily != null)
                            aResult = true;
                        TransactionManager.Instance.ForceCloseTransaction();

                        aFamilyDocument.Close(false);
                    }
                }
            }
            return aResult;
        }

        /// <summary>
        /// Sets parameter to the family loaded to the project.
        /// </summary>
        /// <param name="Family">Family</param>
        /// <param name="ParameterName">Name of parameter</param>
        /// <param name="ParameterType">Type of parameter</param>
        /// <param name="GroupName">Group Name</param>
        /// <param name="IsInstance">Is Instance</param>
        /// <returns name="Set">Boolean result</returns>
        /// <search>
        /// Document, Set Family Paramater, document, set family parameter
        /// </search>
        private static bool SetFamilyParameter(Elements.Family Family, string ParameterName, string ParameterType, string GroupName, bool IsInstance = true)
        {
            bool aResult = false;
            BuiltInParameterGroup aBuiltInParameterGroup = BuiltInParameterGroup.INVALID;
            BuiltInParameterGroup? aTempBuiltInParameterGroup = GetBuiltInParameterGroup(GroupName);
            if (aTempBuiltInParameterGroup != null)
                aBuiltInParameterGroup = aTempBuiltInParameterGroup.Value;

            ParameterType aParameterType = Autodesk.Revit.DB.ParameterType.Invalid;
            ParameterType? aTempParameterType = GetParameterType(ParameterType);
            if (aTempParameterType != null)
            {
                aParameterType = aTempParameterType.Value;
                Autodesk.Revit.DB.Document aDocument = Family.InternalElement.Document;
                if (aDocument != null)
                {
                    Autodesk.Revit.DB.Document aFamilyDocument = aDocument.EditFamily(Family.InternalElement as Autodesk.Revit.DB.Family);
                    if (aFamilyDocument != null)
                    {
                        TransactionManager.Instance.EnsureInTransaction(aFamilyDocument);
                        FamilyManager aFamilyManager = aFamilyDocument.FamilyManager;
                        try
                        {
                            aFamilyManager.AddParameter(ParameterName, aBuiltInParameterGroup, aParameterType, IsInstance);
                        }
                        catch
                        {

                        }
                        TransactionManager.Instance.TransactionTaskDone();



                        TransactionManager.Instance.EnsureInTransaction(aDocument);
                        Autodesk.Revit.DB.Family aFamily = null;
                        try
                        {
                            aFamily = aFamilyDocument.LoadFamily(aDocument, new FamilyLoadOptions());
                        }
                        catch (Exception e)
                        {

                        }
                        if (aFamily != null)
                            aResult = true;
                        TransactionManager.Instance.ForceCloseTransaction();

                        aFamilyDocument.Close(false);
                    }
                }
            }
            return aResult;
        }
        
        internal static BuiltInParameterGroup? GetBuiltInParameterGroup(string Value)
        {
            Array aBuiltInParameterGroups = Enum.GetValues(typeof(BuiltInParameterGroup));
            foreach(BuiltInParameterGroup aBuiltInParameterGroup in aBuiltInParameterGroups)
                if(LabelUtils.GetLabelFor(aBuiltInParameterGroup) == Value)
                    return aBuiltInParameterGroup;
            return null;
        }

        internal static ParameterType? GetParameterType(string Value)
        {
            Array aParameterTypes = Enum.GetValues(typeof(ParameterType));
            foreach (ParameterType aParameterType in aParameterTypes)
                if (LabelUtils.GetLabelFor(aParameterType) == Value)
                    return aParameterType;

            return null;
        }
    }
}
