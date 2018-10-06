using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Specification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Specification()
        {
            this.SpecificationVersions = new HashSet<SpecificationVersion>();
            this.ControlPlans = new HashSet<ControlPlan>();
        }

        public int ID { get; private set; }
        public int StandardID { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecificationVersion> SpecificationVersions { get; set; }
        public virtual Std Standard { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ControlPlan> ControlPlans { get; set; }
    }
}