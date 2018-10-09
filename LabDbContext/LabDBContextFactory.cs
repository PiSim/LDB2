using System.Data.Entity.Infrastructure;

namespace LabDbContext
{
    public class LabDBContextFactory : IDbContextFactory<LabDbEntities>
    {
        private string _connectionName;
        
        public LabDBContextFactory(string connectionName)
        {
            _connectionName = connectionName;
        }

        #region Methods

        public LabDbEntities Create()
        {
            return new LabDbEntities(_connectionName);
        }

        #endregion Methods
    }
}