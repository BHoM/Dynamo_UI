using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

namespace Structural
{
    /// <summary>
    /// Face object
    /// </summary>
    public class Face
        
    {
        internal Face(){}
        /// <summary>
        /// Deconstruct face
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Corners", "Number", "Name" })]
        public static Dictionary<string, object> Deconstruct(BHoM.Structural.Face face)
        {
            List<Autodesk.DesignScript.Geometry.Point> pts = new List<Point>(4);
            foreach (BHoM.Structural.Node n in face.Nodes)
                pts.Add(Autodesk.DesignScript.Geometry.Point.ByCoordinates(n.X, n.Y, n.Z));

            return new Dictionary<string, object>
            {
                {"Corners", pts},
                {"Number", face.Number},
                {"Name", face.Name},
            };
        }

    }
}
