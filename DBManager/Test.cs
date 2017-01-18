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
    
    public partial class Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test()
        {
            this.sub_tests = new HashSet<SubTest>();
        }
    
        public int ID { get; set; }
        public int batchID { get; set; }
        public int methodID { get; set; }
        public int reportID { get; set; }
        public Nullable<int> external_reportID { get; set; }
        public string notes { get; set; }
        public int operatorID { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<int> meets_requirements { get; set; }
        public string stat_skip { get; set; }
    
        public virtual Batch batch { get; set; }
        public virtual Person person { get; set; }
        public virtual Report report { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTest> sub_tests { get; set; }
    }
}
