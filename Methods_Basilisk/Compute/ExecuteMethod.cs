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

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1)
        {
            return ExecuteMethod(methodKey, new object[] { a1 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport]  object a3)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport] object a3, [ArbitraryDimensionArrayImport] object a4)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport] object a3, [ArbitraryDimensionArrayImport] object a4, [ArbitraryDimensionArrayImport] object a5)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport] object a3, [ArbitraryDimensionArrayImport] object a4, [ArbitraryDimensionArrayImport] object a5, [ArbitraryDimensionArrayImport] object a6)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport] object a3, [ArbitraryDimensionArrayImport] object a4, [ArbitraryDimensionArrayImport] object a5, [ArbitraryDimensionArrayImport] object a6, [ArbitraryDimensionArrayImport] object a7)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport] object a3, [ArbitraryDimensionArrayImport] object a4, [ArbitraryDimensionArrayImport] object a5, [ArbitraryDimensionArrayImport] object a6, [ArbitraryDimensionArrayImport] object a7, [ArbitraryDimensionArrayImport] object a8)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport] object a3, [ArbitraryDimensionArrayImport] object a4, [ArbitraryDimensionArrayImport] object a5, [ArbitraryDimensionArrayImport] object a6, [ArbitraryDimensionArrayImport] object a7, [ArbitraryDimensionArrayImport] object a8, [ArbitraryDimensionArrayImport] object a9)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport] object a3, [ArbitraryDimensionArrayImport] object a4, [ArbitraryDimensionArrayImport] object a5, [ArbitraryDimensionArrayImport] object a6, [ArbitraryDimensionArrayImport] object a7, [ArbitraryDimensionArrayImport] object a8, [ArbitraryDimensionArrayImport] object a9, [ArbitraryDimensionArrayImport] object a10)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport] object a3, [ArbitraryDimensionArrayImport] object a4, [ArbitraryDimensionArrayImport] object a5, [ArbitraryDimensionArrayImport] object a6, [ArbitraryDimensionArrayImport] object a7, [ArbitraryDimensionArrayImport] object a8, [ArbitraryDimensionArrayImport] object a9, [ArbitraryDimensionArrayImport] object a10, [ArbitraryDimensionArrayImport] object a11)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 });
        }

        /***************************************************/

        public static object ExecuteMethod(string methodKey, [ArbitraryDimensionArrayImport] object a1, [ArbitraryDimensionArrayImport] object a2, [ArbitraryDimensionArrayImport] object a3, [ArbitraryDimensionArrayImport] object a4, [ArbitraryDimensionArrayImport] object a5, [ArbitraryDimensionArrayImport] object a6, [ArbitraryDimensionArrayImport] object a7, [ArbitraryDimensionArrayImport] object a8, [ArbitraryDimensionArrayImport] object a9, [ArbitraryDimensionArrayImport] object a10, [ArbitraryDimensionArrayImport] object a11, [ArbitraryDimensionArrayImport] object a12)
        {
            return ExecuteMethod(methodKey, new object[] { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 });
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static object ExecuteMethod(string methodKey, object[] arguments)
        {
            if (MethodsToExecute.ContainsKey(methodKey))  // Very Hacky by will get us on the road
            {
                // Get the method and parameters
                MethodBase method = MethodsToExecute[methodKey];
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length != arguments.Count())
                    return null;

                // Takes care of the lacing
                Dictionary<int, List<object>> replicationSet = new Dictionary<int, List<object>>();
                Type enumerableType = typeof(IEnumerable);
                for (int i = 0; i < arguments.Length; i++)
                {
                    if (arguments[i] is IEnumerable)
                    {
                        if (!enumerableType.IsAssignableFrom(parameters[i].ParameterType))
                            replicationSet[i] = ((IEnumerable)arguments[i]).Cast<object>().ToList();
                    }   
                    else
                    {
                        if (enumerableType.IsAssignableFrom(parameters[i].ParameterType))
                            arguments[i] = new ArrayList { arguments[i] };
                    }
                }

                
                if (replicationSet.Count > 0)
                {
                    // Make sure all those replication sets are the same length
                    IEnumerable<int> counts = replicationSet.Values.Select(x => x.Count).Distinct();
                    if (counts.Count() != 1)
                        throw new Exception("All the series must be the same length");
                    int length = counts.First();

                    List<object> result = new List<object>();
                    for (int n = 0; n < length; n++)
                    {
                        List<object> newArgs = new List<object>();
                        for (int i = 0; i < arguments.Length; i++)
                        {
                            if (replicationSet.ContainsKey(i))
                                newArgs.Add(replicationSet[i][n]);
                            else
                                newArgs.Add(arguments[i]);
                        }
                        result.Add(ExecuteMethod(methodKey, newArgs.ToArray()));
                    }
                    return result;
                }
                else
                {
                    // Translate to correct types
                    object[] translations = arguments.Zip(parameters, (a, b) => Engine.Dynamo.Convert.ToTargetType(a, b.ParameterType)).ToArray();

                    // Make sure default values are assigned when the inputs are null
                    for (int i = 0; i < parameters.Length; i++)
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
            }
            else
                return null;
        }

        /***************************************************/

        private static List<List<T>> AddToList<T>(List<List<T>> list, List<T> items)
        {
            List<List<T>> result = new List<List<T>>();
            foreach (T item in items)
            {
                T[] array = new T[] { item };
                result.AddRange(list.Select(x => x.Concat(array).ToList()));
            }
            return result;
        }


        /***************************************************/
        /**** Public Fields                             ****/
        /***************************************************/

        public static Dictionary<string, MethodBase> MethodsToExecute = new Dictionary<string, MethodBase>();


        /***************************************************/
    }
}
