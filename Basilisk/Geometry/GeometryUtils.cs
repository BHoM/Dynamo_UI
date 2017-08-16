using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BH = BHoM.Geometry;

namespace Geometry
{

    /// <summary>
    /// Geometry Utilities for Dynamo
    /// </summary>
    public static class GeometryUtils
    {
        public const double FeetToMetre = 0.3048;
        public const double MetreToFeet = 3.28084;

        public static string PointLocation(BH.Point point, int decimals)
        {
            return Math.Round(point.X, decimals) + ";" + Math.Round(point.Y, decimals) + ";" + Math.Round(point.Z, decimals);
        }

        public static string PointLocation(BH.Vector point, int decimals)
        {
            return Math.Round(point.X, decimals) + ";" + Math.Round(point.Y, decimals) + ";" + Math.Round(point.Z, decimals);
        }

        public static BH.Vector ConvertVector(DSG.Vector v)
        {
            return new BHoM.Geometry.Vector(v.X, v.Y, v.Z);
        }

        public static BH.Point Convert(DSG.Point point, int rounding = 9)
        {
            return new BHoM.Geometry.Point(Math.Round(point.X * FeetToMetre, rounding), Math.Round(point.Y * FeetToMetre, rounding), Math.Round(point.Z * FeetToMetre, rounding));
        }

        public static DSG.Point Convert(BH.Point point)
        {
           
            return DSG.Point.ByCoordinates(point.X * MetreToFeet, point.Y * MetreToFeet, point.Z * MetreToFeet);
        }

        internal static List<BH.Point> Convert(IEnumerable<DSG.Point> points, int rounding)
        {
            List<BH.Point> bhPoints = new List<BHoM.Geometry.Point>();
            foreach (DSG.Point point in points)
            {
                bhPoints.Add(Convert(point, rounding));
            }
            return bhPoints;
        }

        public static BH.Curve Convert(DSG.Curve curve, int rounding)
        {
            if (curve is DSG.Line)
            {
                return new BH.Line(Convert(curve.EndPoint, rounding), Convert(curve.StartPoint, rounding));
            }
            else if (curve is DSG.Arc)
            {
                return new BH.Arc(Convert(curve.StartPoint, rounding), Convert(curve.EndPoint, rounding), Convert(curve.PointAtParameter(0.5), rounding));
            }
            else if (curve is DSG.NurbsCurve)
            {
                DSG.NurbsCurve spline = curve as DSG.NurbsCurve;
                DSG.Point[] ctrlpnts = spline.ControlPoints();
                double[] knots = spline.Knots();
                double[] weights = spline.Weights();
                return BH.NurbCurve.Create(Convert(ctrlpnts, rounding), spline.Degree, knots, weights);
            }
            else if (curve is DSG.Ellipse)
            {
                DSG.Ellipse ellipse = curve as DSG.Ellipse;
                return new BH.Circle((ellipse.MajorAxis.Length + ellipse.MinorAxis.Length) / 2, new BH.Plane(Convert(ellipse.CenterPoint, rounding), new BH.Vector(ConvertVector(ellipse.Normal))));
            }
            return null;
        }

        public static bool IsHorizontal(BH.Plane p)
        {
            return BH.Vector.VectorAngle(p.Normal, BH.Vector.ZAxis()) < Math.PI / 48;
        }

        public static bool IsVertical(BH.Plane p)
        {
            double angle = BH.Vector.VectorAngle(p.Normal, BH.Vector.ZAxis());
            return angle > Math.PI / 2 - Math.PI / 48 && angle < Math.PI / 2 + Math.PI / 48;
        }

        public static BH.Group<BH.Curve> SnapTo(BH.Group<BH.Curve> curves, BH.Plane plane, double tolerance)
        {
            BH.Group<BH.Curve> result = new BH.Group<BH.Curve>();
            for (int i = 0; i < curves.Count; i++)
            {
                List<BH.Curve> segments = curves[i].Explode();
                for (int j = 0; j < segments.Count; j++)
                {
                    if (segments[j] is BH.Line)
                    {
                        BH.Point p1 = segments[j].StartPoint;
                        BH.Point p2 = segments[j].EndPoint;
                        double distance1 = Math.Abs(plane.DistanceTo(p1));
                        double distance2 = Math.Abs(plane.DistanceTo(p2));
                        if (distance1 > 0 && distance1 < tolerance)
                        {
                            if (distance2 > 0 && distance2 < tolerance)
                            {
                                segments[j].Project(plane);
                            }
                            else
                            {
                                p1.Project(plane);
                                segments[j] = new BH.Line(p1, p2);
                            }
                        }
                        else if (distance2 > 0 && distance2 < tolerance)
                        {
                            p2.Project(plane);
                            segments[j] = new BH.Line(p1, p2);
                        }
                    }
                }
                result.AddRange(BH.Curve.Join(segments));
            }
            return result;
        }
    }    
}