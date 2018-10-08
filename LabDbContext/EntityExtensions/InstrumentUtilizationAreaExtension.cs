using System.Data.Entity;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class InstrumentUtilizationAreaExtension
    {
        #region Methods

        public static void Create(this InstrumentUtilizationArea entry)
        {
            // Inserts a new InstrumentUtilizationArea entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.InstrumentUtilizationAreas.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this InstrumentUtilizationArea entry)
        {
            // Deletes an InstrumentUtilizationArea entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities
                        .InstrumentUtilizationAreas
                        .First(iua => iua.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
                entry.ID = 0;
            }
        }

        #endregion Methods
    }
}