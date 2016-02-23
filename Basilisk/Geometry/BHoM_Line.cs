using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using BHoM.Geometry;

namespace Geometry
{
    /// <summary>
    /// BHoM Line object
    /// </summary>
    public class Line : ILine
    {        
        internal Line(){}

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public BHoM.Geometry.Point EndPoint  { get; set;}

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public double Length { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public BHoM.Geometry.Point StartPoint { get; set; }

        /// <summary></summary>
        [IsVisibleInDynamoLibrary(false)]
        public void ToDSLine()
        {
            Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(
                Autodesk.DesignScript.Geometry.Point.ByCoordinates(this.StartPoint.X, this.StartPoint.Y, this.StartPoint.Z),
                Autodesk.DesignScript.Geometry.Point.ByCoordinates(this.EndPoint.X, this.EndPoint.Y, this.EndPoint.Z));
        }
    }
}