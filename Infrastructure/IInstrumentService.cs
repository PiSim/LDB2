using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IInstrumentService
    {
        void AddCalibrationFiles(IEnumerable<CalibrationFiles> fileList);
        IEnumerable<CalibrationResult> GetCalibrationResults();
        void ShowNewCalibrationDialog(Instrument instance);
        void AddInstrumentFiles(IEnumerable<InstrumentFiles> fileList);
        void ShowNewMaintenanceDialog(Instrument instance);
        IEnumerable<Instrument> GetCalibrationCalendar();
        void CreateInstrument();
    }
}
