using DataAccess;
using LabDbContext;
using System.Linq;

namespace Reports.Queries
{
    /// <summary>
    /// Query Object that returns a single Report entitiy
    /// </summary>
    public class ReportQuery : IScalar<Report, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// Gets or sets an in Number to look for in the query
        /// </summary>
        public int? Number { get; set; }

        /// <summary>
        /// TestRecordID that will be used as a filter
        /// </summary>
        public int? TestRecordID { get; set; }

        #endregion Properties

        #region Methods

        public Report Execute(LabDbEntities context)
        {
            if (Number != null)
                return context.Reports.FirstOrDefault(rep => rep.Number == Number);

            else
                return context.Reports.FirstOrDefault(rep => rep.TestRecordID == TestRecordID);
        }

        #endregion Methods
    }
}