using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataAlfa.DB.MongoDB
{
    public class Environment
    {
        internal static MongoClient mongoServer = null;
        internal IMongoDatabase database = null;

        public Environment(string database, string server = "localhost")
        {
            if (mongoServer == null)
                mongoServer = new MongoClient($"mongodb://{server}/{database}");

            this.database = mongoServer.GetDatabase(database);
        }

        ~Environment()
        {
            this.database = null;
        }

        public List<string> ListTables()
        {
            var tables = new List<string>();

            var collection = database.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result;
            foreach (var item in collection)
            {
                if (item["name"] != "system.indexes")
                {
                    tables.Add(item["name"].ToString());
                }
            }
            return tables;
        }
    }
}
