﻿using DataAccess;
using LabDbContext;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns Organization entities
    /// </summary>
    public class OrganizationsQuery : IQuery<Organization, LabDbEntities>
    {
        #region Fields

        private Dictionary<OrganizationRoles?, string> _roleDictionary = new Dictionary<OrganizationRoles?, string>
        {
            { OrganizationRoles.CalibrationLab, "CAL_LAB" },
            { OrganizationRoles.Maintenance, "MAINT" },
            { OrganizationRoles.Manufacturer, "MANUF" },
            { OrganizationRoles.OEM, "OEM" },
            { OrganizationRoles.StandardPublisher, "STD_PUB" },
            { OrganizationRoles.Supplier, "SUPPL" },
            { OrganizationRoles.TestLab, "TEST_LAB" }
        };

        #endregion Fields

        #region Enums

        public enum OrganizationRoles
        {
            CalibrationLab,
            Maintenance,
            Manufacturer,
            OEM,
            StandardPublisher,
            Supplier,
            TestLab
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// If true results are ordered by Name
        /// </summary>
        public bool OrderResults { get; set; } = true;

        /// <summary>
        /// Gets or sets an OrganizationRole To filter for
        /// </summary>
        public OrganizationRoles? Role { get; set; }

        #endregion Properties

        #region Methods

        public IQueryable<Organization> Execute(LabDbEntities context)
        {
            IQueryable<Organization> query = context.Organizations;

            if (Role != null)
            {
                string _roleName = _roleDictionary[Role];
                query = query.Where(org => org.RoleMapping.FirstOrDefault(orm => orm.Role.Name == _roleName).IsSelected == true);
            }

            if (OrderResults)
                query = query.OrderBy(org => org.Name);

            return query;
        }

        #endregion Methods
    }
}