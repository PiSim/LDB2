using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LabDbContext.EntityExtensions.Tests
{
    [TestClass()]
    public class StandardExtensionTests
    {
        #region Methods

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

        #endregion Methods
    }
}