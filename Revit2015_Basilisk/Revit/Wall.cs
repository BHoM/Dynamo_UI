using RevitServices.Transactions;
using Revit.Elements;
using Revit.GeometryConversion;

namespace Revit
{
    /// <summary>
    /// A Wall
    /// </summary>
    public static class Wall
    {
        /// <summary>
        /// Creates wall by curve.
        /// </summary>
        /// <param name="Level">Level</param>
        /// <param name="Curve">Curve</param>
        /// <param name="Structural">Structural</param>
        /// <returns name="Wall">Wall</returns>
        /// <search>
        /// Wall by curve, Wall by curve
        /// </search>
        public static Elements.Wall ByCurve(Revit.Elements.Level Level, Autodesk.DesignScript.Geometry.Curve Curve, bool Structural = false)
        {
            Autodesk.Revit.DB.Document aDocument = Level.InternalElement.Document;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.Wall aWall = Autodesk.Revit.DB.Wall.Create(aDocument, Curve.ToRevitType(false), Level.InternalElement.Id, Structural);
            TransactionManager.Instance.TransactionTaskDone();
            return aWall.ToDSType(true) as Elements.Wall;
        }

        /// <summary>
        /// Returns width of wall.
        /// </summary>
        /// <param name="Wall">Wall</param>
        /// <returns name="Width">Wall width</returns>
        /// <search>
        /// Wall, Wall width, wall, wall width
        /// </search>
        public static double? Width(Elements.Wall Wall)
        {
            Autodesk.Revit.DB.Wall aWall = Wall.InternalElement as Autodesk.Revit.DB.Wall;
            return aWall.Width;
        }

        /// <summary>
        /// Returns curve of wall.
        /// </summary>
        /// <param name="Wall">Wall</param>
        /// <returns name="Curve">Wall curve</returns>
        /// <search>
        /// Wall curve, Wall, wall, wall curve
        /// </search>
        public static Autodesk.DesignScript.Geometry.Curve Curve(Revit.Elements.Wall Wall)
        {
            Autodesk.Revit.DB.Wall aWall = Wall.InternalElement as Autodesk.Revit.DB.Wall;
            Autodesk.Revit.DB.Curve aCurve = (aWall.Location as Autodesk.Revit.DB.LocationCurve).Curve;
            return aCurve.ToProtoType(false);
        }

        /// <summary>
        /// Creates opening in wall.
        /// </summary>
        /// <param name="Wall">Wall</param>
        /// <param name="StartPoint">Start Point</param>
        /// <param name="EndPoint">End Point</param>
        /// <returns name="CreateOpening">Created opening</returns>
        /// <search>
        /// Wall, Create opening, wall, create opening
        /// </search>       
        private static Revit.Elements.Element CreateOpening(Elements.Wall Wall, Autodesk.DesignScript.Geometry.Point StartPoint, Autodesk.DesignScript.Geometry.Point  EndPoint)
        {
            if (Wall != null && Wall.InternalElement != null)
            {
                Autodesk.Revit.DB.Wall aWall = Wall.InternalElement as Autodesk.Revit.DB.Wall;
                if (aWall != null) 
                {
                    Autodesk.Revit.DB.Document aDocument = aWall.Document;
                    TransactionManager.Instance.EnsureInTransaction(aDocument);
                    Autodesk.Revit.DB.Opening aOpening = aDocument.Create.NewOpening(aWall, new Autodesk.Revit.DB.XYZ(StartPoint.X, StartPoint.Y, StartPoint.Z), new Autodesk.Revit.DB.XYZ(EndPoint.X, EndPoint.Y, EndPoint.Z));
                    TransactionManager.Instance.TransactionTaskDone();
                    if (aOpening != null)
                        return aOpening.ToDSType(true);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns WallType of wall.
        /// </summary>
        /// <param name="Wall">Wall</param>
        /// <returns name="WallType">WallType</returns>
        /// <search>
        /// WallType, Wall, walltype, wall type
        /// </search>
        public static Revit.Elements.WallType GetWallType(Revit.Elements.Wall Wall)
        {
            Autodesk.Revit.DB.Wall aWall = Wall.InternalElement as Autodesk.Revit.DB.Wall;
            return aWall.WallType.ToDSType(true) as Revit.Elements.WallType;
        }

        /// <summary>
        /// Sets WallType of wall.
        /// </summary>
        /// <param name="Wall">Wall</param>
        /// <param name="WallType">WallType</param>
        /// <returns name="Wall">Wall</returns>
        /// <search>
        /// Set WallType, Wall, set walltype, wall type
        /// </search>
        public static Revit.Elements.Wall SetWallType(Revit.Elements.Wall Wall, Revit.Elements.WallType WallType)
        {
            Autodesk.Revit.DB.Wall aWall = Wall.InternalElement as Autodesk.Revit.DB.Wall;
            TransactionManager.Instance.EnsureInTransaction(aWall.Document);
            aWall.WallType = WallType.InternalElement as Autodesk.Revit.DB.WallType;
            TransactionManager.Instance.TransactionTaskDone();
            return Wall;
        }
    }
}
