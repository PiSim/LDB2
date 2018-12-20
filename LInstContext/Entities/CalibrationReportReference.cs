namespace LInst
{
    public class CalibrationReportReference
    {
        #region Properties

        public CalibrationReport CalibrationReport { get; set; }
        public int CalibrationReportID { get; set; }
        public Instrument Instrument { get; set; }
        public int InstrumentID { get; set; }

        #endregion Properties
    }
}