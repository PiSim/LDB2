using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Sample
    {

        public void Create()
        {
            // Inserts a Sample entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.Samples.Add(this);

                entities.SaveChanges();
            }
        }

        public void Delete()
        {
            // Deletes the entry from the DB

            using (DBEntities entities = new DBEntities())
            {
                Sample tempEntry = entities.Samples
                                            .FirstOrDefault(smp => smp.ID == ID);

                if (tempEntry == null)
                    return;

                

                entities.Entry(tempEntry)
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();

                ID = 0;
            }

        }
    }
}
