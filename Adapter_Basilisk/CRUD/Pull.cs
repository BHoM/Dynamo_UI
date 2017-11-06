using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHA = BH.Adapter;
using ADR = Autodesk.DesignScript.Runtime;

namespace BH.UI.Basilisk.Adapter
{
    public static partial class Adapter
    {
        public static IEnumerable<object> Pull(object app,
            object query,
            [ADR.DefaultArgument("null")]Dictionary<string, string> config,
            bool active = false)
        {
            BHA.BHoMAdapter adapter = (BHA.BHoMAdapter)app;
            if (!active)
                return null;
            if (config == null)
                config = new Dictionary<string, string>();

            IEnumerable<object> objects = adapter.Pull((BHA.Queries.IQuery)query, config);
            return objects;
        }
    }
}