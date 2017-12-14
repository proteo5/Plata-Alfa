using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlataAlfa.DB.MongoDB;

namespace PlataAlfa.DB.MongoDB.Test
{
    [TestClass]
    public class EnvironmentTest
    {
        internal Environment env;

        public EnvironmentTest()
        {
            env = new Environment("plataalfa", "localhost");
        }

        [TestMethod]
        public void ListDatabasesTest()
        {
            var x = env.ListTables();
        }
    }
}
