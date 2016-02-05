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

namespace ListTools
{
    /// <summary>
    /// Create and manage dictionary type collections of data
    /// BuroHappold
    /// <class name="Dictionary">Dictionary tools</class>
    /// </summary>
    /// <search>BH, dictionary, collection</search>
    
    public class Dictionary
    {
        /// <summary>
        /// Generates a key Dictionary (collection of objects) for later quick retreival of objects 
        /// by 'keys'. Similar to lists, but the 'index' is a reference key.
        /// BuroHappold
        /// </summary>
        /// <param name="objects">Input objects - can be anything</param>
        /// <param name="keys">A list of strings that will attach to each object</param>
        /// <returns></returns>
        /// <search>BH, create, dictionary</search>
        [MultiReturn(new[] {"Dictionary"})]
        public static Dictionary<string, object> ByKeyValuePair(Object[] objects, String[] keys)
        {
            //Output dictionary definition
            Dictionary<string, object> dictionary_out = new Dictionary<string, object>();
            Dictionary<string, List<object>> new_dictionary = new Dictionary<string, List<object>>();

            for (int i = 0; i < objects.Length; i++ )
            {
                List<Object> obj = new List<Object>();
                obj.Add(objects[i]);
                if (new_dictionary.ContainsKey(keys[i]))
                {
                    new_dictionary[keys[i]].Add(objects[i]);
                }
                else
                {
                    new_dictionary.Add(keys[i], obj);
                }
          }
            dictionary_out.Add("Dictionary", new_dictionary);
            return dictionary_out;
        }

        /// <summary>
        /// Retrieves an object from a BHList.dictionary using a key.
        /// BuroHappold
        /// </summary>
        /// <param name="dictionary">dictionary to retrieve item from</param>
        /// <param name="keys">A list of strings used to retrieve the assocated values</param>
        /// <returns></returns>
        /// <search>BH, get, dictionary, key</search>
        [MultiReturn(new[] { "Objects" })]
        public static Dictionary<string, object> GetValuesByKey(Dictionary<string,object> dictionary, string[] keys)
        {
            //Output dictionary definition
            Dictionary<string, object> values_dict_out = new Dictionary<string, object>();
            List<object> objects = new List<object>();
                        
            for (int i = 0; i < keys.Length; i++)
                {
                  objects.Add(dictionary[keys.ToArray()[i]]);
                }
            values_dict_out.Add("Objects",objects);
            return values_dict_out;
        }
  }
}