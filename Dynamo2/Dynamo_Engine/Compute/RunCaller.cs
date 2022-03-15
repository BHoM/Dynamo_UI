/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using BH.oM.Base.Debugging;
using BH.oM.UI;
using BH.UI.Base;
using Dynamo.Graph.Nodes;
using Dynamo.Graph.Workspaces;
using Dynamo.Models;
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

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31, object a32)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, a32 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31, object a32, object a33)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, a32, a33 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31, object a32, object a33, object a34)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, a32, a33, a34 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31, object a32, object a33, object a34, object a35)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, a32, a33, a34, a35 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31, object a32, object a33, object a34, object a35, object a36)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, a32, a33, a34, a35, a36 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31, object a32, object a33, object a34, object a35, object a36, object a37)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, a32, a33, a34, a35, a36, a37 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31, object a32, object a33, object a34, object a35, object a36, object a37, object a38)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, a32, a33, a34, a35, a36, a37, a38 });
        }


        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31, object a32, object a33, object a34, object a35, object a36, object a37, object a38, object a39)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, a32, a33, a34, a35, a36, a37, a38, a39 });
        }

        /***************************************************/

        public static object RunCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12, object a13, object a14, object a15, object a16, object a17, object a18, object a19, object a20, object a21, object a22, object a23, object a24, object a25, object a26, object a27, object a28, object a29, object a30, object a31, object a32, object a33, object a34, object a35, object a36, object a37, object a38, object a39, object a40)
        {
            return RunCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16, a17, a18, a19, a20, a21, a22, a23, a24, a25, a26, a27, a28, a29, a30, a31, a32, a33, a34, a35, a36, a37, a38, a39, a40 });
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static object RunCaller(string callerId, object[] arguments)  // It is super important for this to be private of Dynamo mess things up
        {
            object result = null;
            Engine.Base.Compute.ClearCurrentEvents();

            // Run the caller
            Caller caller = null;
            if (Callers.ContainsKey(callerId) && DataAccessors.ContainsKey(callerId))
            {
                caller = Callers[callerId];
                int nbOutputs = caller.OutputParams.Where(x => x.IsSelected).Count();
                DataAccessor_Dynamo accessor = DataAccessors[callerId];
                accessor.Inputs = arguments;
                accessor.Outputs = new object[nbOutputs];
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
                Base.Compute.RecordError("The method caller cannot be found.");
                result = null;
            }

            // Handle errors and warnings
            List<Event> events = Base.Query.CurrentEvents();
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

            // Log usage
            try
            {
                string fileName = "";
                string fileId = "";
                if (DataAccessors.ContainsKey(callerId))
                {
                    DynamoModel model = DataAccessors[callerId].DynamoModel;
                    if (model != null)
                    {
                        WorkspaceModel workspace = model.CurrentWorkspace;
                        fileName = workspace?.FileName;
                        fileId = workspace?.Guid.ToString();

                    }
                }

                UI.Compute.LogUsage("Dynamo", "2.0", Guid.Parse(callerId), caller?.GetType().Name, caller?.SelectedItem, events, fileId, fileName);
            }
            catch { }

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
                node.State = ElementState.Active;
            }
        }


        /***************************************************/
        /**** Public Static Fileds                      ****/
        /***************************************************/

        public static Dictionary<string, Caller> Callers { get; } = new Dictionary<string, Caller>();

        public static Dictionary<string, DataAccessor_Dynamo> DataAccessors { get; } = new Dictionary<string, DataAccessor_Dynamo>();

        public static Dictionary<string, NodeModel> Nodes { get; } = new Dictionary<string, NodeModel>();

        public static Dictionary<string, object[]> MultiResults { get; } = new Dictionary<string, object[]>();


        /***************************************************/
    }
}



