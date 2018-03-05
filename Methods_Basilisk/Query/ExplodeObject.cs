using Autodesk.DesignScript.Runtime;
using BH.Engine.Dynamo;
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
            Dictionary<string, object> dic = Engine.Reflection.Query.PropertyDictionary(obj);
            foreach( KeyValuePair<string, object> kvp in dic)
                dic[kvp.Key] = kvp.Value.IToDesignScript();
            return dic;
        }

        /***************************************************/
    }
}
