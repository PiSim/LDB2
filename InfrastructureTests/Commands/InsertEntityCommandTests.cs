using LabDbContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Infrastructure.Commands.Tests
{
    [TestClass()]
    public class InsertEntityCommandTests
    {
        #region Fields

        private string connectionName = "name=LabDbTest";

        #endregion Fields

        #region Methods

        [TestMethod()]
        public void ExecuteTest()
        {
            Aspect testEntry = new Aspect() { Code = "999", Name = "TEST_ENTRY" };

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                Aspect existingEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                if (existingEntry != null)
                {
                    testContext.Entry(existingEntry).State = System.Data.Entity.EntityState.Deleted;
                    testContext.SaveChanges();
                }
            }

            new InsertEntityCommand(testEntry).Execute(new LabDbEntities(connectionName));

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                Assert.IsNotNull(testContext.Aspects.FirstOrDefault(asp => asp.Code == "999"));
            }
        }

        public void InsertEntityCommandTest()
        {
        }

        #endregion Methods
    }
}