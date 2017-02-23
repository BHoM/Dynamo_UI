using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;

namespace Base
{
    public class DictionaryWrapper
    {
        public Dictionary<string, object> Dictionary { get; set; }
        public string Name { get; set; }
        internal DictionaryWrapper(string name, Dictionary<string, object> dictionary)
        {
            Dictionary = dictionary;
            Name = name;
        }
        public override string ToString()
        {
            return Name;
        }

        public static implicit operator Dictionary<string, object>(DictionaryWrapper data)
        {
            return data.Dictionary;
        }
    }


    public static class CustomData
    {
        
        public static object CreateCustomData(List<string> Key, List<object> Value)
        {
            if (Key.Count == Value.Count)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                for (int i = 0; i < Key.Count; i++)
                {
                    dictionary[Key[i]] = Value[i];
                }
                return new DictionaryWrapper("CustomData", dictionary);
            }
            else
            {
                return null;
            }  
        }
    }
}
