using Autodesk.DesignScript.Runtime;
using BH.Engine.Dynamo;
using BH.Engine.Dynamo.Objects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Dynamo.Methods
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static TreeWrapper TreeWrapper(List<List<object>> tree)
        {
            return new TreeWrapper { Items = tree };
        }


        /***************************************************/
    }
}
