using System;
using System.Collections;
using ADG = Autodesk.DesignScript.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BH.Engine.Dynamo.Objects;

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

            if (item is ListWrapper)
                item = ((ListWrapper)item).Items;
            else if (item is TreeWrapper)
                item = ((TreeWrapper)item).Items;

            if (item.GetType() == type)
                return item;
            else
            {
                Type enumerableType = typeof(IEnumerable);
                if (enumerableType.IsAssignableFrom(type) && type != typeof(string))
                {
                    MethodInfo converter;
                    Type[] argTypes = type.GetGenericArguments();
                    if (argTypes.Length > 0 && enumerableType.IsAssignableFrom(argTypes[0]) && argTypes[0] != typeof(string))
                        converter = typeof(Convert).GetMethod("ToTypeTree").MakeGenericMethod(new Type[] { type.GetGenericArguments().First() });
                    else
                        converter = typeof(Convert).GetMethod("ToTypeList").MakeGenericMethod(new Type[] { type.GetGenericArguments().First() });
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
                return (T)(item as dynamic);
        }

        /***************************************************/

        public static List<T> ToTypeList<T>(this IEnumerable list)
        {
            List<T> newList = new List<T>();
            foreach (object item in list)
                newList.Add(item.ToType<T>());
            return newList;
        }

        /***************************************************/

        public static List<List<T>> ToTypeTree<T>(this IEnumerable tree)
        {
            List<List<T>> newTree = new List<List<T>>();
            foreach (IEnumerable item in tree)
                newTree.Add(item.ToTypeList<T>());
            return newTree;
        }

        /***************************************************/
    }
}
