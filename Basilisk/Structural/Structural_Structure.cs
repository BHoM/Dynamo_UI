using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Interfaces;

namespace Structural
{
    /// <summary>
    /// Structure class for constructing a structure object consisting of child structure objects and dictionaries
    /// </summary>
    public class Structure 
    {
        internal Structure() { }
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
              
                out_dict.Add("Names", PropertiesDictionary.KeyList());
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
            object obj = new object();
            try
            {
                obj = structuralObject.GetType().GetProperty(name).GetValue(structuralObject);
            }
            catch
            {
                obj = null;
            }
            return obj;
        }

    }

    
}