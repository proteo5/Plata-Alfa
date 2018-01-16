using MongoDB.Bson;
using MongoDB.Bson.IO;
using PlataAlfa.DB.MongoDB;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using MongoDB.Driver.Linq;

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

        internal IQueryable<BsonDocument> Query()
        {
            return crud.Query();
        }

        internal List<dynamic> GetFiltered(Expression<Func<BsonDocument, bool>> predicate)
        {
            return crud.GetFiltered(predicate).ToDynamicList();
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

        
        public Envelope<List<dynamic>> GetAll()
        {
            try
            {
               var data = crud.Query().ToList();
               // var data = crud.GetAll();

                if (data.Count() != 0)
                {
                    return new Envelope<List<dynamic>>() { Result = "ok", Data = data.ToDynamicList() };
                }
                else
                {
                    return new Envelope<List<dynamic>>() { Result = "notSuccess", Message = "Not Found" };
                }
            }
            catch (Exception ex)
            {
                return new Envelope<List<dynamic>> { Result = "error", Message = ex.Message };
            }

        }

        public Envelope<dynamic> GetByID(dynamic data)
        {
            try
            {
                string id = data._id;
                //var dataSet = crud.Query().Where(d => d["_id"] == new ObjectId(id));
                var dataSet = crud.GetFiltered(d => d["_id"] == new ObjectId(id));
                if (dataSet.Count() != 0)
                    return new Envelope<dynamic>() { Result = "ok", Data = dataSet.FirstOrDefault().ToDynamic() };
                else
                    return new Envelope<dynamic>() { Result = "notSuccess", Message = "Not Found" };
            }
            catch (Exception ex)
            {
                return new Envelope<dynamic>() { Result = "error", Message = ex.Message };
            }

        }

        public Envelope Insert(dynamic data)
        {
            try
            {
                MongoDB.Bson.BsonDocument document
                     = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(data.ToString());
                crud.Insert(document);
                return new Envelope() { Result = "ok" };
            }
            catch (Exception ex)
            {
                return new Envelope() { Result = "error", Message = ex.Message };
            }
        }

        public Envelope Save(dynamic data)
        {
            try
            {
                MongoDB.Bson.BsonDocument document
                    = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(data.ToString());
                document["_id"] = new ObjectId((string)document["_id"]);
                crud.Save(document);
                return new Envelope() { Result = "ok" };
            }
            catch (Exception ex)
            {
                return new Envelope() { Result = "error", Message = ex.Message };
            }
        }

        public Envelope Delete(dynamic data)
        {
            try
            {
                var response = this.GetByID(data);

                if (response.Result == "ok")
                {
                    var docJson = Newtonsoft.Json.JsonConvert.SerializeObject(response.Data);
                    MongoDB.Bson.BsonDocument document
                        = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(docJson);
                    document["_id"] = new ObjectId((string)document["_id"]);
                    crud.Delete(document);
                    return new Envelope() { Result = "ok" };
                }
                else
                {
                    return response;
                }
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

        public static string ToJsonArray(this List<BsonDocument> bsonArray)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");

            foreach (var doc in bsonArray)
                json.Append($"{doc.ToJson(jsonWriterSettings)},");

            json.Length--;
            json.Append("]");
            return json.ToString();
        }

        public static List<dynamic> ToDynamicList(this List<BsonDocument> bsonArray)
        {
            var list = new List<dynamic>();
            foreach (var doc in bsonArray)
                list.Add(doc.ToDynamic());

            return list;
        }

        public static dynamic ToDynamic(this BsonDocument bson)
        {
            var json = bson.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
            dynamic e = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(json);
            BsonValue id;
            if (bson.TryGetValue("_id", out id))
            {
                // Lets set _id again so that its possible to save document.
                e._id = id.ToString();
            }
            return e;
        }
    }
}
