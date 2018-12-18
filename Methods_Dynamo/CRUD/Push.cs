using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Dynamo.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<object> Push(BHoMAdapter adapter, IEnumerable<object> objects, string tag = "", CustomObject config = null, bool active = false)
        {
            Dictionary<string, object> conf = (config != null) ? config.CustomData : null;

            if (active)
                return adapter.Push(objects.Cast<IObject>(), tag, conf).ToList<object>();
            else
                return new List<object>();
        }

        /***************************************************/
    }
}
