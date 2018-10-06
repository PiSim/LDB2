using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class SubMethodExtension
    {
        #region Methods

        public static void Delete(this SubMethod entry)
        {
            // Deletes a SubMethod entry from the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.SubMethods.First(smtd => smtd.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();
                entry.ID = 0;
            }
        }

        #endregion Methods
    }
}