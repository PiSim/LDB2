using DataAccess;
using Infrastructure.Queries;
using LabDbContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Infrastructure.Commands.Tests
{
    [TestClass()]
    public class UpdateEntityCommandTests
    {
        #region Fields

        private IDataService<LabDbEntities> _labDbData;
        private IDbContextFactory<LabDbEntities> factory;

        #endregion Fields

        #region Constructors

        public UpdateEntityCommandTests()
        {
            factory = new LabDBContextFactory("LabDb_DEV");
            _labDbData = new LabDbData(factory);
        }

        #endregion Constructors

        #region Methods

        [TestMethod()]
        public void DifferentContextTest()
        {
            Aspect testEntry;
            using (LabDbEntities testContext = factory.Create())
            {
                testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                if (testEntry != null)
                    testContext.Entry(testEntry).State = System.Data.Entity.EntityState.Deleted;

                testContext.Aspects.Add(new Aspect() { Code = "999", Name = "TEST" });
                testContext.SaveChanges();
                testEntry = null;
            }

            using (LabDbEntities testContext = factory.Create())
            {
                testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                Assert.IsNotNull(testEntry);
            }

            using (LabDbEntities testContext = factory.Create())
            {
                testEntry.Name = "MODIFIED_NAME";
                (new UpdateEntityCommand(testEntry)).Execute(testContext);
            }
            using (LabDbEntities testContext = factory.Create())
            {
                Aspect tempEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                Assert.IsTrue(tempEntry.Name == "MODIFIED_NAME");
            }
        }

        [TestMethod()]
        public void MultipleDifferentContextsTest()
        {
            Aspect testEntry;
            using (LabDbEntities testContext = factory.Create())
            {
                testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                if (testEntry != null)
                    testContext.Entry(testEntry).State = System.Data.Entity.EntityState.Deleted;

                testContext.Aspects.Add(new Aspect() { Code = "999", Name = "TEST" });
                testContext.SaveChanges();
                testEntry = null;
            }

            using (LabDbEntities testContext = factory.Create())
            {
                testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                Assert.IsNotNull(testEntry);
            }

            for (int i = 0; i != 5; i++)
            {
                testEntry.Name = i.ToString();
                _labDbData.Execute(new UpdateEntityCommand(testEntry));
            }

            testEntry.Name = "MODIFIED_NAME";
            _labDbData.Execute(new UpdateEntityCommand(testEntry));

            using (LabDbEntities testContext = factory.Create())
            {
                Aspect tempEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                Assert.IsTrue(tempEntry.Name == "MODIFIED_NAME");
            }
        }

        [TestMethod()]
        public void SameContextTest()
        {
            using (LabDbEntities testContext = factory.Create())
            {
                Aspect testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                if (testEntry != null)
                    testContext.Entry(testEntry).State = System.Data.Entity.EntityState.Deleted;

                testContext.Aspects.Add(new Aspect() { Code = "999", Name = "TEST" });
                testContext.SaveChanges();
            }

            LabDbEntities persistingContext = factory.Create();
            Aspect persistingEntry;

            persistingEntry = persistingContext.Aspects.FirstOrDefault(asp => asp.Code == "999");

            persistingEntry.Name = "MODIFIED_NAME";
            UpdateEntityCommand reloadEntityCommandTest = new UpdateEntityCommand(persistingEntry);
            reloadEntityCommandTest.Execute(persistingContext);

            using (LabDbEntities testContext = factory.Create())
            {
                Aspect testEntry = testContext.Aspects.FirstOrDefault(asp => asp.Code == "999");
                Assert.IsTrue(persistingEntry.Name == "MODIFIED_NAME");
            }
        }

        [TestMethod()]
        public void UpdateQueriedEntriesWithNoModificationsTest()
        {
            foreach (ExternalReport exrep in _labDbData.RunQuery(new ExternalReportsQuery()).ToList())
            {
                _labDbData.Execute(new UpdateEntityCommand(exrep));
            }
        }

        #endregion Methods
    }
}