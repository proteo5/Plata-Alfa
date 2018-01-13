using MongoDB.Bson;
using PlataAlfa.DB.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataAlfa.core
{
    public class DataSteward
    {
        internal readonly CRUD crud;

        public DataSteward()
        {
            string entityName = this.GetType().Name.Replace("DS", string.Empty);
            //crud = new CRUD(entityName, Program.Configuration["conString.database"], Program.Configuration["conString.server"]);
            crud = new CRUD(entityName, "plataalfa", "localhost");
        }

        public IQueryable<BsonDocument> Query()
        {
            return crud.Query();
        }

        public Envelope<ObjectId> GenerateNewId()
        {
            try
            {
                return new Envelope<ObjectId>() { Result = "ok", Data = crud.GenerateNewId() };
            }
            catch (Exception ex)
            {
                return new Envelope<ObjectId>() { Result = "error", Message = ex.Message };
            }

        }

        public Envelope<List<BsonDocument>> GetAll()
        {
            try
            {
                var data = crud.Query().ToList();
                if (data.Count() != 0)
                    return new Envelope<List<BsonDocument>>() { Result = "ok", Data = data };
                else
                    return new Envelope<List<BsonDocument>>() { Result = "notSuccess", Message = "Not Found" };
            }
            catch (Exception ex)
            {
                return new Envelope<List<BsonDocument>> { Result = "error", Message = ex.Message };
            }

        }

        public Envelope GetByID(ObjectId id)
        {
            try
            {
                var data = crud.Query().Where(d => d["_id"] == id);
                if (data.Count() != 0)
                    return new Envelope<BsonDocument>() { Result = "ok", Data = data.FirstOrDefault() };
                else
                    return new Envelope<BsonDocument>() { Result = "notSuccess", Message = "Not Found" };
            }
            catch (Exception ex)
            {
                return new Envelope() { Result = "error", Message = ex.Message };
            }

        }

        public Envelope Insert(BsonDocument data)
        {
            try
            {
                crud.Insert(data);
                return new Envelope() { Result = "ok" };
            }
            catch (Exception ex)
            {
                return new Envelope() { Result = "error", Message = ex.Message };
            }
        }

        public Envelope Save(BsonDocument data)
        {
            try
            {
                crud.Save(data);
                return new Envelope() { Result = "ok" };
            }
            catch (Exception ex)
            {
                return new Envelope() { Result = "error", Message = ex.Message };
            }
        }

        public Envelope Delete(BsonDocument data)
        {
            try
            {
                crud.Delete(data);
                return new Envelope() { Result = "ok" };
            }
            catch (Exception ex)
            {
                return new Envelope() { Result = "error", Message = ex.Message };
            }
        }


    }
}
