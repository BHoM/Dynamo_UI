using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.Queries;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<IObject> Push(BHoMAdapter adapter, IEnumerable<IObject> objects, string tag = "", Dictionary<string, object> config = null)
        {
            return adapter.Push(objects, tag, config);
        }

        /***************************************************/
    }
}
