using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;


namespace BH.Engine.Dynamo
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static object IToBHoM(this object obj)
        {
            if (obj is ADG.Geometry || obj is ADG.Vector)
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

        public static BHG.NurbCurve ToBHoM(this ADG.Curve nurbCurve)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static BHG.NurbCurve ToBHoM(this ADG.NurbsCurve nurbCurve)
        {
            return new BHG.NurbCurve
            {
                ControlPoints = nurbCurve.ControlPoints().Select(x => x.ToBHoM()).ToList(),
                Knots = nurbCurve.Knots().ToList().GetRange(1, nurbCurve.Knots().Count()-2),
                Weights = nurbCurve.Weights().ToList()
            };
        }

        /***************************************************/

        public static BHG.Plane ToBHoM(this ADG.Plane plane)
        {
            return Geometry.Create.Plane(plane.Origin.ToBHoM(), plane.Normal.ToBHoM());
        }

        /***************************************************/

        public static BHG.PolyCurve ToBHoM(this ADG.PolyCurve polyCurve)
        {
            return Geometry.Create.PolyCurve(polyCurve.Curves().Select(x => x.ToBHoM()));
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this ADG.Polygon polygon)
        {
            return Geometry.Create.Polyline(polygon.Points.Select(x => x.ToBHoM()));
        }

        /***************************************************/

        public static BHG.BoundingBox ToBHoM(this ADG.BoundingBox boundingBox)
        {
            return Geometry.Create.BoundingBox(boundingBox.MinPoint.ToBHoM(), boundingBox.MaxPoint.ToBHoM());
        }

        /***************************************************/

        public static BHG.NurbSurface ToBHoM(this ADG.Surface surface)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static BHG.NurbSurface ToBHoM(this ADG.NurbsSurface surface)
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
