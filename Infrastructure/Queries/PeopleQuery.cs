using DataAccess;
using LabDbContext;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple Person Entities
    /// </summary>
    public class PeopleQuery : QueryBase<Person, LabDbEntities>
    {
        #region Fields

        private Dictionary<PersonRoles?, string> _roleNames = new Dictionary<PersonRoles?, string>
        {
            {PersonRoles.CalibrationTech,"CAL_TECH"  },
            {PersonRoles.LabManager,"LAB_MANAGER"  },
            {PersonRoles.MaterialTestingTech,"TEST_TECH"  },
            {PersonRoles.ProjectLeader,"PRJ_LEADER"  },
        };

        #endregion Fields

        #region Enums

        public enum PersonRoles
        {
            CalibrationTech,
            LabManager,
            MaterialTestingTech,
            ProjectLeader
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// Gets/Sets the role used to filter the results.
        /// </summary>
        public PersonRoles? Role { get; set; }

        #endregion Properties

        #region Methods

        public override IQueryable<Person> Execute(LabDbEntities context)
        {
            IQueryable<Person> query = context.People;

            if (Role != null)
            {
                string _roleName = _roleNames[Role];

                query = query.Where(per => per.RoleMappings.FirstOrDefault(prm => prm.Role.Name == _roleName).IsSelected);
            }

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(per => per.Name);

            return query;
        }

        #endregion Methods
    }
}