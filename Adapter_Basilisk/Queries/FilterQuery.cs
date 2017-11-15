using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Basilisk.Adapter
{
    public static partial class Query
    {
        public static BH.Adapter.Queries.CustomQuery CustomQuery(string Query = null)
        {
            return new BH.Adapter.Queries.CustomQuery(Query);
        }
    }
}
