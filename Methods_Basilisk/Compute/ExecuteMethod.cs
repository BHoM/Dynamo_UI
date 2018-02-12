using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object ExecuteMethod(string methodKey, List<object> arguments)
        {
            if (MethodsToExecute.ContainsKey(methodKey))  // Very Hacky by will get us on the road
                return MethodsToExecute[methodKey].DynamicInvoke(arguments.ToArray());
            else
                return null;
        }


        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public static Dictionary<string, Delegate> MethodsToExecute = new Dictionary<string, Delegate>();


        /***************************************************/
    }
}
