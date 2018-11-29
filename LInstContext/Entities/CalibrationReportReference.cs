using DataAccessCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class CalibrationReportReference
    {
        public int CalibrationReportID { get; set; }
        public CalibrationReport CalibrationReport { get; set; }

        public int InstrumentID { get; set; }
        public Instrument Instrument { get; set; }
    }
}
