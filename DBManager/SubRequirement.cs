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
    
    public partial class SubRequirement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubRequirement()
        {
            this.SubTests = new HashSet<SubTest>();
        }
    
        public long ID { get; set; }
        public string DeletionFlag { get; set; }
        public string Name { get; set; }
        public long requirementID { get; set; }
        public string RequiredValue { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTest> SubTests { get; set; }
        public virtual Requirement Requirement { get; set; }
    }
}
