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
    public class ReloadEntityCommandTests
    {
        string connectionName = "LabDb_DEV";

        [TestMethod()]
        public void SameContextTest()
        {
            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                Aspect testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                if (testEntry != null)
                    testContext.Entry(testEntry).State = System.Data.Entity.EntityState.Deleted;

                testContext.Aspects.Add(new Aspect() { Code = "999", Name = "TEST" });
                testContext.SaveChanges();
            }

            LabDbEntities persistingContext = new LabDbEntities(connectionName);
            Aspect persistingEntry;

            persistingEntry = persistingContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
            

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                Aspect testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                testEntry.Name = "MODIFIED_NAME";
                testContext.SaveChanges();
            }


            ReloadEntityCommand reloadEntityCommandTest = new ReloadEntityCommand(persistingEntry);
            reloadEntityCommandTest.Execute(persistingContext);
            Assert.IsTrue(persistingEntry.Name == "MODIFIED_NAME");
        }

        [TestMethod()]
        public void DifferentContextTest()
        {
            Aspect testEntry;
            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                if (testEntry != null)
                    testContext.Entry(testEntry).State = System.Data.Entity.EntityState.Deleted;

                testContext.Aspects.Add(new Aspect() { Code = "999", Name = "TEST" });
                testContext.SaveChanges();
                testEntry = null;
            }

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                Assert.IsNotNull(testEntry);
            }
            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                Aspect tempEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                tempEntry.Name = "MODIFIED_NAME";
                testContext.SaveChanges();
            }
            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                testEntry.Name = "ASDFASDFASDFASDF";
                ReloadEntityCommand reloadEntityCommandTest = new ReloadEntityCommand(testEntry);
                reloadEntityCommandTest.Execute(testContext);
                Assert.IsTrue(testEntry.Name == "MODIFIED_NAME");
            }

        }
    }
}