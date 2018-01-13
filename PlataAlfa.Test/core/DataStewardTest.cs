using MongoDB.Bson;
using System.Linq;
using PlataAlfa.core;
using System;
using Xunit;

namespace PlataAlfa.Test
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
            //var x = new CRUD("ticker_data");
            var y = dataSteward.Query().ToList();
        }

        [Fact]
        public void GetAllTest()
        {
            //var x = new CRUD("ticker_data");
            var y = dataSteward.GetAll();
        }

        [Fact]
        public void GetByID()
        {
            //var x = new CRUD("ticker_data");
            var y = dataSteward.GetByID(new ObjectId("5a59d9713fc8e72fb84023e7"));
        }

        [Fact]
        public void InsertTest()
        {
            //var db = new CRUD("DataTest");
            var data = new BsonDocument
                {
                    { "name", "joachim" }
                };

            dataSteward.Insert(data);

        }

        [Fact]
        public void UpdateTest()
        {
            //var db = new CRUD("DataTest");
            var data = dataSteward.Query().FirstOrDefault();
            data["name"] = "joachim modificado";

            dataSteward.Save(data);
        }

        [Fact]
        public void DeleteTest()
        {
            //var db = new CRUD("DataTest");
            var data = dataSteward.Query().FirstOrDefault();

            dataSteward.Delete(data);
        }
    }
}
