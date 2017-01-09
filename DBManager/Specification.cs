//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class Specification
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Specification()
        {
            this.SpecificationVersions = new HashSet<SpecificationVersion>();
            this.ControlPlans = new HashSet<ControlPlan>();
        }
    
        public long ID { get; set; }
        public string deletion_flag { get; set; }
        public long standardID { get; set; }
        public string label { get; set; }
        public string description { get; set; }
        public string old_flag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecificationVersion> SpecificationVersions { get; set; }
        public virtual Standard Standard { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ControlPlan> ControlPlans { get; set; }
    }
}
