using LabDbContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Commands.Tests
{
    [TestClass()]
    public class BulkUpdateEntitiesCommandTests
    {
        #region Fields

        private List<Aspect> _aspectList;
        private string connectionName = "name=LabDbTest";

        #endregion Fields

        #region Constructors

        public BulkUpdateEntitiesCommandTests()
        {
        }

        #endregion Constructors

        #region Methods

        [TestMethod()]
        public void DifferentContextTest()
        {
            _aspectList = new List<Aspect>()
            {
                new Aspect() { Code = "111", Name = "TEST_ENTITY" },
                new Aspect() { Code = "222", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "333", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "444", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "555", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "666", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "777", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "888", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "999", Name = "TEST_ENTITY"  },
            };

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                foreach (Aspect asp in _aspectList)
                {
                    Aspect tempAspect = testContext.Aspects.FirstOrDefault(aet => aet.Code == asp.Code);
                    if (tempAspect != null)
                        testContext.Entry(tempAspect).State = System.Data.Entity.EntityState.Deleted;

                    testContext.Aspects.Add(asp);
                }
                testContext.SaveChanges();
            }

            List<Aspect> tempList = new List<Aspect>();
            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                foreach (Aspect asp in _aspectList)
                {
                    Aspect tempAspect = testContext.Aspects.FirstOrDefault(aet => aet.Code == asp.Code);
                    Assert.IsNotNull(tempAspect);
                    tempAspect.Name = "MODIFIED_NAME";
                    tempList.Add(tempAspect);
                }
            }

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                (new BulkUpdateEntitiesCommand(tempList)).Execute(testContext);
            }

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                foreach (Aspect asp in _aspectList)
                {
                    Aspect tempAspect = testContext.Aspects.FirstOrDefault(aet => aet.Code == asp.Code);
                    Assert.IsTrue(tempAspect.Name == "MODIFIED_NAME");
                }
            }
        }

        [TestMethod()]
        public void SameContextTest()
        {
            _aspectList = new List<Aspect>()
            {
                new Aspect() { Code = "111", Name = "TEST_ENTITY" },
                new Aspect() { Code = "222", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "333", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "444", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "555", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "666", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "777", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "888", Name = "TEST_ENTITY"  },
                new Aspect() { Code = "999", Name = "TEST_ENTITY"  },
            };

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                foreach (Aspect asp in _aspectList)
                {
                    Aspect tempAspect = testContext.Aspects.FirstOrDefault(aet => aet.Code == asp.Code);
                    if (tempAspect != null)
                        testContext.Entry(tempAspect).State = System.Data.Entity.EntityState.Deleted;

                    testContext.Aspects.Add(asp);
                }
                testContext.SaveChanges();
            }

            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                List<Aspect> tempList = new List<Aspect>();
                foreach (Aspect asp in _aspectList)
                {
                    Aspect tempAspect = testContext.Aspects.FirstOrDefault(aet => aet.Code == asp.Code);
                    Assert.IsNotNull(tempAspect);
                    tempAspect.Name = "MODIFIED_NAME";
                    tempList.Add(tempAspect);
                }

                (new BulkUpdateEntitiesCommand(tempList)).Execute(testContext);
            }
            using (LabDbEntities testContext = new LabDbEntities(connectionName))
            {
                foreach (Aspect asp in _aspectList)
                {
                    Aspect tempAspect = testContext.Aspects.FirstOrDefault(aet => aet.Code == asp.Code);
                    Assert.IsTrue(tempAspect.Name == "MODIFIED_NAME");
                }
            }
        }

        #endregion Methods
    }
}