using MongoDB.Bson;
using System.Linq;
using PlataAlfa.core;
using System;
using Xunit;

namespace PlataAlfa.Test.core
{
    public class DataStewardTest
    {
        private readonly DataSteward dataSteward;

        public DataStewardTest()
        {
            dataSteward = new DataSteward();
        }

        [Fact]
        public void GenerateNewIdTest()
        {
            try
            {
                var id = dataSteward.GenerateNewId();
                Assert.Equal("ok", id.Result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Fact]
        public void GetAllQueryTest()
        {
            var y = dataSteward.Query().ToJson();
        }

        [Fact]
        public void GetAllTest()
        {
            var y = dataSteward.GetAll();
        }

        [Fact]
        public void GetByID()
        {
            var y = dataSteward.GetByID("5a59ec588236110698379fcd");
        }

        [Fact]
        public void InsertTest()
        {
            var data = new BsonDocument
                {
                    { "name", "User" },
                    { "lastname", "to Test " },
                    { "user", "user" },
                    { "Password", "123" },
                    { "IsActive", true }
                };

            dataSteward.Insert(data.ToJson());

        }

        [Fact]
        public void UpdateTest()
        {
            //var db = new CRUD("DataTest");
            var data = dataSteward.Query().FirstOrDefault();
            data["name"] = "User updated";

            dataSteward.Save(data.ToJson());
        }

        [Fact]
        public void DeleteTest()
        {
            //var db = new CRUD("DataTest");
            var data = dataSteward.Query().FirstOrDefault().ToJson();

            dataSteward.Delete(data);
        }
    }
}
