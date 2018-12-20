using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Reports.Queries
{
    public class ExternalReportQuery : ScalarBase<ExternalReport, LabDbEntities>
    {
        #region Fields

        private int? _ID;

        #endregion Fields

        #region Constructors

        public ExternalReportQuery(int ID)
        {
            _ID = ID;
        }

        #endregion Constructors

        #region Methods

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

        #endregion Methods
    }
}