using DataAccess;
using System.Data.Entity.Infrastructure;

namespace LabDbContext
{
    public class LabDbData : DataServiceBase<LabDbEntities>
    {
        #region Constructors

        public LabDbData(IDbContextFactory<LabDbEntities> dBContextFactory) : base(dBContextFactory)
        {
        }

        #endregion Constructors
    }
}