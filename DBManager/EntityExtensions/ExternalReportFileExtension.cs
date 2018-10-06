using System.Data.Entity;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class ExternalReportFileExtension
    {
        #region Methods

        public static void Create(this ExternalReportFile entry)
        {
            // Inserts an ExternalReportFile entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.ExternalReportFiles.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this ExternalReportFile entry)
        {
            // Deletes an ExternalReportFile entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.ExternalReportFiles
                        .First(exrf => exrf.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}