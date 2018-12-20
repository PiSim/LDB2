using DataAccess;
using LabDbContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace Reports.Commands.Tests
{
    [TestClass()]
    public class BulkUpdateSubTestResultsCommandTests
    {
        #region Fields

        private string _connectionName = "LabDb_DEV";
        private IDataService<LabDbEntities> _labDbData;
        private List<SubTest> _testList;
        private IDbContextFactory<LabDbEntities> factory;

        #endregion Fields

        #region Constructors

        public BulkUpdateSubTestResultsCommandTests()
        {
            factory = new LabDBContextFactory(_connectionName);
            _labDbData = new LabDbData(factory);
        }

        #endregion Constructors

        #region Methods

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

        #endregion Methods
    }
}