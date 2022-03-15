/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
            if (geometry == null)
                return null;
            else
                return Convert.ToDesignScript(geometry as dynamic);
        }

        /***************************************************/

        public static ADG.Curve IToDesignScript(this BHG.ICurve curve)
        {
            if (curve == null)
                return null;
            else
                return Convert.ToDesignScript(curve as dynamic);
        }

        /***************************************************/

        public static ADG.Surface IToDesignScript(this BHG.ISurface surface)
        {
            if (surface == null)
                return null;
            else
                return Convert.ToDesignScript(surface as dynamic);
        }


        /***************************************************/
        /****         Public Methods  - Vector          ****/
        /***************************************************/

        public static ADG.Point ToDesignScript(this BHG.Point point)
        {
            if (point == null)
                return null;
            else
                return ADG.Point.ByCoordinates(point.X, point.Y, point.Z);
        }

        /***************************************************/

        public static ADG.Vector ToDesignScript(this BHG.Vector vector)
        {
            if (vector == null)
                return null;
            else
                return ADG.Vector.ByCoordinates(vector.X, vector.Y, vector.Z);
        }

        /***************************************************/

        public static ADG.Plane ToDesignScript(this BHG.Plane plane)
        {
            if (plane == null)
                return null;
            else
                return ADG.Plane.ByOriginNormal(plane.Origin?.ToDesignScript(), plane.Normal?.ToDesignScript());
        }

        /***************************************************/

        public static ADG.BoundingBox ToDesignScript(this BHG.BoundingBox boundingBox)
        {
            if (boundingBox == null)
                return null;
            else
                return ADG.BoundingBox.ByCorners(boundingBox.Min?.ToDesignScript(), boundingBox.Max?.ToDesignScript());
        }

        /***************************************************/

        public static ADG.CoordinateSystem ToDesignScript(this BHG.Basis basis)
        {
            if (basis == null)
                return null;
            else
                return ADG.CoordinateSystem.ByOriginVectors(
                ADG.Point.ByCoordinates(0,0,0),
                basis.X?.ToDesignScript(),
                basis.Y?.ToDesignScript(),
                basis.Z?.ToDesignScript()
            );
        }

        /***************************************************/

        public static ADG.CoordinateSystem ToDesignScript(this BHG.CoordinateSystem.Cartesian coordinateSystem)
        {
            if (coordinateSystem == null)
                return null;
            else
                return ADG.CoordinateSystem.ByOriginVectors(coordinateSystem.Origin?.ToDesignScript(), coordinateSystem.X?.ToDesignScript(), coordinateSystem.Y?.ToDesignScript());
        }


        /***************************************************/
        /****         Public Methods  - Curve           ****/
        /***************************************************/

        public static ADG.Line ToDesignScript(this BHG.Line line)
        {
            if (line == null)
                return null;
            else
                return ADG.Line.ByStartPointEndPoint(line.Start?.ToDesignScript(), line.End?.ToDesignScript());
        }

        /***************************************************/

        public static ADG.Arc ToDesignScript(this BHG.Arc arc)
        {
            if (arc == null)
                return null;
            else
                return ADG.Arc.ByThreePoints(arc.StartPoint()?.ToDesignScript(), arc.PointAtParameter(0.5)?.ToDesignScript(), arc.EndPoint()?.ToDesignScript());
        }

        /***************************************************/

        public static ADG.Circle ToDesignScript(this BHG.Circle circle)
        {
            if (circle == null)
                return null;
            else
                return ADG.Circle.ByCenterPointRadiusNormal(circle.Centre?.ToDesignScript(), circle.Radius, circle.Normal?.ToDesignScript());
        }

        /***************************************************/

        public static ADG.Ellipse ToDesignScript(this BHG.Ellipse ellipse)
        {
            if (ellipse == null)
                return null;

            ADG.Point centre = ellipse.Centre?.ToDesignScript();
            ADG.Vector xAxis = (ellipse.Axis1 * ellipse.Radius1)?.ToDesignScript();
            ADG.Vector yAxis = (ellipse.Axis2 * ellipse.Radius2)?.ToDesignScript();
            return ADG.Ellipse.ByOriginVectors(centre, xAxis, yAxis);
        }

        /***************************************************/

        public static ADG.NurbsCurve ToDesignScript(this BHG.NurbsCurve nurbsCurve)
        {
            if (nurbsCurve == null)
                return null;

            List<double> knots = new List<double> { nurbsCurve.Knots.First(), nurbsCurve.Knots.Last() };
            knots.InsertRange(1, nurbsCurve.Knots.ToList());

            return ADG.NurbsCurve.ByControlPointsWeightsKnots(nurbsCurve.ControlPoints.Select(x => x?.ToDesignScript()), nurbsCurve.Weights.ToArray(), knots.ToArray(), nurbsCurve.Degree());
        }

        /***************************************************/

        public static ADG.PolyCurve ToDesignScript(this BHG.Polyline polyline)
        {
            if (polyline == null)
                return null;
            else if (polyline.IsClosed())
                return ADG.Polygon.ByPoints(polyline.ControlPoints.Select(x => x?.ToDesignScript()));
            else
                return ADG.PolyCurve.ByPoints(polyline.ControlPoints.Select(x => x?.ToDesignScript()));
        }

        /***************************************************/

        public static ADG.PolyCurve ToDesignScript(this BHG.PolyCurve polyCurve)
        {
            if (polyCurve == null)
                return null;
            else
                return ADG.PolyCurve.ByJoinedCurves(polyCurve.Curves.Select(x => x?.IToDesignScript()));
        }


        /***************************************************/
        /****         Public Methods  - Surface         ****/
        /***************************************************/

        public static ADG.Surface ToDesignScript(this BHG.NurbsSurface surface)
        {
            if (surface == null)
                return null;

            List<int> uvCount = surface.UVCount(); // Align to Dynamo nurbs definition
            double[][] weights = new double[uvCount[0]][];
            ADG.Point[][] points = new ADG.Point[uvCount[0]][];

            List<double> uKnots = new List<double>(surface.UKnots);
            uKnots.Insert(0, uKnots.First());
            uKnots.Add(uKnots.Last());

            List<double> vKnots = new List<double>(surface.VKnots);
            vKnots.Insert(0, vKnots.First());
            vKnots.Add(vKnots.Last());

            for (int i = 0; i < uvCount[0]; i++)
            {
                points[i] = new ADG.Point[uvCount[1]];
                weights[i] = new double[uvCount[1]];
                for (int j = 0; j < uvCount[1]; j++)
                {
                    points[i][j] = surface.ControlPoints[j + (uvCount[1] * i)]?.ToDesignScript();
                    weights[i][j] = surface.Weights[j + (uvCount[1] * i)];
                }
            }

            ADG.Surface ADGSurface = ADG.NurbsSurface.ByControlPointsWeightsKnots(points, weights,
                uKnots.ToArray(), vKnots.ToArray(), surface.UDegree, surface.VDegree);

            try
            {
                List<ADG.PolyCurve> trims = surface.OuterTrims.Select(x => (x?.Curve3d as BHG.PolyCurve)?.ToDesignScript()).ToList();
                trims.AddRange(surface.InnerTrims.Select(x => (x?.Curve3d as BHG.PolyCurve)?.ToDesignScript()).ToList());
                ADGSurface = ADGSurface.TrimWithEdgeLoops(trims);
            }
            catch
            {
                Base.Compute.RecordWarning("Surface trim failed. Untrimmed surface has been returned instead.");
            }

            return ADGSurface;
        }

        /***************************************************/

        public static ADG.Surface ToDesignScript(this BHG.PlanarSurface surface)
        {
            if (surface == null)
                return null;
            else if (surface.InternalBoundaries.Count != 0)
            {
                BH.Engine.Base.Compute.RecordError("Dynamo does not support surfaces with openings, convert failed.");
                return null;
            }
            else
                return ADG.Surface.ByPatch(surface.ExternalBoundary?.IToDesignScript());
        }

        /***************************************************/

        public static ADG.PolySurface ToDesignScript(this BHG.PolySurface surface)
        {
            if (surface == null)
                return null;
            else
                return ADG.PolySurface.ByJoinedSurfaces(surface.Surfaces.Select(x => x?.IToDesignScript()));
        }


        /***************************************************/
        /****          Public Methods  - Mesh           ****/
        /***************************************************/

        public static ADG.Mesh ToDesignScript(this BHG.Mesh mesh)
        {
            if (mesh == null)
                return null;
            
            List<ADG.IndexGroup> faceIndexes = new List<ADG.IndexGroup>();
            IEnumerable<ADG.Point> vertices = mesh.Vertices.Select(x => x?.ToDesignScript());

            foreach (BHG.Face f in mesh.Faces.Where(x => x != null))
            {
                if (f.IsQuad())
                    faceIndexes.Add(ADG.IndexGroup.ByIndices((uint)f.A, (uint)f.B, (uint)f.C, (uint)f.D));
                else
                    faceIndexes.Add(ADG.IndexGroup.ByIndices((uint)f.A, (uint)f.B, (uint)f.C));
            }

            return ADG.Mesh.ByPointsFaceIndices(vertices, faceIndexes);
        }

        /***************************************************/
    }
}



