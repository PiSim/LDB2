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
            this.SubTests = new HashSet<SubTest>();
        }
    
        public int ID { get; set; }
        public int batchID { get; set; }
        public int methodID { get; set; }
        public int reportID { get; set; }
        public string Notes { get; set; }
        public int operatorID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<int> instrumentID { get; set; }
        public Nullable<int> method_issueID { get; set; }
    
        public virtual Batch Batch { get; set; }
        public virtual Person Person { get; set; }
        public virtual Report Report { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTest> SubTests { get; set; }
        public virtual Method Method { get; set; }
        public virtual Instrument instrument { get; set; }
    }
}
