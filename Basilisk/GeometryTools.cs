using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using DS = Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

namespace GeometryTools
{
     /// <summary>
    /// Geometry operations
    /// BuroHappold
    /// <class name="GeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class Translate
    {
         internal Translate(){}
        
        /// <summary>
        /// Moves geometry to the origin based on centroid of bounding box, to make preview easier. Doesn't work on meshes!!
        /// BuroHappold
        /// </summary>
        /// <param name="geometry">Geometry at origin</param>
        /// <returns></returns>
        /// /// <search>BH, move, translate, preview, origin</search>
        [MultiReturn(new[] { "Geometry" })]
        public static Dictionary<string, object> PreviewAtOrigin(IEnumerable<DS.Geometry> geometry = null)
        {
            //Output dictionary definition
            Dictionary<string, object> PreviewAtOrigin_out = new Dictionary<string, object>();
            List<DS.Geometry> new_geo_list = new List<DS.Geometry>();

            DS.Cuboid b_box = DS.BoundingBox.ByGeometry(geometry).ToCuboid();
            DS.Vector vec = DS.Vector.ByTwoPoints(b_box.Centroid(), DS.Point.ByCoordinates(0, 0, 0));
            DS.Vector vec2 = DS.Vector.ByCoordinates(0, 0, b_box.Height / 2);
            vec = vec.Add(vec2);
            foreach (DS.Geometry geo in geometry)
            {
                DS.Geometry new_geo = geo.Translate(vec, vec.Length);
                new_geo_list.Add(new_geo);

            }

            vec.Dispose(); b_box.Dispose();

            if (geometry != null) PreviewAtOrigin_out.Add("Geometry", new_geo_list);
            return PreviewAtOrigin_out;
        }
    }
        
        /// <summary>
        /// Curve tools
        /// BuroHappold
        /// <class name="CurveTools">Geometry tools for Dynamo</class>
        /// </summary>
    public class Curve
    {
        internal Curve() { }
        /// <summary>
        /// Maps a 2D or 3D vertical curve onto a plane whilst maintaining the curve length and
        /// slope (developed elevation). Increase divisions to improve accuracy.
        /// BuroHappold
        /// </summary>
        /// <param name="curve">2D or 3D vertical curve</param>
        /// <param name="projectionPlane">Vertical plane to project onto (plane rotation is ignored)</param>
        /// <param name="sampleSpacing">Reduce spacing to improve accuracy</param>
        /// <returns name="NurbsCurve">Returns approximated vertical nurbs curve through points</returns>
        /// <returns name="PolyCurve">Returns a vertical PolyCurve with straight segments between points</returns>
        /// <search>BH, develop, curve</search>
        [MultiReturn(new[] { "NurbsCurve", "PolyCurve" })]
        public static Dictionary<string, object> DevelopCurve(DS.Curve curve, DS.Plane projectionPlane, double sampleSpacing = 1)
        {
            //Output dictionary definition
            Dictionary<string, object> devcrv_out = new Dictionary<string, object>();

            //Input variables
            DS.Line ln = DS.Line.ByStartPointDirectionLength(DS.Point.ByCoordinates(0, 0, 0), DS.Vector.XAxis(), curve.Length * 2);
            DS.CoordinateSystem o_coords = DS.CoordinateSystem.ByOriginVectors(DS.Point.ByCoordinates(0, 0), DS.Vector.XAxis(), DS.Vector.ZAxis());
            List<DS.Point> crv_pnts = new List<DS.Point>();
            List<DS.Point> ProfilePointList = new List<DS.Point>();
            ProfilePointList.Add((DS.Point)ln.StartPoint.Transform(o_coords, projectionPlane.ToCoordinateSystem()));
            double horiz_dist = 0;

            DS.Geometry[] exploded_crvs = curve.Explode();
            for (int i = 0; i < exploded_crvs.Length; i++)
            {
                DS.Curve temp_crv = (DS.Curve)exploded_crvs[i];
                int divisions = Math.Max((int)Math.Round((temp_crv.Length / sampleSpacing), 0), 2);
                for (int j = 0; j < divisions; j++)
                {
                    double crv_param = (j == 0) ? 0 : (double)j / (double)divisions;
                    crv_pnts.Add(temp_crv.PointAtParameter(crv_param));
                }
            }
            crv_pnts.Add(curve.EndPoint);

            for (int i = 1; i < crv_pnts.Count; i++)
            {
                DS.Point pnt1 = DS.Point.ByCoordinates(crv_pnts[i - 1].X, crv_pnts[i - 1].Y, 0);
                DS.Point pnt2 = DS.Point.ByCoordinates(crv_pnts[i].X, crv_pnts[i].Y, 0);
                double crv_dist = pnt1.DistanceTo(pnt2);
                horiz_dist = horiz_dist + crv_dist;
                DS.Point pnt = DS.Point.ByCoordinates(ln.PointAtDistance(horiz_dist).X, ln.PointAtDistance(horiz_dist).Y, crv_pnts[i].Z);
                ProfilePointList.Add((DS.Point)pnt.Transform(o_coords, projectionPlane.ToCoordinateSystem()));
            }

            DS.PolyCurve pcrv = DS.PolyCurve.ByPoints(ProfilePointList, false);
            DS.NurbsCurve ncrv = DS.NurbsCurve.ByControlPoints(ProfilePointList);

            devcrv_out.Add("NurbsCurve", ncrv);
            devcrv_out.Add("PolyCurve", pcrv);
            return devcrv_out;
        }

