using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHA = BH.Adapter;

namespace BH.UI.Basilisk.Adapter
{
    public static partial class Adapter
    {
        public static int? Delete(BHA.BHoMAdapter adapter = null, 
            BHA.Queries.FilterQuery query = null, 
            Dictionary<string, object> config = null,
            bool active = false)
        {
            if (!active)
                return null;

            int nb = adapter.Delete(query, config);
            return nb;
        }
    }
}
