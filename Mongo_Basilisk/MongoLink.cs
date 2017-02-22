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
        public static MA.MongoLink CreateMongoLink(string serverLink = "mongodb://localhost:27017", string databaseName = "project", string collectionName = "bhomObjects")
        {
            return new MA.MongoLink(serverLink, databaseName, collectionName);
        }

        /// <summary></summary>
        public static void ToMongo(MA.MongoLink mongoLink, object[] objects, string key, List<string> tags, bool active = false)
        {
            if (!active) return;

            List<BHB.BHoMObject> bhomObjects = new List<BHB.BHoMObject>();
            //foreach (object o in objects)
            //    bhomObjects.Add((BHB.BHoMObject)o);
            //mongoLink.Push(bhomObjects, key, tags);
            mongoLink.Push(objects, key, tags);
        }

        /// <summary></summary>
        public static IEnumerable<object> FromMongo(MA.MongoLink mongoLink, List<string> query, bool toJson = false, bool active = false)
        {
            return mongoLink.Query(query, toJson);
        }

        /// <summary></summary>
        public static void DeleteObjects(string filter = "{}", bool active = false)
        {
            if (active)
                MongoLink.DeleteObjects(filter);
        }
    }
}
