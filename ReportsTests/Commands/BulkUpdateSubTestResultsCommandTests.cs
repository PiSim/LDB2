using DataAccess;
using LabDbContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reports.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Commands.Tests
{
    [TestClass()]
    public class BulkUpdateSubTestResultsCommandTests
    {
        string _connectionName = "LabDb_DEV";
        List<SubTest> _testList;
        IDataService<LabDbEntities> _labDbData;
        IDbContextFactory<LabDbEntities> factory;

        public BulkUpdateSubTestResultsCommandTests()
        {

            factory = new LabDBContextFactory(_connectionName);
            _labDbData = new LabDbData(factory);
        }

        [TestMethod()]
        public void BulkUpdateSubTestResultsCommandTest()
        {
            _testList = new List<SubTest>()
            {
                new SubTest() { TestID = 1, Result = "111"},
                new SubTest() {  TestID = 1, Result = "222"},
                new SubTest() {  TestID = 1, Result = "333"},
                new SubTest() {  TestID = 1, Result = "444"},
                new SubTest() {  TestID = 1, Result = "555"},
                new SubTest() {  TestID = 1, Result = "666"},
                new SubTest() {  TestID = 1, Result = "777"},
                new SubTest() {  TestID = 1, Result = "888"},
                new SubTest() {  TestID = 1, Result = "999"},
            };

            using (LabDbEntities testContext = factory.Create())
            {
                foreach (SubTest asp in _testList)
                {
                    testContext.SubTests.Add(asp);
                }
                testContext.SaveChanges();
            }
            foreach (SubTest sts in _testList)
                sts.Result = "CHANGED_RESULT";

            _labDbData.Execute(new BulkUpdateSubTestResultsCommand(_testList));

            using (LabDbEntities testContext = factory.Create())
            {
                foreach (SubTest asp in _testList)
                {
                    SubTest attached = testContext.SubTests.Find(asp.ID);
                    Assert.IsTrue(attached.Result == "CHANGED_RESULT");
                }
            }
        }

    }
}