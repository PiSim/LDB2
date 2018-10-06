using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LabDbContext
{
    public static class MaterialExtension
    {
        #region Methods

        public static void Create(this Material entry)
        {
            // Inserts a new Material entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Materials.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Update(this Material entry)
        {
            // Updates the DB values of a Material entity

            using (LabDbEntities entities = new LabDbEntities())
            {
                Material tempEntry = entities.Materials.First(mat => mat.ID == entry.ID);
                entities.Entry(tempEntry).CurrentValues.SetValues(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }

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