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

        public static object Read(object geo)
        {

            if (geo is DSG.Line)
            {
                return Read((DSG.Line)geo);
            }
            else if (geo is DSG.NurbsCurve)
            {
                return Read((DSG.NurbsCurve)geo);
            }
            else if (geo is DSG.Point)
            {
                return Read((DSG.Point)geo);
            }
            else if (geo is DSG.Vector)
            {
                return Read((DSG.Vector)geo);
            }
            else if (geo is DSG.BoundingBox)
            {
                return Read((DSG.BoundingBox)geo);
            }

            return null;
        }


        /**********************************************/
        /****  Curves                              ****/
        /**********************************************/

        public static BHG.Line Read(DSG.Line line)
        {
            return new BHG.Line(new BHG.Point(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z), new BHG.Point(line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z));
        }

        /**********************************************/

        public static BHG.Polyline Read(DSG.NurbsCurve polyline)
        {
            List<DSG.Point> DSPoints = polyline.ControlPoints().ToList();
            List<BHG.Point> points = new List<BHG.Point>();
            foreach (DSG.Point point in DSPoints)
                points.Add(new BHG.Point(point.X, point.Y, point.Z));
            return new BHG.Polyline(points);
        }

        /**********************************************/
        /****  Points & Vectors                    ****/
        /**********************************************/

        public static BHG.Point Read(DSG.Point pt)
        {
            return new BHG.Point(pt.X, pt.Y, pt.Z);
        }

        /**********************************************/

        public static BHG.Vector Read(DSG.Vector Vector)
        {
            return new BHG.Vector(Vector.X, Vector.Y, Vector.Z);
        }

        /**********************************************/
        /****  BoundingBox                         ****/
        /**********************************************/

        public static BHG.BoundingBox Read(DSG.BoundingBox box)
        {
            return new BHG.BoundingBox(Read(box.MinPoint), Read(box.MaxPoint));
        }

        /**********************************************/
    }

}
