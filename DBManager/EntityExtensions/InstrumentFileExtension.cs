using System.Linq;

namespace LabDbContext
{
    public partial class InstrumentFiles
    {
        #region Methods

        public void Delete()
        {
            using (LabDbEntities entities = new LabDbEntities())
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

        #endregion Methods
    }
}