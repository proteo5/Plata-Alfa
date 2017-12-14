using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataAlfa.DB.MongoDB
{
    public class Environment
    {
        internal static string conString = "mongodb://localhost/plataalfa"; //ConfigurationManager.AppSettings["MongoServer"];

        internal static string dbString = "plataalfa"; // ConfigurationManager.AppSettings["MongoDatabase"];

        internal static MongoClient server = new MongoClient(conString);
        internal static IMongoDatabase database = server.GetDatabase(dbString);

        public static List<string> ListTables()
        {
            var tables = new List<string>();

            var collection = database.ListCollectionsAsync().Result.ToListAsync<BsonDocument>().Result;
            foreach (var item in collection)
                if (item["name"] != "system.indexes")
                    tables.Add(item["name"].ToString());


            return tables;
        }
    }
}
