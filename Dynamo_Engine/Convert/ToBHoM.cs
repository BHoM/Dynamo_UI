using Autodesk.DesignScript.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;


namespace BH.Engine.Dynamo
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static object IToBHoM(this object obj)
        {
            if (obj is IEnumerable || obj is ADG.Geometry || obj is ADG.Vector)
                return Convert.ToBHoM(obj as dynamic);
            else
                return obj;
        }

        /***************************************************/

        public static BHG.IGeometry IToBHoM(this ADG.Geometry geometry)
        {
            return Convert.ToBHoM(geometry as dynamic);
        }


        /***************************************************/
        /**** Public Methods  - Collections             ****/
        /***************************************************/

        public static IEnumerable ToBHoM(this IEnumerable list)
        {
            if (list is string || list.GetType().Name.StartsWith("Dictionary"))
                return list;
            else
            {
                List<object> newList = new List<object>();
                foreach (object item in list)
                    newList.Add(item.IToBHoM());
                return newList;
            }
        }

        /***************************************************/

        // Issue with natvie Dynamo nodes to be confused (Issue 83)
        //public static IEnumerable<object> ToBHoM(this ArrayList list)
        //{
        //    return list.ToArray().Select(x => x.IToBHoM()).ToList();
        //}

        /***************************************************/
        /**** Public Methods  - Geometry                ****/
        /***************************************************/

        public static BHG.Point ToBHoM(this ADG.Point designScriptPt)
        {
            return Geometry.Create.Point(designScriptPt.X, designScriptPt.Y, designScriptPt.Z);
        }

        /***************************************************/

        public static BHG.Vector ToBHoM(this ADG.Vector designScriptVec)
        {
            return Geometry.Create.Vector(designScriptVec.X, designScriptVec.Y, designScriptVec.Z);
        }

        /***************************************************/

        public static BHG.Arc ToBHoM(this ADG.Arc arc)
        {
            return Geometry.Create.Arc(arc.StartPoint.ToBHoM(), arc.PointAtParameter(0.5).ToBHoM(), arc.EndPoint.ToBHoM());
        }

        /***************************************************/

        public static BHG.Circle ToBHoM(this ADG.Circle circle)
        {
            return Geometry.Create.Circle(circle.CenterPoint.ToBHoM(), circle.Normal.ToBHoM(), circle.Radius);
        }

        /***************************************************/

        public static BHG.Line ToBHoM(this ADG.Line line)
        {
            return Geometry.Create.Line(line.StartPoint.ToBHoM(), line.EndPoint.ToBHoM());
        }

        /***************************************************/

        public static BHG.NurbsCurve ToBHoM(this ADG.Curve nurbsCurve)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static BHG.NurbsCurve ToBHoM(this ADG.NurbsCurve nurbsCurve)
        {
            return new BHG.NurbsCurve
            {
                ControlPoints = nurbsCurve.ControlPoints().Select(x => x.ToBHoM()).ToList(),
                Knots = nurbsCurve.Knots().ToList().GetRange(1, nurbsCurve.Knots().Count()-2),
                Weights = nurbsCurve.Weights().ToList()
            };
        }

        /***************************************************/

        public static BHG.Plane ToBHoM(this ADG.Plane plane)
        {
            return Geometry.Create.Plane(plane.Origin.ToBHoM(), plane.Normal.ToBHoM());
        }

        /***************************************************/

        public static BHG.CoordinateSystem.Cartesian ToBHoM(this ADG.CoordinateSystem coordinateSystem)
        {
            return Geometry.Create.CartesianCoordinateSystem(coordinateSystem.Origin.ToBHoM(), coordinateSystem.XAxis.ToBHoM(), coordinateSystem.YAxis.ToBHoM());
        }

        /***************************************************/

        public static BHG.PolyCurve ToBHoM(this ADG.PolyCurve polyCurve)
        {
            return Geometry.Create.PolyCurve(polyCurve.Curves().Select(x => x.ToBHoM()));
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this ADG.Polygon polygon)
        {
            List<BH.oM.Geometry.Point> pts = polygon.Points.Select(x => x.ToBHoM()).ToList();
            if (pts.Count == 0)
                return new BHG.Polyline();

            pts.Add(pts[0]);
            return Geometry.Create.Polyline(pts);
        }

        /***************************************************/

        public static BHG.BoundingBox ToBHoM(this ADG.BoundingBox boundingBox)
        {
            return Geometry.Create.BoundingBox(boundingBox.MinPoint.ToBHoM(), boundingBox.MaxPoint.ToBHoM());
        }

        /***************************************************/

        public static BHG.NurbsSurface ToBHoM(this ADG.Surface surface)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static BHG.NurbsSurface ToBHoM(this ADG.NurbsSurface surface)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static BHG.Mesh ToBHoM(this ADG.Mesh DSMesh)
        {
            List<BHG.Point> vertices = DSMesh.VertexPositions.ToList().Select(x => x.ToBHoM()).ToList();
            List<ADG.IndexGroup> DSFacesIndex = DSMesh.FaceIndices.ToList();
            List<BHG.Face> Faces = new List<BHG.Face>();

            for (int i = 0; i < DSFacesIndex.Count; i++)
            {
                if (DSFacesIndex[i].Count == 4)
                {
                    Faces.Add(Geometry.Create.Face((int)DSFacesIndex[i].A, (int)DSFacesIndex[i].B, (int)DSFacesIndex[i].C, (int)DSFacesIndex[i].D));
                }
                if (DSFacesIndex[i].Count == 3)
                {
                    Faces.Add(Geometry.Create.Face((int)DSFacesIndex[i].A, (int)DSFacesIndex[i].B, (int)DSFacesIndex[i].C));
                }
            }
            return Geometry.Create.Mesh(vertices, Faces);
        }

        /***************************************************/
    }
}
