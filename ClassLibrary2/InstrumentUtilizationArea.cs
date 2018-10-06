using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class InstrumentUtilizationArea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InstrumentUtilizationArea()
        {
            this.Instruments = new HashSet<Instrument>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Plant { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Instrument> Instruments { get; set; }
    }
}