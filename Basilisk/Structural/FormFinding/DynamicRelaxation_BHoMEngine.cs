using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using Dynamo.Visualization;
using Dynamo.Graph;

namespace Structural.FormFinding
{
    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class DynamicRelaxation_BHoMEngine : IGraphicItem
    {
        private DynamicRelaxation_BHoMEngine() { }

        //This part is for trying to update dynamo preview each iteration, not working yet
        private List<Line> drawLines = new List<Line>();
        [IsVisibleInDynamoLibrary(false)]
        public static DynamicRelaxation_BHoMEngine Draw(List<Line>relaxedLines)
        {
            DynamicRelaxation_BHoMEngine drawModel = new DynamicRelaxation_BHoMEngine();
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
        [MultiReturn(new[] {"test", "drawmodel", "Lines", "Counter" })]
        [CanUpdatePeriodically(true)]
        public static Dictionary<string, object> RelaxLines(List<Line> lines, List<Point> lockedPoints, List<double> gravity, List<double> barStiffnesses, List<double> lengthMultiplier, double treshold, int maxNoIt, bool run)
        {
            List<BHoM.Structural.Bar> BHoMBars = new List<BHoM.Structural.Bar>();
            List<BHoM.Structural.Node> BHoMNodes = new List<BHoM.Structural.Node>();

            Dictionary<string, object> DR_out = new Dictionary<string, object>();
            Dictionary<string, object> DR_loop = new Dictionary<string, object>();

            List<Line> drawLines = new List<Line>();

            /*if (run)   // TODO - Code does not compile -- please fix
            {
                foreach (Line ln in lines)
                    BHoMBars.Add(new BHoM.Structural.Bar(new BHoM.Geometry.Point(ln.StartPoint.X, ln.StartPoint.Y, ln.StartPoint.Z), new BHoM.Geometry.Point(ln.EndPoint.X, ln.EndPoint.Y, ln.EndPoint.Z)));

                foreach (Point pt in lockedPoints)
                    BHoMNodes.Add(new BHoM.Structural.Node(pt.X, pt.Y, pt.Z));

                Structure structure = BHoM_Engine.FormFinding.DynamicRelaxation.SetStructure(BHoMBars, BHoMNodes, barStiffnesses, lengthMultiplier, treshold);
                DefaultRenderPackageFactory packageFactory = new DefaultRenderPackageFactory();
                IRenderPackage package = packageFactory.CreateRenderPackage();

                int counter = 0;
                DynamicRelaxation_BHoMEngine drawModel = Draw(lines);
                CustomRenderExample test = CustomRenderExample.Create(Point.ByCoordinates(0, 0, 0), Point.ByCoordinates(5, 5, 5));
                DR_out.Add("drawmodel", drawModel);
                DR_out.Add("test", test);

                for (int i = 0; i < maxNoIt; i++)
                {
                    counter += 1;

                    BHoM_Engine.FormFinding.DynamicRelaxation.RelaxStructure(structure, gravity);

                    for (int j = 0; j < structure.Bars.Count; j++)
                        lines[j] = Line.ByStartPointEndPoint(Point.ByCoordinates(structure.Bars[j].StartNode.Point.X, structure.Bars[j].StartNode.Point.Y, structure.Bars[j].StartNode.Point.Z), Point.ByCoordinates(structure.Bars[j].EndNode.Point.X, structure.Bars[j].EndNode.Point.Y, structure.Bars[j].EndNode.Point.Z));

                    //This part is for trying to update dynamo preview each iteration, not working yet
                    drawModel = Draw(lines);
                    test = CustomRenderExample.Create(Point.ByCoordinates(0, 0, 0), Point.ByCoordinates(i, i, i));

                    test.Tessellate(package, packageFactory.TessellationParameters);
                    drawModel.Tessellate(package, packageFactory.TessellationParameters);

                    DR_out["drawmodel"] = drawModel;
                    DR_out["test"] = test;

                    if (structure.HasConverged())
                        break;
                }
                DR_out.Add("Lines", lines);
                DR_out.Add("Counter", counter);
            }*/
            return DR_out;
        }


        //This part is for trying to update dynamo preview each iteration, not working yet
        #region IGraphicItem interface

        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            package.RequiresPerVertexColoration = true;
            foreach (Line ln in drawLines)
                AddColoredLineToPackage(package, ln.StartPoint, ln.EndPoint);
        }

        private static void AddColoredLineToPackage(IRenderPackage package, Point pt1, Point pt2)
        {
            package.AddLineStripVertex(pt1.X, pt1.Y, pt1.Z);
            package.AddLineStripVertex(pt2.X, pt2.Y, pt2.Z);

            package.AddLineStripVertexColor(255, 0, 0, 255);
            package.AddLineStripVertexColor(255, 0, 0, 255);

            // Specify line segments by adding a line vertex count.
            // Ex. The above line has two vertices, so we add a line
            // vertex count of 2. If we had tessellated a curve with n
            // vertices, we would add a line vertex count of n.
            package.AddLineStripVertexCount(2);
        }

        #endregion

    }
}
