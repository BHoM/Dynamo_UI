/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;
using BH.Engine.Geometry;

namespace BH.Engine.Dynamo
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static object IToDesignScript(this object obj)
        {
            if (obj is BHG.IGeometry)
                return Convert.ToDesignScript(obj as dynamic);
            else
                return obj;
        }

        /***************************************************/

        public static ADG.Geometry IToDesignScript(this BHG.IGeometry geometry)
        {
            return Convert.ToDesignScript(geometry as dynamic);
        }


        /***************************************************/
        /**** Public Methods - Geometry                 ****/
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
            return ADG.Arc.ByThreePoints(arc.StartPoint().ToDesignScript(), arc.PointAtParameter(0.5).ToDesignScript(), arc.EndPoint().ToDesignScript());
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

        public static ADG.NurbsCurve ToDesignScript(this BHG.NurbsCurve nurbsCurve)
        {

            List<double> knots = new List<double> { 0, nurbsCurve.Knots.Last() };
            knots.InsertRange(1, nurbsCurve.Knots.ToList());          
          
            return ADG.NurbsCurve.ByControlPointsWeightsKnots(nurbsCurve.ControlPoints.Select(x => x.ToDesignScript()), nurbsCurve.Weights.ToArray(), knots.ToArray(), nurbsCurve.Degree());
        }

        /***************************************************/

        public static ADG.Plane ToDesignScript(this BHG.Plane plane)
        {
            return ADG.Plane.ByOriginNormal(plane.Origin.ToDesignScript(), plane.Normal.ToDesignScript());
        }

        /***************************************************/

        public static ADG.CoordinateSystem ToDesignScript(this BHG.CoordinateSystem.Cartesian coordinateSystem)
        {
            return ADG.CoordinateSystem.ByOriginVectors(coordinateSystem.Origin.ToDesignScript(), coordinateSystem.X.ToDesignScript(), coordinateSystem.Y.ToDesignScript());
        }

        /***************************************************/

        public static ADG.PolyCurve ToDesignScript(this BHG.Polyline polyLine)
        {
            return ADG.PolyCurve.ByPoints(polyLine.ControlPoints.Select(x => x.ToDesignScript()));
        }

        /***************************************************/

        public static ADG.PolyCurve ToDesignScript(this BHG.PolyCurve polyCurve)
        {
            List<ADG.PolyCurve> aPolyCurveList = new List<ADG.PolyCurve>();
            foreach (BHG.ICurve ICurve in polyCurve.Curves)
            {
                if (ICurve is BHG.PolyCurve)
                    aPolyCurveList.Add(((BHG.PolyCurve)ICurve).ToDesignScript());
                else
                    aPolyCurveList.Add(ADG.PolyCurve.ByJoinedCurves(new ADG.Curve[] { ToDesignScript((ICurve as dynamic)) }));
            }

            return ADG.PolyCurve.ByJoinedCurves(aPolyCurveList);
        }

        /***************************************************/

        public static ADG.BoundingBox ToDesignScript(this BHG.BoundingBox boundingBox)
        {
            return ADG.BoundingBox.ByCorners(boundingBox.Min.ToDesignScript(), boundingBox.Max.ToDesignScript());
        }

        /***************************************************/

        public static ADG.Surface ToDesignScript(this BHG.NurbsSurface surface)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static ADG.Mesh ToDesignScript(this BHG.Mesh mesh)
        {
            List<ADG.IndexGroup> faceIndexes = new List<ADG.IndexGroup>();
            IEnumerable<ADG.Point> vertices = mesh.Vertices.Select(x => x.ToDesignScript());

            foreach (BHG.Face f in mesh.Faces)
            {
                if (f.IsQuad())
                    faceIndexes.Add(ADG.IndexGroup.ByIndices((uint)f.A, (uint)f.B, (uint)f.C, (uint)f.D));
                else
                    faceIndexes.Add(ADG.IndexGroup.ByIndices((uint)f.A, (uint)f.B, (uint)f.C));
            }

            return ADG.Mesh.ByPointsFaceIndices(vertices, faceIndexes);
        }

        /***************************************************/

        //TODO: implement proper conversion for PlanarSurfce
        public static ADG.Geometry ToDesignScript(this BHG.PlanarSurface planarSurface)
        {
            return planarSurface.ExternalBoundary.IToDesignScript();
        }

        /***************************************************/
    }
}
