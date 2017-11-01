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
        public static bool? Execute(BHA.BHoMAdapter adapter = null,
            string command = "",
            Dictionary<string, object> parameters = null,
            Dictionary<string, string> config = null,
            bool active = false)
        {
            if (!active)
                return null;

            bool success = adapter.Execute(command, parameters, config);
            return success;
        }
    }
}
