using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using BHoM.Geometry;

namespace Geometry
{
    /// <summary>
    /// BHoM Point object
    /// </summary>
    public class Point : IPoint
    {
        /// <summary>X coordinate</summary>
        [IsVisibleInDynamoLibrary(false)]
        public double X { get; set; }

        /// <summary>Y coordinate</summary>
        [IsVisibleInDynamoLibrary(false)]
        public double Y { get; set; }

        /// <summary>Z coordinate</summary>
        [IsVisibleInDynamoLibrary(false)]
        public double Z { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public void ToDSPoint()
        {
            Autodesk.DesignScript.Geometry.Point.ByCoordinates(this.X, this.Y, this.Z);
        }
    }
}