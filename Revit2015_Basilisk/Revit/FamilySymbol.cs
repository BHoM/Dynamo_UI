using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Revit.Elements;
using RevitServices.Transactions;

namespace Revit
{
    /// <summary>
    /// A Family Type
    /// </summary>
    public static class FamilyType
    {
        /// <summary>
        /// Duplicate existing type. If symbol with specified name exists then existing symbol will be returned. 
        /// </summary>
        /// <param name="FamilyType">Family Type</param>
        /// <param name="Name">Name</param>
        /// <returns name="FamilyType">Family Type</returns>
        /// <search>
        /// Duplicate family type
        /// </search>
        public static Revit.Elements.FamilyType Duplicate(Revit.Elements.FamilyType FamilyType, string Name)
        {
            if (FamilyType != null && FamilyType.InternalElement != null)
            {
                Autodesk.Revit.DB.FamilySymbol aFamilySymbol = FamilyType.InternalElement as Autodesk.Revit.DB.FamilySymbol;
                if (aFamilySymbol.Family != null)
                {
                    Autodesk.Revit.DB.Document aDocument = aFamilySymbol.Document;
                    List<Autodesk.Revit.DB.ElementId> aElementIdList = aFamilySymbol.Family.GetFamilySymbolIds().Cast<Autodesk.Revit.DB.ElementId>().ToList();
                    List<Autodesk.Revit.DB.FamilySymbol> aFamilySymbolList = aElementIdList.ConvertAll(x => aDocument.GetElement(x) as Autodesk.Revit.DB.FamilySymbol);
                    Autodesk.Revit.DB.FamilySymbol aResult = aFamilySymbolList.Find(x => x.Name == Name);
                    if (aResult == null)
                    {
                        TransactionManager.Instance.EnsureInTransaction(aFamilySymbol.Document);
                        try
                        {
                            aResult = aFamilySymbol.Duplicate(Name) as Autodesk.Revit.DB.FamilySymbol;
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        TransactionManager.Instance.TransactionTaskDone();
                    }

                    return ElementWrapper.ToDSType(aResult, true) as Revit.Elements.FamilyType;
                }
            }
            return null;
        }
    }
}
