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

namespace Structural
{
    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class Truss2D
    {
        internal Truss2D() { }

        /// <summary>
        /// Creates a 2D Warren type truss between a top chord and a bottom chord. The angle between 
        /// the lacers can be modified (not strictly a Warren truss unless all internal angles are 60 degrees).
        /// BuroHappold
        /// </summary>
        /// <param name="topChord">Curve representing the top chord of the truss. Reversed curves will be automatically flipped</param>
        /// <param name="bottomChord">Curve representing the bottom chord of the truss</param>
        /// <param name="lacerAngle">Angle between lacers (set to 60 for true Warren truss</param>
        /// <param name="splitChords">Set to true to break the chords into straight segments for input to analysis (forced if chords are not straight)</param>
        /// <returns></returns>
        /// <search>BH, truss, warren</search>
        [MultiReturn(new[] { "LacerCentrelines", "tcParameters", "bcParameters", "Bars", "LacerAngles", "LacerPlanes", "LacerRotPlanes" })]
        public static Dictionary<string, object> WarrenByChords(DS.Curve topChord, DS.Curve bottomChord, double lacerAngle = 60, bool equalizeLength = false, bool splitChords = false)
        {
            List<BHoM.Structural.Bar> str_bars = new List<BHoM.Structural.Bar>();
            List<DS.Line> lacers = new List<DS.Line>();
            List<double> t_params = new List<double>();
            List<double> b_params = new List<double>();
            List<double> lacer_angles = new List<double>();
            List<DS.Plane> lacer_planes = new List<DS.Plane>();
            List<DS.Plane> lacer_rot_planes = new List<DS.Plane>();

            //lacerAngle = (lacerAngle < 30) ? 30 : lacerAngle;
            //lacerAngle = (lacerAngle > 90) ? 120 : lacerAngle;

            if (topChord != bottomChord)
            {
                DS.Curve bc = bottomChord;
                //If the chord curves are reversed with respect to one another, flip the top chord for the purpose of generating lacers
                DS.Curve tc = (bc.StartPoint.DistanceTo(topChord.StartPoint) > bc.StartPoint.DistanceTo(topChord.EndPoint)) ? topChord.Reverse() : topChord;
                
                //Define a plane that all lacers will be aligned to - set by a line between the start points of the top and bottom chord and the bottom chord tangent
                DS.Plane start_pln = DS.Plane.ByOriginNormalXAxis(bc.StartPoint, bc.TangentAtParameter(0), DS.Vector.ByTwoPoints(bc.StartPoint, tc.StartPoint));

                int intersections = 1;
               
                    int k = 0;
                    while (intersections > 0)
                    {
                        DS.Curve c1 = (k % 2 == 0) ? bc : tc;
                        DS.Curve c2 = (k % 2 == 0) ? tc : bc;
                        DS.Point pnt = (k == 0) ? bc.StartPoint : lacers[k-1].EndPoint;
                                              
                        DS.Vector tangent = c1.TangentAtParameter(c1.ParameterAtPoint(pnt));

                        DS.Plane lacer_pln = (k == 0)? start_pln : DS.Plane.ByOriginNormalXAxis(pnt, tangent, DS.Vector.ByTwoPoints(pnt, (DS.Point)start_pln.Intersect(c2)[0]));
                        start_pln = lacer_pln;

                        DS.Plane rotation_pln = DS.Plane.ByOriginNormalXAxis(pnt, lacer_pln.YAxis, lacer_pln.Normal);                  
                        
                        lacer_pln = (DS.Plane)lacer_pln.Rotate(rotation_pln, (k == 0)? -lacerAngle/2 : -lacerAngle);

                        intersections = 0;
                        DS.Geometry[] intersection = lacer_pln.Intersect(c2);
                        intersections = intersection.Length;
                        
                        if (intersection.Length > 0) lacers.Add(DS.Line.ByStartPointEndPoint(pnt, (DS.Point)lacer_pln.Intersect(c2)[0]));
                        if (intersection.Length == 0 && k % 2 != 0) lacers.RemoveAt(lacers.Count - 1);
                        k++;
                        lacer_planes.Add(lacer_pln);
                        lacer_rot_planes.Add(rotation_pln);
                    }

                    for (int i = 0; i < lacers.Count; i++ )
                    {                     
                        if (i % 2 == 0)
                        {
                            b_params.Add(bottomChord.ParameterAtPoint(lacers[i].StartPoint));
                        }
                        else
                        {
                            t_params.Add(topChord.ParameterAtPoint(lacers[i].StartPoint));
                        }    
                        if (i > 0)
                        {
                            lacer_angles.Add(DS.Vector.ByTwoPoints(lacers[i - 1].StartPoint, lacers[i - 1].EndPoint).AngleBetween(DS.Vector.ByTwoPoints(lacers[i].StartPoint, lacers[i].EndPoint)));
                        }
                      }
                   b_params.Add(bottomChord.ParameterAtPoint(lacers.Last().EndPoint));


               }
            return new Dictionary<string, object>
            {
                {"LacerCentrelines", lacers.ToArray()},
                {"tcParameters", t_params.ToArray()},
                {"bcParameters", b_params.ToArray()},
                {"Bars", str_bars},
                {"LacerAngles", lacer_angles.ToArray()},
                {"LacerPlanes", lacer_planes.ToArray()},
                {"LacerRotPlanes", lacer_rot_planes.ToArray()}
                
            };
        }

        [MultiReturn(new[] { "LacerCentrelines", "tcParameters", "bcParameters", "Bars", "LacerAngles", "LacerPlanes", "LacerRotPlanes" })]
        public static Dictionary<string, object> GoalSeekWarrenByChords(DS.Curve topChord, DS.Curve bottomChord, double lacerAngle = 60, bool equalizeLength = false, bool splitChords = false)
        {
            List<BHoM.Structural.Bar> str_bars = new List<BHoM.Structural.Bar>();
            List<DS.Line> lacers = new List<DS.Line>();
            List<double> t_params = new List<double>();
            List<double> b_params = new List<double>();
            List<double> lacer_angles = new List<double>();
            List<DS.Plane> lacer_planes = new List<DS.Plane>();
            List<DS.Plane> lacer_rot_planes = new List<DS.Plane>();

            //lacerAngle = (lacerAngle < 30) ? 30 : lacerAngle;
            //lacerAngle = (lacerAngle > 90) ? 120 : lacerAngle;

            if (topChord != bottomChord)
            {
                DS.Curve bc = bottomChord;
                //If the chord curves are reversed with respect to one another, flip the top chord for the purpose of generating lacers
                DS.Curve tc = (bc.StartPoint.DistanceTo(topChord.StartPoint) > bc.StartPoint.DistanceTo(topChord.EndPoint)) ? topChord.Reverse() : topChord;

                //Define a plane that all lacers will be aligned to - set by a line between the start points of the top and bottom chord and the bottom chord tangent
                DS.Plane start_pln = DS.Plane.ByOriginNormalXAxis(bc.StartPoint, bc.TangentAtParameter(0), DS.Vector.ByTwoPoints(bc.StartPoint, tc.StartPoint));

                double lbound_distance = 0.5* System.Math.Tan(lacerAngle*(Math.PI/180))*bc.StartPoint.DistanceTo(tc.StartPoint);
                double ubound_distance = 2 * lbound_distance;
                       
                double tolerance = 0.1;
                DS.Vector vector1 = DS.Vector.ByTwoPoints(bc.StartPoint, tc.StartPoint);
                DS.Vector vector2 = DS.Vector.ByTwoPoints(bc.StartPoint, tc.PointAtDistance(lbound_distance));
                
                double angle_diff = vector1.AngleBetween(vector2);
                double dist = 0;
                int kounta = 0;
                while (Math.Abs(angle_diff) > tolerance && angle_diff != 0 && kounta < 20)
                {
                    dist = (ubound_distance + lbound_distance) / 2;
                    vector2 = DS.Vector.ByTwoPoints(bc.StartPoint, tc.PointAtDistance(dist));
                    double angle2 = vector2.AngleBetween(vector1);
                    angle_diff = lacerAngle - angle2;
                    if (angle_diff > 0)
                    {
                        lbound_distance = dist;
                    }
                    else
                    {
                        ubound_distance = dist;
                    }
                    kounta++;
                }
                             
                lacers.Add(DS.Line.ByStartPointEndPoint(bc.StartPoint, tc.PointAtDistance(lbound_distance)));
                lacer_angles.Add(vector1.AngleBetween(vector2));

            }
            return new Dictionary<string, object>
            {
                {"LacerCentrelines", lacers.ToArray()},
                {"tcParameters", t_params.ToArray()},
                {"bcParameters", b_params.ToArray()},
                {"Bars", str_bars},
                {"LacerAngles", lacer_angles.ToArray()},
                {"LacerPlanes", lacer_planes.ToArray()},
                {"LacerRotPlanes", lacer_rot_planes.ToArray()}
                
            };
        }


    }
}