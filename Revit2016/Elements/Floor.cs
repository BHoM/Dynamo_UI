using System.Collections.Generic;
using Autodesk.Revit.DB;
using Revit.GeometryConversion;
using Revit.Elements;
using RevitServices.Transactions;
using Autodesk.DesignScript.Interfaces;


namespace Elements
{
    /// <summary>
    /// A Revit floor
    /// </summary>
    public static class Floor
    {
        /// <summary>
        /// Creates floor based on Curves
        /// </summary>
        /// <param name="Curves">Curves</param>
        /// <param name="FloorType">Floor Type</param>
        /// <param name="Level">Level</param>
        /// <param name="Structural">Is floor structural element</param>
        /// <returns name="Floor">Floor</returns>
        /// <search>
        /// Create floor, create floor
        /// </search>
        public static Revit.Elements.Floor ByCurves(List<Autodesk.DesignScript.Geometry.Curve> Curves, Revit.Elements.FloorType FloorType, Revit.Elements.Level Level, bool Structural = false)
        {
            CurveArray aCurveArray = new CurveArray();
            Curves.ForEach(x => aCurveArray.Append(x.ToRevitType(false)));
            Autodesk.Revit.DB.Document aDocument = FloorType.InternalElement.Document;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.Floor aFloor = aDocument.Create.NewFloor(aCurveArray, FloorType.InternalElement as Autodesk.Revit.DB.FloorType, Level.InternalElement as Autodesk.Revit.DB.Level, Structural);
            TransactionManager.Instance.TransactionTaskDone();
            return aFloor.ToDSType(true) as Revit.Elements.Floor;
        }
    }
}
