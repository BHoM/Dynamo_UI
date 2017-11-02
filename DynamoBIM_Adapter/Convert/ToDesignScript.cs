using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;

namespace BH.Adapter.DesignScript
{
    public static partial class Convert
    {

        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static ADG.Geometry IToDesignScript(this BHG.IBHoMGeometry geometry)
        {
            return Convert.ToDesignScript(geometry as dynamic);
        }

        /***************************************************/

        public static ADG.Point ToDesignScript(this BHG.Point point)
        {
            return ADG.Point.ByCoordinates(point.X, point.Y, point.Z);
        }

        /***************************************************/

        public static ADG.Vector ToDesignScript(this BHG.Vector vector)
        {
            return ADG.Vector.ByCoordinates(vector.X, vector.Y, vector.Z);
        }

        /***************************************************/

        public static ADG.Arc ToDesignScript(this BHG.Arc arc)
        {
            return ADG.Arc.ByCenterPointStartPointEndPoint(arc.Middle.ToDesignScript(), arc.Start.ToDesignScript(), arc.End.ToDesignScript());
        }

        /***************************************************/

        public static ADG.Circle ToDesignScript(this BHG.Circle circle)
        {
            return ADG.Circle.ByCenterPointRadiusNormal(circle.Centre.ToDesignScript(), circle.Radius, circle.Normal.ToDesignScript());
        }

        /***************************************************/

        public static ADG.Line ToDesignScript(this BHG.Line line)
        {
            return ADG.Line.ByStartPointEndPoint(line.Start.ToDesignScript(), line.End.ToDesignScript());
        }

        /***************************************************/

        public static ADG.NurbsCurve ToDesignScript(this BHG.NurbCurve nurbsCurve)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static ADG.Plane ToRhino(this BHG.Plane plane)
        {
            return ADG.Plane.ByOriginNormal(plane.Origin.ToDesignScript(), plane.Normal.ToDesignScript());
        }

        /***************************************************/

        public static ADG.PolyCurve ToDesignScript(this BHG.Polyline polyLine)
        {
            return ADG.PolyCurve.ByPoints(polyLine.ControlPoints.Select(x => x.ToDesignScript()));
        }

        /***************************************************/

        public static ADG.BoundingBox ToDesignScript(this BHG.BoundingBox boundingBox)
        {
            return ADG.BoundingBox.ByCorners(boundingBox.Min.ToDesignScript(), boundingBox.Max.ToDesignScript());
        }

        /***************************************************/

        public static ADG.Surface ToDesignScript(this BHG.NurbSurface surface)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        //public static ADG.Mesh ToRhino(this BHG.Mesh mesh)
        //{
        //    ADG.Mesh.ByPointsFaceIndices(mesh.Vertices.Select(x => x.ToDesignScript()), )
        //    List<ADG.Point> DSVertices = mesh.Vertices.Select(x => x.ToDesignScript()).ToList();
        //    List<BHG.Face> faces = mesh.Faces;
        //    List<ADG.Face> DSFaces = new List<ADG.Face>();
        //    for (int i = 0; i < faces.Count; i++)
        //    {
        //        if (faces[i].IsQuad())
        //        {
        //            DSFaces.Add(ADG.Mesh.)
        //        }
        //        else
        //        {
        //            rFaces.Add(new RHG.MeshFace(faces[i].A, faces[i].B, faces[i].C));
        //        }
        //    }
        //    RHG.Mesh rMesh = new RHG.Mesh();
        //    rMesh.Faces.AddFaces(rFaces);
        //    rMesh.Vertices.AddVertices(rVertices);
        //    return rMesh;
        //}


    }
}
