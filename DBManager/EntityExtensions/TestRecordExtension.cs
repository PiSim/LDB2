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
    }
}
