using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MA = Mongo_Adapter;
using BHB = BHoM.Base;

namespace Mongo
{
    public static class MongoLink
    {
        /// <summary></summary>
        public static MA.MongoLink CreateMongoLink(string serverLink = "mongodb://host:27017", string databaseName = "project", string collectionName = "bhomObjects")
        {
            return new MA.MongoLink(serverLink, databaseName, collectionName);
        }

        /// <summary></summary>
        public static void ToMongo(MA.MongoLink mongoLink, object[] objects, string key, bool active = false)
        {
            if (!active) return;

            List<BHB.BHoMObject> bhomObjects = new List<BHB.BHoMObject>();
            foreach (object o in objects)
                bhomObjects.Add((BHB.BHoMObject)o);
            mongoLink.SaveObjects(bhomObjects, key);
        }

        /// <summary></summary>
        public static IEnumerable<object> FromMongo(MA.MongoLink mongoLink, string filter)
        {
            return mongoLink.GetObjects(filter);
        }

        /// <summary></summary>
        public static void DeleteObjects(string filter = "{}", bool active = false)
        {
            if (active)
                MongoLink.DeleteObjects(filter);
        }
    }
}
