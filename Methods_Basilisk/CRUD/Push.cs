using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Basilisk.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<IObject> Push(BHoMAdapter adapter, IEnumerable<object> objects, string tag = "", CustomObject config = null, bool active = false)
        {
            Dictionary<string, object> conf = (config != null) ? config.CustomData : null;

            if (active)
                return adapter.Push(objects.Cast<IObject>(), tag, conf);
            else
                return new List<IObject>();
        }

        /***************************************************/
    }
}
