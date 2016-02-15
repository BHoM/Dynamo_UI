using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace Structural
{
    /// <summary>
    /// Structure class for constructing a structure object consisting of child structure objects and dictionaries
    /// </summary>
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
                n0 = str.AddOrGetNode(n0);
                n1 = str.AddOrGetNode(n1);

                str.AddBar(n0, n1);
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
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <search>BH</search>
        [AllowRankReduction()]
        [MultiReturn(new[] { "Names", "Properties"})]
        public static Dictionary<string, object> Deconstruct(dynamic obj)
        {
            Dictionary<string, object> out_dict = new Dictionary<string, object>();
            try
            {
                BHoM.Collections.Dictionary<string, object> PropertiesDictionary = obj.GetProperties();
              
                out_dict.Add("Names", obj.GetType());
                out_dict.Add("Properties", PropertiesDictionary);
            }
            catch
            {
                out_dict.Add("Names", null);
                out_dict.Add("Properties", null);
            }

            return out_dict;
        }

        /// <summary>
        /// Get the property of a structural object by property name
        /// </summary>
        /// <param name="structuralObject"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static object GetPropertyByName(dynamic structuralObject, string name)
        {
            BHoM.Collections.Dictionary<string, object> properties = new BHoM.Collections.Dictionary<string, object>();
            object obj = new object();
            try
            {
                obj = structuralObject.GetProperties()[name];
            }
            catch
            {
                obj = null;
            }
            return obj;
        }

    }

    
}