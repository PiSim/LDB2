using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace LabDbContext.Services.Tests
{
    [TestClass()]
    public class SpecificationServiceTests
    {
        #region Methods

        [TestMethod()]
        public void SpecificationLoadTest()
        {
            Specification tempSpec;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                tempSpec = entities.Specifications.First(spec => spec.ID == 30);
            }

            tempSpec.Load();

            Assert.IsNotNull(tempSpec.Standard);
            Assert.IsNotNull(tempSpec.Standard.Organization);

            Assert.IsTrue(tempSpec.ControlPlans.Count != 0);
            Assert.IsTrue(tempSpec.SpecificationVersions.Count != 0);

            SpecificationVersion main = tempSpec.SpecificationVersions.First(spcv => spcv.IsMain);

            Assert.IsTrue(tempSpec.ControlPlans.Count != 0);
        }

        [TestMethod()]
        public void SpecificationVersionLoadTest()
        {
            SpecificationVersion temp;

            using (LabDbEntities entities = new LabDbEntities())
            {
                temp = entities.SpecificationVersions.First(spcv => spcv.ID == 26);
            }

            temp.Load();

            Assert.IsNotNull(temp.Specification);
            Assert.IsNotNull(temp.Specification.Standard);
            Assert.IsNotNull(temp.Specification.Standard.Organization);

            Assert.IsTrue(temp.Requirements.Count != 0);

            foreach (Requirement req in temp.Requirements)
            {
            }
        }

        #endregion Methods
    }
}