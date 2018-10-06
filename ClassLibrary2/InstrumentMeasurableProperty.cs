using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class InstrumentMeasurableProperty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InstrumentMeasurableProperty()
        {
            this.Description = "";
            this.CalibrationReportMappings = new HashSet<CalibrationReportInstrumentPropertyMapping>();
        }

        public int ID { get; set; }
        public int InstrumentID { get; set; }
        public string Description { get; set; }
        public float TargetUncertainty { get; set; }
        public int UnitID { get; set; }
        public float Resolution { get; set; }
        public int MeasurableQuantityID { get; set; }
        public double CalibrationRangeUpperLimit { get; set; }
        public double CalibrationRangeLowerLimit { get; set; }
        public double RangeUpperLimit { get; set; }
        public double RangeLowerLimit { get; set; }

        public virtual Instrument Instrument { get; set; }
        public virtual MeasurableQuantity MeasurableQuantity { get; set; }
        public virtual MeasurementUnit UnitOfMeasurement { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationReportInstrumentPropertyMapping> CalibrationReportMappings { get; set; }
    }
}