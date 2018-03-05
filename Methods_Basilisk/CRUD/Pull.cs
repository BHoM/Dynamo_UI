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

        public static IEnumerable<object> Pull(BHoMAdapter adapter, IQuery query = null, CustomObject config = null, bool active = false)
        {
            if (query == null)
                query = new FilterQuery();

            if (active)
            {
                Dictionary<string, object> conf = (config != null) ? config.CustomData : null;
                IEnumerable<object> result = adapter.Pull(query, conf);
                return result;
            }
            else
                return new List<object>();
        }

        /***************************************************/
    }
}
