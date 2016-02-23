using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using BHoM.Geometry;

namespace Geometry
{
    /// <summary>
    /// BHoM Vector object
    /// </summary>
    public class Vector : IVector
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
        public void ToDSVector()
        {
            Autodesk.DesignScript.Geometry.Vector.ByCoordinates(this.X, this.Y, this.Z);
        }
    }
}