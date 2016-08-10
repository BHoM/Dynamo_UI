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
    /// A Sheet
    /// </summary>
    public static class Sheet
    {
        /// <summary>
        /// Gets all title blocks placed on sheet
        /// </summary>
        /// <param name="Sheet">Sheet</param>
        /// <param name="View">View</param>
        /// <param name="Point">Point</param>
        /// <returns name="AddView">Viewport</returns>
        /// <search>
        /// Sheet, Add view, sheet, add view
        /// </search>
        public static Revit.Elements.Element AddView(Revit.Elements.Views.Sheet Sheet, Revit.Elements.Element View, Autodesk.DesignScript.Geometry.Point Point)
        {
            if (Sheet != null && Sheet.InternalElement != null && Point != null)
                if (Sheet.InternalElement is Autodesk.Revit.DB.ViewSheet && View.InternalElement != null && View.InternalElement is Autodesk.Revit.DB.View)
                {
                    Autodesk.Revit.DB.ViewSheet aViewSheet = Sheet.InternalElement as Autodesk.Revit.DB.ViewSheet;
                    TransactionManager.Instance.EnsureInTransaction(aViewSheet.Document);
                    Viewport aViewport = Viewport.Create(aViewSheet.Document, aViewSheet.Id, View.InternalElement.Id, new XYZ(Point.X, Point.Y, Point.Z));
                    TransactionManager.Instance.TransactionTaskDone();
                    if (aViewport != null)
                        return aViewport.ToDSType(true);
                }
            return null;
        }

        /// <summary>
        /// Creates new sheet based on specified Name and Number. View will be placed automatically on specified point at sheet.
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="SheetNumber">Sheet Number</param>
        /// <param name="FamilyType">Family type for title block</param>
        /// <param name="View">View to be placed on sheet</param>
        /// <param name="Point">Point where view will be placed</param>
        /// <returns name="Sheet">Created sheet</returns>
        /// <search>
        /// Sheet, Create Sheet, sheet, create sheet
        /// </search>
        public static Revit.Elements.Views.Sheet ByNameAndSheetNumber(string Name, string SheetNumber, Revit.Elements.FamilyType FamilyType, Revit.Elements.Element View, Autodesk.DesignScript.Geometry.Point Point)
        {

            FamilySymbol aFamilySymbol = FamilyType.InternalElement as FamilySymbol;
            if (aFamilySymbol.Category.Id.IntegerValue == (int)(BuiltInCategory.OST_TitleBlocks) && aFamilySymbol.Document != null)
            {
                Autodesk.Revit.DB.Document aDocument = aFamilySymbol.Document;
                Revit.Elements.Views.Sheet aResult = null;
                TransactionManager.Instance.EnsureInTransaction(aDocument);
                ViewSheet aViewSheet = ViewSheet.Create(aDocument, aFamilySymbol.Id);
                if (aViewSheet != null)
                {
                    aViewSheet.Name = Name;
                    aViewSheet.SheetNumber = SheetNumber;
                    Autodesk.Revit.DB.Element aElement = View.InternalElement as Autodesk.Revit.DB.Element;
                    if (aElement != null && Point != null && aElement is Autodesk.Revit.DB.View)
                        Viewport.Create(aDocument, aViewSheet.Id, aElement.Id, new XYZ(Point.X, Point.Y, Point.Z));
                    aResult = aViewSheet.ToDSType(true) as Revit.Elements.Views.Sheet;
                }
                TransactionManager.Instance.TransactionTaskDone();
                return aResult;
            }
            return null;
        }

        /// <summary>
        /// Returns elements of specified category placed on sheet
        /// </summary>
        /// <param name="Sheet">View Sheet</param>
        /// <param name="Category">Category</param>
        /// <returns name="Elements">Elements</returns>
        /// <search>
        /// Sheet Elements, Sheet, sheet, get sheet elements by category 
        /// </search>
        public static List<Revit.Elements.Element> GetElements(Revit.Elements.Views.Sheet Sheet, Revit.Elements.Category Category)
        {
            ViewSheet aViewSheet = Sheet.InternalElement as ViewSheet;
            Autodesk.Revit.DB.Document aDocument = aViewSheet.Document;
            List<Autodesk.Revit.DB.Element> aElementList = new FilteredElementCollector(aDocument, aViewSheet.Id).OfCategory((BuiltInCategory)Category.Id).ToElements().ToList();
            return aElementList.ConvertAll(x => x.ToDSType(true));
        }

        /// <summary>
        /// Sets Additional Revision Ids for sheet
        /// </summary>
        /// <param name="Sheet">Sheet</param>
        /// <param name="RevisionIds">Revision Ids List</param>
        /// <returns name="RevisionIds">List of revision Ids</returns>
        /// <search>
        /// Sheet, Sets Additional Revision Ids, sheet, sets additional revision ids
        /// </search>
        public static bool SetAdditionalRevisionIds(Revit.Elements.Views.Sheet Sheet, List<int> RevisionIds)
        {
            if (Sheet != null && Sheet.InternalElement != null)
                if (Sheet.InternalElement is Autodesk.Revit.DB.ViewSheet && RevisionIds != null)
                {
                    Autodesk.Revit.DB.ViewSheet aViewSheet = Sheet.InternalElement as Autodesk.Revit.DB.ViewSheet;
                    List<int> aRevisonIds = new List<int>(RevisionIds);
                    aRevisonIds.RemoveAll(x => x == -1);
                    List<ElementId> aElementIdList = aRevisonIds.ConvertAll(x => new ElementId(x));
                    if (aViewSheet.Document != null)
                    {
                        TransactionManager.Instance.EnsureInTransaction(aViewSheet.Document);
                        aViewSheet.SetAdditionalRevisionIds(aElementIdList);
                        TransactionManager.Instance.TransactionTaskDone();
                        return true;
                    }
                }
            return false;
        }

        /// <summary>
        /// Gets All Revision Ids for sheet
        /// </summary>
        /// <param name="Sheet">Sheet</param>
        /// <returns name="RevisionIds">List of revision Ids</returns>
        /// <search>
        /// Sheet, Get All Revision Ids, sheet, sets additional revision ids
        /// </search>
        public static List<int> GetAllRevisionIds(Revit.Elements.Views.Sheet Sheet)
        {
            ViewSheet aViewSheet = Sheet.InternalElement as ViewSheet;
            return aViewSheet.GetAllRevisionIds().ToList().ConvertAll(x => x.IntegerValue);
        }
    }
}
