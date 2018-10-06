using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class CalibrationReportInstrumentPropertyMapping
    {
        public int ID { get; set; }
        public int CalibrationReportID { get; set; }
        public int MeasurablePropertyID { get; set; }
        public double UpperRangeValue { get; set; }
        public double LowerRangeValue { get; set; }
        public double ExtendedUncertainty { get; set; }
        public int MeasurementUnitID { get; set; }

        public virtual CalibrationReport CalibrationReport { get; set; }
        public virtual InstrumentMeasurableProperty InstrumentMeasurableProperty { get; set; }
        public virtual MeasurementUnit MeasurementUnit { get; set; }
    }
}