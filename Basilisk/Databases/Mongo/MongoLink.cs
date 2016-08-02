using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DE = Databases_Engine;

namespace Databases.Mongo
{
    public static class MongoLink
    {
        /// <summary></summary>
        public static DE.Mongo.MongoLink CreateMongoLink(string serverLink = "mongodb://host:27017", string databaseName = "project", string collectionName = "bhomObjects")
        {
            return new DE.Mongo.MongoLink(serverLink, databaseName, collectionName);
        }

        /// <summary></summary>
        public static void ToMongo(DE.Mongo.MongoLink mongoLink, object[] objects, string key, bool active = false)
        {
            if (!active) return;

            List<BHoM.Global.BHoMObject> bhomObjects = new List<BHoM.Global.BHoMObject>();
            foreach (object o in objects)
                bhomObjects.Add((BHoM.Global.BHoMObject)o);
            mongoLink.SaveObjects(bhomObjects, key);
        }

        /// <summary></summary>
        public static IEnumerable<object> FromMongo(DE.Mongo.MongoLink mongoLink, string filter)
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
