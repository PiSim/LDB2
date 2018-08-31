using DBManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBManager.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBManager.EntityExtensions;

namespace DBManager.Services.Tests
{
    [TestClass()]
    public class SpecificationServiceTests
    {
        [TestMethod()]
        public void SpecificationLoadTest()
        {
            Specification tempSpec;

            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
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
    }
}