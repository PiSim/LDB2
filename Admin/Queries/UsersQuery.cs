using DataAccessCore;
using LInst;
using System.Data.Entity;
using System.Linq;

namespace Admin.Queries
{
    /// <summary>
    /// Query object that returns multiple User Entities
    /// </summary>
    public class UsersQuery : QueryBase<User, LInstContext>
    {
        #region Methods

        public override IQueryable<User> Execute(LInstContext context)
        {
            IQueryable<User> query = context.Users;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(usr => usr.UserName);

            return query;
        }

        #endregion Methods
    }
}