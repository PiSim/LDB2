using System.Data.Entity;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class ReportFileExtension
    {
        #region Methods

        public static void Create(this ReportFile entry)
        {
            // Inserts a ReportFile entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.ReportFiles.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this ReportFile entry)
        {
            // Deletes a ReportFile entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.ReportFiles
                        .First(repf => repf.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        #endregion Methods
    }
}