using System.Linq;

namespace LabDbContext
{
    public partial class Material
    {
        #region Methods

        /// <summary>
        /// Removes the ExternalConstruction Association from the material
        /// </summary>
        public void UnsetConstruction()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                Material attachedEntry = entities.Materials.First(mat => mat.ID == ID);

                attachedEntry.ExternalConstruction.Materials.Remove(attachedEntry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}