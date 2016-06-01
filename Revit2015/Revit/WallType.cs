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
    /// A WallType
    /// </summary>
    public static class WallType
    {
        /// <summary>
        /// Duplicates existing WallTypes. If WallType existis then returns existing type.
        /// </summary>
        /// <param name="WallType">Base WallType</param>
        /// <param name="Name">Name</param>
        /// <returns name="WallType">WallType</returns>
        /// <search>
        /// Duplicate WallType, WallType, walltype, duplicate walltype, wall, type, Wall Type
        /// </search>
        public static Revit.Elements.WallType Duplicate(Revit.Elements.WallType WallType, string Name)
        {
            Autodesk.Revit.DB.WallType aWallType = WallType.InternalElement as Autodesk.Revit.DB.WallType;
            Autodesk.Revit.DB.Document aDocument = aWallType.Document;
            List<Autodesk.Revit.DB.WallType> aWallTypeList = new FilteredElementCollector(aDocument).OfClass(typeof(Autodesk.Revit.DB.WallType)).ToElements().Cast<Autodesk.Revit.DB.WallType>().ToList();
            Autodesk.Revit.DB.WallType aNewWallType = aWallTypeList.Find(x => x.Name == Name);
            if (aNewWallType == null)
            {
                TransactionManager.Instance.EnsureInTransaction(aDocument);
                aNewWallType = aWallType.Duplicate(Name) as Autodesk.Revit.DB.WallType;
                TransactionManager.Instance.TransactionTaskDone();
                return aNewWallType.ToDSType(true) as Revit.Elements.WallType;
            }
            return aNewWallType.ToDSType(true) as Revit.Elements.WallType;
        }
    }
}
