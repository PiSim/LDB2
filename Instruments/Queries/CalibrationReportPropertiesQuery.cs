using DataAccessCore;
using LInst;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns all  the CalibrationReportProperty entries with a given CalibrationReportID
    /// </summary>
    public class CalibrationReportPropertiesQuery : QueryBase<CalibrationReportProperty, LInstContext>
    {
        #region Constructors

        public CalibrationReportPropertiesQuery(int? calibrationReportID)
        {
            CalibrationReportID = calibrationReportID;
        }

        #endregion Constructors

        #region Properties

        public int? CalibrationReportID { get; set; }

        #endregion Properties

        #region Methods

        public override IQueryable<CalibrationReportProperty> Execute(LInstContext context)
        {
            IQueryable<CalibrationReportProperty> query = context.CalibrationReportProperties;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(crp => crp.Name);

            return query.Where(crp => crp.CalibrationReportID == CalibrationReportID);
        }

        #endregion Methods
    }
}