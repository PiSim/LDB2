using DataAccess;
using LabDbContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Materials.Commands.Tests
{
    [TestClass()]
    public class BatchUpdateCommandTests
    {
        #region Fields

        string connectionString = "connection string=&quot;server=192.168.1.22;user id=LabDBClient;Pwd=910938356;persistsecurityinfo=True;database=labdb_dev;port=3306;SslMode=none&quot;";

        #endregion Fields

        #region Methods

        [TestMethod()]
        public void BatchUpdateCommandTest()
        {
        }

        [TestMethod()]
        public void ExecuteTest()
        {
            Assert.Fail();
        }

        #endregion Methods
    }
}