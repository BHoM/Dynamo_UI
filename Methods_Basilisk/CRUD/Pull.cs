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

        public static IEnumerable<object> Pull(BHoMAdapter adapter, IQuery query, Dictionary<string, object> config = null)
        {
            return adapter.Pull(query, config);
        }

        /***************************************************/
    }
}