        /// <summary>
        /// Maps a vertical or 3D curve onto a projection curve. 
        /// Resulting curve should be same length.
        /// BuroHappold
        /// </summary>
        /// <param name="curve">3D input curve</param>
        /// <param name="projectionCurve">Planar projection curve at Z=0</param>
        /// <param name="sampleSpacing">Reduce spacing to improve accuracy</param>
        /// <returns></returns>
        /// <search>BH, develop, curve</search>
        [MultiReturn(new[] { "NurbsCurve", "PolyCurve" })]
        public static Dictionary<string, object> ProjectCurvetoCurve(DS.Curve curve, DS.Curve projectionCurve, double sampleSpacing = 1)
        {
            //Output dictionary definition
            Dictionary<string, object> devcrv_out = new Dictionary<string, object>();
            DS.Curve _curve = (DS.Curve)curve.Translate(DS.Vector.ByTwoPoints(curve.StartPoint, projectionCurve.StartPoint));
            List<DS.Point> crv_pnts = new List<DS.Point>();
            double horiz_dist = 0;
            List<DS.Point> ProfilePointList = new List<DS.Point>();

            DS.Geometry[] exploded_crvs = curve.Explode();
            for (int i = 0; i < exploded_crvs.Length; i++)
            {
                DS.Curve temp_crv = (DS.Curve)exploded_crvs[i];
                int divisions = Math.Max((int)Math.Round((temp_crv.Length / sampleSpacing), 0), 2);
                for (int j = 0; j < divisions; j++)
                {
                    double crv_param = (j == 0) ? 0 : (double)j / (double)divisions;
                    crv_pnts.Add(temp_crv.PointAtParameter(crv_param));
                }
            }
            crv_pnts.Add(curve.EndPoint);

            ProfilePointList.Add(DS.Point.ByCoordinates(projectionCurve.StartPoint.X, projectionCurve.StartPoint.Y, crv_pnts[0].Z));
            for (int i = 0; i < crv_pnts.Count; i++)
            {
                if (i > 0)
                {
                    DS.Point pnt1 = DS.Point.ByCoordinates(crv_pnts[i - 1].X, crv_pnts[i - 1].Y, 0);
                    DS.Point pnt2 = DS.Point.ByCoordinates(crv_pnts[i].X, crv_pnts[i].Y, 0);
                    double crv_dist = pnt1.DistanceTo(pnt2);
                    horiz_dist = horiz_dist + crv_dist;
                    DS.Point pnt = DS.Point.ByCoordinates(projectionCurve.PointAtDistance(horiz_dist).X, projectionCurve.PointAtDistance(horiz_dist).Y, crv_pnts[i].Z);
                    ProfilePointList.Add(pnt);
                }
            }
            DS.PolyCurve pcrv = DS.PolyCurve.ByPoints(ProfilePointList, false);
            DS.NurbsCurve ncrv = DS.NurbsCurve.ByControlPoints(ProfilePointList);

            devcrv_out.Add("PolyCurve", pcrv);
            devcrv_out.Add("NurbsCurve", ncrv);
            return devcrv_out;
        }

