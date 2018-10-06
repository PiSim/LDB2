using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class MeasurementUnit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MeasurementUnit()
        {
            this.InstrumentMeasurableProperties = new HashSet<InstrumentMeasurableProperty>();
            this.calibration_report_measurable_property_mappings = new HashSet<CalibrationReportInstrumentPropertyMapping>();
        }

        public int ID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public int MeasurableQuantityID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstrumentMeasurableProperty> InstrumentMeasurableProperties { get; set; }
        public virtual MeasurableQuantity MeasurableQuantities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalibrationReportInstrumentPropertyMapping> calibration_report_measurable_property_mappings { get; set; }
    }
}