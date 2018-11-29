using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class CalibrationReport
    {
        public CalibrationReport()
        {

        }

        public int ID { get; set; }

        public int CalibrationResultID { get; set; }
        public int LaboratoryID { get; set; }
        public int InstrumentID { get; set; }
        public int? TechID { get; set; }

        public CalibrationResult CalibrationResult { get; set; }
        public Organization Laboratory { get; set; }
        public Instrument Instrument { get; set; }
        public Person Tech { get; set; }
        
        public DateTime Date { get; set; }

        public int Year { get; set; }
        public int Number { get; set; }

        public string Notes { get; set; }

        public ICollection<CalibrationReportReference> CalibrationReportReferences { get; }
    }
}
