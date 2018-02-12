using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Queries;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static int UpdateProperty(BHoMAdapter adapter, FilterQuery filter, string property, object newValue, Dictionary<string, object> config = null)
        {
            return adapter.UpdateProperty(filter, property, newValue, config);
        }

        /***************************************************/
    }
}
