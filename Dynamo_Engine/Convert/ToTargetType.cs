using System;
using System.Collections;
using ADG = Autodesk.DesignScript.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BH.Engine.Dynamo
{
    public static partial class Convert 
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object ToTargetType(this object item, Type type)
        {
            if (item == null)
                return null; 

            if (item.GetType() == type)
                return item;
            else
            {
                if (typeof(IEnumerable).IsAssignableFrom(type))
                {
                    MethodInfo converter = typeof(Convert).GetMethod("ToTypeList").MakeGenericMethod(new Type[] { type.GetGenericArguments().First() });
                    return converter.Invoke(null, new object[] { item });
                }
                else
                {
                    MethodInfo converter = typeof(Convert).GetMethod("ToType").MakeGenericMethod(new Type[] { type });
                    return converter.Invoke(null, new object[] { item });
                }
                
            }
        }

        /***************************************************/

        public static T ToType<T>(this object item)
        {
            if (item is ADG.Geometry || item is ADG.Vector)
                return (T)Convert.ToBHoM(item as dynamic);
            else
                return (T)item;
        }

        /***************************************************/

        public static List<T> ToTypeList<T>(this IEnumerable list)
        {
            List<T> newList = new List<T>();
            foreach (object item in list)
                newList.Add(item.ToType<T>());
            return newList;
        }
    }
}
