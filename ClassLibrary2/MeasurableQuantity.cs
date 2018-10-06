using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class MeasurableQuantity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MeasurableQuantity()
        {
            this.InstrumentMeasurableProperties = new HashSet<InstrumentMeasurableProperty>();
            this.UnitsOfMeasurement = new HashSet<MeasurementUnit>();
            this.InstrumentTypes = new HashSet<InstrumentType>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstrumentMeasurableProperty> InstrumentMeasurableProperties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeasurementUnit> UnitsOfMeasurement { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InstrumentType> InstrumentTypes { get; set; }
    }
}