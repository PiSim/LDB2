using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabDbContext;

namespace Infrastructure.Commands.Tests
{
    [TestClass()]
    public class InsertEntityCommandTests
    {
        string connectionName = "name=LabDbTest";

        public void InsertEntityCommandTest()
        {

        }

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
    }
}