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

        public static BHG.IBHoMGeometry IToBHoM(this ADG.Geometry geometriy)
        {
            return Convert.ToBHoM(geometriy as dynamic);
        }

        /***************************************************/

        public static BHG.CompositeGeometry ToBHoM(this List<ADG.Geometry> geometries)
        {
            throw new NotFiniteNumberException();
            //return new BHG.CompositeGeometry(geometries.Select(x => x.ToBHoM()));
        }

        /***************************************************/
        /**** Public Methods  - 1D                      ****/
        /***************************************************/

        public static BHG.Point ToBHoM(this ADG.Point designScriptPt)
        {
            return new BHG.Point(designScriptPt.X, designScriptPt.Y, designScriptPt.Z);
        }

        /***************************************************/

        public static BHG.Vector ToBHoM(this ADG.Vector designScriptVec)
        {
            return new BHG.Vector(designScriptVec.X, designScriptVec.Y, designScriptVec.Z);
        }

        /***************************************************/

        public static BHG.Arc ToBHoM(this ADG.Arc arc)
        {
            return new BHG.Arc(arc.StartPoint.ToBHoM(), arc.PointAtParameter(0.5).ToBHoM(), arc.EndPoint.ToBHoM());
        }

        /***************************************************/

        public static BHG.Circle ToBHoM(this ADG.Circle circle)
        {
            return new BHG.Circle(circle.CenterPoint.ToBHoM(), circle.Normal.ToBHoM(), circle.Radius);
        }

        /***************************************************/

        public static BHG.Line ToBHoM(this ADG.Line line)
        {
            return new BHG.Line(line.StartPoint.ToBHoM(), line.EndPoint.ToBHoM());
        }

        /***************************************************/

        public static BHG.NurbCurve ToBHoM(this ADG.Curve Curve)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static BHG.NurbCurve ToBHoM(this ADG.NurbsCurve nurbCurve)
        {
            IEnumerable<ADG.Point> dPoints = nurbCurve.ControlPoints().ToList();
            IEnumerable<double> dWeights= nurbCurve.Weights().ToList();
            IEnumerable<double> dKnots = nurbCurve.Knots().ToList();
            return new BHG.NurbCurve(dPoints.Select(x => x.ToBHoM()), dWeights, dKnots);
        }

        /***************************************************/

        public static BHG.Plane ToBHoM(this ADG.Plane plane)
        {
            return new BHG.Plane(plane.Origin.ToBHoM(), plane.Normal.ToBHoM());
        }

        /***************************************************/

        public static BHG.PolyCurve ToBHoM(this ADG.PolyCurve polyCurve)
        {
            return new BHG.PolyCurve(polyCurve.Curves().Select(x => x.ToBHoM()));
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this ADG.Polygon polygon)
        {
            return new BHG.Polyline(polygon.Points.Select(x => x.ToBHoM()));
        }

        /***************************************************/

        public static BHG.BoundingBox ToBHoM(this ADG.BoundingBox boundingBox)
        {
            return new BHG.BoundingBox(boundingBox.MinPoint.ToBHoM(), boundingBox.MaxPoint.ToBHoM());
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
                    Faces.Add(new BHG.Face((int)DSFacesIndex[i].A, (int)DSFacesIndex[i].B, (int)DSFacesIndex[i].C, (int)DSFacesIndex[i].D));
                }
                if (DSFacesIndex[i].Count == 3)
                {
                    Faces.Add(new BHG.Face((int)DSFacesIndex[i].A, (int)DSFacesIndex[i].B, (int)DSFacesIndex[i].C));
                }
            }
            return new BHG.Mesh(vertices, Faces);
        }
    }
}
