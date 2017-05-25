using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tests
{
    [TestClass()]
    public class CommonProceduresTests
    {
        [TestMethod()]
        public void GetMaterialTest()
        {
            Material tempEntry = CommonProcedures.GetMaterial("31SA",
                                                            "999",
                                                            "XXX",
                                                            "XXXX");

            Assert.IsNotNull(tempEntry);

            // Post-test cleanup

            Aspect tempAspect = tempEntry.Construction.Aspect;
            Recipe tempRecipe = tempEntry.Recipe;
            Construction tempConstruction = tempEntry.Construction;
            
            tempEntry.Delete();
            tempConstruction.Delete();
            tempAspect.Delete();
            tempRecipe.Delete();

            tempEntry = CommonProcedures.GetMaterial("11LA",
                                                     "143",
                                                     "EFC",
                                                     "3770");

            Assert.IsNotNull(tempEntry);
            
        }
    }
}