using System.Linq;

namespace LabDbContext
{
    public partial class Sample
    {
        #region Methods

        public void Create()
        {
            // Inserts a Sample entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Samples.Add(this);

                entities.SaveChanges();
            }
        }

        public void Delete()
        {
            // Deletes the entry from the DB

            using (LabDbEntities entities = new LabDbEntities())
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

        #endregion Methods
    }
}