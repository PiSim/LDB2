﻿using DataAccess;
using LabDbContext;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Queries
{
    /// <summary>
    /// Query Object that returns multiple OrganizationRoles entities
    /// </summary>
    public class OrganizationRolesQuery : QueryBase<OrganizationRole, LabDbEntities>
    {
        public override IQueryable<OrganizationRole> Execute(LabDbEntities context)
        {
            IQueryable<OrganizationRole> query = context.OrganizationRoles;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(orm => orm.Name);

            return query;
        }
    }
}
