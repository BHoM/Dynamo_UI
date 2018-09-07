using Autodesk.DesignScript.Runtime;
using BH.Engine.Dynamo;
using BH.Engine.Dynamo.Objects;
using BH.UI.Templates;
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

        public static object RunMethodCaller(string callerId)
        {
            return RunMethodCaller(callerId, new object[] { });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1)
        {
            return RunMethodCaller(callerId, new object[] { a1 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2,  object a3)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2, object a3, object a4)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3, a4 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2, object a3, object a4, object a5)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3, a4, a5 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 });
        }

        /***************************************************/

        public static object RunMethodCaller(string callerId, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12)
        {
            return RunMethodCaller(callerId, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 });
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static object RunMethodCaller(string callerId, object[] arguments)  // It is super important for this to be private of Dynamo mess things up
        {
            if (Callers.ContainsKey(callerId))
            {
                MethodCaller caller = Callers[callerId];
                DataAccessor_Dynamo accessor = caller.DataAccessor as DataAccessor_Dynamo;
                accessor.Inputs = arguments;
                caller.Run();

                if (accessor.Outputs.Length == 1)
                    return accessor.Outputs.First();
                else if (accessor.Outputs.Length > 1)
                {
                    MultiResults[callerId] = accessor.Outputs;
                    return accessor.Outputs;
                }
                else
                    return null;    
            }
            else
            {
                BH.Engine.Reflection.Compute.RecordError("The method caller cannot be found.");
                return null;
            }
                
        }

        /***************************************************/
        /**** Public Static Fileds                      ****/
        /***************************************************/

        public static Dictionary<string, MethodCaller> Callers { get; } = new Dictionary<string, MethodCaller>();

        public static Dictionary<string, object[]> MultiResults { get; } = new Dictionary<string, object[]>();


        /***************************************************/
    }
}
