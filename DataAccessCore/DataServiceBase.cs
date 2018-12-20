using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq;

namespace DataAccessCore
{
    public class DataServiceBase<T> : IDataService<T> where T : DbContext
    {
        #region Fields

        private IDesignTimeDbContextFactory<T> _contextFactory;

        #endregion Fields

        #region Constructors

        public DataServiceBase(IDesignTimeDbContextFactory<T> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        #endregion Constructors

        #region Methods

        public void Execute(ICommand<T> commandObject)
        {
            commandObject.Execute(_contextFactory.CreateDbContext(new string[] { }));
        }

        public T2 RunQuery<T2>(IScalar<T2, T> queryObject)
        {
            return queryObject.Execute(_contextFactory.CreateDbContext(new string[] { }));
        }

        public IQueryable<T2> RunQuery<T2>(IQuery<T2, T> queryObject)
        {
            return queryObject.Execute(_contextFactory.CreateDbContext(new string[] { }));
        }

        #endregion Methods
    }
}