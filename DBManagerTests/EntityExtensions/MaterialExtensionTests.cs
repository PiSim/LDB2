using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace LabDbContext.EntityExtensions.Tests
{
    [TestClass()]
    public class MaterialExtensionTests
    {
        #region Methods

        [TestMethod()]
        public void MaterialLoadTest()
        {
            Material tempMaterial;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                tempMaterial = entities.Materials.First(mat => mat.ID == 1);
            }

            Assert.IsNotNull(tempMaterial);

            tempMaterial.Load();

            Assert.IsNotNull(tempMaterial.Recipe);
        }

        #endregion Methods
    }
}