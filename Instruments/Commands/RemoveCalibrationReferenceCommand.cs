using DataAccessCore;
using LInst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments.Commands
{
    /// <summary>
    /// Command object that removes a CalibrationReportReference with the given InstrumentID and CalibrationReportID, if it exists
    /// </summary>
    
    public class RemoveCalibrationReferenceCommand : ICommand<LInstContext>
    {
        public int CalibrationReportID { get; set; }

        public int InstrumentID { get; set; }

        public RemoveCalibrationReferenceCommand(int calibrationReportID,
                                                int instrumentID)
        {
            CalibrationReportID = calibrationReportID;
            InstrumentID = instrumentID;
        }

        public void Execute(LInstContext context)
        {
            CalibrationReportReference entry = context.CalibrationReportReferences.FirstOrDefault(crr => crr.CalibrationReportID == CalibrationReportID
                                                                                                        && crr.InstrumentID == InstrumentID);

            if (entry != null)
            {
                context.Remove(entry);
                context.SaveChanges();
            }

            context.Dispose();
        }
    }
}
