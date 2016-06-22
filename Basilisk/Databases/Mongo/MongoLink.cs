using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases.Mongo
{
    public static class MongoLink
    {
        /// <summary></summary>
        public static BHoM_Engine.Databases.Mongo.MongoLink CreateMongoLink(string serverLink = "mongodb://host:27017", string databaseName = "project", string collectionName = "bhomObjects")
        {
            return new BHoM_Engine.Databases.Mongo.MongoLink(serverLink, databaseName, collectionName);
        }

        /// <summary></summary>
        public static void SaveObjects(BHoM_Engine.Databases.Mongo.MongoLink mongoLink, object[] objects)
        {
            List<BHoM.Global.BHoMObject> bhomObjects = new List<BHoM.Global.BHoMObject>();
            foreach (object o in objects)
                bhomObjects.Add((BHoM.Global.BHoMObject)o);
            mongoLink.SaveObjects(bhomObjects);
        }


        /// <summary></summary>
        public static void SaveObject(BHoM_Engine.Databases.Mongo.MongoLink mongoLink, object obj)
        {
            mongoLink.SaveObject((BHoM.Global.BHoMObject)obj);
        }

        /// <summary></summary>
        public static IEnumerable<object> GetObjects(BHoM_Engine.Databases.Mongo.MongoLink mongoLink, string filter)
        {
            return mongoLink.GetObjects(filter);
        }
    }
}
