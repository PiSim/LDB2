using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public partial class Person
    {
        #region Methods

        public void Create()
        {
            // inserts a new Person entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.People.Add(this);
                entities.SaveChanges();
            }
        }

        public void Load()
        {
            // Loads the relevant Related Entities into a given Person Instance

            using (LabDbEntities entities = new LabDbEntities())
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
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.People.AddOrUpdate(this);
                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}