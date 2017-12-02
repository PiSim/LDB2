using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Person
    {
        public void Create()
        {
            // inserts a new Person entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.People.Add(this);
                entities.SaveChanges();
            }
        }
        
        public void Load()
        {
            // Loads the relevant Related Entities into a given Person Instance
            
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.People.Attach(this);

                Person tempEntry = entities.People.Include(per => per.RoleMappings
                                                    .Select(prm => prm.Role))
                                                    .First(per => per.ID == ID);

                entities.Entry(this).CurrentValues.SetValues(tempEntry);
            }
        }

        public void Update()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.People.AddOrUpdate(this);
                entities.SaveChanges();
            }
        }
    }
}
