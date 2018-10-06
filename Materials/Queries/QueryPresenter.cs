using DataAccess;
using System.Data.Entity;

namespace Materials.Queries
{
    public class QueryPresenter<T, T2> : IQueryPresenter<T, T2> where T2 : DbContext
    {
        #region Constructors

        public QueryPresenter(IQuery<T, T2> query,
                                string name,
                                string description = "")
        {
            Description = description;
            Name = name;
            Query = query;
        }

        #endregion Constructors

        #region Properties

        public string Description { get; }
        public string Name { get; }
        public IQuery<T, T2> Query { get; }

        #endregion Properties
    }
}