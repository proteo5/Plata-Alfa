using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using MongoDB.Bson;

namespace PlataAlfa.DB.MongoDB.Test
{
    [TestClass]
    public class CRUDTest
    {
        internal CRUD x;

        public CRUDTest() {
            x = new CRUD("ticker_data", "plataalfa", "localhost");
        }

        [TestMethod]
        public void GetAllTest()
        {
            //var x = new CRUD("ticker_data");
            var y = x.Query().Where(z => z["success"] == true).ToList();
        }

        [TestMethod]
        public void InsertTest()
        {
            //var db = new CRUD("DataTest");
            var data = new BsonDocument
                {
                    { "name", "joachim" }
                };

            x.Insert(data);

        }

        [TestMethod]
        public void UpdateTest()
        {
            //var db = new CRUD("DataTest");
            var data = x.Query().FirstOrDefault();
            data["name"] = "joachim modificado";

            x.Save(data);
        }

        [TestMethod]
        public void DeleteTest()
        {
            //var db = new CRUD("DataTest");
            var data = x.Query().FirstOrDefault();

            x.Delete(data);
        }
    }
}
