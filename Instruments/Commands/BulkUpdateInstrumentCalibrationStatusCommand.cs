using DataAccessCore;
using LInst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Instruments.Commands
{
    public class BulkUpdateInstrumentCalibrationStatusCommand : ICommand<LInstContext>
    {
        #region Fields

        private IEnumerable<Instrument> _instrumentEntries;

        #endregion Fields

        #region Constructors

        public BulkUpdateInstrumentCalibrationStatusCommand(IEnumerable<Instrument> instrumentEntries)
        {
            _instrumentEntries = instrumentEntries;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LInstContext context)
        {
            foreach (Instrument entry in _instrumentEntries)
            {
                if (!entry.IsInService || !entry.IsUnderControl)
                    entry.CalibrationDueDate = null;
                else
                {
                    IQueryable<CalibrationReport> entryCalibrations = context.CalibrationReports.Where(crep => crep.InstrumentID == entry.ID);

                    if (entryCalibrations.Count() != 0)
                    {
                        DateTime lastCalibrationDate = entryCalibrations.Max(crep => crep.Date);
                        entry.CalibrationDueDate = lastCalibrationDate.AddMonths((int)entry.CalibrationInterval);
                    }
                    else
                        entry.CalibrationDueDate = DateTime.Today.Date;
                }

                context.Entry(entry).Property(ins => ins.CalibrationDueDate).IsModified = true;
            }

            context.SaveChanges();
            context.Dispose();
        }

        #endregion Methods
    }
}