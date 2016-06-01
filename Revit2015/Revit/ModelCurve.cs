using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Revit.GeometryConversion;
using RevitServices.Transactions;
using RevitServices.Persistence;

namespace Revit
{
    /// <summary>
    /// A Revit model Curve
    /// </summary>
    public static class ModelCurve
    {
        /// <summary>
        /// Creates Model Curve on view by specified curve
        /// </summary>
        /// <param name="Curve">First Point</param>
        /// <returns name="ModelCurve">Model Curve</returns>
        /// <search>
        /// Model Curve, Create Model Curve, By Curve, model curve, create model curve, by curve 
        /// </search>
        public static Autodesk.Revit.DB.ModelCurve ByCurve(Autodesk.DesignScript.Geometry.Curve Curve)
        {

            Autodesk.Revit.DB.XYZ aNormal = Curve.Normal.ToRevitType();
            Autodesk.Revit.DB.XYZ aPoint = Curve.StartPoint.ToRevitType();
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;

            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.Plane aPlane = DocumentManager.Instance.CurrentUIApplication.Application.Create.NewPlane(aNormal, aPoint);
            Autodesk.Revit.DB.SketchPlane aSketchPlane = Autodesk.Revit.DB.SketchPlane.Create(aDocument, aPlane);

            Autodesk.Revit.DB.ModelCurve aDetailCurve = aDocument.Create.NewModelCurve(Curve.ToRevitType(), aSketchPlane);
            TransactionManager.Instance.TransactionTaskDone();

            if (aDetailCurve != null)
                return aDetailCurve;

            return null;
        }

        /// <summary>
        /// Creates Model Curve - Space Separation Line
        /// </summary>
        /// <param name="ModelCurve">Model Curve</param>
        /// <param name="View">View</param>
        /// <returns name="ModelCurve">Space Separation Line</returns>
        /// <search>
        /// Space Separation Line, space separartion line, model curve, Model Curve
        /// </search>
        public static Autodesk.Revit.DB.ModelCurve SpaceSeparationLine(Revit.Elements.Views.View View, Revit.Elements.ModelCurve ModelCurve)
        {
            Autodesk.Revit.DB.ModelCurve aModelCurve = ModelCurve.InternalElement as Autodesk.Revit.DB.ModelCurve;
            Autodesk.Revit.DB.CurveArray aCurveArray = new Autodesk.Revit.DB.CurveArray();
            aCurveArray.Append(aModelCurve.GeometryCurve.Clone());
            Autodesk.Revit.DB.View aView = View.InternalElement as Autodesk.Revit.DB.View;

            TransactionManager.Instance.EnsureInTransaction(aView.Document);
            Autodesk.Revit.DB.SketchPlane aSketchPlane = Autodesk.Revit.DB.SketchPlane.Create(aView.Document, aModelCurve.SketchPlane.GetPlane());
            Autodesk.Revit.DB.ModelCurveArray aModelCurveArray = aView.Document.Create.NewSpaceBoundaryLines(aSketchPlane, aCurveArray, aView);
            TransactionManager.Instance.TransactionTaskDone();
            if (aModelCurveArray.Size > 0)
                return aModelCurveArray.get_Item(0);
            return null;
        }

        /// <summary>
        /// Creates Model Curve - Room Separation Line
        /// </summary>
        /// <param name="ModelCurve">Model Curve</param>
        /// <param name="View">View</param>
        /// <returns name="ModelCurve">Room Separation Line</returns>
        /// <search>
        /// Room Separation Line, room separartion line, model curve, Model Curve
        /// </search>
        public static Autodesk.Revit.DB.ModelCurve RoomSeparationLine(Revit.Elements.Views.View View, Revit.Elements.ModelCurve ModelCurve)
        {
            Autodesk.Revit.DB.ModelCurve aModelCurve = ModelCurve.InternalElement as Autodesk.Revit.DB.ModelCurve;
            Autodesk.Revit.DB.CurveArray aCurveArray = new Autodesk.Revit.DB.CurveArray();
            aCurveArray.Append(aModelCurve.GeometryCurve.Clone());
            Autodesk.Revit.DB.View aView = View.InternalElement as Autodesk.Revit.DB.View;

            TransactionManager.Instance.EnsureInTransaction(aView.Document);
            Autodesk.Revit.DB.SketchPlane aSketchPlane = Autodesk.Revit.DB.SketchPlane.Create(aView.Document, aModelCurve.SketchPlane.GetPlane());
            Autodesk.Revit.DB.ModelCurveArray aModelCurveArray = aView.Document.Create.NewRoomBoundaryLines(aSketchPlane, aCurveArray, aView);
            TransactionManager.Instance.TransactionTaskDone();
            if (aModelCurveArray.Size > 0)
                return aModelCurveArray.get_Item(0);
            return null;
        }
    }
}
