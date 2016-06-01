using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using Revit.GeometryConversion;
using Revit.Elements;
using RevitServices.Transactions;

namespace Revit
{
    /// <summary>
    /// A Revit Foot Print Roof (Roof)
    /// </summary>
    public static class FootPrintRoof
    {
        /// <summary>
        /// Creates roof based on Curves
        /// </summary>
        /// <param name="Curves">Curves</param>
        /// <param name="RoofType">Roof Type</param>
        /// <param name="Level">Level</param>
        /// <returns name="FootPrintRoof">FootPrintRoof</returns>
        /// <search>
        /// Create Foot Print Roof, Create roof, create roof, create foot print roof
        /// </search>
        public static Elements.Element ByCurves(List<Autodesk.DesignScript.Geometry.Curve> Curves, Revit.Elements.Element RoofType, Revit.Elements.Level Level)
        {
            CurveArray aCurveArray = new CurveArray();
            Curves.ForEach(x => aCurveArray.Append(x.ToRevitType(false)));
            Autodesk.Revit.DB.Document aDocument = RoofType.InternalElement.Document;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            ModelCurveArray aModelCurveArray = new ModelCurveArray();
            Autodesk.Revit.DB.FootPrintRoof aFootPrintRoof = aDocument.Create.NewFootPrintRoof(aCurveArray, Level.InternalElement as Autodesk.Revit.DB.Level, RoofType.InternalElement as RoofType, out aModelCurveArray);
            TransactionManager.Instance.TransactionTaskDone();
            return aFootPrintRoof.ToDSType(true);
        }
    }
}
