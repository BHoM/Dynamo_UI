using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Graph.Nodes;
using RSE = Revit2018_Adapter;
using RSE_Structural = Revit2018_Adapter.Structural;
using BHoME = BHoM.Structural.Elements;
using BHoMB = BHoM.Base;
using Autodesk.DesignScript.Geometry;
using BHoMArch = BHoM.Architectural.Elements;


namespace Revit2018.Elements
{
    /// <summary>
    /// </summary>
    public static class Grid
    {
        /// <summary>
        /// </summary>
        public static object ToBHomGrid(List<Revit.Elements.Grid> grids)
        {
            BHoMB.ObjectManager<BHoMArch.Grid> bhGrids = new BHoMB.ObjectManager<BHoMArch.Grid>();
            //List<BHoME.Grid> bhGrids = new List<BHoME.Grid>();
            foreach (Revit.Elements.Grid grid in grids)
            {
                //List<BHoM.Geometry.Curve> bhCurves = new List<BHoM.Geometry.Curve>();
                //bhCurves.Add(Geometry.GeometryUtils.Convert(grid.Curve, 9));
                BHoM.Geometry.Point startpoint = new BHoM.Geometry.Point(grid.Curve.StartPoint.X, grid.Curve.StartPoint.Y, grid.Curve.StartPoint.Z);
                BHoM.Geometry.Point endpoint = new BHoM.Geometry.Point(grid.Curve.EndPoint.X, grid.Curve.EndPoint.Y, grid.Curve.EndPoint.Z);

                bhGrids.Add(grid.Name, new BHoMArch.Grid(new BHoM.Geometry.Line(startpoint,endpoint)));
            }
            return bhGrids;
        }
    }
}


