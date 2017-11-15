using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHA = BH.Adapter;
using BH.oM.Base;


namespace BH.UI.Basilisk.Adapter
{
    public static partial class Adapter
    {
        public static bool? Push(object adapter,
            List<object> objects,
            string tag = "",
            bool a = true,
            bool active = false)
        {
            if (!active)
                return null;

            List<BHoMObject> BHoM_objects = new List<BHoMObject>();
            BHA.BHoMAdapter BHAdapter = (BHA.BHoMAdapter)adapter;
            
            foreach(object o in objects)
                BHoM_objects.Add(o as BHoMObject);

            Dictionary<string, object> config = new Dictionary<string, object>();

            bool success = BHAdapter.Push(BHoM_objects, tag, config);
            return success;
        }
    }
}
