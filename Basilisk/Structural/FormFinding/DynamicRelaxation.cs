using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using DynamicRelaxationToolkit;
using Dynamo.Visualization;

namespace Structural.FormFinding
{
    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class DynamicRelaxation : IGraphicItem
    {
        private DynamicRelaxation() { }

        //This part is for trying to update dynamo preview each iteration, not working yet
        private List<Line> drawLines = new List<Line>();
        [IsVisibleInDynamoLibrary(false)]
        public static DynamicRelaxation Draw(List<Line>relaxedLines)
        {
            DynamicRelaxation drawModel = new DynamicRelaxation();
            drawModel.drawLines = relaxedLines;           
            return drawModel;
        }

        /// <summary>
        /// Performs dynamic relaxation on a structure of lines
        /// BuroHappold
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="lockedPoints"></param>
        /// <param name="gravity"></param>
        /// <param name="barStiffnesses"></param>
        /// <param name="lengthMultiplier"></param>
        /// <param name="treshold"></param>
        /// <param name="maxNoIt"></param>
        /// <param name="run"></param>
        /// <returns></returns>
        /// <search>BH, dynamic relaxation, formfind, relax</search>
        /// 
        [MultiReturn(new[] { "Lines", "Counter" })]
        [CanUpdatePeriodically(true)]
        public static Dictionary<string, object> RelaxLines(List<Line> lines, List<Point> lockedPoints, List<double> gravity, List<double> barStiffnesses, List<double> lengthMultiplier, double treshold, int maxNoIt, bool run)
        {
            List<BHoM.Geometry.Line> BHoMLines = new List<BHoM.Geometry.Line>();
            List<BHoM.Geometry.Point> BHoMPoints = new List<BHoM.Geometry.Point>();

            Dictionary<string, object> DR_out = new Dictionary<string, object>();
            Dictionary<string, object> DR_loop = new Dictionary<string, object>();

            List<Line> drawLines = new List<Line>();

            if (run)
            {
                foreach (Line ln in lines)
                    BHoMLines.Add(new BHoM.Geometry.Line(new BHoM.Geometry.Point(ln.StartPoint.X, ln.StartPoint.Y, ln.StartPoint.Z), new BHoM.Geometry.Point(ln.EndPoint.X, ln.EndPoint.Y, ln.EndPoint.Z)));

                foreach (Point pt in lockedPoints)
                    BHoMPoints.Add(new BHoM.Geometry.Point(pt.X, pt.Y, pt.Z));

                Structure structure = DynamicRelaxationToolkit.DynamicRelaxation.SetStructure(BHoMLines, BHoMPoints, barStiffnesses, lengthMultiplier, treshold);

                int counter = 0; 

                for (int i = 0; i < maxNoIt; i++)
                {
                    counter += 1;

                    DynamicRelaxationToolkit.DynamicRelaxation.RelaxStructure(structure, gravity);

                    for (int j = 0; j < structure.Bars.Count; j++)
                        lines[j] = Line.ByStartPointEndPoint(Point.ByCoordinates(structure.Bars[j].StartNode.NodePt.X, structure.Bars[j].StartNode.NodePt.Y, structure.Bars[j].StartNode.NodePt.Z), Point.ByCoordinates(structure.Bars[j].EndNode.NodePt.X, structure.Bars[j].EndNode.NodePt.Y, structure.Bars[j].EndNode.NodePt.Z));

                    //This part is for trying to update dynamo preview each iteration, not working yet
                    IGraphicItem drawModel = Draw(lines);
                    DefaultRenderPackageFactory packageFactory = new DefaultRenderPackageFactory(); 
                    IRenderPackage package = packageFactory.CreateRenderPackage();                  
                    drawModel.Tessellate(package, packageFactory.TessellationParameters);


                    if (structure.HasConverged())
                        break;
                }
                DR_out.Add("Lines", lines);
                DR_out.Add("Counter", counter);
            }
            return DR_out;
        }


        //This part is for trying to update dynamo preview each iteration, not working yet
        #region IGraphicItem interface

        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            package.RequiresPerVertexColoration = true;
            foreach (Line ln in drawLines)
            {
                package.AddLineStripVertex(ln.StartPoint.X, ln.StartPoint.Y, ln.StartPoint.Z);
                package.AddLineStripVertex(ln.EndPoint.X, ln.EndPoint.Y, ln.EndPoint.Z);
                package.AddLineStripVertexColor(255, 0, 0, 0);
                package.AddLineStripVertexCount(2);
            }

        }

        #endregion

    }
}
