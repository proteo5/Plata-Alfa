﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlataAlfa.DB.MongoDB
{
     public class CRUD
    {
        internal IMongoCollection<BsonDocument> _collection;

        public CRUD(string Table)
        {
            _collection = Environment.database.GetCollection<BsonDocument>(Table);
        }

        public IQueryable<BsonDocument> Query()
        {
            return _collection.AsQueryable<BsonDocument>();
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

    }
}