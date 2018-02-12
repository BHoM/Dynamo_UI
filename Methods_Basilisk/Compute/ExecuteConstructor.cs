using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BH.UI.Basilisk.Constructors
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object ExecuteConstructor(string methodKey, List<object> arguments)
        {
            if (ConstructorsToExecute.ContainsKey(methodKey))  // Very Hacky by will get us on the road
                return ConstructorsToExecute[methodKey].Invoke(arguments.ToArray());
            else
                return null;
        }


        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public static Dictionary<string, ConstructorInfo> ConstructorsToExecute = new Dictionary<string, ConstructorInfo>();


        /***************************************************/
    }
}
