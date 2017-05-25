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
    public class PeopleServiceTests
    {
        [TestMethod()]
        public void GetPeopleTest()
        {
            IEnumerable<Person> _allPeopleList = PeopleService.GetPeople();
            IEnumerable<Person> _perRolePeopleList = PeopleService.GetPeople(PersonRoleNames.MaterialTestingTech);

            Assert.IsTrue(_allPeopleList.Count() != 0);
            Assert.IsTrue(_perRolePeopleList.Count() != 0);
        }
    }
}