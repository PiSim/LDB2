using System.Data.Entity;
using System.Linq;

namespace DataAccess
{
    public interface IDataService<T2> where T2 : DbContext
    {
        #region Methods

        void Execute(ICommand<T2> commandObject);

        T RunQuery<T>(IScalar<T, T2> queryObject);

        IQueryable<T> RunQuery<T>(IQuery<T, T2> queryObject);

        #endregion Methods
    }
}