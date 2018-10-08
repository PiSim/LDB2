﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            }

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
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