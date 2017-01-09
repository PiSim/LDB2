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
    
    public partial class Report
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Report()
        {
            this.Tests = new HashSet<Test>();
            this.ReportFiles = new HashSet<ReportFiles>();
        }
    
        public long ID { get; set; }
        public string deletion_flag { get; set; }
        public long authorID { get; set; }
        public long batchID { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string end_date { get; set; }
        public Nullable<long> number { get; set; }
        public long projectID { get; set; }
        public Nullable<long> specificationID { get; set; }
        public Nullable<long> specification_versionID { get; set; }
        public string start_date { get; set; }
        public Nullable<long> requestID { get; set; }
        public string label { get; set; }
    
        public virtual Batch Batch { get; set; }
        public virtual Person Author { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        public virtual RequestedReports RequestedReport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportFiles> ReportFiles { get; set; }
    }
}
