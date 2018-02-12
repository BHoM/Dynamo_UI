using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Queries;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static int Delete(BHoMAdapter adapter, FilterQuery query, Dictionary<string, object> config = null)
        {
            if (query == null)
                query = new FilterQuery();

            return adapter.Delete(query, config);
        }

        /***************************************************/
    }
}
