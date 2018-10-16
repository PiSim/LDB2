﻿using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class SpecificationVersionsQuery : QueryBase<SpecificationVersion, LabDbEntities>
    {
        public int? SpecificationID { get; set; }

        public override IQueryable<SpecificationVersion> Execute(LabDbEntities context)
        {
            IQueryable<SpecificationVersion> query = context.SpecificationVersions;

            if (LazyLoadingDisabled)
                context.Configuration.LazyLoadingEnabled = false;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (SpecificationID != null)
                query = query.Where(specv => specv.SpecificationID == SpecificationID);

            if (OrderResults)
                query = query.OrderBy(specv => specv.Name);

            return query;
        }
    }
}
