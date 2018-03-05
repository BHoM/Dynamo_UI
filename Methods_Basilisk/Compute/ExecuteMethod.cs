using Autodesk.DesignScript.Runtime;
using BH.Engine.Dynamo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Methods
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object ExecuteMethod(string methodKey)
        {
            return ExecuteMethod(methodKey, new object[] { });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1)
        {
            return ExecuteMethod(methodKey, new object[] { a1 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3, object a4)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3, object a4, object a5)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3, object a4, object a5, object a6)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3, object a4, object a5, object a6, object a7)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 });
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static object ExecuteMethod(string methodKey, IEnumerable<object> arguments)
        {
            if (MethodsToExecute.ContainsKey(methodKey))  // Very Hacky by will get us on the road
            {
                object[] translations = arguments.Select(x => Engine.Dynamo.Convert.IToBHoM(x)).ToArray();
                MethodBase method = MethodsToExecute[methodKey];

                // Make sure default values are assigned when the inputs are null
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length != translations.Length)
                    return null;

                for(int i = 0; i < parameters.Length; i++)
                {
                    if (translations[i] == null && parameters[i].HasDefaultValue)
                        translations[i] = parameters[i].DefaultValue;
                }

                // Invoke the method
                object result = null;
                if (method is ConstructorInfo)
                    result = ((ConstructorInfo)method).Invoke(translations);
                else
                    result = method.Invoke(null, translations);

                if (result is IList)
                    return ((IList)result).Cast<object>().Select(x => x.IToDesignScript());
                else if (result != null)
                    return result.IToDesignScript();
                else
                    return null;
            }
            else
                return null;
        }


        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public static Dictionary<string, MethodBase> MethodsToExecute = new Dictionary<string, MethodBase>();


        /***************************************************/
    }
}
