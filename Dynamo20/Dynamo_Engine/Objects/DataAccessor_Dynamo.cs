/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using Autodesk.DesignScript.Runtime;
using BH.Engine.Dynamo;
using BH.Engine.Dynamo.Objects;
using BH.oM.UI;
using BH.UI.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Dynamo.Objects
{
    [IsVisibleInDynamoLibrary(false)]
    public class DataAccessor_Dynamo : IDataAccessor
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        public object[] Inputs { get; set; } = new object[] { };

        public object[] Outputs { get; set; } = new object[] { };


        /*************************************/
        /**** Input Getter Methods        ****/
        /*************************************/

        public T GetDataItem<T>(int index)
        {
            object aObject = GetInputAt(index);

            if (aObject == null)
                return default(T);

            Type aType = aObject.GetType();

            if (typeof(T).Namespace.StartsWith("BH.oM") && (aType.Namespace.StartsWith("Autodesk.DesignScript") || aType.Namespace.StartsWith("DesignScript.Builtin")))
                return (T)(aObject.IFromDesignScript() as dynamic);

            if (aType == typeof(T) || typeof(T).IsAssignableFrom(aType))
                return (T)aObject;

            if (typeof(T) == typeof(double))
            {
                if (aObject is string)
                {
                    double aValue;
                    if (double.TryParse((string)aObject, out aValue))
                        return (T)(object)aValue;
                }
                else if (aObject is sbyte || aObject is byte || aObject is short || aObject is ushort || aObject is int || aObject is uint || aObject is long || aObject is ulong || aObject is float || aObject is double || aObject is decimal)
                {
                    return (T)(object)System.Convert.ToDouble(aObject);
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (aObject is string)
                {
                    int aValue;
                    if (int.TryParse((string)aObject, out aValue))
                        return (T)(object)aValue;
                }
                else if (aObject is sbyte || aObject is byte || aObject is short || aObject is ushort || aObject is int || aObject is uint || aObject is long || aObject is ulong || aObject is float || aObject is double || aObject is decimal)
                {
                    return (T)(object)System.Convert.ToInt32(aObject);
                }
            }

            return default(T);
        }

        /*************************************/

        public List<T> GetDataList<T>(int index)
        {
            object content = GetInputAt(index);
            if (content is ListWrapper)
                content = ((ListWrapper)content).Items;
            if (content == null)
                content = new List<T>();
            IEnumerable data = content as IEnumerable;

            return data.Cast<object>().Select(x => x.IFromDesignScript()).Cast<T>().ToList();
        }

        /*************************************/

        public List<List<T>> GetDataTree<T>(int index)
        {
            object content = GetInputAt(index);
            if (content is TreeWrapper)
                content = ((TreeWrapper)content).Items;
            if (content == null)
                content = new List<List<T>>();
            IEnumerable<IEnumerable> data = content as IEnumerable<IEnumerable>;

            return data.Select(y => y.Cast<object>().Select(x => x.IFromDesignScript()).Cast<T>().ToList()).ToList();
        }

        /*************************************/

        public List<object> GetAllData(int index)
        {
            //TODO
            return new List<object>();
        }

        /*************************************/
        /**** Output Setter Methods       ****/
        /*************************************/

        public bool SetDataItem<T>(int index, T data)
        {
            SetOutputAt(index, data.IToDesignScript());
            return true;
        }

        /*************************************/

        public bool SetDataList<T>(int index, IEnumerable<T> data)
        {
            SetOutputAt(index, data.Select(x => x.IToDesignScript()).ToList());
            return true;
        }

        /*************************************/

        public bool SetDataTree<T>(int index, IEnumerable<IEnumerable<T>> data)
        {
            SetOutputAt(index, data.Select(y => y.Select(x => x.IToDesignScript()).ToList()).ToList());
            return true;
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public object GetInputAt(int index)
        {
            if (index >= 0 && index < Inputs.Length)
                return Inputs[index];
            else
                return null;
        }

        /***************************************************/

        public void SetOutputAt(int index, object data)
        {
            if (index < 0)
                return;

            if (Outputs.Length <= index)
            {
                object[] outputs = Outputs;
                Array.Resize(ref outputs, index + 1);
                Outputs = outputs;
            }

            Outputs[index] = data;
        }

        /***************************************************/
    }
}

