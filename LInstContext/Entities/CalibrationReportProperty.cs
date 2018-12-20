namespace LInst
{
    public class CalibrationReportProperty
    {
        #region Constructors

        public CalibrationReportProperty()
        {
            Name = "";
            Value = 0;
            TargetValue = 0;
            UpperLimit = 0;
            LowerLimit = 0;
            UM = "";
        }

        #endregion Constructors

        #region Properties

        public CalibrationReport CalibrationReport { get; set; }
        public int CalibrationReportID { get; set; }
        public int ID { get; set; }

        public double LowerLimit { get; set; }
        public string Name { get; set; }
        public InstrumentProperty ParentProperty { get; set; }
        public int? ParentPropertyID { get; set; }
        public double TargetValue { get; set; }
        public string UM { get; set; }
        public double UpperLimit { get; set; }
        public double Value { get; set; }

        #endregion Properties
    }
}