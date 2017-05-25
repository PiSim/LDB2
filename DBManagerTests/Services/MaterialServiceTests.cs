using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBManager.EntityExtensions;
using DBManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services.Tests
{
    [TestClass()]
    public class MaterialServiceTests
    {
        [TestMethod()]
        public void AspectCRUDTest()
        {
            Aspect testEntry = new Aspect();
            testEntry.Code = "XXX";
            testEntry.Name = "TESTASPECT";

            testEntry.Create();

            testEntry.Name = "UPDATED";
            testEntry.Update();

            Aspect loadedEntry = MaterialService.GetAspect("XXX");

            Assert.IsTrue(loadedEntry.Name == "UPDATED");

            testEntry.Delete();

            loadedEntry = MaterialService.GetAspect("XXX");

            Assert.IsNull(loadedEntry);
        }

        [TestMethod()]
        public void BatchCRUDTest()
        {
            Batch testEntry = new Batch();
            testEntry.Number = "TESTBATCH";
            testEntry.Notes = "";

            testEntry.Create();

            testEntry.Number = "TESTB_U";
            testEntry.Material = MaterialService.GetMaterial(1);
            testEntry.Update();

            Batch loadedEntry = MaterialService.GetBatch("TESTB_U");

            Assert.IsNotNull(loadedEntry);
            Assert.IsTrue(loadedEntry.Number == "TESTB_U");

            testEntry.Delete();

            loadedEntry = MaterialService.GetBatch("TESTBATCH_UPD");

            Assert.IsNull(loadedEntry);
        }

        [TestMethod()]
        public void ConstructionCRUDTest()
        {
            Construction testEntry = new Construction();
            testEntry.Aspect = MaterialService.GetAspect("PSY");
            testEntry.Line = "XXX";
            testEntry.Type = MaterialService.GetMaterialType("31SA");

            testEntry.Create();

            testEntry.Line = "999";
            testEntry.Update();

            Construction loadedEntry = MaterialService.GetConstruction("31SA",
                                                                        "999",
                                                                        "PSY");

            Assert.IsNotNull(loadedEntry);
            Assert.IsTrue(loadedEntry.Line == "999");

            testEntry.Delete();

            loadedEntry = MaterialService.GetConstruction("31SA",
                                                          "999",
                                                          "PSY");

            Assert.IsNull(loadedEntry);
        }

        

        [TestMethod()]
        public void GetAspectTest()
        {
            Aspect readEntry = MaterialService.GetAspect("PSY");
            
            Assert.IsNotNull(readEntry);
            Assert.IsNotNull(readEntry.Code);
            Assert.IsNotNull(readEntry.ID);
            Assert.IsNotNull(readEntry.Name);
        }
    }

}