        /// <summary>
        /// Gets the start mid and end point of a curve (saves using multiple nodes)
        /// BuroHappold
        /// </summary>
        /// <param name="curve">Input curve</param>
        /// <returns></returns>
        /// <search>BH, curve, midpoint, endpoint</search>
        [MultiReturn(new[] { "StartPoint", "MidPoint", "EndPoint" })]
        public static Dictionary<string, object> CurveStartMidEnd(DS.Curve curve)
        {
            //Output dictionary definition
            Dictionary<string, object> curveStartMidEnd_out = new Dictionary<string, object>();

            curveStartMidEnd_out.Add("StartPoint", (DS.Point)curve.StartPoint);
            curveStartMidEnd_out.Add("MidPoint", (DS.Point)curve.PointAtParameter(0.5));
            curveStartMidEnd_out.Add("EndPoint", (DS.Point)curve.EndPoint);
            return curveStartMidEnd_out;
        }


        /// <summary>
        /// Splits a list of curves by itself. Returns the new list of split curves
        /// BuroHappold
        /// </summary>
        /// <param name="curves">Input curve</param>
        /// <returns></returns>
        /// <search>BH, curve, split, intersect</search>
        //[MultiReturn(new[] { "SplitCurves" })]
        public static List<DS.Curve> SplitCurvesBySelf(List<DS.Curve> curves)
        {
            List<DS.Curve> splitCurves = new List<DS.Curve>();

            int noCrvs = curves.Count;

            for (int i = 0; i < noCrvs; i++)
            {
                List<double> dParams = new List<double>(noCrvs);

                for (int j = 0; j < noCrvs; j++)
                {
                    if (i != j) //don't want to check for self intersection - will not return typeof(DS.Point) anyway
                    {
                        DS.Geometry[] intersect = curves[i].Intersect(curves[j]);
                        foreach (DS.Geometry g in intersect)
                        {
                            if (g.GetType() == typeof(DS.Point))
                                dParams.Add(curves[i].ParameterAtPoint((DS.Point)g));
                        }
                    }
                }

                DS.Curve[] splitCrv = curves[i].ParameterSplit(dParams.ToArray());

                foreach (DS.Curve c in splitCrv)
                    splitCurves.Add(c);
            }
            return splitCurves;
        }
    }

    /// <summary>
    /// Point class for all extra point utilities
    /// BuroHappold
    /// </summary>
        public class Point
        {
            internal Point() { }
            /// <summary>
            /// Uses Sorted dictionary method of finding one or more closest points to a point. The tolerance of point distance collection
            /// is set to 6 decimal places. 
            /// BuroHappold
            /// </summary>
            /// <param name="point">Point from which to search</param>
            /// <param name="pointsToSearch">Collection of points to get the closest points from</param>
            /// <param name="numberOfPoints">The number of closest points to return</param>
            /// <returns></returns>
            /// <search>BH, closest</search>
            [MultiReturn(new[] { "Points" })]
            public static Dictionary<string, object> ClosestPoint(DS.Point point, IEnumerable<DS.Point> pointsToSearch, int numberOfPoints = 1)
            {
                //Output dictionary definition
                Dictionary<string, object> points_out = new Dictionary<string, object>();
                SortedDictionary<double, DS.Point> pnt_dists = new SortedDictionary<double, DS.Point>();

                foreach (DS.Point pnt in pointsToSearch)
                {
                    double dist = point.DistanceTo(pnt);
                    try {pnt_dists.Add(Math.Round(dist,6),pnt);} catch{;};
                }

                List<DS.Point> closest_pnts = new List<DS.Point>();
                for (int i = 0; i < numberOfPoints;i++)
                {
                    closest_pnts.Add(pnt_dists.ElementAt(i).Value);
                }
                pnt_dists.Clear();
                points_out.Add("Point", closest_pnts.ToArray());
                return points_out;
            }
        }
    }