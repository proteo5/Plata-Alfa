using MongoDB.Bson;
using MongoDB.Bson.IO;
using PlataAlfa.DB.MongoDB;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataAlfa.core
{
    public class DataSteward
    {
        internal readonly CRUD crud;
        private readonly JsonWriterSettings jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };

        public DataSteward()
        {
            string entityName = this.GetType().Name.Replace("DS", string.Empty).ToLower();
            //crud = new CRUD(entityName, Program.Configuration["conString.database"], Program.Configuration["conString.server"]);
            crud = new CRUD(entityName, "plataalfa", "localhost");
        }

        public IQueryable<BsonDocument> Query()
        {
            return crud.Query();
        }

        public Envelope<string> GenerateNewId()
        {
            try
            {
                return new Envelope<string>() { Result = "ok", Data = crud.GenerateNewId().ToString() };
            }
            catch (Exception ex)
            {
                return new Envelope<string>() { Result = "error", Message = ex.Message };
            }

        }

        public Envelope<string> GetAll()
        {
            try
            {
                var data = crud.Query().ToList();

                if (data.Count() != 0)
                {
                    return new Envelope<string>() { Result = "ok", Data = data.ToJsonArray() };
                }
                else
                {
                    return new Envelope<string>() { Result = "notSuccess", Message = "Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new Envelope<string> { Result = "error", Message = ex.Message };
            }

        }

        public Envelope<string> GetByID(string id)
        {
            try
            {
                var data = crud.Query().Where(d => d["_id"] == new ObjectId(id));
                if (data.Count() != 0)
                    return new Envelope<string>() { Result = "ok", Data = data.FirstOrDefault().ToJson(jsonWriterSettings) };
                else
                    return new Envelope<string>() { Result = "notSuccess", Message = "Not Found" };
            }
            catch (Exception ex)
            {
                return new Envelope<string>() { Result = "error", Message = ex.Message };
            }

        }

        public Envelope Insert(string data)
        {
            try
            {
                MongoDB.Bson.BsonDocument document
                     = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(data);
                crud.Insert(document);
                return new Envelope() { Result = "ok" };
            }
            catch (Exception ex)
            {
                return new Envelope() { Result = "error", Message = ex.Message };
            }
        }

        public Envelope Save(string data)
        {
            try
            {
                MongoDB.Bson.BsonDocument document
                    = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(data);
                crud.Save(document);
                return new Envelope() { Result = "ok" };
            }
            catch (Exception ex)
            {
                return new Envelope() { Result = "error", Message = ex.Message };
            }
        }

        public Envelope Delete(string data)
        {
            try
            {
                MongoDB.Bson.BsonDocument document
                    = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(data);
                crud.Delete(document);
                return new Envelope() { Result = "ok" };
            }
            catch (Exception ex)
            {
                return new Envelope() { Result = "error", Message = ex.Message };
            }
        }
    }
    public static class DataStewardExtensions
    {
        private static readonly JsonWriterSettings jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };

        public static string ToJsonArray(this List<BsonDocument> jsonArray)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");

            foreach (var doc in jsonArray)
                json.Append($"{doc.ToJson(jsonWriterSettings)},");

            json.Length--;
            json.Append("]");
            return json.ToString();
        }

    }
}
