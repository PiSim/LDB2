using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services.Tests
{
    [TestClass()]
    public class OrganizationServiceTests
    {
        [TestMethod()]
        public void OrganizationLoadTest()
        {
            Organization temp;

            using (DBEntities entities = new DBEntities())
            {
                temp = entities.Organizations.First(org => org.ID == 1);
            }

            temp.Load();

            Assert.IsTrue(temp.RoleMapping.Count != 0);
        }

        [TestMethod()]
        public void GetOrganizationsTest()
        {
            IEnumerable<Organization> _tempList;

            _tempList = OrganizationService.GetOrganizations();

            Assert.IsNotNull(_tempList);
            Assert.IsTrue(_tempList.Count() != 0);

            _tempList = OrganizationService.GetOrganizations(OrganizationRoleNames.OEM);

            Assert.IsNotNull(_tempList);
            Assert.IsTrue(_tempList.Count() != 0);
            Assert.IsTrue(_tempList.All(org => org.RoleMapping
                                    .FirstOrDefault(orm => orm.Role.Name == OrganizationRoleNames.OEM)
                                    .IsSelected));
        }
    }
}