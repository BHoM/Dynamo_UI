using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

using BHoM.Structural;

namespace Structural
{
         
    public static class Structure
    {

        /// <summary>
        ///Calculate gamma angles for rotation of beam/column/bar objects
        /// BuroHappold
        /// </summary>
        /// <param name="CLs"></param>
        /// <param name="tol"></param>
        /// <returns></returns>
        /// <search>BH</search>
        public static BHoM.Structural.Structure FromLines(List<Line> CLs, 
            [DefaultArgument("{0.001}")] double tol)
        {
            BHoM.Structural.Structure str = new BHoM.Structural.Structure();
            str.SetTolerance(tol);

            foreach (Line CL in CLs)
            {
                BHoM.Structural.Node n0 = new BHoM.Structural.Node(CL.StartPoint.X, CL.StartPoint.Y, CL.StartPoint.Z);
                BHoM.Structural.Node n1 = new BHoM.Structural.Node(CL.EndPoint.X, CL.EndPoint.Y, CL.EndPoint.Z);
                BHoM.Structural.Bar b = new BHoM.Structural.Bar(n0, n1);

                str.AddBar(b);
            }
            return str;
        }


        /// <summary>
        /// TEST FUNCTION
        /// BuroHappold
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <search>BH</search>
        public static BHoM.Structural.Structure AutoCreateFaces(BHoM.Structural.Structure str)
        {

            str.CreateFacesFromBars();


            return str;


        }

        /// <summary>
        /// BuroHappold
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        /// <search>BH</search>
        [MultiReturn(new[] { "Nodes", "Bars", "Faces"})]
        public static Dictionary<string, object> Deconstruct(BHoM.Structural.Structure structure)
        {
            return new Dictionary<string, object>
            {
                {"Nodes", structure.Nodes},
                {"Bars", structure.Bars},
                {"Faces", structure.Faces},
            };

        }

    }
}