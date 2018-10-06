using DataAccess;
using LabDbContext;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query Object that returns multiple Report entities
    /// </summary>
    public class ReportsQuery : IQuery<Report, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If true the results are ordered by number descending
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<Report> Execute(LabDbEntities context)
        {
            IQueryable<Report> query = context.Reports;

            if (OrderResults)
                query = query.OrderByDescending(rep => rep.Number);

            return query;
        }

        #endregion Methods
    }
}