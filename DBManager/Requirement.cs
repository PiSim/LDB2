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
    
    public partial class Requirement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requirement()
        {
            this.Name = "";
            this.SubRequirements = new HashSet<SubRequirement>();
            this.Overrides = new HashSet<Requirement>();
            this.TaskItems = new HashSet<TaskItem>();
        }
    
        public int ID { get; set; }
        public string Description { get; set; }
        public int MethodID { get; private set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int SpecificationVersionID { get; set; }
        public bool IsOverride { get; set; }
        public Nullable<int> OverriddenID { get; set; }
    
        public virtual Method Method { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubRequirement> SubRequirements { get; set; }
        public virtual SpecificationVersion SpecificationVersions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requirement> Overrides { get; set; }
        public virtual Requirement Overridden { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems { get; set; }
    }
}
