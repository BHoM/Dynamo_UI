using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHAM = BH.Adapter.Mongo;
using ADR = Autodesk.DesignScript.Runtime;

namespace Basilisk.Mongo
{
    public static class Mongo
    {
        public static BHAM.MongoAdapter MongoApplication(string serverName, string dataBaseName, string collectionName)
        {
            BHAM.MongoAdapter app = new BHAM.MongoAdapter(serverName, dataBaseName, collectionName);
            return app;
        }
    }
}
