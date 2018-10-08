using System.Data.Entity;
using System.Linq;

namespace LabDbContext
{
    public partial class TestRecord
    {
        #region Methods
        /// <summary>
        /// Returns the report which uses this TestRecord
        /// </summary>
        /// <returns></returns>
        public object GetReport()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                if (RecordTypeID == 1)
                    return entities.TestRecords
                                    .Include(tstr => tstr.Reports).First(tsr => tsr.ID == ID)
                                    .Reports
                                    .FirstOrDefault();
                else if (RecordTypeID == 2)
                    return entities.TestRecords
                                    .Include(tstr => tstr.ExternalReports)
                                    .First(tsr => tsr.ID == ID)
                                    .ExternalReports
                                                .FirstOrDefault();
                else
                    return null;
            }
        }

        #endregion Methods
    }
}