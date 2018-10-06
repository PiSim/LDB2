using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class InstrumentType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InstrumentType()
        {
            this.Instruments = new HashSet<Instrument>();
            this.MeasurableQuantities = new HashSet<MeasurableQuantity>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Instrument> Instruments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeasurableQuantity> MeasurableQuantities { get; set; }
    }
}