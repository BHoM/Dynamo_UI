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
        public static IEnumerable<object> Pull(BHA.BHoMAdapter adapter = null,
            BHA.Queries.IQuery query = null,
            Dictionary<string, string> config = null,
            bool active = false)
        {
            if (!active)
                return null;

            IEnumerable<object> objects = adapter.Pull(query, config);
            return objects;
        }
    }
}