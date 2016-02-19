using System;
using System.Collections.Generic;
using System.Linq;
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
        internal Dictionary() { }

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
        public static Dictionary<string, object> ByKeyValuePair(Object[] objects, object[] keys)
        {
            //Output dictionary definition
            Dictionary<string, object> dictionary_out = new Dictionary<string, object>();

            BHoM.Collections.Dictionary<object, List<object>> new_dictionary = new BHoM.Collections.Dictionary<object, List<object>>();

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
        public static List<object> GetValuesByKey(BHoM.Collections.Dictionary<string, object> dictionary, string[] keys)
        {
            List<object> objects = new List<object>();

            for (int i = 0; i < keys.Length; i++)
            {
                try
                {
                    objects.Add(dictionary[keys.ToArray()[i]]);
                }
                catch
                {
                    objects.Add("Key not found");
                }
            }
            return objects;
        }

        /// <summary>
        /// Returns a list of keys as strings of a BHoM dictionary. 
        /// BuroHappold
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static List<string> GetKeys(BHoM.Collections.Dictionary<string, object> dictionary)
        {
            return dictionary.KeyList();
        }

        /// <summary>
        /// Returns a list of keys as strings of a BHoM dictionary. 
        /// BuroHappold
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static List<object> GetValues(BHoM.Collections.Dictionary<string, object> dictionary)
        {
            return dictionary.Values.ToList();
        }
    }
}