using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccessCore
{
    public interface IQuery<out T, T2> where T2 : DbContext
    {
        #region Methods

        IQueryable<T> Execute(T2 context);

        #endregion Methods
    }
}