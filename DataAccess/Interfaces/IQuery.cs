using System.Data.Entity;
using System.Linq;

namespace DataAccess
{
    public interface IQuery<out T, T2> where T2 : DbContext
    {
        #region Methods

        IQueryable<T> Execute(T2 context);

        #endregion Methods
    }
}