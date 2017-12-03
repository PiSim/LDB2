using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBManager.EntityExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBManager.Services;

namespace DBManager.EntityExtensions.Tests
{
    [TestClass()]
    public class StandardExtensionTests
    {
        [TestMethod()]
        public void StandardCRUDTest()
        {
            Std tempStd;
            tempStd = new Std();
            tempStd.Name = "PROVA";
            tempStd.OrganizationID = 1;
            tempStd.Create();
            
            tempStd.Delete();
        }
    }
}