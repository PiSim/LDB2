using DataAccessCore;
using LInst;
using System;
using System.Linq;

namespace Instruments.Commands
{
    public class UpdateInstrumentCalibrationStatusCommand : ICommand<LInstContext>
    {
        #region Fields

        private Instrument _instrumentEntry;

        #endregion Fields

        #region Constructors

        public UpdateInstrumentCalibrationStatusCommand(Instrument instrumentEntry)
        {
            _instrumentEntry = instrumentEntry;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LInstContext context)
        {
            if (!_instrumentEntry.IsInService || !_instrumentEntry.IsUnderControl)
                _instrumentEntry.CalibrationDueDate = null;
            else
            {
                IQueryable<CalibrationReport> entryCalibrations = context.CalibrationReports.Where(crep => crep.InstrumentID == _instrumentEntry.ID);

                if (entryCalibrations.Count() != 0)
                {
                    DateTime lastCalibrationDate = entryCalibrations.Max(crep => crep.Date);
                    _instrumentEntry.CalibrationDueDate = lastCalibrationDate.AddMonths((int)_instrumentEntry.CalibrationInterval);
                }
                else
                    _instrumentEntry.CalibrationDueDate = DateTime.Today.Date;
            }

            context.Update(_instrumentEntry);
            context.SaveChanges();
            context.Dispose();
        }

        #endregion Methods
    }
}