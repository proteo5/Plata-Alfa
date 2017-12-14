using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlataAlfa.DB.MongoDB;

namespace PlataAlfa.DB.MongoDB.Test
{
    [TestClass]
    public class EnvironmentTest
    {
        [TestMethod]
        public void ListDatabasesTest()
        {
            var x = Environment.ListTables();
        }
    }
}
