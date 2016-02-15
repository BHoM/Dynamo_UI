using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Revit.Elements;
using RevitServices.Transactions;

using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

using RevitServices.Persistence;

using Autodesk.DesignScript.Geometry;

namespace Basilisk.Structural
{
    public static class Rakers
    {
        public static bool ComparePoints(Point pt1, Point pt2)
        {
            bool ret = false;
            if (Math.Abs(pt1.X - pt2.X) < 0.01 && Math.Abs(pt1.Y - pt2.Y) < 0.01 && Math.Abs(pt1.Z - pt2.Z) < 0.01)
            {
                ret = true;
            }
            return ret;
        }

        public static List<Curve> RemoveVerticals(List<Curve> lns)
        {
            List<Curve> outLine = new List<Curve>();
            Vector tempVec;
            Curve tempLn;
            bool check;
            for (int i = 0; i < lns.Count; i++)
            {
                check = true;
                tempLn = lns[i];
                tempVec = Vector.ByTwoPoints(tempLn.StartPoint, tempLn.EndPoint);
                tempVec.Normalized();

                if (Math.Abs(tempVec.X) < 0.001 && Math.Abs(tempVec.Y) < 0.001)
                {
                    check = false;
                }

                if (check)
                {
                    outLine.Add(tempLn);
                }
            }

            return outLine;
        }

        public static List<Curve> RemoveShortLines(List<Curve> lns, double length)
        {
            List<Curve> outLine = new List<Curve>();
            Curve tempLn;
            double tempLength;

            for (int i = 0; i < lns.Count; i++)
            {
                tempLn = lns[i];
                tempLength = tempLn.Length;

                if (tempLength > length)
                {
                    outLine.Add(tempLn);
                }
            }


            return outLine;
        }

        public static Curve RemoveShortLines2(Curve ln, double length)
        {
            if (ln.Length > length)
                return ln;
            else
                return null;
        }

        public static Point GetMidPoint(Curve ln)
        {
            Point midPt;
            Curve[] tempSeg;

            tempSeg = ln.DivideEqually(2);

            if (ComparePoints(tempSeg[0].StartPoint, tempSeg[1].EndPoint) || (ComparePoints(tempSeg[0].StartPoint, tempSeg[1].StartPoint)))
            {
                midPt = tempSeg[0].StartPoint;
            }
            else
            {
                midPt = tempSeg[0].EndPoint;
            }


            return midPt;
        }

        public static List<Curve> RemoveTopLines(List<Curve> lns)
        {
            List<Curve> outLine = new List<Curve>();
            List<Curve[]> pair = new List<Curve[]>();
            Point tempMidA;
            Point tempMidB;
            double dist = 0;
            double tempDist = 0;
            int tempIndex = 0;
            List<int> index = new List<int>();

            for (int i = 0; i < lns.Count; i++)
            {
                int counter = 0;
                Curve[] tempPair = new Curve[2];
                tempMidA = GetMidPoint(lns[i]);
                if (!index.Contains(i))
                {
                    for (int j = 0; j < lns.Count; j++)
                    {
                        tempMidB = GetMidPoint(lns[j]);
                        tempDist = tempMidA.DistanceTo(tempMidB);

                        if ((tempDist < dist || counter == 0) && i != j)
                        {
                            dist = tempDist;
                            tempIndex = j;
                            counter = 1;
                        }
                    }
                    if (!index.Contains(tempIndex))
                    {
                        tempPair[0] = lns[i];
                        tempPair[1] = lns[tempIndex];
                        pair.Add(tempPair);
                        index.Add(tempIndex);
                    }
                }
            }

            double zValA = 0;
            double zValB = 0;

            for (int i = 0; i < pair.Count; i++)
            {
                zValA = pair[i][0].StartPoint.Z;
                zValB = pair[i][1].StartPoint.Z;

                if (zValA < zValB)
                {
                    outLine.Add(pair[i][0]);
                }

                else
                {
                    outLine.Add(pair[i][1]);
                }
            }



            return outLine;
        }

        public static List<Point> GetPoints(List<Curve> lns, Point centerPt)
        {
            List<Point> outPts = new List<Point>();
            double distA = 0;
            double distB = 0;

            for (int i = 0; i < lns.Count; i++)
            {
                distA = centerPt.DistanceTo(lns[i].StartPoint);
                distB = centerPt.DistanceTo(lns[i].EndPoint);

                if (distA > distB)
                {
                    outPts.Add(lns[i].StartPoint);
                }

                else
                {
                    outPts.Add(lns[i].EndPoint);
                }
            }

            return outPts;
        }

        public static List<Point> ReorderPoints(List<Point> pts)
        {
            List<Point> outPts = new List<Point>();
            List<int> indexes = new List<int>();
            double[] zValues = new double[pts.Count];

            for (int i = 0; i < pts.Count; i++)
            {
                zValues[i] = pts[i].Z;
            }

            Array.Sort(zValues);

            for (int i = 0; i < zValues.Length; i++)
            {
                for (int j = 0; j < pts.Count; j++)
                {
                    if (zValues[i] == pts[j].Z && !indexes.Contains(j))
                    {
                        indexes.Add(j);
                    }
                }
            }

            for (int i = 0; i < indexes.Count; i++)
            {
                outPts.Add(pts[indexes[i]]);
            }

            return outPts;
        }

        public static List<List<Point>> RemoveShortLists(List<List<Point>> pts, int listLength)
        {
            List<List<Point>> retList = new List<List<Point>>();

            for (int i = 0; i < pts.Count; i++)
            {
                if (pts[i].Count > listLength)
                {
                    retList.Add(pts[i]);
                }
            }


            return retList;
        }


        public static double shortestDistance(List<Point> originalPts, List<Point> closestPts)
        {
            double retDist;
            double tempdist = 0;
            double dist = 0;

            for (int i = 0; i < originalPts.Count; i++)
            {
                if (originalPts[i].Z < closestPts[i].Z)
                {
                    tempdist = originalPts[i].DistanceTo(closestPts[i]);

                    if (tempdist > dist)
                    {
                        dist = tempdist;
                    }
                }
            }

            retDist = dist;

            return retDist;
        }
    }
}