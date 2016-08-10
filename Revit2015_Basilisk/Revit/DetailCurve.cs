using Revit.GeometryConversion;
using RevitServices.Transactions;

namespace Revit
{
    /// <summary>
    /// A Revit Detail Curve
    /// </summary>
    public static class DetailCurve
    {
        /// <summary>
        /// Creates Detail Curve on view by specified curve
        /// </summary>
        /// <param name="View">View</param>
        /// <param name="Curve">Curve</param>
        /// <returns name="DetailCurve">Detail Curve</returns>
        /// <search>
        /// Detail Curve, Create Detail Curve, By View And Curve, detail curve, create detail curve, by view and curve 
        /// </search>
        public static Autodesk.Revit.DB.DetailCurve ByViewAndCurve(Revit.Elements.Element View, Autodesk.DesignScript.Geometry.Curve Curve)
        {
            Autodesk.Revit.DB.View aView = View.InternalElement as Autodesk.Revit.DB.View;
            Autodesk.Revit.DB.Document aDocument = aView.Document;

            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.DetailCurve aDetailCurve = aDocument.Create.NewDetailCurve(aView, Curve.ToRevitType());
            TransactionManager.Instance.TransactionTaskDone();

            return aDetailCurve;
        }
    }
}
