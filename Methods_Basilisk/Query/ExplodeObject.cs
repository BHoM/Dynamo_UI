using Autodesk.DesignScript.Runtime;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Dictionary<string, object> ExplodeObject(object obj)
        {
            return Engine.Reflection.Query.PropertyDictionary(obj);
        }

        /***************************************************/
    }
}
