using System.Data.Entity.Infrastructure;

namespace LabDbContext
{
    public class LabDBContextFactory : IDbContextFactory<LabDbEntities>
    {
        #region Methods

        public LabDbEntities Create()
        {
            return new LabDbEntities();
        }

        #endregion Methods
    }
}