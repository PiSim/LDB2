using DataAccessCore;
using LInst;
using System.Data.Entity;
using System.Linq;

namespace Admin.Queries
{
    /// <summary>
    /// Query object that returns multiple OrganizationRoleMapping entries given an Organization ID
    /// </summary>
    public class OrganizationRoleMappingsQuery : QueryBase<OrganizationRoleMapping, LInstContext>
    {
        #region Constructors

        public OrganizationRoleMappingsQuery(int organizationID)
        {
            OrganizationID = organizationID;
        }

        #endregion Constructors

        #region Properties

        public int? OrganizationID { get; set; }

        #endregion Properties

        #region Methods

        public override IQueryable<OrganizationRoleMapping> Execute(LInstContext context)
        {
            IQueryable<OrganizationRoleMapping> query = context.OrganizationRoleMappings;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (EagerLoadingEnabled)
                query = query.Include(orm => orm.OrganizationRole);

            if (OrderResults)
                query = query.OrderBy(orm => orm.OrganizationRole.Name);

            return query.Where(orm => orm.OrganizationID == OrganizationID);
        }

        #endregion Methods
    }
}