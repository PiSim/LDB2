using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class CalibrationFile
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public int CalibrationReportID { get; set; }

        public CalibrationReport CalibrationReport { get; set; }
    }
}
