using System.Collections.Generic;
using Revit.Elements;
using RevitServices.Transactions;

using RevitServices.Persistence;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

using Autodesk.Revit.DB;

namespace Revit
{
    /// <summary>
    /// Revit Annotation
    /// </summary>
    public class Annotation
    {
        internal Annotation() { }

        /// <summary>
        /// Create aligned, horisontal or vertical dimensions between two points on a specified view. 
        /// </summary>
        /// <param name="StartPoint"></param>First point for dimension
        /// <param name="EndPoint"></param>Second point for dimension
        /// <param name="viewName"></param>Name of view to annotate
        /// <param name="type"></param>Type of dimension, i.e "aligned", "horisontal" or "vertical"
        /// <param name="offsetDistance"></param>Distance from actual line between points to place dimension at
        /// <returns></returns>
        [MultiReturn(new[] { "AnnotationLine", "DetailLineStart", "DetailLineEnd" })]
        public static Dictionary<string, object> AlignedAndLinearDimension(Autodesk.DesignScript.Geometry.Point StartPoint, Autodesk.DesignScript.Geometry.Point EndPoint, string viewName, string type, double offsetDistance)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);

            FilteredElementCollector viewCollector = new FilteredElementCollector(aDocument);
            viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));

            Autodesk.Revit.DB.View view = null;

            foreach (Autodesk.Revit.DB.Element viewElement in viewCollector)
            {
                Autodesk.Revit.DB.View viewTemp = (Autodesk.Revit.DB.View)viewElement;
                if (viewTemp.Name == viewName)
                    view = viewTemp;
            }

            if (view != null)
            {
                double startX = UnitUtils.ConvertToInternalUnits(StartPoint.X, DisplayUnitType.DUT_MILLIMETERS);
                double startY = UnitUtils.ConvertToInternalUnits(StartPoint.Y, DisplayUnitType.DUT_MILLIMETERS);
                double startZ = UnitUtils.ConvertToInternalUnits(StartPoint.Z, DisplayUnitType.DUT_MILLIMETERS);
                double endX = UnitUtils.ConvertToInternalUnits(EndPoint.X, DisplayUnitType.DUT_MILLIMETERS);
                double endY = UnitUtils.ConvertToInternalUnits(EndPoint.Y, DisplayUnitType.DUT_MILLIMETERS);
                double endZ = UnitUtils.ConvertToInternalUnits(EndPoint.Z, DisplayUnitType.DUT_MILLIMETERS);
                double offset = UnitUtils.ConvertToInternalUnits(offsetDistance, DisplayUnitType.DUT_MILLIMETERS);

                XYZ planeX = view.RightDirection;
                XYZ planeY = new XYZ(0, 0, 1);
                XYZ planeNormal = planeX.CrossProduct(planeY);

                Autodesk.Revit.DB.Line aLine = Autodesk.Revit.DB.Line.CreateBound(new XYZ(startX, startY, startZ), new XYZ(endX, endY, endZ));

                if (aLine != null)
                {
                    Autodesk.Revit.DB.Line aDimensionLine;
                    Autodesk.Revit.DB.DetailCurve aFirstDetailCurve;
                    Autodesk.Revit.DB.DetailCurve aSecondDetailCurve;

                    if (type == "aligned")
                    {
                        aDimensionLine = Autodesk.Revit.DB.Line.CreateBound((new XYZ(startX, startY, startZ) + aLine.Direction.CrossProduct(planeNormal) * offset), (new XYZ(endX, endY, endZ) + aLine.Direction.CrossProduct(planeNormal) * offset));
                        aFirstDetailCurve = aDocument.Create.NewDetailCurve(view, Autodesk.Revit.DB.Line.CreateBound(new XYZ(startX, startY, startZ), new XYZ(startX, startY, startZ) - aLine.Direction.CrossProduct(planeNormal) * offset));
                        aSecondDetailCurve = aDocument.Create.NewDetailCurve(view, Autodesk.Revit.DB.Line.CreateBound(new XYZ(endX, endY, endZ), new XYZ(endX, endY, endZ) - aLine.Direction.CrossProduct(planeNormal) * offset));
                    }
                    else if (type == "vertical")
                    {
                        aDimensionLine = Autodesk.Revit.DB.Line.CreateBound((new XYZ(startX, startY, startZ) + planeX * offset), (new XYZ(startX, startY, endZ) + planeX * offset));
                        aFirstDetailCurve = aDocument.Create.NewDetailCurve(view, Autodesk.Revit.DB.Line.CreateBound(new XYZ(startX, startY, startZ), new XYZ(startX, startY, startZ) - planeX * offset));
                        aSecondDetailCurve = aDocument.Create.NewDetailCurve(view, Autodesk.Revit.DB.Line.CreateBound(new XYZ(endX, endY, endZ), new XYZ(endX, endY, endZ) - planeX * offset));
                    }
                    else if (type == "horisontal")
                    {
                        aDimensionLine = Autodesk.Revit.DB.Line.CreateBound((new XYZ(startX, startY, startZ) + planeY * offset), (new XYZ(endX, endY, startZ) + planeY * offset));
                        aFirstDetailCurve = aDocument.Create.NewDetailCurve(view, Autodesk.Revit.DB.Line.CreateBound(new XYZ(startX, startY, startZ), new XYZ(startX, startY, startZ) - planeY * offset));
                        aSecondDetailCurve = aDocument.Create.NewDetailCurve(view, Autodesk.Revit.DB.Line.CreateBound(new XYZ(endX, endY, endZ), new XYZ(endX, endY, endZ) - planeY * offset));
                    }
                    else
                        return null;

                    ReferenceArray aReferenceArray = new ReferenceArray();
                    aReferenceArray.Append(aFirstDetailCurve.GeometryCurve.Reference);
                    aReferenceArray.Append(aSecondDetailCurve.GeometryCurve.Reference);
                    Dimension aDimension = aDocument.Create.NewDimension(view, aDimensionLine, aReferenceArray);

                    TransactionManager.Instance.TransactionTaskDone();
                    return new Dictionary<string, object>
                        {
                            {"AnnotationLine", aDimension.ToDSType(true) as Revit.Elements.Element},
                            {"DetailLineStart", aFirstDetailCurve.ToDSType(true) as Revit.Elements.Element},
                            {"DetailLineEnd", aSecondDetailCurve.ToDSType(true) as Revit.Elements.Element},
                        };

                }
            }
            TransactionManager.Instance.TransactionTaskDone();
            return null;
        }


        /// <summary>
        /// Create spot elevations at points. 
        /// </summary>
        /// <param name="Point"></param>Point for elevation
        /// <param name="viewName"></param>Name of view to annotate
        /// <param name="bendPoint"></param>Bend point for annotation line
        /// <param name="endPoint"></param>Endpoint for annotation line
        /// <returns></returns>
        public static Revit.Elements.Element SpotDimensionOnNode(Autodesk.DesignScript.Geometry.Point Point, string viewName,
                             Autodesk.DesignScript.Geometry.Point bendPoint, Autodesk.DesignScript.Geometry.Point endPoint)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);

            FilteredElementCollector viewCollector = new FilteredElementCollector(aDocument);
            viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));

            Autodesk.Revit.DB.View view = null;

            foreach (Autodesk.Revit.DB.Element viewElement in viewCollector)
            {
                Autodesk.Revit.DB.View viewTemp = (Autodesk.Revit.DB.View)viewElement;
                if (viewTemp.Name == viewName)
                    view = viewTemp;
            }

            if (view != null)
            {
                XYZ pt = new XYZ(Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(Point.X, DisplayUnitType.DUT_MILLIMETERS),
                    Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(Point.Y, DisplayUnitType.DUT_MILLIMETERS),
                    Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(Point.Z, DisplayUnitType.DUT_MILLIMETERS));
                XYZ bendPt = new XYZ(Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(bendPoint.X, DisplayUnitType.DUT_MILLIMETERS),
                    Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(bendPoint.Y, DisplayUnitType.DUT_MILLIMETERS),
                    Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(bendPoint.Z, DisplayUnitType.DUT_MILLIMETERS));
                XYZ endPt = new XYZ(Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(endPoint.X, DisplayUnitType.DUT_MILLIMETERS),
                    Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(endPoint.Y, DisplayUnitType.DUT_MILLIMETERS),
                    Autodesk.Revit.DB.UnitUtils.ConvertToInternalUnits(endPoint.Z, DisplayUnitType.DUT_MILLIMETERS));

                Autodesk.Revit.DB.DetailCurve aDetailCurve = aDocument.Create.NewDetailCurve(view, Autodesk.Revit.DB.Line.CreateBound(pt, bendPt));

                SpotDimension spotDimension = aDocument.Create.NewSpotElevation(view, aDetailCurve.GeometryCurve.Reference, pt, bendPt, endPt, pt, true);
                TransactionManager.Instance.TransactionTaskDone();
                return spotDimension.ToDSType(true);
            }
            TransactionManager.Instance.TransactionTaskDone();
            return null;
        }
    }
}
