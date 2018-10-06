using System.Data.Entity;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class StandardFileExtension
    {
        #region Methods

        public static void Create(this StandardFile entry)
        {
            // Inserts a StandardFile entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.StandardFiles.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this StandardFile entry)
        {
            // Deletes a standardFile entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.StandardFiles
                        .First(stdf => stdf.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}