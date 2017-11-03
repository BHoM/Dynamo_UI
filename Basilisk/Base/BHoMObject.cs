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

        //public static BHB.CustomObject CreateCustomObject(string Name, List<string> Tags, params List<Tuple<string, object>>[] customData)
        //{

        //    BHB.CustomObject customObj = new BHB.CustomObject();

        //    customObj.Name = Name;
        //    customObj.Tags = new HashSet<string>(Tags);


        //    foreach (List<Tuple<string, object>> o in customData)
        //        foreach (Tuple<string, object> pair in o)
        //            customObj.CustomData.Add(pair.Item1, pair.Item2);

        //    return customObj;
        //}

        //public static List<Tuple<string, object>> CreateDictionary(List<string> key, List<object> value)
        //{
        //    List<Tuple<string, object>> dic = new List<Tuple<string, object>>();

        //    if (key.Count != value.Count)
        //        throw new Exception("Lists must have the same length");

        //    for (int i = 0; i < key.Count; i++)
        //    {
        //        Tuple<string, object> tempTup = new Tuple<string, object>(key[i], value[i]);
        //        dic.Add(tempTup);
        //    }

        //    return dic;
        //}
    }
}
