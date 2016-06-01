using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using Dynamo.Visualization;

namespace Structural.FormFinding
{
    public class CustomRenderExample : IGraphicItem
    {
        private CustomRenderExample() { }

        private Point pt1;
        private Point pt2;
        private Point pt3;

        /// <summary>
        /// Create an object which renders custom geometry.
        /// </summary>
        public static CustomRenderExample Create(Point pt1, Point pt2)
        {
            CustomRenderExample ex = new CustomRenderExample();
            ex.pt1 = pt1;
            ex.pt2 = pt2;
            return ex;
        }

        public static CustomRenderExample CustomRenderTest()
        {
            Point pt1 = Point.ByCoordinates(0, 0, 0);
            Point pt2 = Point.ByCoordinates(5, 5, 5);

            CustomRenderExample test = CustomRenderExample.Create(pt1, pt2);

            DefaultRenderPackageFactory packageFactory = new DefaultRenderPackageFactory();
            IRenderPackage package = packageFactory.CreateRenderPackage();

            test.Tessellate(package, packageFactory.TessellationParameters);

            return test;
        }

        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            // Dynamo's renderer uses IRenderPackage objects
            // to store data for rendering. The Tessellate method
            // give you an IRenderPackage object which you can fill
            // with render data.

            // Set RequiresPerVertexColoration to let the renderer
            // know that you needs to use a per-vertex color shader.
            package.RequiresPerVertexColoration = true;

         //   AddColoredQuadToPackage(package, pt1, pt2, pt3);
              AddColoredLineToPackage(package, pt1, pt2);
        }

        private static void AddColoredQuadToPackage(IRenderPackage package, Point pt1, Point pt2, Point pt3)
        {
            // Triangle 1
            package.AddTriangleVertex(pt1.X, pt1.Y, pt1.Z);
            package.AddTriangleVertex(pt2.X, pt2.Y, pt2.Z);
            package.AddTriangleVertex(pt3.X, pt3.Y, pt3.Z);

            // For each vertex, add a color.
            package.AddTriangleVertexColor(255, 0, 0, 255);
            package.AddTriangleVertexColor(0, 255, 0, 255);
            package.AddTriangleVertexColor(0, 0, 255, 255);

            //Triangle 2
            package.AddTriangleVertex(0, 0, 0);
            package.AddTriangleVertex(1, 1, 0);
            package.AddTriangleVertex(0, 1, 0);
            package.AddTriangleVertexColor(255, 0, 0, 255);
            package.AddTriangleVertexColor(0, 255, 0, 255);
            package.AddTriangleVertexColor(0, 0, 255, 255);

            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);
            package.AddTriangleVertexNormal(0, 0, 1);

            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
            package.AddTriangleVertexUV(0, 0);
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
    }
}
