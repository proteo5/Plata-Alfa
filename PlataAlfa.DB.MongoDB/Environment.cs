using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataAlfa.DB.MongoDB
{
    public class Environment
    {
        internal readonly  MongoClient server = null;
        internal readonly IMongoDatabase database = null;

        public Environment( string database, string server ="localhost")
        {
            this.server = new MongoClient($"mongodb://{server}/{database}");
            this.database =  this.server.GetDatabase(database);
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
