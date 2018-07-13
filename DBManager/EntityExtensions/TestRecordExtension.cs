using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class TestRecord
    {
        public void Create()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.TestRecords.Add(this);

                entities.SaveChanges();
            }
        }

        /// <summary>
        /// Removes this entry from the database
        /// </summary>
        public void Delete()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.TestRecords.First(tstr => tstr.ID == ID))
                        .State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        /// <summary>
        /// Returns the report which uses this TestRecord
        /// </summary>
        /// <returns></returns>
        public object GetReport()
        {
            using (DBEntities entities = new DBEntities())
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
    }
}
