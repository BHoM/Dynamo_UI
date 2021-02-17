/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using Dynamo.Engine;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Models;
using ProtoCore.Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<PortModel> InPorts { get; set; } = new ObservableCollection<PortModel>();

        public static EngineController DynamoEngine { get; set; } = null;

        public DynamoModel DynamoModel { get; set; } = null;


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
            if (DynamoEngine == null || InPorts.Count <= index || InPorts[index].Connectors.Count == 0)
                return new List<object>();

            // Get the accessor to the data feeding into this input
            NodeModel valuesNode = InPorts[index].Connectors[0].Start.Owner;
            int outIndex = InPorts[index].Connectors[0].Start.Index;
            string startId = valuesNode.GetAstIdentifierForOutputIndex(outIndex).Name;
            RuntimeMirror colorsMirror = DynamoEngine.GetMirror(startId);
            if (colorsMirror == null || colorsMirror.GetData() == null)
                return new List<object>();

            // return the collected data
            return CollectData(colorsMirror.GetData());
        }

        /*************************************/
        /**** Output Setter Methods       ****/
        /*************************************/

        public bool SetDataItem<T>(int index, T data)
        {
            if (data == null)
            {
                SetOutputAt(index, null);
                return true;
            }

            try
            {
                SetOutputAt(index, data.IToDesignScript());
                return true;
            }
            catch (Exception e)
            {
                SetOutputAt(index, data);
                Engine.Reflection.Compute.RecordWarning("Failed to convert output " + index + " to DesignScript. Returning it without conversion.\nError: " + e.Message);
                return false;
            }
        }

        /*************************************/

        public bool SetDataList<T>(int index, IEnumerable<T> data)
        {
            if (data == null)
            {
                SetOutputAt(index, new List<T>());
                return true;
            }

            try
            {
                SetOutputAt(index, data.Select(x => x == null ? null : x.IToDesignScript()).ToList());
                return true;
            }
            catch (Exception e)
            {
                SetOutputAt(index, data);
                Engine.Reflection.Compute.RecordWarning("Failed to convert output " + index + " to DesignScript. Returning it without conversion.\nError: " + e.Message);
                return false;
            }
        }

        /*************************************/

        public bool SetDataTree<T>(int index, IEnumerable<IEnumerable<T>> data)
        {
            if (data == null)
            {
                SetOutputAt(index, new List<List<T>>());
                return true;
            }

            try
            {
                SetOutputAt(index, data.Select(y => y.Select(x => x == null ? null : x.IToDesignScript()).ToList()).ToList());
                return true;
            }
            catch (Exception e)
            {
                SetOutputAt(index, data);
                Engine.Reflection.Compute.RecordWarning("Failed to convert output " + index + " to DesignScript. Returning it without conversion.\nError: " + e.Message);
                return false;
            }
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private object GetInputAt(int index)
        {
            if (index >= 0 && index < Inputs.Length)
                return Inputs[index];
            else
                return null;
        }

        /***************************************************/

        private void SetOutputAt(int index, object data)
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

        /*******************************************/

        private List<object> CollectData(MirrorData mirrorData)
        {
            if (mirrorData.IsCollection)
            {
                IEnumerable<MirrorData> elements = mirrorData.GetElements();
                if (elements.Count() > 0)
                    return elements.SelectMany(x => CollectData(x)).ToList();
                else
                    return new List<object>();
            }
            else
                return new List<object> { mirrorData.Data.IFromDesignScript() };
        }

        /***************************************************/
    }
}


