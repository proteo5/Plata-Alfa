using PlataAlfa.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace PlataAlfa.data.V1_0.Admin
{
    public class UsersDS : DataSteward
    {
        //ToDo: Implement
        //public Envelope<string> GetByUser(string user)
        //{
        //    try
        //    {
        //        var data = crud.Query().Where(d => d["user"] == user);
        //        if (data.Count() != 0)
        //            return new Envelope<string>() { Result = "ok", Data = data.FirstOrDefault().ToJson() };
        //        else
        //            return new Envelope<string>() { Result = "notSuccess", Message = "Not Found" };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new Envelope<string>() { Result = "error", Message = ex.Message };
        //    }

        //}

        public void Seeds()
        {
            
            var data = new BsonDocument
                {
                    { "name", "Administrator" },
                    { "lastname", "of the System" },
                    { "user", "admin" },
                    { "Password", "123" },
                    { "IsActive", true }
                };

            crud.Insert(data);
        }
    }
}
