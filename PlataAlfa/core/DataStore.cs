using MongoDB.Bson;
using PlataAlfa.DB.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PlataAlfa.core
{
    public class DataStore
    {
        private readonly CRUD mongoDBCrud = null;

        public DataStore(string entity)
        {
            string server = Program.Configuration["conString.server"];
            string database = Program.Configuration["conString.database"];
            mongoDBCrud = new CRUD(entity, database, server);
        }

        public string GenerateNewId()
        {
            return mongoDBCrud.GenerateNewId().ToString();
        }

        public IQueryable<BsonDocument> Query()
        {
            return mongoDBCrud.Query();
        }

        public void Insert(string document)
        {
            BsonDocument documentbson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(document);
            mongoDBCrud.Insert(documentbson);
        }

        public void Save(string document)
        {
            BsonDocument documentbson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(document);
            mongoDBCrud.Save(documentbson);
        }
    }
}
