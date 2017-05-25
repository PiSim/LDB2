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
        public void MaterialCRUDTest()
        {
            Construction tempConstruction = MaterialService.GetConstruction(1);
            Recipe tempRecipe = MaterialService.GetRecipe("3D52");

            Material testEntry = new Material();
            testEntry.Construction = tempConstruction;
            testEntry.Recipe = tempRecipe;

            testEntry.Create();

            tempRecipe = MaterialService.GetRecipe("3D18");

            testEntry.Recipe = tempRecipe;
            testEntry.RecipeID = tempRecipe.ID;
            testEntry.Update();

            Material loadedEntry = MaterialService.GetMaterial(tempConstruction,
                                                                tempRecipe);

            Assert.IsNotNull(loadedEntry);
            Assert.IsTrue(loadedEntry.Recipe.Code == "3D18");

            testEntry.Delete();

            loadedEntry = MaterialService.GetMaterial(tempConstruction,
                                                                tempRecipe);

            Assert.IsNull(loadedEntry);
        }

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

            Assert.IsNotNull(tempMaterial.Construction);
            Assert.IsNotNull(tempMaterial.Recipe);

        }
    }
}