using DataAccess;
using LabDbContext;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple ExternalReport entities
    /// </summary>
    public class ExternalReportsQuery : IQuery<ExternalReport, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If true the results are ordered by Year descending then by Number descending
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<ExternalReport> Execute(LabDbEntities context)
        {
            IQueryable<ExternalReport> query = context.ExternalReports;

            if (OrderResults)
                query = query.OrderByDescending(exrep => exrep.Year)
                            .ThenByDescending(exrep => exrep.Number);

            return query;
        }

        #endregion Methods
    }
}