using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.Engine.Base;
using BH.Engine.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHB = BH.oM.Base;
using BHG = BH.oM.Geometry;
using ADG = Autodesk.DesignScript.Geometry;
using DA = BH.Adapter.DesignScript;
using ADR = Autodesk.DesignScript.Runtime;

namespace Basilisk.Base
{
    public static class BHoMObject
    {
        public static object BHoMGeometry(ADG.Geometry geometry)
        {
            return DA.Convert.IToBHoM(geometry);
        }

        public static BHB.CustomObject CreateCustomObject(string Name, List<string> Tags, params Dictionary<string, object>[] customData)
        {
            
            BHB.CustomObject customObj = new BHB.CustomObject();
            customObj.Name = Name;
            customObj.Tags = new HashSet<string>(Tags);

            foreach (Dictionary<string, object> o in customData)
               foreach(KeyValuePair<string, object> pair in o)
                    customObj.CustomData.Add(pair.Key, pair.Value);

            return customObj;
        }

        public static Dictionary<string, object> CreateDictionary(List<string> key, List<object> value)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (key.Count != value.Count)
                throw new Exception("Lists must have the same length");

            for (int i = 0; i < key.Count; i++)
            {
                dic.Add(key[i], value[i]);
            }

            return dic;
        }

        public static object GetProperty(object BHoMObject, string key)
        {
            return BH.Engine.Reflection.Query.GetPropertyValue(BHoMObject, key);
        }
    }
}
