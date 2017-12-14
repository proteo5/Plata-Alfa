using MongoDB.Bson;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace PlataAlfa.DB.MongoDB
{
    public static class MongoExtensions
    {
        /// <summary>
        /// deserializes this bson doc to a .net dynamic object
        /// </summary>
        /// <param name="bson">bson doc to convert to dynamic</param>
        public static dynamic ToDynamic(this BsonDocument bson)
        {
            var json = bson.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });
            dynamic e = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(json);
            BsonValue id;
            if (bson.TryGetValue("_id", out id))
            {
                // Lets set _id again so that its possible to save document.
                e._id = new ObjectId(id.ToString());
            }
            return e;
        }
    }
}
