using LabDbContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Materials.Commands.Tests
{
    [TestClass()]
    public class BatchUpdateCommandTests
    {
        #region Fields

        private LabDbEntities testContext;

        #endregion Fields

        #region Methods

        [TestMethod()]
        public void BatchUpdateCommandTest()
        {
            testContext = new LabDbEntities()
            {
            };
        }

        [TestMethod()]
        public void ExecuteTest()
        {
            Assert.Fail();
        }

        #endregion Methods
    }
}