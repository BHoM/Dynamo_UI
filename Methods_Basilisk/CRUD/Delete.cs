using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static int Delete(BHoMAdapter adapter, FilterQuery query = null, CustomObject config = null, bool active = false)
        {
            if (query == null)
                query = new FilterQuery();

            Dictionary<string, object> conf = (config != null) ? config.CustomData : null;

            if (active)
                return adapter.Delete(query, conf);
            else
                return 0;
        }

        /***************************************************/
    }
}
