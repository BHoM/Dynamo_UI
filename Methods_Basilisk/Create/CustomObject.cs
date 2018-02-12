using Autodesk.DesignScript.Runtime;
using BH.oM.Base;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static CustomObject CustomObject(List<string> names, List<object> values)
        {
            return Engine.Base.Create.CustomObject(names, values);
        }

        /***************************************************/
    }
}
