using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace DataAccess
{
    public class DataServiceBase<T> : IDataService<T> where T : DbContext
    {
        #region Fields

        private IDbContextFactory<T> _contextFactory;

        #endregion Fields

        #region Constructors

        public DataServiceBase(IDbContextFactory<T> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        #endregion Constructors

        #region Methods

        public void Execute(ICommand<T> commandObject)
        {
            commandObject.Execute(_contextFactory.Create());
        }

        public T2 RunQuery<T2>(IScalar<T2, T> queryObject)
        {
            return queryObject.Execute(_contextFactory.Create());
        }

        public IQueryable<T2> RunQuery<T2>(IQuery<T2, T> queryObject)
        {
            return queryObject.Execute(_contextFactory.Create());
        }

        #endregion Methods
    }
}