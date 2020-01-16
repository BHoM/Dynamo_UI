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

using Autodesk.DesignScript.Runtime;
using BH.Engine.Dynamo;
using BH.Engine.Dynamo.Objects;
using BH.oM.Base;
using BH.oM.Reflection.Debugging;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.Engine.Dynamo
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object RunCaller(string callerId)
        {
            return RunCaller(callerId, new object[] { });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1)
        {
            return RunCaller(callerId, new object[] { a1 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2)
        {
            return RunCaller(callerId, new object[] { a1, a2 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2,  object a3)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 });
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static object RunCaller(string callerId, object[] arguments)  // It is super important for this to be private of Dynamo mess things up
        {
            object result = null;
            Engine.Reflection.Compute.ClearCurrentEvents();

            // Run the caller
            if (Callers.ContainsKey(callerId))
            {
                Caller caller = Callers[callerId];
                DataAccessor_Dynamo accessor = caller.DataAccessor as DataAccessor_Dynamo;
                accessor.Inputs = arguments;
                caller.Run();

                if (accessor.Outputs.Length == 1)
                    result = accessor.Outputs.First();
                else if (accessor.Outputs.Length > 1)
                {
                    MultiResults[callerId] = accessor.Outputs;
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    for (int i = 0; i < accessor.Outputs.Length; i++)
                        dic[i.ToString()] = accessor.Outputs[i];
                    result = new CustomObject { CustomData = dic };
                    //return accessor.Outputs;
                }
                else
                    result = null;    
            }
            else
            {
                Reflection.Compute.RecordError("The method caller cannot be found.");
                result = null;
            }

            // Handle errors and warnings
            List<Event> events = Reflection.Query.CurrentEvents();
            if (events != null && events.Count != 0)
            {
                List<Event> errors = events.FindAll(x => x.Type == EventType.Error);
                if (errors.Count > 0)
                    throw new Exception(errors.Select(x => x.Message).Aggregate((a, b) => a + "\n" + b));
                else if (Nodes.ContainsKey(callerId))
                {
                    List<Event> warnings = events.FindAll(x => x.Type == EventType.Warning);
                    if (warnings.Count > 0)
                        Nodes[callerId].Warning(warnings.Select(x => x.Message).Aggregate((a, b) => a + "\n" + b), true);
                    else
                        ClearWarnings(callerId);
                }
            }
            else
                ClearWarnings(callerId);

            return result;
        }

        /***************************************************/

        private static void ClearWarnings(string id)
        {
            if (Nodes.ContainsKey(id))
            {
                NodeModel node = Nodes[id];
                FieldInfo field = typeof(NodeModel).GetField("persistentWarning", BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                    field.SetValue(node, "");
                node.ToolTipText = "";
            }
        }


        /***************************************************/
        /**** Public Static Fileds                      ****/
        /***************************************************/

        public static Dictionary<string, Caller> Callers { get; } = new Dictionary<string, Caller>();

        public static Dictionary<string, NodeModel> Nodes { get; } = new Dictionary<string, NodeModel>();

        public static Dictionary<string, object[]> MultiResults { get; } = new Dictionary<string, object[]>();


        /***************************************************/
    }
}
