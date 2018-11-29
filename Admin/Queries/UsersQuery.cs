using DataAccessCore;
using LInst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Queries
{
    /// <summary>
    /// Query object that returns multiple User Entities
    /// </summary>
    public class UsersQuery : QueryBase<User, LInstContext>
    {
        public override IQueryable<User> Execute(LInstContext context)
        {
            IQueryable<User> query = context.Users;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(usr => usr.UserName);

            return query;
        }
    }
}
