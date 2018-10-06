using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class ExternalConstruction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExternalConstruction()
        {
            this.Materials = new HashSet<Material>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> OemID { get; set; }
        public Nullable<int> DefaultSpecVersionID { get; set; }

        public virtual Organization Oem { get; set; }
        public virtual SpecificationVersion DefaultSpecVersion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> Materials { get; set; }
    }
}