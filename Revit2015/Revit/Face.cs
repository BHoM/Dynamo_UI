using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Revit.GeometryConversion;

namespace Revit
{
    /// <summary>
    /// A Face
    /// </summary>
    public static class Face
    {
        /// <summary>
        /// Returns Normal of face.
        /// </summary>
        /// <param name="Face">Face</param>
        /// <returns name="Normal">Normal</returns>
        /// <search>
        /// Face normal, face, normal
        /// </search>
        public static Autodesk.DesignScript.Geometry.Vector Normal(Autodesk.Revit.DB.Face Face)
        {
            PlanarFace aPlanarFace = Face as PlanarFace;
            if (aPlanarFace != null)
            {
                return Autodesk.DesignScript.Geometry.Vector.ByCoordinates(aPlanarFace.Normal.X, aPlanarFace.Normal.Y, aPlanarFace.Normal.Z);
            }

            return null;
        }

        /// <summary>
        /// Project point on the face.
        /// </summary>
        /// <param name="Face">Face</param>
        /// <param name="Point">Point</param>
        /// <returns name="Point">Projected Point</returns>
        /// <search>
        /// Project point on face, face, project
        /// </search>
        public static Autodesk.DesignScript.Geometry.Point Project(Autodesk.Revit.DB.Face Face, Autodesk.DesignScript.Geometry.Point Point)
        {
            if (Face != null && Point != null)
            {
                IntersectionResult aIntersectionResult = Face.Project(new XYZ(Point.X, Point.Y, Point.Z));
                if (aIntersectionResult != null)
                    return Autodesk.DesignScript.Geometry.Point.ByCoordinates(aIntersectionResult.XYZPoint.X, aIntersectionResult.XYZPoint.Y, aIntersectionResult.XYZPoint.Z);
            }

            return null;
        }

        /// <summary>
        /// Points of the face.
        /// </summary>
        /// <param name="Face">Face</param>
        /// <returns name="Points">Points</returns>
        /// <search>
        /// face points, face points
        /// </search>
        public static List<Autodesk.DesignScript.Geometry.Point> Points(Autodesk.Revit.DB.Face Face)
        {
            if(Face != null)
            {
                List<Autodesk.DesignScript.Geometry.Point> aResult = new List<Autodesk.DesignScript.Geometry.Point>();
                foreach(EdgeArray aEdgeArray in Face.EdgeLoops)
                    foreach (Edge aEdge in aEdgeArray)
                    {
                        Curve aCurve = aEdge.AsCurve();
                        if (aCurve != null)
                        {
                            XYZ aXYZ = aCurve.GetEndPoint(0);
                            if (aXYZ != null)
                                aResult.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(aXYZ.X, aXYZ.Y, aXYZ.Z));
                        }
                    }
                return aResult;
            }
            return null;
        }

        /// <summary>
        /// Area of the face.
        /// </summary>
        /// <param name="Face">Face</param>
        /// <returns name="Area">Area</returns>
        /// <search>
        /// Face Area, face area
        /// </search>
        public static double Area(Autodesk.Revit.DB.Face Face)
        {
            return Face.Area;
        }

        /// <summary>
        /// Gets Face Surfaces
        /// </summary>
        /// <param name="Face">Face</param>
        /// <returns name="Surfaces">List of Surfaces</returns>
        /// <search>
        /// Face Surfaces, Surfaces
        /// </search>
        public static List<Autodesk.DesignScript.Geometry.Surface> Surfaces(Autodesk.Revit.DB.Face Face)
        {
            return Face.ToProtoType(false, null).ToList();
        }


    }
}
