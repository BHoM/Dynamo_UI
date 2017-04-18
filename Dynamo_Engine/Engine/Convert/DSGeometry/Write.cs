using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;


namespace Engine.Convert
{
    public static partial class DSGeometry
    {
        /**********************************************/
        /****  Generic                             ****/
        /**********************************************/

        public static object Write(object geo)
        {

            if (geo is BHG.Line)
            {
                return Write(geo as BHG.Line);
            }
            else if (geo is BHG.Polyline)
            {
                return Write(geo as BHG.Polyline);
            }
            else if (geo is BHG.Point)
            {
                return Write(geo as BHG.Point);
            }
            else if (geo is BHG.Vector)
            {
                return Write(geo as BHG.Vector);
            }
            else if (geo is BHG.BoundingBox)
            {
                return Write(geo as BHG.BoundingBox);
            }


            return null;
        }

        /**********************************************/
        /****  Curves                              ****/
        /**********************************************/

        public static DSG.Line Write(BHG.Line line)
        {
            return DSG.Line.ByStartPointEndPoint(Write(line.StartPoint), Write(line.EndPoint));
        }

        /**********************************************/

        public static DSG.PolyCurve Write(BHG.Polyline polyline)
        {
            List<DSG.Point> points = new List<DSG.Point>();
            foreach (BHG.Point point in polyline.ControlPoints)
                points.Add(Write(point));

            return DSG.PolyCurve.ByPoints(points);
        }

        /**********************************************/
        /****  Points & Vectors                    ****/
        /**********************************************/

        public static DSG.Point Write(BHG.Point pt)
        {
            return DSG.Point.ByCoordinates(pt.X, pt.Y, pt.Z);
        }

        /**********************************************/

        public static DSG.Vector Write(BHG.Vector Vector)
        {
            return DSG.Vector.ByCoordinates(Vector.X, Vector.Y, Vector.Z);
        }

        /**********************************************/
        /****  BoundingBox                         ****/
        /**********************************************/

        public static DSG.BoundingBox Write(BHG.BoundingBox box)
        {
            return DSG.BoundingBox.ByCorners(Write(box.Min), Write(box.Max));
        }

        /**********************************************/
    }

}
