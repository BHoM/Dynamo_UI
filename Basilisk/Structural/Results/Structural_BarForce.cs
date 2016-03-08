using Autodesk.DesignScript.Runtime;
using BHoM.Structural.Loads;
using BHoM.Structural.Results.Bars;
using Autodesk.DesignScript.Interfaces;
using System;

namespace Structural.Results.Bars
{
    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class BarForce : IBarForce, IGraphicItem
    {
        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public int BarNumber { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public int Position { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double RelativePosition { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public int Divisions { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double FX { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double FY { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double FZ { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public Loadcase Loadcase { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public string LoadcaseName { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public int LoadcaseNumber { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double MX { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double MY { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double MZ { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public BHoM.Geometry.Plane OrientationPlane { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double SMax { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double SMin { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public string UserData { get; set; }

        ////////////////////
        ////Constructors////
        ////////////////////

        ///<summary></summary>
        internal BarForce() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barForces"></param>
        /// <param name="barNumber"></param>
        /// <param name="loadcaseNumber"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static BHoM.Structural.Results.Bars.BarForce FromCollection(
            BHoM.Structural.Results.Bars.BarForceCollection barForces, int loadcaseNumber, int barNumber, int position)
        {
            return barForces.TryGetBarForce(loadcaseNumber, barNumber, position);
        }
        

        ////////////////////
        //// Methods    ////
        ////////////////////


        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="parameters"></param>
        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            PushCentrelines(package, OrientationPlane);
        }

        private void PushCentrelines(IRenderPackage package, BHoM.Geometry.Plane plane)
        {
            Autodesk.DesignScript.Geometry.Plane DSplane = Geometry.Plane.ToDSPlane(plane);

            package.AddLineStripVertex(plane.Origin.X, plane.Origin.Y, plane.Origin.Z);
            package.AddLineStripVertexColor(255, 0, 0, 255);
                        
        }

    }
}