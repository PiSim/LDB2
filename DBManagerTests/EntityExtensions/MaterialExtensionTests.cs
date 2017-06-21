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
    public class MaterialExtensionTests
    {
        

        [TestMethod()]
        public void MaterialLoadTest()
        {
            Material tempMaterial;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                tempMaterial = entities.Materials.First(mat => mat.ID == 1);
            }

            Assert.IsNotNull(tempMaterial);

            tempMaterial.Load();
            
            Assert.IsNotNull(tempMaterial.Recipe);

        }
    }
}