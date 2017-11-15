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
        public static int? UpdateProperty(BHA.BHoMAdapter adapter = null,
            BHA.Queries.FilterQuery filter = null,
            string property = "",
            object newValue = null,
            Dictionary<string, object> config = null,
            bool active = false)
        {
            if (!active)
                return null;

            int nb = adapter.UpdateProperty(filter, property, newValue, config);
            return nb;
        }
    }
}