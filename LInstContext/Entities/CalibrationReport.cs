using System;
using System.Collections.Generic;

namespace LInst
{
    public class CalibrationReport
    {
        #region Constructors

        public CalibrationReport()
        {
            CalibrationReportReferences = new HashSet<CalibrationReportReference>();
            CalibrationReportProperties = new HashSet<CalibrationReportProperty>();
        }

        #endregion Constructors

        #region Properties

        public ICollection<CalibrationReportProperty> CalibrationReportProperties { get; set; }
        public ICollection<CalibrationReportReference> CalibrationReportReferences { get; set; }
        public CalibrationResult CalibrationResult { get; set; }
        public int CalibrationResultID { get; set; }
        public DateTime Date { get; set; }
        public int ID { get; set; }
        public Instrument Instrument { get; set; }
        public int InstrumentID { get; set; }
        public Organization Laboratory { get; set; }
        public int LaboratoryID { get; set; }
        public string Notes { get; set; }
        public int Number { get; set; }
        public Person Tech { get; set; }
        public int? TechID { get; set; }
        public int Year { get; set; }

        #endregion Properties
    }
}