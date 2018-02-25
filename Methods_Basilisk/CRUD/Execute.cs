using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.DataManipulation.Queries;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static bool Execute(BHoMAdapter adapter, string command, Dictionary<string, object> parameters = null, Dictionary<string, object> config = null)
        {
            return adapter.Execute(command, parameters, config);
        }

        /***************************************************/
    }
}
