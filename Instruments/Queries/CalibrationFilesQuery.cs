using DataAccessCore;
using LInst;
using System.Data.Entity;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple CalibrationReport entities
    /// </summary>
    public class CalibrationFilesQuery : QueryBase<CalibrationFile, LInstContext>
    {
        public int? CalibrationReportID { get; set; }

        #region Methods

        public override IQueryable<CalibrationFile> Execute(LInstContext context)
        {
            IQueryable<CalibrationFile> query = context.CalibrationFiles;
            
            if (AsNoTracking)
                query = query.AsNoTracking();

            if (CalibrationReportID != null)
                query = query.Where(cf => cf.CalibrationReportID == CalibrationReportID);

            return query;
        }

        #endregion Methods
    }
}