using DataAccessCore;
using LInst;
using System.Data.Entity;
using System.Linq;

namespace Admin.Queries
{
    /// <summary>
    /// Query object that returns multiple PersonRole entities
    /// </summary>
    public class PersonRolesQuery : QueryBase<PersonRole, LInstContext>
    {
        #region Methods

        public override IQueryable<PersonRole> Execute(LInstContext context)
        {
            IQueryable<PersonRole> query = context.PersonRoles;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(pr => pr.Name);

            return query;
        }

        #endregion Methods
    }
}