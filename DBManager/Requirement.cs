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
            this.sub_requirements = new HashSet<SubRequirement>();
        }
    
        public int ID { get; set; }
        public string description { get; set; }
        public int methodID { get; set; }
        public string name { get; set; }
        public int position { get; set; }
        public Nullable<int> specificationID { get; set; }
        public Nullable<int> specification_versionID { get; set; }
    
        public virtual Method method { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubRequirement> sub_requirements { get; set; }
        public virtual Specification specification { get; set; }
        public virtual SpecificationVersion specification_versions { get; set; }
    }
}
