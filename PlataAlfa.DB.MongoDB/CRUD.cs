using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PlataAlfa.DB.MongoDB
{
    public class CRUD
    {
        internal readonly Environment environment = null;
        internal IMongoCollection<BsonDocument> _collection;

        public CRUD(string Table, string database, string server = "localhost")
        {
            environment = new Environment(database, server);
            _collection = environment.database.GetCollection<BsonDocument>(Table);
        }

        public IQueryable<BsonDocument> Query()
        {
            return _collection.AsQueryable<BsonDocument>();
        }

        public List<BsonDocument> GetFiltered(Expression<Func<BsonDocument, bool>> predicate)
        {
            return _collection.AsQueryable().Where(predicate).ToListAsync().Result ;
        }

        public List<BsonDocument> GetAll()
        {
            return _collection.AsQueryable().ToListAsync().Result;
        }

        public void Insert(BsonDocument document)
        {
            BsonValue id;
            if (document.TryGetValue("_id", out id))
            {
                throw new Exception("Can't insert document when _id is already defined, this could cause unexpected duplicates.");
            }
            else
            {
                _collection.InsertOne(document);
            }
        }

        public void Save(BsonDocument document)
        {
            BsonValue id;

            if (document.TryGetValue("_id", out id))
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                _collection.ReplaceOne(filter, document);
            }
            else
            {
                throw new Exception("Can't save an document when _id is not defined.");
            }
        }

        public void Delete(BsonDocument document)
        {
            BsonValue id;

            if (document.TryGetValue("_id", out id))
            {
                _collection.DeleteOne(Builders<BsonDocument>.Filter.Eq("_id", document["_id"]));
            }
            else
            {
                throw new Exception("Can't delete an document when _id is not defined.");
            }
        }

        public ObjectId GenerateNewId()
        {
            return ObjectId.GenerateNewId();
        }

    }
}