using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static bool Execute(BHoMAdapter adapter, string command, CustomObject parameters = null, CustomObject config = null, bool active = false)
        {
            if (active)
            {
                Dictionary<string, object> conf = (config != null) ? config.CustomData : null;
                Dictionary<string, object> para = (config != null) ? parameters.CustomData : null;
                return adapter.Execute(command, para, conf);
            }
            else
                return false;
        }

        /***************************************************/
    }
}
