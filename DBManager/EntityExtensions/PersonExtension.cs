using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class PersonExtension
    {
        public static void Create(this Person entry)
        {
            // inserts a new Person entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.People.Add(entry);
                entities.SaveChanges();
            }
        }
    }
}
