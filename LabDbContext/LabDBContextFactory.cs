using System.Data.Entity.Infrastructure;

namespace LabDbContext
{
    public class LabDBContextFactory : IDbContextFactory<LabDbEntities>
    {
        #region Fields

        private string _connectionName;

        #endregion Fields

        #region Constructors

        public LabDBContextFactory(string connectionName)
        {
            _connectionName = connectionName;
        }

        #endregion Constructors

        #region Methods

        public LabDbEntities Create()
        {
            return new LabDbEntities(_connectionName);
        }

        #endregion Methods
    }
}