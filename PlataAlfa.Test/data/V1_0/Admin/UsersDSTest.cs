using MongoDB.Bson;
using System.Linq;
using PlataAlfa.core;
using System;
using Xunit;
using PlataAlfa.data.V1_0.Admin;

namespace PlataAlfa.Test.data.V1_0.Admin
{
    public class UsersDSTest
    {
        private readonly UsersDS usersDS;

        public UsersDSTest()
        {
            usersDS = new UsersDS();
        }

        [Fact]
        public void SeedsTest()
        {
            usersDS.Seeds();
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

            usersDS.Insert(data.ToJson());

        }

        [Fact]
        public void GetAllTest()
        {
            var y = usersDS.GetAll();
            Assert.Equal("ok", y.Result);
        }

        [Fact]
        public void GetByID()
        {
            var y = usersDS.GetByID("5a59fb202fe24304fcc21773");
            Assert.Equal("ok", y.Result);
        }

        [Fact]
        public void GetByUser()
        {
            var y = usersDS.GetByUser("user");
            Assert.Equal("ok", y.Result);
        }


        [Fact]
        public void UpdateTest()
        {
            var data = usersDS.Query().Where(x=> x["_id"] == new ObjectId("5a59fd7c34c747107055550f")).FirstOrDefault();
            data["name"] = "User updated";
            data["IsActive"] = false;

            usersDS.Save(data.ToJson());
        }

        [Fact]
        public void DeleteTest()
        {
            var data = usersDS.Query().Where(x => x["_id"] == new ObjectId("5a59fb202fe24304fcc21773")).FirstOrDefault();

            usersDS.Delete(data.ToJson());
        }
    }
}
