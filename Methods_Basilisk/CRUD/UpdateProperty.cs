using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static int UpdateProperty(BHoMAdapter adapter, FilterQuery filter, string property, object newValue, CustomObject config = null, bool active = false)
        {
            if (active)
            {
                Dictionary<string, object> conf = (config != null) ? config.CustomData : null;
                return adapter.UpdateProperty(filter, property, newValue, conf);
            }
            else
                return 0;
        }

        /***************************************************/
    }
}
