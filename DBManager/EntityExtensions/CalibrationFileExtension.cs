using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class CalibrationFileExtension
    {
        #region Methods

        public static void Delete(this CalibrationFiles entry)
        {
            // DEletes a CalibrationFile entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities
                        .CalibrationFiles
                        .First(calf => calf.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();
                entry.ID = 0;
            }
        }

        #endregion Methods
    }
}