using DataAccessCore;
using LInst;
using System;
using System.Linq;

namespace Instruments.ViewModels
{
    public class LastCalibrationDateQuery : ScalarBase<DateTime?, LInstContext>
    {
        #region Constructors

        public LastCalibrationDateQuery(int instrumentID)
        {
            InstrumentID = instrumentID;
        }

        #endregion Constructors

        #region Properties

        public int InstrumentID { get; set; }

        #endregion Properties

        #region Methods

        public override DateTime? Execute(LInstContext context)
        {
            IQueryable<CalibrationReport> reports = context.CalibrationReports.Where(crep => crep.InstrumentID == InstrumentID);

            if (reports.Count() == 0)
                return null;

            return reports.Max(crep => crep.Date);
        }

        #endregion Methods
    }
}