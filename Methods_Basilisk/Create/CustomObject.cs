using Autodesk.DesignScript.Runtime;
using BH.oM.Base;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Basilisk.Methods
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object CustomObject(List<string> names)
        {
            return CustomObject(names, new List<object> { });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1)
        {
            return CustomObject(names, new List<object> { a1 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2)
        {
            return CustomObject(names, new List<object> { a1, a2 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3)
        {
            return CustomObject(names, new List<object> { a1, a2, a3 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3, object a4)
        {
            return CustomObject(names, new List<object> { a1, a2, a3, a4 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3, object a4, object a5)
        {
            return CustomObject(names, new List<object> { a1, a2, a3, a4, a5 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3, object a4, object a5, object a6)
        {
            return CustomObject(names, new List<object> { a1, a2, a3, a4, a5, a6 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3, object a4, object a5, object a6, object a7)
        {
            return CustomObject(names, new List<object> { a1, a2, a3, a4, a5, a6, a7 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8)
        {
            return CustomObject(names, new List<object> { a1, a2, a3, a4, a5, a6, a7, a8 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9)
        {
            return CustomObject(names, new List<object> { a1, a2, a3, a4, a5, a6, a7, a8, a9 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10)
        {
            return CustomObject(names, new List<object> { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11)
        {
            return CustomObject(names, new List<object> { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11 });
        }

        /***************************************************/

        public static object CustomObject(List<string> names, object a1, object a2, object a3, object a4, object a5, object a6, object a7, object a8, object a9, object a10, object a11, object a12)
        {
            return CustomObject(names, new List<object> { a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12 });
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static CustomObject CustomObject(List<string> names, IEnumerable<object> values)
        {
            List<object> translations = values.Select(x => Engine.Dynamo.Convert.IToBHoM(x)).ToList();
            return Engine.Base.Create.CustomObject(names, translations);
        }

        /***************************************************/
    }
}
