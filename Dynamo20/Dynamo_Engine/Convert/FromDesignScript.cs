/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using Autodesk.DesignScript.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;


namespace BH.Engine.Dynamo
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods  - Interfaces              ****/
        /***************************************************/

        public static object IFromDesignScript(this object obj)
        {
            return Convert.FromDesignScript(obj as dynamic);
        }

        /***************************************************/

        public static BHG.IGeometry IFromDesignScript(this ADG.Geometry geometry)
        {
            return Convert.FromDesignScript(geometry as dynamic);
        }


        /***************************************************/
        /**** Public Methods  - Collections             ****/
        /***************************************************/

        public static IEnumerable FromDesignScript(this IEnumerable list)
        {
            if (list is string || list.GetType().Name.StartsWith("Dictionary"))
                return list;
            else
            {
                List<object> newList = new List<object>();
                foreach (object item in list)
                    newList.Add(item.IFromDesignScript());
                return newList;
            }
        }


        /***************************************************/

        public static Dictionary<string, object> FromDesignScript(this DesignScript.Builtin.Dictionary dic)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string key in dic.Keys)
                result[key] = dic.ValueAtKey(key);

            return result;
        }

        /***************************************************/

        // Issue with natvie Dynamo nodes to be confused (Issue 83)
        //public static IEnumerable<object> FromDesignScript(this ArrayList list)
        //{
        //    return list.ToArray().Select(x => x.IFromDesignScript()).ToList();
        //}


        /***************************************************/
        /**** Public Methods  - Fallback                ****/
        /***************************************************/

        public static object FromDesignScript(this object obj)
        {
            return obj;
        }

        /***************************************************/
        /**** Public Methods  - Geometry                ****/
        /***************************************************/

        public static BHG.Point FromDesignScript(this ADG.Point designScriptPt)
        {
            return Geometry.Create.Point(designScriptPt.X, designScriptPt.Y, designScriptPt.Z);
        }

        /***************************************************/

        public static BHG.Vector FromDesignScript(this ADG.Vector designScriptVec)
        {
            return Geometry.Create.Vector(designScriptVec.X, designScriptVec.Y, designScriptVec.Z);
        }

        /***************************************************/

        public static BHG.Arc FromDesignScript(this ADG.Arc arc)
        {
            return Geometry.Create.Arc(arc.StartPoint.FromDesignScript(), arc.PointAtParameter(0.5).FromDesignScript(), arc.EndPoint.FromDesignScript());
        }

        /***************************************************/

        public static BHG.Circle FromDesignScript(this ADG.Circle circle)
        {
            return Geometry.Create.Circle(circle.CenterPoint.FromDesignScript(), circle.Normal.FromDesignScript(), circle.Radius);
        }

        /***************************************************/

        public static BHG.Line FromDesignScript(this ADG.Line line)
        {
            return Geometry.Create.Line(line.StartPoint.FromDesignScript(), line.EndPoint.FromDesignScript());
        }

        /***************************************************/

        public static BHG.NurbsCurve FromDesignScript(this ADG.Curve nurbsCurve)
        {
            throw new NotImplementedException();
        }

        /***************************************************/

        public static BHG.NurbsCurve FromDesignScript(this ADG.NurbsCurve nurbsCurve)
        {
            return new BHG.NurbsCurve
            {
                ControlPoints = nurbsCurve.ControlPoints().Select(x => x.FromDesignScript()).ToList(),
                Knots = nurbsCurve.Knots().ToList().GetRange(1, nurbsCurve.Knots().Count()-2),
                Weights = nurbsCurve.Weights().ToList()
            };
        }

        /***************************************************/

        public static BHG.Plane FromDesignScript(this ADG.Plane plane)
        {
            return Geometry.Create.Plane(plane.Origin.FromDesignScript(), plane.Normal.FromDesignScript());
        }

        /***************************************************/

        public static BHG.CoordinateSystem.Cartesian FromDesignScript(this ADG.CoordinateSystem coordinateSystem)
        {
            return Geometry.Create.CartesianCoordinateSystem(coordinateSystem.Origin.FromDesignScript(), coordinateSystem.XAxis.FromDesignScript(), coordinateSystem.YAxis.FromDesignScript());
        }

        /***************************************************/

        public static BHG.PolyCurve FromDesignScript(this ADG.PolyCurve polyCurve)
        {
            return Geometry.Create.PolyCurve(polyCurve.Curves().Select(x => x.FromDesignScript()));
        }

        /***************************************************/

        public static BHG.Polyline FromDesignScript(this ADG.Polygon polygon)
        {
            List<BH.oM.Geometry.Point> pts = polygon.Points.Select(x => x.FromDesignScript()).ToList();
            if (pts.Count == 0)
                return new BHG.Polyline();

            pts.Add(pts[0]);
            return Geometry.Create.Polyline(pts);
        }

        /***************************************************/

        public static BHG.BoundingBox FromDesignScript(this ADG.BoundingBox boundingBox)
        {
            return Geometry.Create.BoundingBox(boundingBox.MinPoint.FromDesignScript(), boundingBox.MaxPoint.FromDesignScript());
        }

        /***************************************************/

        public static BHG.ISurface FromDesignScript(this ADG.Surface surface)
        {
            return FromDesignScript(surface as dynamic);
        }

        /***************************************************/

        public static BHG.NurbsSurface FromDesignScript(this ADG.NurbsSurface surface)
        {
            List<double> uKnots = new List<double>(surface.UKnots());
            uKnots.RemoveAt(0);
            uKnots.RemoveAt(uKnots.Count - 1);

            List<double> vKnots = new List<double>(surface.VKnots());
            vKnots.RemoveAt(0);
            vKnots.RemoveAt(vKnots.Count - 1);

            return surface == null ? null : new BHG.NurbsSurface
            (
                surface.ControlPoints().SelectMany(x => x.Select(y => y.FromDesignScript())).ToList(),
                surface.Weights().SelectMany(x => x).ToList(),
                uKnots,
                vKnots,
                surface.DegreeU,
                surface.DegreeV,
                new List<BHG.SurfaceTrim>(),
                new List<BHG.SurfaceTrim>()
            );
        }

        /***************************************************/

        public static BHG.Mesh FromDesignScript(this ADG.Mesh dSMesh)
        {
            List<BHG.Point> vertices = dSMesh.VertexPositions.ToList().Select(x => x.FromDesignScript()).ToList();
            List<ADG.IndexGroup> DSFacesIndex = dSMesh.FaceIndices.ToList();
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

