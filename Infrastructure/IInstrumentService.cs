using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public interface IInstrumentService
    {
        void AddCalibrationFiles(IEnumerable<CalibrationFiles> fileList);
        IEnumerable<CalibrationResult> GetCalibrationResults();
        CalibrationReport ShowNewCalibrationDialog(Instrument instance);
        InstrumentMaintenanceEvent ShowNewMaintenanceDialog(Instrument instance);
        IEnumerable<Instrument> GetCalibrationCalendar();
        Instrument CreateInstrument();
    }
}
