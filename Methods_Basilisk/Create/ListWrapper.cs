using Autodesk.DesignScript.Runtime;
using BH.Engine.Dynamo;
using BH.Engine.Dynamo.Objects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Basilisk.Methods
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static ListWrapper ListWrapper(List<object> list)
        {
            return new ListWrapper { Items = list };
        }


        /***************************************************/
    }
}
