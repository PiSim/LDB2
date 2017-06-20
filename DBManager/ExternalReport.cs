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
    
    public partial class ExternalReport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExternalReport()
        {
            this.ExternalReportFiles = new HashSet<ExternalReportFile>();
            this.Methods = new HashSet<Method>();
            this.Batches = new HashSet<Batch>();
        }
    
        public int ID { get; set; }
        public string Description { get; set; }
        public string ExternalNumber { get; set; }
        public int InternalNumber { get; set; }
        public int ExternalLabID { get; set; }
        public bool MaterialSent { get; set; }
        public bool RequestDone { get; set; }
        public string PurchaseOrder { get; set; }
        public double Price { get; set; }
        public string Samples { get; set; }
        public string Currency { get; set; }
        public bool ReportReceived { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> PurchaseOrderID { get; set; }
        public Nullable<System.DateTime> ArrivalDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalReportFile> ExternalReportFiles { get; set; }
        public virtual Organization ExternalLab { get; set; }
        public virtual Project Project { get; set; }
        public virtual PurchaseOrder PO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Method> Methods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Batch> Batches { get; set; }
    }
}
