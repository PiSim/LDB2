using DataAccess;
using System.Data.Entity;

namespace Materials.Queries
{
    public interface IQueryPresenter<T, T2> where T2 : DbContext
    {
        #region Properties

        string Description { get; }
        string Name { get; }
        IQuery<T, T2> Query { get; }

        #endregion Properties
    }
}