using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabDbContext;
using System.Data.Entity.Core.EntityClient;

namespace Infrastructure.Commands.Tests
{
    [TestClass()]
    public class DeleteEntityCommandTests
    {
        string connectionName = "name=LabDbTest";

        [TestMethod()]
        public void ExecuteTest()
        {
            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                Assert.IsNull(testContext.Aspects.FirstOrDefault(asp => asp.Code == "999"));
                testContext.Aspects.Add(new Aspect() { Code = "999", Name = "DELETION_TEST" });
                testContext.SaveChanges();
            }
            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                Aspect testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                DeleteEntityCommand deleteEntityCommandTest = new DeleteEntityCommand(testEntry);
                deleteEntityCommandTest.Execute(testContext);
            }
            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                Assert.IsNull(testContext.Aspects.FirstOrDefault(asp => asp.Code == "999"));
            }
        }
    }
}