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
    /// <summary>
    /// Query Object that returns multiple TestRecord entities.
    /// </summary>
    public class TestRecordsQuery : QueryBase<TestRecord, LabDbEntities>
    {
        /// <summary>
        /// If set only the records for the ExternalReport with the given ID are returned
        /// </summary>
        public int? ExternalReportID { get; set; }

        public override IQueryable<TestRecord> Execute(LabDbEntities context)
        {
            if (LazyLoadingDisabled)
                context.Configuration.LazyLoadingEnabled = false;

            IQueryable<TestRecord> query = context.TestRecords;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (EagerLoadingEnabled)
                query = query.Include(tstr => tstr.Batch.Material.Aspect)
                            .Include(tstr => tstr.Batch.Material.MaterialLine)
                            .Include(tstr => tstr.Batch.Material.MaterialType)
                            .Include(tstr => tstr.Batch.Material.Recipe.Colour)
                            .Include(tstr => tstr.Tests.Select(tst => tst.MethodVariant.Method.Property))
                            .Include(tstr => tstr.Tests.Select(tst => tst.MethodVariant.Method.Standard.Organization))
                            .Include(tstr => tstr.Tests.Select(tst => tst.SubTests));

            if (ExternalReportID != null)
                query = query.Where(tstr => tstr.ExternalReports.Any(exrep => exrep.ID == ExternalReportID));

            if (OrderResults)
                query = query.OrderBy(tstr => tstr.Batch.Number);

            return query;
        }
    }
}
