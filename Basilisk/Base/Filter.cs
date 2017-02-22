using BHoM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    public static class Filter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Objects"></param>
        /// <param name="Key"></param>
        /// <param name="Values"></param>
        /// <param name="filterOption">Name = 0, Guid = 1, Property = 2, CustomData = 3</param>
        /// <returns></returns>
        public static List<object> ObjectFilter(List<object> Objects, string Key, List<object> Values, int filterOption)
        {
            Dictionary<string, string> addedTypes = new Dictionary<string, string>();
            List<BHoM.Base.BHoMObject> objects = Objects.Cast<BHoM.Base.BHoMObject>().ToList();
            FilterOption option = (FilterOption)filterOption;

            Dictionary<string, BHoM.Base.BHoMObject> filter = new BHoM.Base.ObjectFilter(objects).ToDictionary<string>(Key, option);

            List<object> result = new List<object>();

            for (int i = 0; i < Values.Count; i++)
            {
                BHoM.Base.BHoMObject obj = null;
                filter.TryGetValue(Values[i].ToString(), out obj);
                result.Add(obj);
            }

            return result;
        }

        public static List<object> KeyMap(List<string> Key, List<object> Map, List<string> Values)
        {
            if (Key.Count == Map.Count)
            {
                Dictionary<string, object> keyMap = new Dictionary<string, object>();
                for (int i = 0; i < Key.Count; i++)
                {
                    keyMap.Add(Key[i], Map[i]);
                }
                List<object> result = new List<object>();
                foreach (string val in Values)
                {
                    result.Add(keyMap[val]);
                }
                return result;
            }
            return null;
        }
    }
}
