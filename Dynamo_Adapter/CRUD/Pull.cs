using BH.Adapter.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.DynamoBIM
{
    public partial class DynamoBIMAdapter
    {
        public IEnumerable<object> Pull(IEnumerable<IQuery> query, Dictionary<string, string> config = null)
        {
            throw new NotImplementedException();
        }
    }
}

