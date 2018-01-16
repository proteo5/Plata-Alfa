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
        public Envelope<dynamic> GetByUser(dynamic data)
        {
            try
            {
                string user = data.user;
                //var dataSet = crud.Query().Where(d => d["user"] == user);
                var dataSet = crud.GetFiltered(d => d["user"] == user);
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

        public void Seeds()
        {
            
            var data = new BsonDocument
                {
                    { "name", "Administrator" },
                    { "lastname", "of the System" },
                    { "user", "admin" },
                    { "password", "123" },
                    { "isActive", true }
                };

            crud.Insert(data);
        }
    }
}
