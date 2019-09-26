/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections;
using ADG = Autodesk.DesignScript.Geometry;
using System.Collections.Generic;
using System.Linq;
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

            if (item == null)
                return null;

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
