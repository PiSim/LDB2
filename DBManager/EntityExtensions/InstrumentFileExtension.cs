using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class InstrumentFiles
    {
        public void Delete()
        {
            using (DBEntities entities = new DBEntities())
            {
                InstrumentFiles tempEntry = entities.InstrumentFiles
                                                    .FirstOrDefault(inf => inf.ID == ID);

                if (tempEntry != null)
                {
                    entities.Entry(tempEntry)
                           .State = System.Data.Entity.EntityState.Deleted;
                    entities.SaveChanges();
                }

                ID = 0;
            }
        }
    }
}
