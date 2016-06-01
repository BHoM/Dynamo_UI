using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Interfaces;
using DSCore;

namespace Structural
{
    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class Preview : IGraphicItem
    {
        internal Preview() { }
        internal BHoM.Structural.Bar Bar;
        internal bool Analytical;
        internal bool Meshes;
        internal DSCore.Color Color = DSCore.Color.ByARGB(255,0,0,0);
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="color"></param>
        public static Preview Centrelines(dynamic obj,  [DefaultArgument("\"Unassigned\"")] object color)
        {
            Preview preview = new Preview();
            if(obj.GetType() == typeof(BHoM.Structural.Bar)) preview.Bar = obj;
            preview.Analytical = true;
            preview.Meshes = false;
            try { preview.Color = (DSCore.Color)color; } catch { }
            return preview;
        }


        /// <summary>
        /// Tessellation method
        /// </summary>
        /// <param name="package"></param>
        /// <param name="parameters"></param>
        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
                PushCentrelines(package, Bar);
        }

        private void PushCentrelines(IRenderPackage package, BHoM.Structural.Bar bar)
        {
            if (this.Analytical == true)
            {
                package.AddLineStripVertex(bar.StartNode.Point.X, bar.StartNode.Y, bar.StartNode.Z);
                package.AddLineStripVertexColor(Color.Red, Color.Green, Color.Blue, Color.Alpha);
                package.AddLineStripVertex(bar.EndNode.Point.X, bar.EndNode.Y, bar.EndNode.Z);
                package.AddLineStripVertexColor(Color.Red, Color.Green, Color.Blue, Color.Alpha);
                package.AddPointVertex(bar.StartNode.Point.X, bar.StartNode.Y, bar.StartNode.Z);
                package.AddPointVertexColor(Color.Red, Color.Green, Color.Blue, Color.Alpha);
                package.AddPointVertex(bar.EndNode.Point.X, bar.EndNode.Y, bar.EndNode.Z);
                package.AddPointVertexColor(Color.Red, Color.Green, Color.Blue, Color.Alpha);
            }

            if (this.Meshes == true)
            {
                if (bar.SectionProperty.ShapeType == BHoM.Structural.Sections.ShapeType.SteelI)
                {
                  
                }
            }
        }
    }

    
}