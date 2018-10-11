using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Queries
{
    public class ExternalReportQuery : ScalarBase<ExternalReport, LabDbEntities>
    {
        int? _ID;

        public ExternalReportQuery(int ID)
        {
            _ID = ID;
        }

        public override ExternalReport Execute(LabDbEntities context)
        {
            IQueryable<ExternalReport> query = context.ExternalReports;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (LazyLoadingDisabled)
                context.Configuration.LazyLoadingEnabled = false;

            if (EagerLoadingEnabled)
                query = query.Include(exrep => exrep.ExternalLab);

            return query.FirstOrDefault(exrep => exrep.ID == _ID);
        }
    }
}
