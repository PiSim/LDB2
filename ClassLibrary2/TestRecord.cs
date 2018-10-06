using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class TestRecord
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TestRecord()
        {
            this.Notes = "\'\'";
            this.Tests = new HashSet<Test>();
            this.Reports = new HashSet<Report>();
            this.ExternalReports = new HashSet<ExternalReport>();
        }

        public int ID { get; set; }
        public int BatchID { get; set; }
        public int RecordTypeID { get; set; }
        public string Notes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Reports { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual TestRecordType RecordType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalReport> ExternalReports { get; set; }
    }
}