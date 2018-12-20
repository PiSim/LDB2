namespace LInst
{
    public class InstrumentProperty
    {
        #region Constructors

        public InstrumentProperty()
        {
            Name = "";
            Value = 0;
            TargetValue = 0;
            UpperLimit = 0;
            LowerLimit = 0;
            UM = "";
            IsCalibrationProperty = false;
        }

        #endregion Constructors

        #region Properties

        public int ID { get; set; }

        public Instrument Instrument { get; set; }
        public int InstrumentID { get; set; }

        public bool IsCalibrationProperty { get; set; }
        public double LowerLimit { get; set; }
        public string Name { get; set; }
        public double TargetValue { get; set; }
        public string UM { get; set; }
        public double UpperLimit { get; set; }
        public double Value { get; set; }

        #endregion Properties
    }
}